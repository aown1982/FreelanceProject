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
    public class mAllergiesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UserDataEntities dbErr = new UserDataEntities();


        // POST: api/DynamoDB/mAllergies
        [Route("api/DynamoDB/mAllergies")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(Allergies value)
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

                    List<tUserAllergy> userAllergies = db.tUserAllergies
                                            .Include("tReaction")
                                            .Where(x => x.SourceObjectID == value.Id).ToList() ?? new List<tUserAllergy>();

                     tUserAllergy newUserAllergy = new tUserAllergy();

                    //matching records for the SourceObjectID provided - do update & possibly insert
                    if (userAllergies.Count > 0)
                    {
                        //if reactions
                        if (value.reactionsFull != null)
                        {
                            foreach (Reaction allergyReaction in value.reactionsFull)
                            {
                                bool foundMatch = false;
                                //loop to find existing allergy with current reaction
                                foreach (tUserAllergy userAllergy in userAllergies)
                                {
                                    if (userAllergy.tReaction != null &&
                                        userAllergy.tReaction.Name == allergyReaction.name)
                                    {
                                        //update allergy/reaction with new data
                                        UpdateAllergy(newUserAllergy, value, allergyReaction, credentialObj, userSourceOrganization);

                                        foundMatch = true;
                                    }
                                }

                                //insert new allergy/reaction
                                if (foundMatch == false)
                                {
                                    newUserAllergy = CreateAllergy(newUserAllergy, value, null, credentialObj,
                                                                   userSourceServiceObj, userSourceOrganization);

                                    userAllergies.Add(newUserAllergy);
                                }
                            }
                        }
                        else if(userAllergies.Count == 1 && value.reactionsFull == null) //one allergen and no reactions
                        {
                            //update allergy with new data
                            UpdateAllergy(userAllergies[0], value, null, credentialObj, userSourceOrganization);
                        }
                        else 
                        {
                            //this means multiple entries were made for same allergy with each reaction previously, 
                            //but now no reactions are listed in new values. We cant do anything because we dont know 
                            //which to invalidate or update.
                        }
                                
                    }
                    //no existing entries - do insert
                    else
                    {
                        //if reactions
                        if (value.reactionsFull != null)
                        {
                            foreach (Reaction allergyReaction in value.reactionsFull)
                            {
                                newUserAllergy = CreateAllergy(newUserAllergy, value, allergyReaction, credentialObj, userSourceServiceObj, userSourceOrganization);

                            }

                            userAllergies.Add(newUserAllergy);
                        }
                        else //no reactions
                        {
                            newUserAllergy = CreateAllergy(newUserAllergy, value, null, credentialObj, userSourceServiceObj, userSourceOrganization);

                            userAllergies.Add(newUserAllergy);
                        }
                    }

                    db.SaveChanges();

                    dbContextTransaction.Commit();

                    return Ok(userAllergies);
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


        private tUserAllergy CreateAllergy(tUserAllergy newUserAllergy,
                                            Allergies newValues,
                                            Reaction userReaction,
                                            tCredential credentialObj,
                                            tUserSourceService userSourceServiceObj,
                                            tSourceOrganization userSourceOrganization)
        {
            newUserAllergy.SystemStatusID = 1;
            newUserAllergy.SourceObjectID = newValues.Id;
            newUserAllergy.UserID = credentialObj.UserID;

            if (userSourceOrganization != null)
            {
                newUserAllergy.tSourceOrganization = userSourceOrganization;
                newUserAllergy.SourceOrganizationID = userSourceOrganization.ID;
            }

            newUserAllergy.tUserSourceService = userSourceServiceObj;
            newUserAllergy.UserSourceServiceID = userSourceServiceObj.ID;

            //allergen name
            tAllergen allergenObj = db.tAllergens
                                        .Include("tAllergen1")
                                        .SingleOrDefault(x => x.Name == newValues.name);
            if (allergenObj == null)
            {
                allergenObj = new tAllergen();
                allergenObj.Name = newValues.name;
                db.tAllergens.Add(allergenObj);

                newUserAllergy.tAllergen = allergenObj;
                newUserAllergy.AllergenID = allergenObj.ID;
            }
            else //check to see if allergen has a mapped parent
            {
                if (allergenObj.ParentID != null)
                {
                    newUserAllergy.tAllergen = allergenObj.tAllergen1;
                    newUserAllergy.AllergenID = allergenObj.tAllergen1.ID;
                }
                else
                {
                    newUserAllergy.tAllergen = allergenObj;
                    newUserAllergy.AllergenID = allergenObj.ID;
                }
            }

            //status
            if (newValues.status != null)
            {
                tAllergyStatus allergyStatusObj = db.tAllergyStatuses
                                        .Include("tAllergyStatus1")
                                        .SingleOrDefault(x => x.Status == newValues.status);
                if (allergyStatusObj == null)
                {
                    allergyStatusObj = new tAllergyStatus();
                    allergyStatusObj.Status = newValues.status;
                    db.tAllergyStatuses.Add(allergyStatusObj);

                    newUserAllergy.tAllergyStatus = allergyStatusObj;
                    newUserAllergy.StatusID = allergyStatusObj.ID;
                }
                else //check to see if reaction has a mapped parent
                {
                    if (allergyStatusObj.ParentID != null)
                    {
                        newUserAllergy.tAllergyStatus = allergyStatusObj.tAllergyStatus1;
                        newUserAllergy.StatusID = allergyStatusObj.tAllergyStatus1.ID;
                    }
                    else
                    {
                        newUserAllergy.tAllergyStatus = allergyStatusObj;
                        newUserAllergy.StatusID = allergyStatusObj.ID;
                    }
                }
            }

            //severity
            if (newValues.severity != null)
            {
                tAllergySeverity allergySeverity = db.tAllergySeverities
                                        .Include("tAllergySeverity1")
                                        .SingleOrDefault(x => x.Severity == newValues.severity);
                if (allergySeverity == null)
                {
                    allergySeverity = new tAllergySeverity();
                    allergySeverity.Severity = newValues.severity;
                    db.tAllergySeverities.Add(allergySeverity);

                    newUserAllergy.tAllergySeverity = allergySeverity;
                    newUserAllergy.SeverityID = allergySeverity.ID;
                }
                else //check to see if reaction has a mapped parent
                {
                    if (allergySeverity.ParentID != null)
                    {
                        newUserAllergy.tAllergySeverity = allergySeverity.tAllergySeverity1;
                        newUserAllergy.SeverityID = allergySeverity.tAllergySeverity1.ID;
                    }
                    else
                    {
                        newUserAllergy.tAllergySeverity = allergySeverity;
                        newUserAllergy.SeverityID = allergySeverity.ID;
                    }
                }
            }

            //Dates
            if (newValues.dateRange.start != null && newValues.dateRange.start != DateTime.MinValue) { newUserAllergy.StartDateTime = newValues.dateRange.start; }
            if (newValues.dateRange.end != null && newValues.dateRange.end != DateTime.MinValue) { newUserAllergy.EndDateTime = newValues.dateRange.end; }

            //Allergy codes
            if (newValues.codes != null)
            {
                foreach (Codes code in newValues.codes)
                {
                    if (code.code != null && code.codeSystem != null)
                    {
                        tCode AllergyCode = null;
                        AllergyCode = db.tCodes
                            .SingleOrDefault(x => x.Code == code.code && x.CodeSystem == code.codeSystem);

                        if (AllergyCode == null)
                        {
                            AllergyCode = new tCode();
                            AllergyCode.Code = code.code;
                            AllergyCode.CodeSystem = code.codeSystem;
                            AllergyCode.CodeSystemName = code.codeSystemName;
                            AllergyCode.Name = code.name;

                            db.tCodes.Add(AllergyCode);
                            db.SaveChanges();
                        }

                        tXrefUserAllergiesCode userXrefAllergensCodes = null;
                        userXrefAllergensCodes = db.tXrefUserAllergiesCodes
                                                        .SingleOrDefault(x => x.CodeID == AllergyCode.ID &&
                                                                         x.UserAllergiesID == newUserAllergy.ID);

                        if (userXrefAllergensCodes == null)
                        {
                            userXrefAllergensCodes = new tXrefUserAllergiesCode();
                            userXrefAllergensCodes.tUserAllergy = newUserAllergy;
                            userXrefAllergensCodes.tCode = AllergyCode;
                            userXrefAllergensCodes.CodeID = AllergyCode.ID;
                            userXrefAllergensCodes.UserAllergiesID = newUserAllergy.ID;

                            db.tXrefUserAllergiesCodes.Add(userXrefAllergensCodes);
                        }
                    }
                }
            }

            //reactions
            if (userReaction != null)
            {
                tReaction reactionObj = null;

                if (userReaction.reactionType.name != null)
                {
                    reactionObj = db.tReactions
                                            .Include("tReaction1")
                                            .SingleOrDefault(x => x.Name == userReaction.reactionType.name);
                    if (reactionObj == null)
                    {
                        reactionObj = new tReaction();
                        reactionObj.Name = userReaction.reactionType.name;
                        reactionObj.ReactionTypeID = 1; //unspecified
                        db.tReactions.Add(reactionObj);

                        newUserAllergy.tReaction = reactionObj;
                        newUserAllergy.ReactionID = reactionObj.ID;
                    }
                    else //check to see if reaction has a mapped parent
                    {
                        if (reactionObj.ParentID != null)
                        {
                            newUserAllergy.tReaction = reactionObj.tReaction1;
                            newUserAllergy.ReactionID = reactionObj.tReaction1.ID;
                        }
                        else
                        {
                            newUserAllergy.tReaction = reactionObj;
                            newUserAllergy.ReactionID = reactionObj.ID;
                        }
                    }
                }

                //Reaction codes
                if (userReaction.reactionType.codes != null)
                {
                    foreach (Codes code in userReaction.reactionType.codes)
                    {
                        if (code.code != null && code.codeSystem != null)
                        {
                            tCode ReactionCode = null;
                            ReactionCode = db.tCodes
                                .SingleOrDefault(x => x.Code == code.code && x.CodeSystem == code.codeSystem);

                            if (ReactionCode == null)
                            {
                                ReactionCode = new tCode();
                                ReactionCode.Code = code.code;
                                ReactionCode.CodeSystem = code.codeSystem;
                                ReactionCode.CodeSystemName = code.codeSystemName;
                                ReactionCode.Name = code.name;

                                db.tCodes.Add(ReactionCode);
                                db.SaveChanges();
                            }

                            tXrefReactionsCode userXrefReactionsCode = null;
                            userXrefReactionsCode = db.tXrefReactionsCodes
                                                            .SingleOrDefault(x => x.CodeID == ReactionCode.ID &&
                                                                             x.ReactionID == reactionObj.ID);

                            if (userXrefReactionsCode == null)
                            {
                                userXrefReactionsCode = new tXrefReactionsCode();
                                userXrefReactionsCode.tReaction = reactionObj;
                                userXrefReactionsCode.tCode = ReactionCode;
                                userXrefReactionsCode.CodeID = ReactionCode.ID;
                                userXrefReactionsCode.ReactionID = reactionObj.ID;

                                db.tXrefReactionsCodes.Add(userXrefReactionsCode);
                            }
                        }
                    }
                }
            }

            db.tUserAllergies.Add(newUserAllergy);

            return newUserAllergy;
        }

        private tUserAllergy UpdateAllergy(tUserAllergy userAllergy,
                                            Allergies newValues,
                                            Reaction userReaction,
                                            tCredential credentialObj,
                                            tSourceOrganization userSourceOrganization)
        {

            userAllergy.LastUpdateDateTime = DateTime.Now;

            //allergen name
            tAllergen allergenObj = db.tAllergens
                                        .Include("tAllergen1")
                                        .SingleOrDefault(x => x.Name == newValues.name);

            if (allergenObj == null)
            {
                allergenObj = new tAllergen();
                allergenObj.Name = newValues.name;
                db.tAllergens.Add(allergenObj);
                db.SaveChanges();           
            }

            int tmpAllergyID = 0;
            if (allergenObj.ParentID != null)
            {
                tmpAllergyID = allergenObj.tAllergen1.ID;
            }
            else
            {
                tmpAllergyID = allergenObj.ID;
            }

            //allergen changed?
            if (userAllergy.AllergenID != tmpAllergyID)
            {
                //invalidate current and add new
                userAllergy.SystemStatusID = 4;
                return CreateAllergy(new tUserAllergy(), newValues, userReaction, credentialObj, userAllergy.tUserSourceService, userAllergy.tSourceOrganization);

            }
            else//same allergen
            {
                //status
                if (newValues.status != null)
                {
                    tAllergyStatus allergyStatusObj = db.tAllergyStatuses
                                            .Include("tAllergyStatus1")
                                            .SingleOrDefault(x => x.Status == newValues.status);
                    if (allergyStatusObj == null)
                    {
                        allergyStatusObj = new tAllergyStatus();
                        allergyStatusObj.Status = newValues.status;
                        db.tAllergyStatuses.Add(allergyStatusObj);

                        userAllergy.tAllergyStatus = allergyStatusObj;
                        userAllergy.StatusID = allergyStatusObj.ID;
                    }
                    else //check to see if reaction has a mapped parent
                    {
                        if (allergyStatusObj.ParentID != null)
                        {
                            userAllergy.tAllergyStatus = allergyStatusObj.tAllergyStatus1;
                            userAllergy.StatusID = allergyStatusObj.tAllergyStatus1.ID;
                        }
                        else
                        {
                            userAllergy.tAllergyStatus = allergyStatusObj;
                            userAllergy.StatusID = allergyStatusObj.ID;
                        }
                    }
                }

                //severity
                if (newValues.severity != null)
                {
                    tAllergySeverity allergySeverity = db.tAllergySeverities
                                            .Include("tAllergySeverity1")
                                            .SingleOrDefault(x => x.Severity == newValues.severity);
                    if (allergySeverity == null)
                    {
                        allergySeverity = new tAllergySeverity();
                        allergySeverity.Severity = newValues.severity;
                        db.tAllergySeverities.Add(allergySeverity);

                        userAllergy.tAllergySeverity = allergySeverity;
                        userAllergy.SeverityID = allergySeverity.ID;
                    }
                    else //check to see if reaction has a mapped parent
                    {
                        if (allergySeverity.ParentID != null)
                        {
                            userAllergy.tAllergySeverity = allergySeverity.tAllergySeverity1;
                            userAllergy.SeverityID = allergySeverity.tAllergySeverity1.ID;
                        }
                        else
                        {
                            userAllergy.tAllergySeverity = allergySeverity;
                            userAllergy.SeverityID = allergySeverity.ID;
                        }
                    }
                }

                //Dates
                if (newValues.dateRange.start != null && newValues.dateRange.start != DateTime.MinValue) { userAllergy.StartDateTime = newValues.dateRange.start; }
                if (newValues.dateRange.end != null && newValues.dateRange.end != DateTime.MinValue) { userAllergy.EndDateTime = newValues.dateRange.end; }

                //Allergy codes
                if (newValues.codes != null)
                {
                    foreach (Codes code in newValues.codes)
                    {
                        if (code.code != null && code.codeSystem != null)
                        {
                            tCode AllergyCode = null;
                            AllergyCode = db.tCodes
                                .SingleOrDefault(x => x.Code == code.code && x.CodeSystem == code.codeSystem);

                            if (AllergyCode == null)
                            {
                                AllergyCode = new tCode();
                                AllergyCode.Code = code.code;
                                AllergyCode.CodeSystem = code.codeSystem;
                                AllergyCode.CodeSystemName = code.codeSystemName;
                                AllergyCode.Name = code.name;

                                db.tCodes.Add(AllergyCode);
                                db.SaveChanges();
                            }

                            tXrefUserAllergiesCode userXrefAllergensCodes = null;
                            userXrefAllergensCodes = db.tXrefUserAllergiesCodes
                                                            .SingleOrDefault(x => x.CodeID == AllergyCode.ID &&
                                                                             x.UserAllergiesID == userAllergy.ID);

                            if (userXrefAllergensCodes == null)
                            {
                                userXrefAllergensCodes = new tXrefUserAllergiesCode();
                                userXrefAllergensCodes.tUserAllergy = userAllergy;
                                userXrefAllergensCodes.tCode = AllergyCode;
                                userXrefAllergensCodes.CodeID = AllergyCode.ID;
                                userXrefAllergensCodes.UserAllergiesID = userAllergy.ID;

                                db.tXrefUserAllergiesCodes.Add(userXrefAllergensCodes);
                            }
                        }
                    }
                }
            }

            //reactions
            if (userReaction != null)
            {
                tReaction reactionObj = null;

                if (userReaction.reactionType.name != null)
                {
                    reactionObj = db.tReactions
                                            .Include("tReaction1")
                                            .SingleOrDefault(x => x.Name == userReaction.reactionType.name);
                    if (reactionObj == null)
                    {
                        reactionObj = new tReaction();
                        reactionObj.Name = userReaction.reactionType.name;
                        reactionObj.ReactionTypeID = 1; //unspecified
                        db.tReactions.Add(reactionObj);

                        userAllergy.tReaction = reactionObj;
                        userAllergy.ReactionID = reactionObj.ID;
                    }
                    else //check to see if reaction has a mapped parent
                    {
                        if (reactionObj.ParentID != null)
                        {
                            userAllergy.tReaction = reactionObj.tReaction1;
                            userAllergy.ReactionID = reactionObj.tReaction1.ID;
                        }
                        else
                        {
                            userAllergy.tReaction = reactionObj;
                            userAllergy.ReactionID = reactionObj.ID;
                        }
                    }
                }

                //Reaction codes
                if (userReaction.reactionType.codes != null)
                {
                    foreach (Codes code in userReaction.reactionType.codes)
                    {
                        if (code.code != null && code.codeSystem != null)
                        {
                            tCode ReactionCode = null;
                            ReactionCode = db.tCodes
                                .SingleOrDefault(x => x.Code == code.code && x.CodeSystem == code.codeSystem);

                            if (ReactionCode == null)
                            {
                                ReactionCode = new tCode();
                                ReactionCode.Code = code.code;
                                ReactionCode.CodeSystem = code.codeSystem;
                                ReactionCode.CodeSystemName = code.codeSystemName;
                                ReactionCode.Name = code.name;

                                db.tCodes.Add(ReactionCode);
                                db.SaveChanges();
                            }

                            tXrefReactionsCode userXrefReactionsCode = null;
                            userXrefReactionsCode = db.tXrefReactionsCodes
                                                            .SingleOrDefault(x => x.CodeID == ReactionCode.ID &&
                                                                             x.ReactionID == reactionObj.ID);

                            if (userXrefReactionsCode == null)
                            {
                                userXrefReactionsCode = new tXrefReactionsCode();
                                userXrefReactionsCode.tReaction = reactionObj;
                                userXrefReactionsCode.tCode = ReactionCode;
                                userXrefReactionsCode.CodeID = ReactionCode.ID;
                                userXrefReactionsCode.ReactionID = reactionObj.ID;

                                db.tXrefReactionsCodes.Add(userXrefReactionsCode);
                            }
                        }
                    }
                }
            }
            return userAllergy;
        }
    }
}