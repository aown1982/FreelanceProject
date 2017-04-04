using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DAL;
using DAL.UserData;
using System.Web.Http;
using System.Web.Http.Description;
using RESTfulBAL.Models.DynamoDB.Wellness;
using RESTfulBAL.Utilities;
using RESTfulBAL.Utilities.ErrorHandling;
using RESTfulBAL.Utilities.AuditHandling;

namespace RESTfulBAL.Controllers.DynamoDB
{
    public class wBloodPressureController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UserDataEntities dbErr = new UserDataEntities();

        // POST: api/DynamoDB/wBloodPressure
        [Route("api/DynamoDB/wBloodPressure")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(BloodPressure value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (value.source == null || value.humanId == null)
            {
                return BadRequest();
            }

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    tSourceService sourceServiceObj = db.tSourceServices
                        .SingleOrDefault(x => x.ServiceName == value.source && x.SourceID == 5);

                    if (sourceServiceObj == null)
                    {
                        sourceServiceObj = new tSourceService();
                        sourceServiceObj.ServiceName = value.source;
                        sourceServiceObj.TypeID = 2; //Wellness
                        sourceServiceObj.SourceID = 5; //HumanAPI

                        db.tSourceServices.Add(sourceServiceObj);
                    }

                    tUserSourceService userSourceServiceObj = null;

                    //Get credentials
                    tCredential credentialObj =
                                    db.tCredentials.SingleOrDefault(x => x.SourceID == 5 && x.SourceUserID == value.humanId &&
                                                             x.SystemStatusID == 1);
                    if (credentialObj == null)
                    {
                        throw new NoUserCredentialsException("Unable to find any matching HAPI user credentials");
                    }
                    else
                    {
                        userSourceServiceObj = db.tUserSourceServices.SingleOrDefault(
                                                                        x => x.SourceServiceID == sourceServiceObj.ID &&
                                                                             x.CredentialID == credentialObj.ID &&
                                                                             x.SystemStatusID == 1);

                        if (userSourceServiceObj == null)
                        {
                            userSourceServiceObj = new tUserSourceService();
                            userSourceServiceObj.SourceServiceID = sourceServiceObj.ID;
                            userSourceServiceObj.UserID = credentialObj.UserID;
                            userSourceServiceObj.CredentialID = credentialObj.ID;
                            userSourceServiceObj.ConnectedOnDateTime = DateTime.Now;
                            userSourceServiceObj.LastSyncDateTime = DateTime.Now;
                            userSourceServiceObj.LatestDateTime = value.updatedAt;
                            userSourceServiceObj.StatusID = 3; //connected
                            userSourceServiceObj.SystemStatusID = 1; //valid
                            userSourceServiceObj.tCredential = credentialObj;

                            db.tUserSourceServices.Add(userSourceServiceObj);
                        }
                        else
                        {
                            //update LatestDateTime to the most recent datetime
                            if (userSourceServiceObj.LatestDateTime == null ||
                                userSourceServiceObj.LatestDateTime < value.updatedAt)
                            {
                                userSourceServiceObj.LatestDateTime = value.updatedAt;
                            }
                        }
                    }

                    string[] vitalNameArray = { "Systolic Blood Pressure", "Diastolic Blood Pressure", "Heart Rate" };
                    List<tUserVital> userVitalsArray = new List<tUserVital>();

                    foreach (string vitalNameItem in vitalNameArray)
                    {
                        tUserVital userVital = null;
                        userVital = db.tUserVitals
                            .SingleOrDefault(x => x.SourceObjectID == value.id && x.Name == vitalNameItem);

                        if (userVital == null)
                        {
                            userVital = new tUserVital();
                            userVital.tUserSourceService = userSourceServiceObj;
                            userVital.UserID = credentialObj.UserID;
                            userVital.SourceObjectID = value.id;
                            userVital.UserSourceServiceID = sourceServiceObj.ID;
                            userVital.Name = vitalNameItem;

                            switch (vitalNameItem)
                            {
                                case "Systolic Blood Pressure":
                                    userVital.Value = decimal.Parse(value.systolic);
                                    userVital.UOMID = 470;//mmHg
                                    break;
                                case "Diastolic Blood Pressure":
                                    userVital.Value = decimal.Parse(value.diastolic);
                                    userVital.UOMID = 470;//mmHg
                                    break;
                                case "Heart Rate":
                                    userVital.Value = decimal.Parse(value.heartRate);
                                    userVital.UOMID = 30;//bpm
                                    break;
                            }

                            //Dates
                            DateTimeOffset dtoStart;
                            if (RESTfulBAL.Models.DynamoDB.Utilities.ConvertToDateTimeOffset(value.timestamp,
                                value.tzOffset,
                                out dtoStart))
                                userVital.ResultDateTime = dtoStart;
                            else
                                userVital.ResultDateTime = value.timestamp;

                            userVital.SystemStatusID = 1;

                            db.tUserVitals.Add(userVital);
                        }
                        else
                        {
                            userVital.Name = vitalNameItem;

                            switch (vitalNameItem)
                            {
                                case "Systolic Blood Pressure":
                                    userVital.Value = decimal.Parse(value.systolic);
                                    userVital.UOMID = 470;//mm[Hg]
                                    break;
                                case "Diastolic Blood Pressure":
                                    userVital.Value = decimal.Parse(value.diastolic);
                                    userVital.UOMID = 470;//mm[Hg]
                                    break;
                                case "Heart Rate":
                                    userVital.Value = decimal.Parse(value.heartRate);
                                    userVital.UOMID = 30;//bpm
                                    break;
                            }

                            //Dates
                            DateTimeOffset dtoStart;
                            if (RESTfulBAL.Models.DynamoDB.Utilities.ConvertToDateTimeOffset(value.timestamp,
                                value.tzOffset,
                                out dtoStart))
                                userVital.ResultDateTime = dtoStart;
                            else
                                userVital.ResultDateTime = value.timestamp;

                            userVital.LastUpdatedDateTime = DateTime.Now;
                            userVital.tUserSourceService = userSourceServiceObj;
                        }
                        userVitalsArray.Add(userVital);
                    }
                    
                    db.SaveChanges();

                    dbContextTransaction.Commit();

                    return Ok(userVitalsArray);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    //Insert Error Log
                    tUserDataErrLog userErrorLog = new tUserDataErrLog();

                    userErrorLog.ErrTypeID = (int) ErrorLogging.enumErrorType.Application;
                    userErrorLog.ErrSourceID = (int) AuditLogging.enumApplication.SFCBAL;
                    userErrorLog.Code = ex.HResult.ToString();
                    userErrorLog.Description = ex.Message;
                    userErrorLog.Trace = ex.StackTrace;

                    dbErr.tUserDataErrLogs.Add(userErrorLog);
                    dbErr.SaveChanges();

                    string ErrMsg = "An error occured and we have logged the error. Please try again later.";

                    Exception Err = new Exception(ErrMsg, ex);

                    return InternalServerError(Err);
                }
            }
        }
    }
}
