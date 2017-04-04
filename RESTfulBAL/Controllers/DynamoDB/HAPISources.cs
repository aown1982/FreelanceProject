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
using RESTfulBAL.Models.DynamoDB.Utility;
using RESTfulBAL.Utilities;
using RESTfulBAL.Utilities.ErrorHandling;
using RESTfulBAL.Utilities.AuditHandling;

namespace RESTfulBAL.Controllers.DynamoDB
{
    public class HAPISourcesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UserDataEntities dbErr = new UserDataEntities();

        // POST: api/DynamoDB/HAPISources
        [Route("api/DynamoDB/HAPISources")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(Sources value)
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
                        if(value.organization == null)
                        { 
                            sourceServiceObj.TypeID = 2; //Wellness
                        }
                        else
                        {
                            sourceServiceObj.TypeID = 1; //Medical
                        }

                        sourceServiceObj.SourceID = 5; //HumanAPI

                        db.tSourceServices.Add(sourceServiceObj);
                    }

                    //get user source service status
                    tUserSourceServiceStatus statusObj = null;
                    string strStatus = "";
                    if (value.syncStatus != null) //for wellness
                    {
                        strStatus = value.syncStatus.status;
                    }
                    else if(value.historySync != null) //for medical
                    {
                        strStatus = value.historySync.status;
                    }

                    statusObj = db.tUserSourceServiceStatuses.SingleOrDefault(x => x.Status == strStatus);

                    if (statusObj == null)
                    {
                        statusObj = new tUserSourceServiceStatus();
                        statusObj.Status = strStatus;

                        db.tUserSourceServiceStatuses.Add(statusObj);
                    }

                    //Get credentials
                    tUserSourceService userSourceServiceObj = null;

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
                            userSourceServiceObj.ConnectedOnDateTime = value.connectedSince;
                            userSourceServiceObj.LastSyncDateTime = DateTime.Now.AddDays(-30d);
                            userSourceServiceObj.LatestDateTime = DateTime.Now;
                            userSourceServiceObj.StatusID = statusObj.ID;
                            userSourceServiceObj.SystemStatusID = 1; //valid
                            userSourceServiceObj.tCredential = credentialObj;

                            db.tUserSourceServices.Add(userSourceServiceObj);
                        }                       
                    }

                    foreach (string device in value.devices)
                    {
                        tSourceServiceDevice sourceServiceDevices = db.tSourceServiceDevices.SingleOrDefault(
                                                                            x => x.Name == device);
                        if (sourceServiceDevices == null)
                        {
                            sourceServiceDevices = new tSourceServiceDevice();
                            sourceServiceDevices.Name = device;

                            db.tSourceServiceDevices.Add(sourceServiceDevices);
                        }

                        tXrefUserSourceServiceDevice userdevices = db.tXrefUserSourceServiceDevices.SingleOrDefault(
                                                                            x => x.UserSourceServiceID == userSourceServiceObj.ID &&
                                                                            x.SourceServiceDeviceID == sourceServiceDevices.ID);
                        if (userdevices == null)
                        {
                            userdevices = new tXrefUserSourceServiceDevice();
                            userdevices.UserSourceServiceID = userSourceServiceObj.ID;
                            userdevices.SourceServiceDeviceID = sourceServiceDevices.ID;

                            userdevices.tSourceServiceDevice = sourceServiceDevices;
                            userdevices.tUserSourceService = userSourceServiceObj;

                            db.tXrefUserSourceServiceDevices.Add(userdevices);
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

                    db.SaveChanges();

                    dbContextTransaction.Commit();

                    return Ok(userSourceServiceObj);
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
