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
using RESTfulBAL.Models.DynamoDB.Medical;
using RESTfulBAL.Utilities;
using RESTfulBAL.Utilities.ErrorHandling;
using RESTfulBAL.Utilities.AuditHandling;

namespace RESTfulBAL.Controllers.DynamoDB
{
    public class mImmunizationsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UserDataEntities dbErr = new UserDataEntities();


        // POST: api/DynamoDB/mImmunizations
        [Route("api/DynamoDB/mImmunizations")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(Immunizations value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (value.source == null || value.access_token == null)
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
                        sourceServiceObj.TypeID = 1; //Medical
                        sourceServiceObj.SourceID = 5; //HumanAPI

                        db.tSourceServices.Add(sourceServiceObj);
                    }

                    tUserSourceService userSourceServiceObj = null;

                    //Get credentials
                    tCredential credentialObj =
                        db.tCredentials.SingleOrDefault(x => x.SourceID == 5 &&
                                                             x.AccessToken == value.access_token &&
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

                    tSourceOrganization userSourceOrganization = null;
                    if (value.organization != null)
                    {
                        //Get source org
                        userSourceOrganization = db.tSourceOrganizations
                                .Include("tOrganization")
                                .SingleOrDefault(x => x.SourceObjectID == value.organization.id);

                        if (userSourceOrganization == null)
                        {
                            //create org 
                            tOrganization userOrganization = new tOrganization();
                            userOrganization.Name = value.organization.name;

                            //create new source org entry
                            userSourceOrganization = new tSourceOrganization();
                            userSourceOrganization.SourceObjectID = value.organization.id;
                            userSourceOrganization.tOrganization = userOrganization;
                            userSourceOrganization.OrganizationID = userOrganization.ID;
                            userSourceOrganization.SourceServiceID = sourceServiceObj.ID;

                            db.tSourceOrganizations.Add(userSourceOrganization);
                        }
                    }
                    else
                    {
                        //Get source org via sourceserviceid [hack]
                        userSourceOrganization =
                            db.tSourceOrganizations.SingleOrDefault(x => x.SourceServiceID == sourceServiceObj.ID);
                    }

                    tUserImmunization userImmunization = null;
                    userImmunization = db.tUserImmunizations
                                                .SingleOrDefault(x => x.SourceObjectID == value.Id);

                    if (userImmunization == null)
                    {
                        //insert
                        userImmunization = new tUserImmunization();
                        userImmunization.SourceObjectID = value.Id;
                        userImmunization.UserID = credentialObj.UserID;
                        if (userSourceOrganization != null)
                        {
                            userImmunization.tSourceOrganization = userSourceOrganization;
                            userImmunization.SourceOrganizationID = userSourceOrganization.ID;
                        }
                        userImmunization.tUserSourceService = userSourceServiceObj;
                        userImmunization.UserSourceServiceID = userSourceServiceObj.ID;

                        userImmunization.Name = value.name;

                        if(value.dates != null)
                        {
                            foreach(DateTimeOffset immunDate in value.dates)
                            {
                                tUserImmunizationsDate userImmunDate = new tUserImmunizationsDate();
                                userImmunDate.DateTime = immunDate;
                                userImmunDate.UserImmunizationID = userImmunization.ID;
                                userImmunDate.SystemStatusID = 1;

                                userImmunization.tUserImmunizationsDates.Add(userImmunDate);
                            }
                        }

                        userImmunization.SystemStatusID = 1;
                        
                        db.tUserImmunizations.Add(userImmunization);
                    }
                    else
                    {
                        if (userSourceOrganization != null)
                        {
                            userImmunization.tSourceOrganization = userSourceOrganization;
                            userImmunization.SourceOrganizationID = userSourceOrganization.ID;
                        }
                        userImmunization.tUserSourceService = userSourceServiceObj;
                        userImmunization.UserSourceServiceID = userSourceServiceObj.ID;

                        userImmunization.Name = value.name;

                        List<tUserImmunizationsDate> existingEntries = db.tUserImmunizationsDates
                                                                            .Where(x => x.UserImmunizationID == userImmunization.ID).ToList();
                        existingEntries.ForEach(e => e.SystemStatusID = 4);

                        if (value.dates != null)
                        {
                            foreach (DateTimeOffset immunDate in value.dates)
                            {
                                tUserImmunizationsDate userImmunDate = new tUserImmunizationsDate();
                                userImmunDate.DateTime = immunDate;
                                userImmunDate.UserImmunizationID = userImmunization.ID;
                                userImmunDate.SystemStatusID = 1;

                                userImmunization.tUserImmunizationsDates.Add(userImmunDate);
                            }
                        }

                        userImmunization.LastUpdatedDateTime = DateTime.Now;                        
                    }

                    //codes
                    if (value.codes != null)
                    {
                        foreach (Codes code in value.codes)
                        {
                            if (code.code != null && code.codeSystem != null)
                            {
                                tCode immunCode = null;
                                immunCode = db.tCodes
                                                .SingleOrDefault(x => x.Code == code.code && x.CodeSystem == code.codeSystem);
                                if (immunCode == null)
                                {
                                    immunCode = new tCode();
                                    immunCode.Code = code.code;
                                    immunCode.CodeSystem = code.codeSystem;
                                    immunCode.CodeSystemName = code.codeSystemName;
                                    immunCode.Name = code.name;

                                    db.tCodes.Add(immunCode);
                                    db.SaveChanges();
                                }

                                tXrefUserImmunizationsCode userXrefImmunCodes = null;
                                userXrefImmunCodes = db.tXrefUserImmunizationsCodes
                                                                .SingleOrDefault(x => x.CodeID == immunCode.ID &&
                                                                                      x.UserImmunizationID == userImmunization.ID);

                                if (userXrefImmunCodes == null)
                                {
                                    userXrefImmunCodes = new tXrefUserImmunizationsCode();
                                    userXrefImmunCodes.tUserImmunization = userImmunization;
                                    userXrefImmunCodes.tCode = immunCode;
                                    userXrefImmunCodes.CodeID = immunCode.ID;
                                    userXrefImmunCodes.UserImmunizationID = userImmunization.ID;

                                    userImmunization.tXrefUserImmunizationsCodes.Add(userXrefImmunCodes);
                                }
                            }
                        }
                    }

                    db.SaveChanges();

                    dbContextTransaction.Commit();

                    return Ok(userImmunization);
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
