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
    public class wBloodOxygenController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UserDataEntities dbErr = new UserDataEntities();

        // POST: api/DynamoDB/wBloodOxygen
        [Route("api/DynamoDB/wBloodOxygen")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(BloodOxygen value)
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

                    tUserTestResult userTestResult = null;
                    userTestResult = db.tUserTestResults
                                        .SingleOrDefault(x => x.SourceObjectID == value.id);

                    if (userTestResult == null)
                    {
                        //insert
                        userTestResult = new tUserTestResult();
                        userTestResult.SourceObjectID = value.id;
                        userTestResult.UserID = credentialObj.UserID;
                        userTestResult.tUserSourceService = userSourceServiceObj;
                        userTestResult.UserSourceServiceID = userSourceServiceObj.ID;
                        userTestResult.Name = "Blood Oxygen";
                        userTestResult.StatusID = 3; //captured
                        userTestResult.SystemStatusID = 1;

                        //Dates
                        DateTimeOffset dtoStart;
                        if (RESTfulBAL.Models.DynamoDB.Utilities.ConvertToDateTimeOffset(value.timestamp,
                            value.tzOffset,
                            out dtoStart))
                            userTestResult.ResultDateTime = dtoStart;
                        else
                            userTestResult.ResultDateTime = value.timestamp;

                        tUserTestResultComponent userTestResultComponent = new tUserTestResultComponent();
                        userTestResultComponent.SystemStatusID = 1;
                        userTestResultComponent.Name = "Blood Oxygen";
                        userTestResultComponent.Value = value.value.ToString();

                        //UOM
                        if (value.unit != null)
                        {
                            tUnitsOfMeasure uom = null;
                            uom = db.tUnitsOfMeasures.SingleOrDefault(x => x.UnitOfMeasure == value.unit);
                            if (uom == null)
                            {
                                uom = new tUnitsOfMeasure();
                                uom.UnitOfMeasure = value.unit;

                                db.tUnitsOfMeasures.Add(uom);
                            }

                            userTestResultComponent.tUnitsOfMeasure = uom;
                            userTestResultComponent.UOMID = uom.ID;
                        }
                        userTestResult.tUserTestResultComponents.Add(userTestResultComponent);

                        tXrefUserTestResultComponentsCode xRefComponentCode = new tXrefUserTestResultComponentsCode();
                        xRefComponentCode.UserTestResultComponentID = userTestResultComponent.ID;
                        xRefComponentCode.CodeID = 6720;
                        userTestResultComponent.tXrefUserTestResultComponentsCodes.Add(xRefComponentCode);
                        
                        userTestResult.tUserSourceService = userSourceServiceObj;

                        db.tUserTestResults.Add(userTestResult);
                    }
                    else 
                    {
                        //update
                       
                        //Dates
                        DateTimeOffset dtoStart;
                        if (RESTfulBAL.Models.DynamoDB.Utilities.ConvertToDateTimeOffset(value.timestamp,
                            value.tzOffset,
                            out dtoStart))
                            userTestResult.ResultDateTime = dtoStart;
                        else
                            userTestResult.ResultDateTime = value.timestamp;


                        tUserTestResultComponent userTestResultComponent = db.tUserTestResultComponents
                                                                                .SingleOrDefault(x => x.TestResultID == userTestResult.ID);
                        if (userTestResultComponent != null)
                        {
                            userTestResultComponent.Value = value.value.ToString();

                            //UOM
                            if (value.unit != null)
                            {
                                tUnitsOfMeasure uom = null;
                                uom = db.tUnitsOfMeasures.SingleOrDefault(x => x.UnitOfMeasure == value.unit);
                                if (uom == null)
                                {
                                    uom = new tUnitsOfMeasure();
                                    uom.UnitOfMeasure = value.unit;

                                    db.tUnitsOfMeasures.Add(uom);
                                }

                                if (!uom.UnitOfMeasure.Equals(value.unit))
                                {
                                    userTestResultComponent.tUnitsOfMeasure = uom;
                                    userTestResultComponent.UOMID = uom.ID;
                                }
                            }

                            userTestResult.tUserTestResultComponents.Add(userTestResultComponent);
                        }
                        
                        userTestResult.LastUpdatedDateTime = DateTime.Now;
                        userTestResult.tUserSourceService = userSourceServiceObj;
                    }

                    db.SaveChanges();

                    dbContextTransaction.Commit();

                    return Ok(userTestResult);
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
