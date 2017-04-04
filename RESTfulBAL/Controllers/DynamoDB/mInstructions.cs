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
    public class mInstructionsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UserDataEntities dbErr = new UserDataEntities();


        // POST: api/DynamoDB/mInstructions
        [Route("api/DynamoDB/mInstructions")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(Instructions value)
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
                    //Get source org via sourceserviceid [hack]
                    userSourceOrganization =
                        db.tSourceOrganizations.SingleOrDefault(x => x.SourceServiceID == sourceServiceObj.ID);

                    tUserInstruction userInstruction = null;
                    userInstruction = db.tUserInstructions
                                                .SingleOrDefault(x => x.SourceObjectID == value.Id);

                    if (userInstruction == null)
                    {
                        //insert
                        userInstruction = new tUserInstruction();
                        userInstruction.SourceObjectID = value.Id;
                        userInstruction.UserID = credentialObj.UserID;
                        if (userSourceOrganization != null)
                        {
                            userInstruction.tSourceOrganization = userSourceOrganization;
                            userInstruction.SourceOrganizationID = userSourceOrganization.ID;
                        }
                        userInstruction.tUserSourceService = userSourceServiceObj;
                        userInstruction.UserSourceServiceID = userSourceServiceObj.ID;

                        userInstruction.Name = value.name;
                        userInstruction.Text = value.text;

                        userInstruction.StartDateTime = value.dateTime;
                        userInstruction.SystemStatusID = 1;
                        
                        db.tUserInstructions.Add(userInstruction);
                    }
                    else
                    {
                        //update
                        if (userSourceOrganization != null)
                        {
                            userInstruction.tSourceOrganization = userSourceOrganization;
                            userInstruction.SourceOrganizationID = userSourceOrganization.ID;
                        }
                        userInstruction.tUserSourceService = userSourceServiceObj;
                        userInstruction.UserSourceServiceID = userSourceServiceObj.ID;

                        userInstruction.Name = value.name;
                        userInstruction.Text = value.text;

                        userInstruction.StartDateTime = value.dateTime;
                        userInstruction.LastUpdatedDateTime = DateTime.Now;                        
                    }

                    //codes
                    if (value.codes != null)
                    {
                        foreach (Codes code in value.codes)
                        {
                            if (code.code != null && code.codeSystem != null)
                            {
                                tCode instructionCode = null;
                                instructionCode = db.tCodes
                                                .SingleOrDefault(x => x.Code == code.code && x.CodeSystem == code.codeSystem);
                                if (instructionCode == null)
                                {
                                    instructionCode = new tCode();
                                    instructionCode.Code = code.code;
                                    instructionCode.CodeSystem = code.codeSystem;
                                    instructionCode.CodeSystemName = code.codeSystemName;
                                    instructionCode.Name = code.name;

                                    db.tCodes.Add(instructionCode);
                                    db.SaveChanges();
                                }

                                tXrefUserInstructionsCode userXrefInsCodes = null;
                                userXrefInsCodes = db.tXrefUserInstructionsCodes
                                                                .SingleOrDefault(x => x.CodeID == instructionCode.ID &&
                                                                                        x.UserInstructionID == userInstruction.ID);

                                if (userXrefInsCodes == null)
                                {
                                    userXrefInsCodes = new tXrefUserInstructionsCode();
                                    userXrefInsCodes.tUserInstruction = userInstruction;
                                    userXrefInsCodes.tCode = instructionCode;
                                    userXrefInsCodes.CodeID = instructionCode.ID;
                                    userXrefInsCodes.UserInstructionID = userInstruction.ID;

                                    userInstruction.tXrefUserInstructionsCodes.Add(userXrefInsCodes);
                                }
                            }
                        }
                    }

                    db.SaveChanges();

                    dbContextTransaction.Commit();

                    return Ok(userInstruction);
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
