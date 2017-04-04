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
    public class mTestResultsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UserDataEntities dbErr = new UserDataEntities();


        // POST: api/DynamoDB/mTestResults
        [Route("api/DynamoDB/mTestResults")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(TestResults value)
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

                    tProvider userOrderingProvider = new tProvider();
                    if (value.orderedBy != null)
                    {
                        userOrderingProvider.Name = value.orderedBy;
                        if (userSourceOrganization != null)
                        {
                            userOrderingProvider.tOrganization = userSourceOrganization.tOrganization;
                            userOrderingProvider.OrganizationID = userSourceOrganization.OrganizationID;
                        }

                        db.tProviders.Add(userOrderingProvider);
                    }

                    tTestResultStatus testResultStatus = null;
                    if (value.status != null)
                    {
                        testResultStatus =
                            db.tTestResultStatuses.SingleOrDefault(x => x.Status == value.status);
                        if (testResultStatus == null)
                        {
                            testResultStatus = new tTestResultStatus();
                            testResultStatus.Status = value.status;

                            db.tTestResultStatuses.Add(testResultStatus);
                        }
                    }

                    tUserTestResult userTestResult = null;
                    userTestResult = db.tUserTestResults
                        .SingleOrDefault(x => x.SourceObjectID == value.Id);

                    if (userTestResult == null)
                    {
                        //insert
                        userTestResult = new tUserTestResult();
                        userTestResult.SourceObjectID = value.Id;
                        userTestResult.UserID = credentialObj.UserID;
                        if (userSourceOrganization != null)
                        {
                            userTestResult.tSourceOrganization = userSourceOrganization;
                            userTestResult.SourceOrganizationID = userSourceOrganization.ID;
                        }
                        userTestResult.tUserSourceService = userSourceServiceObj;
                        userTestResult.UserSourceServiceID = userSourceServiceObj.ID;
                        userTestResult.Name = value.name;
                        if (value.orderedBy != null)
                        {
                            userTestResult.tProvider = userOrderingProvider;
                            userTestResult.OrderingProviderID = userOrderingProvider.ID;
                        }
                        if (testResultStatus != null)
                        {
                            userTestResult.tTestResultStatus = testResultStatus;
                            userTestResult.StatusID = testResultStatus.ID;
                        }
                        userTestResult.Comments = value.comments;
                        userTestResult.Narrative = value.narrative;
                        userTestResult.Impression = value.impression;
                        userTestResult.Transcriptions = value.transcriptions;
                        userTestResult.ResultDateTime = value.resultDateTime;
                        userTestResult.SystemStatusID = 1;


                        db.tUserTestResults.Add(userTestResult);


                    }
                    else
                    {
                        //update
                        if (userSourceOrganization != null)
                        {
                            userTestResult.tSourceOrganization = userSourceOrganization;
                            userTestResult.SourceOrganizationID = userSourceOrganization.ID;
                        }
                        userTestResult.tUserSourceService = userSourceServiceObj;
                        userTestResult.UserSourceServiceID = userSourceServiceObj.ID;
                        userTestResult.Name = value.name;
                        if (value.orderedBy != null)
                        {
                            userTestResult.tProvider = userOrderingProvider;
                            userTestResult.OrderingProviderID = userOrderingProvider.ID;
                        }
                        if (testResultStatus != null)
                        {
                            userTestResult.tTestResultStatus = testResultStatus;
                            userTestResult.StatusID = testResultStatus.ID;
                        }
                        userTestResult.Comments = value.comments;
                        userTestResult.Narrative = value.narrative;
                        userTestResult.Impression = value.impression;
                        userTestResult.Transcriptions = value.transcriptions;
                        userTestResult.ResultDateTime = value.resultDateTime;
                        userTestResult.LastUpdatedDateTime = DateTime.Now;

                        userTestResult.tTestResultStatus = testResultStatus;
                    }

                    if (value.codes != null)
                    {
                        foreach (Codes code in value.codes)
                        {
                            if (code.code != null && code.codeSystem != null)
                            {
                                tCode testResultCode = null;
                                testResultCode =
                                    db.tCodes.SingleOrDefault(
                                        x => x.Code == code.code && x.CodeSystem == code.codeSystem);
                                if (testResultCode == null)
                                {
                                    testResultCode = new tCode();
                                    testResultCode.Code = code.code;
                                    testResultCode.CodeSystem = code.codeSystem;
                                    testResultCode.CodeSystemName = code.codeSystemName;
                                    testResultCode.Name = code.name;

                                    db.tCodes.Add(testResultCode);
                                    db.SaveChanges();
                                }

                                tXrefUserTestResultsCode userXrefTestCodes = null;
                                userXrefTestCodes =
                                    db.tXrefUserTestResultsCodes.SingleOrDefault(
                                        x => x.CodeID == testResultCode.ID && x.UserTestResultID == userTestResult.ID);
                                if (userXrefTestCodes == null)
                                {
                                    userXrefTestCodes = new tXrefUserTestResultsCode();
                                    userXrefTestCodes.tUserTestResult = userTestResult;
                                    userXrefTestCodes.tCode = testResultCode;
                                    userXrefTestCodes.CodeID = testResultCode.ID;
                                    userXrefTestCodes.UserTestResultID = userTestResult.ID;

                                    db.tXrefUserTestResultsCodes.Add(userXrefTestCodes);
                                }
                            }
                        }
                    }
                    db.SaveChanges();

                    //components & component codes
                    if (value.components != null)
                    {
                        foreach (Components component in value.components)
                        {
                            tUserTestResultComponent userTestResultComponent = null;
                            userTestResultComponent =
                                db.tUserTestResultComponents.SingleOrDefault(x => x.TestResultID == userTestResult.ID &&
                                                                                  x.Name == component.name);
                            if (userTestResultComponent == null)
                            {
                                userTestResultComponent = new tUserTestResultComponent();
                                userTestResultComponent.tUserTestResult = userTestResult;
                                userTestResultComponent.TestResultID = userTestResult.ID;
                                userTestResultComponent.SystemStatusID = 1;
                            }

                            userTestResultComponent.Name = component.name;
                            if (component.value == null)
                            {
                                userTestResultComponent.Value = "No Data";
                            }
                            else
                            {
                                if (component.unit == null && component.value.Split(' ').Length == 2)
                                {
                                    StringParser sParse = FindUOM(component.value);
                                    if (sParse.matchCount == 1)
                                    {
                                        userTestResultComponent.Value = sParse.newValue;
                                        userTestResultComponent.UOMID = sParse.uomID;
                                        component.unit = sParse.newUnit;
                                    }
                                    else { userTestResultComponent.Value = component.value; }
                                }
                                else
                                {
                                    userTestResultComponent.Value = component.value;
                                }
                            }
                            userTestResultComponent.LowValue = component.low;
                            userTestResultComponent.HighValue = component.high;
                            userTestResultComponent.RefRange = component.refRange;
                            userTestResultComponent.Comments = component.componentComments;

                            //UOM
                            if (component.unit != null)
                            {
                                tUnitsOfMeasure uom = null;
                                uom = db.tUnitsOfMeasures.SingleOrDefault(x => x.UnitOfMeasure == component.unit);
                                if (uom == null)
                                {
                                    uom = new tUnitsOfMeasure();
                                    uom.UnitOfMeasure = component.unit;

                                    db.tUnitsOfMeasures.Add(uom);
                                    db.SaveChanges();
                                }

                                userTestResultComponent.tUnitsOfMeasure = uom;
                                userTestResultComponent.UOMID = uom.ID;
                            }
                            if (userTestResultComponent.ObjectID == null)
                            {
                                db.tUserTestResultComponents.Add(userTestResultComponent);
                            }

                            //component codes
                            foreach (Codes code in component.codes)
                            {
                                if (code.code != null && code.codeSystem != null)
                                {
                                    tCode componentCode = null;
                                    componentCode =
                                        db.tCodes.SingleOrDefault(
                                            x => x.Code == code.code && x.CodeSystem == code.codeSystem);
                                    if (componentCode == null)
                                    {
                                        componentCode = new tCode();
                                        componentCode.Code = code.code;
                                        componentCode.CodeSystem = code.codeSystem;
                                        componentCode.CodeSystemName = code.codeSystemName;
                                        componentCode.Name = code.name;

                                        db.tCodes.Add(componentCode);
                                        db.SaveChanges();
                                    }

                                    tXrefUserTestResultComponentsCode userXrefComponentCodes = null;
                                    userXrefComponentCodes =
                                        db.tXrefUserTestResultComponentsCodes.SingleOrDefault(
                                            x =>
                                                x.CodeID == componentCode.ID &&
                                                x.UserTestResultComponentID == userTestResultComponent.ID);
                                    if (userXrefComponentCodes == null)
                                    {
                                        userXrefComponentCodes = new tXrefUserTestResultComponentsCode();
                                        userXrefComponentCodes.tUserTestResultComponent = userTestResultComponent;
                                        userXrefComponentCodes.tCode = componentCode;
                                        userXrefComponentCodes.CodeID = componentCode.ID;
                                        userXrefComponentCodes.UserTestResultComponentID = userTestResultComponent.ID;

                                        db.tXrefUserTestResultComponentsCodes.Add(userXrefComponentCodes);
                                    }
                                }
                            }
                        }
                    }

                    //Recipients
                    if (value.recipients != null)
                    {
                        foreach (Recipients recip in value.recipients)
                        {
                            tProvider userProviderRecipients = null;
                            userProviderRecipients =
                                db.tProviders.SingleOrDefault(x => x.SourceProviderID == recip.objectID);
                            if (userProviderRecipients == null)
                            {
                                userProviderRecipients = new tProvider();
                                userProviderRecipients.SourceProviderID = recip.objectID;
                                userProviderRecipients.Name = recip.name;
                                if (userSourceOrganization != null)
                                {
                                    userProviderRecipients.OrganizationID = userSourceOrganization.OrganizationID;
                                }

                                db.tProviders.Add(userProviderRecipients);
                            }

                            tXrefUserTestResultRecipientsProvider userXrefProviderRecip =
                                new tXrefUserTestResultRecipientsProvider();
                            userXrefProviderRecip.tProvider = userProviderRecipients;
                            userXrefProviderRecip.tUserTestResult = userTestResult;
                            userXrefProviderRecip.IsPCP = recip.isPCP;
                            userXrefProviderRecip.ProviderID = userProviderRecipients.ID;
                            userXrefProviderRecip.UserTestResultID = userTestResult.ID;

                            db.tXrefUserTestResultRecipientsProviders.Add(userXrefProviderRecip);
                        }
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
