﻿using System;
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
    public class wActivitiesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UserDataEntities dbErr = new UserDataEntities();


        // POST: api/DynamoDB/wActivities
        [Route("api/DynamoDB/wActivities")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(Activities value)
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
                    tActivity activityObj = db.tActivities.SingleOrDefault(x => x.Name == value.type);

                    if (activityObj == null)
                    {
                        activityObj = new tActivity();
                        activityObj.Name = value.type;

                        db.tActivities.Add(activityObj);
                    }

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
                        db.tCredentials.SingleOrDefault(x => x.SourceID == 5 &&
                                                             x.SourceUserID == value.humanId &&
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

                    tUserActivity userActivity = null;
                    userActivity = db.tUserActivities
                        .SingleOrDefault(x => x.SourceObjectID == value.id);

                    if (userActivity == null)
                    {
                        userActivity = new tUserActivity();
                        userActivity.UserID = credentialObj.UserID;
                        userActivity.SourceObjectID = value.id;
                        userActivity.UserSourceServiceID = sourceServiceObj.ID;
                        userActivity.ActivityID = activityObj.ID;

                        //Dates
                        DateTimeOffset dtoStart, dtoEnd;
                        if (RESTfulBAL.Models.DynamoDB.Utilities.ConvertToDateTimeOffset(value.startTime,
                                                                                        value.tzOffset,
                                                                                        out dtoStart))
                            userActivity.StartDateTime = dtoStart;
                        else
                            userActivity.StartDateTime = value.startTime;

                        if (RESTfulBAL.Models.DynamoDB.Utilities.ConvertToDateTimeOffset(value.endTime, value.tzOffset,
                            out dtoEnd))
                            userActivity.EndDateTime = dtoEnd;
                        else
                            userActivity.EndDateTime = value.endTime;

                        userActivity.Duration = value.duration;
                        userActivity.DurationUOMID = 8;
                        userActivity.Distance = value.distance;
                        userActivity.DistanceUOMID = 9;
                        userActivity.Steps = value.steps;
                        userActivity.Calories = value.calories;
                        userActivity.SystemStatusID = 1;
                        userActivity.tUserSourceService = userSourceServiceObj;

                        db.tUserActivities.Add(userActivity);
                    }
                    else
                    {
                        userActivity.ActivityID = activityObj.ID;

                        //Dates
                        DateTimeOffset dtoStart, dtoEnd;
                        if (RESTfulBAL.Models.DynamoDB.Utilities.ConvertToDateTimeOffset(value.startTime,
                                                                                        value.tzOffset,
                                                                                        out dtoStart))
                            userActivity.StartDateTime = dtoStart;
                        else
                            userActivity.StartDateTime = value.startTime;

                        if (RESTfulBAL.Models.DynamoDB.Utilities.ConvertToDateTimeOffset(value.endTime, value.tzOffset,
                            out dtoEnd))
                            userActivity.EndDateTime = dtoEnd;
                        else
                            userActivity.EndDateTime = value.endTime;

                        userActivity.Duration = value.duration;
                        userActivity.DurationUOMID = 8;
                        userActivity.Distance = value.distance;
                        userActivity.DistanceUOMID = 9;
                        userActivity.Steps = value.steps;
                        userActivity.Calories = value.calories;
                        userActivity.SystemStatusID = 1;
                        userActivity.tUserSourceService = userSourceServiceObj;
                        userActivity.LastUpdatedDateTime = DateTime.Now;
                    }

                    db.SaveChanges();

                    dbContextTransaction.Commit();

                    return Ok(userActivity);
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
