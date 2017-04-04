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
    public class mNarrativesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UserDataEntities dbErr = new UserDataEntities();


        // POST: api/DynamoDB/mNarratives
        [Route("api/DynamoDB/mNarratives")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(Narratives value)
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
                        userSourceOrganization =
                            db.tSourceOrganizations.SingleOrDefault(x => x.SourceObjectID == value.organization.id);
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

                    tProvider userProvider = new tProvider();
                    if (value.author != null)
                    {
                        userProvider.Name = value.author;
                        if (userSourceOrganization != null)
                        {
                            userProvider.tOrganization = userSourceOrganization.tOrganization;
                            userProvider.OrganizationID = userSourceOrganization.OrganizationID;
                        }

                        db.tProviders.Add(userProvider);
                    }

                    tUserNarrative userNarrative = null;
                    userNarrative = db.tUserNarratives
                                        .SingleOrDefault(x => x.SourceObjectID == value.Id);

                    if (userNarrative == null)
                    {
                        //insert
                        userNarrative = new tUserNarrative();
                        userNarrative.SourceObjectID = value.Id;
                        userNarrative.UserID = credentialObj.UserID;
                        if (userSourceOrganization != null)
                        {
                            userNarrative.tSourceOrganization = userSourceOrganization;
                            userNarrative.SourceOrganizationID = userSourceOrganization.ID;
                        }
                        userNarrative.tUserSourceService = userSourceServiceObj;
                        userNarrative.UserSourceServiceID = userSourceServiceObj.ID;
                        
                        if (value.author != null)
                        {
                            userNarrative.tProvider = userProvider;
                            userNarrative.ProviderID = userProvider.ID;
                        }

                        userNarrative.StartDateTime = value.dateTime;                        
                        userNarrative.SystemStatusID = 1;

                        int seqNum = 0;
                        foreach(entries narrativeEntry in value.entries)
                        {
                            tUserNarrativeEntry userNarrativeEntry = new tUserNarrativeEntry();
                            userNarrativeEntry.SectionSeqNum = seqNum++;
                            userNarrativeEntry.SectionText = narrativeEntry.text;
                            userNarrativeEntry.SectionTitle = narrativeEntry.title;
                            userNarrativeEntry.NarrativeID = userNarrative.ID;
                            userNarrativeEntry.SystemStatusID = 1;
                            userNarrative.tUserNarrativeEntries.Add(userNarrativeEntry);
                        }
                        
                        db.tUserNarratives.Add(userNarrative);

                    }
                    else
                    {
                        //update
                        if (userSourceOrganization != null)
                        {
                            userNarrative.tSourceOrganization = userSourceOrganization;
                            userNarrative.SourceOrganizationID = userSourceOrganization.ID;
                        }
                        userNarrative.tUserSourceService = userSourceServiceObj;
                        userNarrative.UserSourceServiceID = userSourceServiceObj.ID;

                        if (value.author != null)
                        {
                            userNarrative.tProvider = userProvider;
                            userNarrative.ProviderID = userProvider.ID;
                        }

                        userNarrative.StartDateTime = value.dateTime;

                        List<tUserNarrativeEntry> existingEntries = db.tUserNarrativeEntries
                                                                            .Where(x => x.NarrativeID == userNarrative.ID).ToList();
                        existingEntries.ForEach(e => e.SystemStatusID = 4);                        

                        int seqNum = 0;
                        foreach (entries narrativeEntry in value.entries)
                        {
                            tUserNarrativeEntry userNarrativeEntry = new tUserNarrativeEntry();
                            userNarrativeEntry.SectionSeqNum = seqNum++;
                            userNarrativeEntry.SectionText = narrativeEntry.text;
                            userNarrativeEntry.SectionTitle = narrativeEntry.title;
                            userNarrativeEntry.NarrativeID = userNarrative.ID;
                            userNarrativeEntry.SystemStatusID = 1;
                            userNarrative.tUserNarrativeEntries.Add(userNarrativeEntry);
                        }

                        userNarrative.LastUpdatedDateTime = DateTime.Now;
                        
                    }                                       

                    db.SaveChanges();

                    dbContextTransaction.Commit();

                    return Ok(userNarrative);
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
