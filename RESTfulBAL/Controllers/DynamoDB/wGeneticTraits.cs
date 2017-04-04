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
    public class wGeneticTraitsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UserDataEntities dbErr = new UserDataEntities();

        // POST: api/DynamoDB/wGeneticTraits
        [Route("api/DynamoDB/wGeneticTraits")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(GeneticTrait value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (value.humanId == null)
            {
                return BadRequest();
            }

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
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
                        userSourceServiceObj = db.tUserSourceServices
                            .SingleOrDefault(x => x.SourceServiceID == 1 && x.CredentialID == credentialObj.ID &&
                                                                             x.SystemStatusID == 1);

                        if (userSourceServiceObj == null)
                        {
                            userSourceServiceObj = new tUserSourceService();
                            userSourceServiceObj.SourceServiceID = 1;
                            userSourceServiceObj.UserID = credentialObj.UserID;
                            userSourceServiceObj.CredentialID = credentialObj.ID;
                            userSourceServiceObj.ConnectedOnDateTime = DateTime.Now;
                            userSourceServiceObj.LastSyncDateTime = DateTime.Now;
                            userSourceServiceObj.LatestDateTime = DateTime.Now;
                            userSourceServiceObj.StatusID = 3; //connected
                            userSourceServiceObj.SystemStatusID = 1; //valid
                            userSourceServiceObj.tCredential = credentialObj;

                            db.tUserSourceServices.Add(userSourceServiceObj);
                        }
                        else
                        {
                            //update LatestDateTime to the most recent datetime
                            userSourceServiceObj.LatestDateTime = DateTime.Now;
                        }
                    }

                    tGeneticTrait geneticTrait = db.tGeneticTraits
                                        .SingleOrDefault(x => x.Description == value.description && x.Trait == value.trait);
                    if (geneticTrait == null)
                    {
                        geneticTrait = new tGeneticTrait();
                        geneticTrait.Description = value.description;
                        geneticTrait.Trait = value.trait;

                        db.tGeneticTraits.Add(geneticTrait);
                        db.SaveChanges();
                    }

                    tUserGeneticTrait userGeneticTrait = db.tUserGeneticTraits
                                        .SingleOrDefault(x => x.UserID == credentialObj.UserID && x.GeneticTraitID == geneticTrait.ID);

                    if (userGeneticTrait == null)
                    {
                        //insert
                        userGeneticTrait = new tUserGeneticTrait();
                        userGeneticTrait.UserID = credentialObj.UserID;
                        userGeneticTrait.tUserSourceService = userSourceServiceObj;
                        userGeneticTrait.UserSourceServiceID = userSourceServiceObj.ID;
                        userGeneticTrait.tGeneticTrait = geneticTrait;

                        userGeneticTrait.SystemStatusID = 1;

                        userGeneticTrait.GeneticTraitID = geneticTrait.ID;

                        db.tUserGeneticTraits.Add(userGeneticTrait);
                    }

                    db.SaveChanges();

                    dbContextTransaction.Commit();

                    return Ok(userGeneticTrait);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    //Insert Error Log
                    tUserDataErrLog userErrorLog = new tUserDataErrLog();

                    userErrorLog.ErrTypeID = (int)ErrorLogging.enumErrorType.Application;
                    userErrorLog.ErrSourceID = (int)AuditLogging.enumApplication.SFCBAL;
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
