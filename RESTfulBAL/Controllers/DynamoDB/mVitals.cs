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
    public class mVitalsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UserDataEntities dbErr = new UserDataEntities();


        // POST: api/DynamoDB/mVitals
        [Route("api/DynamoDB/mVitals")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(Vitals value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (value.source == null || value.access_token == null)
            {
                return BadRequest();
            }

            if (value.results == null)
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

                    tProvider userApprovingProvider = null;
                    if (value.author != null)
                    {
                        userApprovingProvider =
                            db.tProviders.SingleOrDefault(x => x.Name == value.author && x.OrganizationID == userSourceOrganization.OrganizationID);

                        if (userApprovingProvider == null)
                        {
                            userApprovingProvider = new tProvider();
                            userApprovingProvider.Name = value.author;

                            if (userSourceOrganization != null)
                            {
                                userApprovingProvider.tOrganization = userSourceOrganization.tOrganization;
                                userApprovingProvider.OrganizationID = userSourceOrganization.tOrganization.ID;
                            }

                            db.tProviders.Add(userApprovingProvider);
                        }
                    }

                    List<tUserVital> userVitalsList = new List<tUserVital>();

                    //loop
                    foreach (var result in value.results)
                    {
                        tUserVital userVitals = null;
                        userVitals = db.tUserVitals
                            .SingleOrDefault(x => x.SourceObjectID == value.Id && x.Name == result.name);

                        //insert
                        if (userVitals == null)
                        {
                            userVitals = new tUserVital();
                            userVitals.SourceObjectID = value.Id;
                            userVitals.UserID = credentialObj.UserID;
                            if (userSourceOrganization != null)
                            {
                                userVitals.tSourceOrganization = userSourceOrganization;
                                userVitals.SourceOrganizationID = userSourceOrganization.ID;
                            }
                            userVitals.tUserSourceService = userSourceServiceObj;
                            userVitals.UserSourceServiceID = userSourceServiceObj.ID;

                            if (value.author != null)
                            {
                                userVitals.tProvider = userApprovingProvider;
                                userVitals.ProviderID = userApprovingProvider.ID;
                            }

                            userVitals.Name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(result.name.ToLower());
                            userVitals.ResultDateTime = value.dateTime;
                            userVitals.SystemStatusID = 1;

                            //parse value
                            if (result.value == null)
                            {
                                userVitals.Value = 0M;
                            }
                            else
                            {
                                if (result.unit == null && result.value.Split(' ').Length == 2)
                                {
                                    StringParser sParse = FindUOM(result.value);
                                    if (sParse.matchCount == 1)
                                    {
                                        userVitals.Value = decimal.Parse(sParse.newValue);
                                        userVitals.UOMID = sParse.uomID;
                                        result.unit = sParse.newUnit;
                                    }
                                    else { userVitals.Value = decimal.Parse(result.value); }
                                }
                                else
                                {
                                    userVitals.Value = decimal.Parse(result.value);
                                }
                            }

                            //UOM
                            if (result.unit != null)
                            {
                                tUnitsOfMeasure uom = null;
                                uom = db.tUnitsOfMeasures.SingleOrDefault(x => x.UnitOfMeasure == result.unit);
                                if (uom == null)
                                {
                                    uom = new tUnitsOfMeasure();
                                    uom.UnitOfMeasure = result.unit;

                                    db.tUnitsOfMeasures.Add(uom);
                                    db.SaveChanges();
                                }

                                userVitals.tUnitsOfMeasure = uom;
                                userVitals.UOMID = uom.ID;
                            }

                            //codes
                            if (result.codes != null)
                            {
                                foreach (Codes code in result.codes)
                                {
                                    if (code.code != null && code.codeSystem != null)
                                    {
                                        tCode vitalCode = null;
                                        vitalCode = db.tCodes
                                            .SingleOrDefault(x => x.Code == code.code && x.CodeSystem == code.codeSystem);
                                        if (vitalCode == null)
                                        {
                                            vitalCode = new tCode();
                                            vitalCode.Code = code.code;
                                            vitalCode.CodeSystem = code.codeSystem;
                                            vitalCode.CodeSystemName = code.codeSystemName;
                                            vitalCode.Name = code.name;

                                            db.tCodes.Add(vitalCode);
                                            db.SaveChanges();
                                        }

                                        tXrefUserVitalsCode userXrefVitalsCodes = null;
                                        userXrefVitalsCodes = db.tXrefUserVitalsCodes
                                            .SingleOrDefault(x => x.CodeID == vitalCode.ID &&
                                                                  x.UserVitalsID == userVitals.ID);

                                        if (userXrefVitalsCodes == null)
                                        {
                                            userXrefVitalsCodes = new tXrefUserVitalsCode();
                                            userXrefVitalsCodes.tUserVital = userVitals;
                                            userXrefVitalsCodes.tCode = vitalCode;
                                            userXrefVitalsCodes.CodeID = vitalCode.ID;
                                            userXrefVitalsCodes.UserVitalsID = userVitals.ID;

                                            db.tXrefUserVitalsCodes.Add(userXrefVitalsCodes);
                                        }
                                    }
                                }
                            }

                            db.tUserVitals.Add(userVitals);
                        }
                        else
                        {
                            //update
                            if (value.author != null)
                            {
                                userVitals.tProvider = userApprovingProvider;
                                userVitals.ProviderID = userApprovingProvider.ID;
                            }

                            userVitals.Name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(result.name.ToLower());
                            userVitals.ResultDateTime = value.dateTime;
                            userVitals.tUserSourceService = userSourceServiceObj;
                            userVitals.LastUpdatedDateTime = DateTime.Now;

                            //parse value
                            if (result.value == null)
                            {
                                userVitals.Value = 0M;
                            }
                            else
                            {
                                if (result.unit == null && result.value.Split(' ').Length == 2)
                                {
                                    StringParser sParse = FindUOM(result.value);
                                    if (sParse.matchCount == 1)
                                    {
                                        userVitals.Value = decimal.Parse(sParse.newValue);
                                        userVitals.UOMID = sParse.uomID;
                                        result.unit = sParse.newUnit;
                                    }
                                    else { userVitals.Value = decimal.Parse(result.value); }
                                }
                                else
                                {
                                    userVitals.Value = decimal.Parse(result.value);
                                }
                            }

                            //UOM
                            if (result.unit != null)
                            {
                                tUnitsOfMeasure uom = null;
                                uom = db.tUnitsOfMeasures.SingleOrDefault(x => x.UnitOfMeasure == result.unit);
                                if (uom == null)
                                {
                                    uom = new tUnitsOfMeasure();
                                    uom.UnitOfMeasure = result.unit;

                                    db.tUnitsOfMeasures.Add(uom);
                                    db.SaveChanges();
                                }

                                userVitals.tUnitsOfMeasure = uom;
                                userVitals.UOMID = uom.ID;
                            }

                            //codes
                            if (result.codes != null)
                            {
                                foreach (Codes code in result.codes)
                                {
                                    if (code.code != null && code.codeSystem != null)
                                    {
                                        tCode vitalCode = null;
                                        vitalCode = db.tCodes
                                            .SingleOrDefault(x => x.Code == code.code && x.CodeSystem == code.codeSystem);
                                        if (vitalCode == null)
                                        {
                                            vitalCode = new tCode();
                                            vitalCode.Code = code.code;
                                            vitalCode.CodeSystem = code.codeSystem;
                                            vitalCode.CodeSystemName = code.codeSystemName;
                                            vitalCode.Name = code.name;

                                            db.tCodes.Add(vitalCode);
                                            db.SaveChanges();
                                        }

                                        tXrefUserVitalsCode userXrefVitalsCodes = null;
                                        userXrefVitalsCodes = db.tXrefUserVitalsCodes
                                            .SingleOrDefault(x => x.CodeID == vitalCode.ID &&
                                                                  x.UserVitalsID == userVitals.ID);

                                        if (userXrefVitalsCodes == null)
                                        {
                                            userXrefVitalsCodes = new tXrefUserVitalsCode();
                                            userXrefVitalsCodes.tUserVital = userVitals;
                                            userXrefVitalsCodes.tCode = vitalCode;
                                            userXrefVitalsCodes.CodeID = vitalCode.ID;
                                            userXrefVitalsCodes.UserVitalsID = userVitals.ID;

                                            db.tXrefUserVitalsCodes.Add(userXrefVitalsCodes);
                                        }
                                    }
                                }
                            }
                        }
                        userVitalsList.Add(userVitals);
                    }

                    db.SaveChanges();

                    dbContextTransaction.Commit();

                    return Ok(userVitalsList);
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

        private StringParser FindUOM(string value)
        {
            StringParser sParse = new StringParser();
            sParse.parsing = value.Split(' ');

            foreach (string str in sParse.parsing)
            {
                tUnitsOfMeasure uom = db.tUnitsOfMeasures.SingleOrDefault(x => x.UnitOfMeasure == str);
                if (uom != null)
                {
                    sParse.newValue = value.Remove(value.IndexOf(str), str.Length).Trim();
                    sParse.uomID = uom.ID;
                    sParse.newUnit = uom.UnitOfMeasure;
                    sParse.matchCount += 1;
                }
            }

            return sParse;
        }

        private class StringParser
        {
            public string[] parsing { get; set; }
            public int matchCount { get; set; }
            public string newValue { get; set; }
            public int uomID { get; set; }
            public string newUnit { get; set; }
        }
    }
}
