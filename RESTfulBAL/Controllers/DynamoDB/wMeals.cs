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
    public class wMealsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UserDataEntities dbErr = new UserDataEntities();

        // POST: api/DynamoDB/wMeals
        [Route("api/DynamoDB/wMeals")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(Meals value)
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
                    tDietCategory dietCategoryObj = db.tDietCategories.SingleOrDefault(x => x.Name == value.type);

                    if (dietCategoryObj == null)
                    {
                        dietCategoryObj = new tDietCategory();
                        dietCategoryObj.Name = value.type;

                        db.tDietCategories.Add(dietCategoryObj);
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
                    tCredential credentialObj = db.tCredentials.SingleOrDefault(x => x.SourceID == 5 &&
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

                    tUserDiet userDiet = null;
                    userDiet = db.tUserDiets
                        .SingleOrDefault(x => x.SourceObjectID == value.id);

                    if (userDiet == null)
                    {
                        userDiet = new tUserDiet();
                        userDiet.tUserSourceService = userSourceServiceObj;
                        userDiet.tDietCategory = dietCategoryObj;
                        userDiet.UserID = credentialObj.UserID;
                        userDiet.SourceObjectID = value.id;
                        userDiet.UserSourceServiceID = sourceServiceObj.ID;
                        userDiet.Name = value.name;
                        if (value.amount != null)
                        {
                            userDiet.Servings = value.amount.value;

                            //UOM
                            if (value.amount.unit != null)
                            {
                                tUnitsOfMeasure uom = null;
                                uom = db.tUnitsOfMeasures.SingleOrDefault(x => x.UnitOfMeasure == value.amount.unit);
                                if (uom == null)
                                {
                                    uom = new tUnitsOfMeasure();
                                    uom.UnitOfMeasure = value.amount.unit;

                                    db.tUnitsOfMeasures.Add(uom);
                                    //db.SaveChanges();
                                }

                                userDiet.tUnitsOfMeasure = uom;
                                userDiet.ServingUOMID = uom.ID;
                            }
                        }
                        userDiet.DietCategoryID = dietCategoryObj.ID;

                        //Dates
                        DateTimeOffset dtoStart;
                        if (RESTfulBAL.Models.DynamoDB.Utilities.ConvertToDateTimeOffset(value.timestamp,
                            value.tzOffset,
                            out dtoStart))
                            userDiet.EnteredDateTime = dtoStart;
                        else
                            userDiet.EnteredDateTime = value.timestamp;

                        userDiet.SystemStatusID = 1;

                        db.tUserDiets.Add(userDiet);
                    }
                    else
                    {
                        userDiet.tUserSourceService = userSourceServiceObj;
                        userDiet.Name = value.name;
                        if (value.amount != null)
                        {
                            userDiet.Servings = value.amount.value;

                            //UOM
                            if (value.amount.unit != null)
                            {
                                tUnitsOfMeasure uom = null;
                                uom = db.tUnitsOfMeasures.SingleOrDefault(x => x.UnitOfMeasure == value.amount.unit);
                                if (uom == null)
                                {
                                    uom = new tUnitsOfMeasure();
                                    uom.UnitOfMeasure = value.amount.unit;

                                    db.tUnitsOfMeasures.Add(uom);
                                    //db.SaveChanges();
                                }

                                userDiet.tUnitsOfMeasure = uom;
                                userDiet.ServingUOMID = uom.ID;
                            }
                        }
                        userDiet.DietCategoryID = dietCategoryObj.ID;

                        //Dates
                        DateTimeOffset dtoStart;
                        if (RESTfulBAL.Models.DynamoDB.Utilities.ConvertToDateTimeOffset(value.timestamp,
                            value.tzOffset,
                            out dtoStart))
                            userDiet.EnteredDateTime = dtoStart;
                        else
                            userDiet.EnteredDateTime = value.timestamp;

                        userDiet.LastUpdatedDateTime = DateTime.Now;
                    }
                    
                    AddorUpdateNutrients(userDiet.ID, 170, value.calories, 460);
                    AddorUpdateNutrients(userDiet.ID, 161, value.carbohydrate, 5);
                    AddorUpdateNutrients(userDiet.ID, 248, value.fat, 5);
                    AddorUpdateNutrients(userDiet.ID, 209, value.protein, 5);
                    AddorUpdateNutrients(userDiet.ID, 214, value.sodium, 5);
                    AddorUpdateNutrients(userDiet.ID, 218, value.sugar, 5);
                    AddorUpdateNutrients(userDiet.ID, 177, value.fiber, 5);
                    AddorUpdateNutrients(userDiet.ID, 173, value.saturatedFat, 5);
                    AddorUpdateNutrients(userDiet.ID, 171, value.monounsaturatedFat, 5);
                    AddorUpdateNutrients(userDiet.ID, 172, value.polyunsaturatedFat, 5);
                    AddorUpdateNutrients(userDiet.ID, 164, value.cholesterol, 7);
                    AddorUpdateNutrients(userDiet.ID, 233, value.vitaminA, 7);
                    AddorUpdateNutrients(userDiet.ID, 238, value.vitaminC, 7);
                    AddorUpdateNutrients(userDiet.ID, 159, value.calcium, 7);
                    AddorUpdateNutrients(userDiet.ID, 190, value.iron, 7);
                    AddorUpdateNutrients(userDiet.ID, 207, value.potassium, 7);

                    db.SaveChanges();

                    dbContextTransaction.Commit();

                    return Ok(userDiet);
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

        private void AddorUpdateNutrients(int userDietID, int nutrientID, decimal? value, int uomID)
        {
            if (value != null)
            {
                tXrefUserDietNutrient userDietNutrient = null;
                userDietNutrient = db.tXrefUserDietNutrients.SingleOrDefault(x => x.UserDietID == userDietID && x.NutrientID == nutrientID);

                if (userDietNutrient == null)
                {
                    userDietNutrient = new tXrefUserDietNutrient();
                    userDietNutrient.NutrientID = nutrientID;
                    userDietNutrient.UserDietID = userDietID;
                    userDietNutrient.Value = value;
                    userDietNutrient.UOMID = uomID;
                    userDietNutrient.SystemStatusID = 1;

                    db.tXrefUserDietNutrients.Add(userDietNutrient);
                }
                else
                {
                    userDietNutrient.Value = value;
                    userDietNutrient.UOMID = uomID;
                    userDietNutrient.LastUpdatedDateTime = DateTime.Now;
                }
            }
        }
    }
}
