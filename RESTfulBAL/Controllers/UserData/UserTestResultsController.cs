using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;
using DAL.UserData;
using RESTfulBAL.Models;
using RESTfulBAL.Models.UserData;

namespace RESTfulBAL.Controllers.UserData
{
    public class UserTestResultsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserTestResults
        [Route("api/UserData/GetUserTestResults")]
        public IQueryable<tUserTestResult> GettUserTestResults()
        {
            return db.tUserTestResults;
        }

        // GET: api/UserTestResults/5
        [Route("api/UserData/GetUserTestResults/{id}")]
        [ResponseType(typeof(tUserTestResult))]
        public async Task<IHttpActionResult> GettUserTestResult(int id)
        {
            tUserTestResult tUserTestResult = await db.tUserTestResults.FindAsync(id);
            if (tUserTestResult == null)
            {
                return NotFound();
            }

            return Ok(tUserTestResult);
        }

        // PUT: api/UserTestResults/5
        [Route("api/UserData/UpdateUserTestResults/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserTestResult(int id, tUserTestResult tUserTestResult)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserTestResult.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserTestResult).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserTestResultExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UserTestResults
        [Route("api/UserData/PostUserTestResults")]
        [ResponseType(typeof(tUserTestResult))]
        public async Task<IHttpActionResult> PosttUserTestResult(tUserTestResult tUserTestResult)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.tUserTestResults.Add(tUserTestResult);
                await db.SaveChangesAsync();

                // return CreatedAtRoute("DefaultApi", new { id = tUserTestResult.ID }, tUserTestResult);
                return Ok(tUserTestResult);
            }
            catch (Exception ex)
            {

                throw;
            }

          
        }

        // DELETE: api/UserTestResults/5
        [Route("api/UserData/DeleteUserTestResults/{id}")]
        [ResponseType(typeof(tUserTestResult))]
        public async Task<IHttpActionResult> DeletetUserTestResult(int id)
        {
            tUserTestResult tUserTestResult = await db.tUserTestResults.FindAsync(id);
            if (tUserTestResult == null)
            {
                return NotFound();
            }

            db.tUserTestResults.Remove(tUserTestResult);
            await db.SaveChangesAsync();

            return Ok(tUserTestResult);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserTestResultExists(int id)
        {
            return db.tUserTestResults.Count(e => e.ID == id) > 0;
        }

        // GET: api/UserVitals
        [Route("api/UserData/GetUserBloodGlucose")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserBloodGlucose(HealthGoalModel healthGoalModel)
        {
            if (healthGoalModel.Last5 != null && healthGoalModel.Last5 == true)
            {
                var userModel = await db.tUserTestResultComponents.Include(u => u.tUserTestResult)
                                                                    .Include(u => u.tXrefUserTestResultComponentsCodes)
                                                                    .Where(u => u.tXrefUserTestResultComponentsCodes.Any(x => x.CodeID == 604) && 
                                                                                                u.tUserTestResult.SystemStatusID == 1 &&
                                                                                                u.SystemStatusID == 1 && 
                                                                                                u.tUserTestResult.UserID == healthGoalModel.UserId)
                                                                     .Select(t => new UserTestModel
                                                                     {
                                                                         ResultDateTime = t.tUserTestResult.ResultDateTime,
                                                                         Value = t.Value
                                                                     })
                                                                     .OrderByDescending(u => u.ResultDateTime)
                                                                     .Take(5)
                                                                     .ToListAsync();

                return Ok(userModel);
            }
            else
            {
                var userModel = await db.tUserTestResultComponents.Include(u => u.tUserTestResult)
                                     .Include(u => u.tXrefUserTestResultComponentsCodes)
                                     .Where(u => u.tXrefUserTestResultComponentsCodes.Any(x => x.CodeID == 604) && 
                                                                                         u.tUserTestResult.SystemStatusID == 1 &&
                                                                                         u.SystemStatusID == 1 && 
                                                                                         u.tUserTestResult.UserID == healthGoalModel.UserId && 
                                                                                         u.tUserTestResult.ResultDateTime >= healthGoalModel.StartDate && 
                                                                                         u.tUserTestResult.ResultDateTime <= healthGoalModel.EndDate)
                                     .Select(t => new UserTestModel
                                     {
                                         ResultDateTime = t.tUserTestResult.ResultDateTime,
                                         Value = t.Value
                                     }).OrderBy(o => o.ResultDateTime)
                                     .ToListAsync();

                return Ok(userModel);
            }

        }

        // GET: api/UserVitals
        [Route("api/UserData/GetUserCholesterol")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserCholesterol(HealthGoalModel healthGoalModel)
        {
            var idList = new int[] { 568, 570, 572 };

            if (healthGoalModel.Last5 != null && healthGoalModel.Last5 == true)
            {
                var result = await db.tUserTestResults
                .Join(db.tUserTestResultComponents, test => test.ID, comp => comp.TestResultID, (test, comp) => new { TestResult = test, Component = comp })
                .Join(db.tXrefUserTestResultComponentsCodes,
                            tc => tc.Component.ID, compcode => compcode.UserTestResultComponentID, (tc, compcode) =>
                            new { tc.TestResult.UserID, tc.TestResult.ResultDateTime, tc.Component.Name, tc.Component.Value, compcode.CodeID, tc.TestResult.SystemStatusID })
                .Where(u => u.UserID == healthGoalModel.UserId &&
                            u.SystemStatusID == 1 &&
                            idList.Contains(u.CodeID))
                .GroupBy(u => new { ResulDateTime = u.ResultDateTime })
                .Select(g => new UserTestModel
                {
                    ResultDateTime = g.Key.ResulDateTime,
                    Hdl = g.FirstOrDefault(u => (u.CodeID == 570)).Value,
                    Ldl = g.FirstOrDefault(u => (u.CodeID == 572)).Value,
                    Cholesterol = g.FirstOrDefault(u => (u.CodeID == 568)).Value
                })
                .OrderByDescending(u => u.ResultDateTime)
                .Take(5)
                .ToListAsync();

                return Ok(result);
            }
            else
            {
                var result = await db.tUserTestResults
                .Join(db.tUserTestResultComponents, test => test.ID, comp => comp.TestResultID, (test, comp) => new { TestResult = test, Component = comp })
                .Join(db.tXrefUserTestResultComponentsCodes,
                tc => tc.Component.ID, compcode => compcode.UserTestResultComponentID, (tc, compcode) =>
                    new { tc.TestResult.UserID, tc.TestResult.ResultDateTime, tc.Component.Name, tc.Component.Value, compcode.CodeID, tc.TestResult.SystemStatusID })
                    .Where(u => u.UserID == healthGoalModel.UserId && 
                                u.ResultDateTime >= healthGoalModel.StartDate && 
                                u.ResultDateTime <= healthGoalModel.EndDate && 
                                u.SystemStatusID == 1 &&
                                idList.Contains(u.CodeID))
                .GroupBy(u => new { ResulDateTime = u.ResultDateTime })
                .Select(g => new UserTestModel
                {
                    ResultDateTime = g.Key.ResulDateTime,
                    Hdl = g.FirstOrDefault(u => (u.CodeID == 570)).Value,
                    Ldl = g.FirstOrDefault(u => (u.CodeID == 572)).Value,
                    Cholesterol = g.FirstOrDefault(u => (u.CodeID == 568)).Value
                })
                .OrderBy(u => u.ResultDateTime)
                .ToListAsync();

                return Ok(result);
            }
        }

        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendBloodGlucose")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserTrendBloodGlucose(HealthGoalModel healthGoalModel)
        {
            try
            {


                var result = await
                            db.tUserTestResultComponents.Include(u => u.tUserTestResult)
                            .Include(u => u.tUnitsOfMeasure)
                            .Include(u => u.tXrefUserTestResultComponentsCodes)
                            .Include(u => u.tUserTestResult.tSourceOrganization.tOrganization)
                            .Include(u => u.tUserTestResult.tUserSourceService.tSourceService)
                            .Include(u => u.tUserTestResult.tTestResultStatus)
                            .Where(u => u.tXrefUserTestResultComponentsCodes.Any(x => x.CodeID == 604) && u.tUserTestResult.SystemStatusID == 1 
                            && u.SystemStatusID == 1 && u.tUserTestResult.UserID == healthGoalModel.UserId && u.tUserTestResult.ResultDateTime >= 
                            healthGoalModel.StartDate && u.tUserTestResult.ResultDateTime <= healthGoalModel.EndDate)
                            .Select(t => new {
                                t.tUserTestResult.UserID,
                                t.tUserTestResult.ResultDateTime,
                                TestResultID = t.tUserTestResult.ID,
                                tSourceOrganization = t.tUserTestResult.tSourceOrganization.tOrganization,
                                t.tUserTestResult.tTestResultStatus,
                                tUserSourceService = t.tUserTestResult.tUserSourceService.tSourceService,
                                t.Name,
                                t.ObjectID,
                                t.tUserTestResult.SourceObjectID,
                                t.tUserTestResult.SourceOrganizationID,
                                t.tUserTestResult.StatusID,
                                t.tUserTestResult.UserSourceServiceID,
                                t.Value,
                                t.UOMID,
                                t.Comments,
                                t.tUserTestResult.SystemStatusID,
                                t.tUnitsOfMeasure.UnitOfMeasure,
                                TestResultComponentID = t.ID,
                                Count = db.tUserTestResultComponents.Where(u=> u.TestResultID == t.TestResultID).Count(),
                                Note = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == t.ObjectID)
                            }).OrderBy(u=>u.ResultDateTime)
                            .ToListAsync();                                


                           // db.tUserTestResults
                           //.Include(u => u.tSourceOrganization.tOrganization)
                           //.Include(u => u.tUserSourceService.tSourceService)
                           //.Include(u => u.tTestResultStatus)
                           //.Include(u => u.tUserTestResultComponents)
                           //.Include(u => u.tXrefUserTestResultsCodes)
                           //.Where(u => u.tXrefUserTestResultsCodes.Any(x => x.CodeID == 604))
                           //.SelectMany(u => u.tUserTestResultComponents.SelectMany(y=> y.tXrefUserTestResultComponentsCodes).Select(t => new
                           //{
                           //    u.UserID,
                           //    u.ResultDateTime,
                           //    TestResultID = u.ID,
                           //    tSourceOrganization=  u.tSourceOrganization.tOrganization,
                           //    u.tTestResultStatus,
                           //    tUserSourceService =  u.tUserSourceService.tSourceService,
                           //    u.Name,
                           //    u.ObjectID,
                           //    u.SourceObjectID,
                           //    u.SourceOrganizationID,
                           //    u.StatusID,
                           //    u.UserSourceServiceID,
                           //   t.tUserTestResultComponent.Value,
                           //    t.tUserTestResultComponent.UOMID,
                           //    t.tUserTestResultComponent.Comments,
                           //    u.SystemStatusID,
                           //    t.tUserTestResultComponent.tUnitsOfMeasure.UnitOfMeasure,
                           //    TestResultComponentID = t.tUserTestResultComponent.ID,
                               
                           //}))
                           //.ToListAsync();

                var retVal = new List<UserBloodGlucose>();

                foreach (var ts in result)
                {


                    var glucoitem = new UserBloodGlucose
                    {
                        NoteID = ts.Note?.ID ?? 0,
                        TestResultID = ts.TestResultID,
                        TestResultComponentID = ts.TestResultComponentID,
                        Note = ts.Note?.Note ?? "",
                        ResultDateTime = ts.ResultDateTime,
                        Status = ts.tTestResultStatus.Status,
                        Source =  ts.SourceOrganizationID  != null ? ts.tSourceOrganization.Name : ts.tUserSourceService.ServiceName    ,
                        UOMID = ts.UOMID,
                        Value = ts.Value,
                        Comments = ts.Comments,
                        UserID = ts.UserID,
                        SourceObjectID = ts.SourceObjectID,
                        SourceOrganizationID = ts.SourceOrganizationID,
                        StatusID = ts.StatusID,
                        UserSourceServiceID = ts.UserSourceServiceID,
                        SystemStatusID = ts.SystemStatusID,
                        UnitOfMeasure = ts.UnitOfMeasure,
                        Count = ts.Count
                    };
                    retVal.Add(glucoitem);
                }

                return Ok(retVal);

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        // GET: api/TestResultStatus
        [Route("api/UserData/GetTestResultStatus")]
        public IQueryable<tTestResultStatus> GetTestResultStatus()
        {
            return db.tTestResultStatuses;
        }


        // DELETE: api/UserTest/5
        [Route("api/UserData/SoftDeleteUserTestResult/{id}/{status}")]
        [ResponseType(typeof(tUserTestResult))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeletetUserTestResult(int id, int status)
        {
            tUserTestResult tUserTestResult = await db.tUserTestResults.FindAsync(id);
            if (tUserTestResult == null)
            {
                return NotFound();
            }
            tUserTestResult.SystemStatusID = status;
            tUserTestResult.LastUpdatedDateTime = DateTime.Now;
            await db.SaveChangesAsync();
            return Ok(tUserTestResult);
        }


        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendCholesterol")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserTrendCholesterol(HealthGoalModel healthGoalModel)
        {
            try
            {
                var idList = new int[] { 568, 570, 572 };

                var result = await
                            db.tUserTestResultComponents.Include(u => u.tUserTestResult)
                            .Include(u => u.tUnitsOfMeasure)
                            .Include(u => u.tXrefUserTestResultComponentsCodes)
                            .Include(u => u.tUserTestResult.tSourceOrganization.tOrganization)
                            .Include(u => u.tUserTestResult.tUserSourceService.tSourceService)
                            .Include(u => u.tUserTestResult.tTestResultStatus)
                            .Where(u => u.tXrefUserTestResultComponentsCodes.Any(x => idList.Contains(x.CodeID)) && u.tUserTestResult.SystemStatusID == 1
                            && u.SystemStatusID == 1 && u.tUserTestResult.UserID == healthGoalModel.UserId && u.tUserTestResult.ResultDateTime >=
                            healthGoalModel.StartDate && u.tUserTestResult.ResultDateTime <= healthGoalModel.EndDate)
                            .Select(t => new {
                                t.tUserTestResult.UserID,
                                t.tUserTestResult.ResultDateTime,
                                TestResultID = t.tUserTestResult.ID,
                                tSourceOrganization = t.tUserTestResult.tSourceOrganization.tOrganization,
                                t.tUserTestResult.tTestResultStatus,
                                tUserSourceService = t.tUserTestResult.tUserSourceService.tSourceService,
                                t.Name,
                                t.ObjectID,
                                t.tUserTestResult.SourceObjectID,
                                t.tUserTestResult.SourceOrganizationID,
                                t.tUserTestResult.StatusID,
                                t.tUserTestResult.UserSourceServiceID,
                                t.Value,
                                t.UOMID,
                                t.Comments,
                                t.HighValue,
                                t.LowValue,
                                t.RefRange,
                                t.tUserTestResult.SystemStatusID,
                                t.tUnitsOfMeasure.UnitOfMeasure,
                                TestResultComponentID = t.ID,
                                Count = db.tUserTestResultComponents.Where(u => u.TestResultID == t.TestResultID).Count(),
                                Note = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == t.ObjectID),
                                CodeID = db.tXrefUserTestResultComponentsCodes.FirstOrDefault(c=>c.UserTestResultComponentID == t.ID).CodeID
                            }).OrderBy(u=>u.ResultDateTime)
                            .ToListAsync();


                var retVal = new List<UserBloodGlucose>();

                foreach (var ts in result)
                {


                    var glucoitem = new UserBloodGlucose
                    {
                        NoteID = ts.Note?.ID ?? 0,
                        TestResultID = ts.TestResultID,
                        TestResultComponentID = ts.TestResultComponentID,
                        Note = ts.Note?.Note ?? "",
                        ResultDateTime = ts.ResultDateTime,
                        Status = ts.tTestResultStatus.Status,
                        Source = ts.SourceOrganizationID != null ? ts.tSourceOrganization.Name : ts.tUserSourceService.ServiceName,
                        UOMID = ts.UOMID,
                        Value = ts.Value,
                        Comments = ts.Comments,
                        UserID = ts.UserID,
                        SourceObjectID = ts.SourceObjectID,
                        SourceOrganizationID = ts.SourceOrganizationID,
                        StatusID = ts.StatusID,
                        UserSourceServiceID = ts.UserSourceServiceID,
                        SystemStatusID = ts.SystemStatusID,
                        UnitOfMeasure = ts.UnitOfMeasure,
                        Count = ts.Count,
                        HighValue = ts.HighValue,
                        LowValue = ts.LowValue,
                        RefRange = ts.RefRange
                        ,CodeID = ts.CodeID,
                        Name = ts.Name
                    };
                    retVal.Add(glucoitem);
                }

                return Ok(retVal);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendBloodGlucoseAccounts/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserTrendBloodGlucoseAccounts(int id)
        {
            try
            {
                var list = await (from tr in db.tUserTestResults
                            join uss in db.tUserSourceServices on new { p1 = tr.UserSourceServiceID, p2 = tr.UserID } equals new { p1 = (int?)uss.ID, p2 = uss.UserID }
                            join so in db.tSourceOrganizations on uss.SourceServiceID equals so.SourceServiceID
                            join o in db.tOrganizations on so.OrganizationID equals o.ID
                            join xc in db.tUserTestResultComponents on tr.ID equals xc.TestResultID
                            join xcc in db.tXrefUserTestResultComponentsCodes on xc.ID equals xcc.UserTestResultComponentID
                            where uss.SystemStatusID == 1 && tr.UserID == id && xcc.CodeID == 604
                            select new {
                                ID = o.ID,
                                Name = o.Name
                            }).Distinct().ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendCholesterolAccounts/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserTrendCholesterolAccounts(int id)
        {
            try
            {
                var idList = new int[] { 568, 570, 572 };

                var list = await (from tr in db.tUserTestResults
                                  join uss in db.tUserSourceServices on new { p1 = tr.UserSourceServiceID, p2 = tr.UserID } equals new { p1 = (int?)uss.ID, p2 = uss.UserID }
                                  join so in db.tSourceOrganizations on uss.SourceServiceID equals so.SourceServiceID
                                  join o in db.tOrganizations on so.OrganizationID equals o.ID
                                  join xc in db.tUserTestResultComponents on tr.ID equals xc.TestResultID
                                  join xcc in db.tXrefUserTestResultComponentsCodes on xc.ID equals xcc.UserTestResultComponentID
                                  where uss.SystemStatusID == 1 && tr.UserID == id && idList.Contains(xcc.CodeID)
                                  select new
                                  {
                                      ID = o.ID,
                                      Name = o.Name
                                  }).Distinct().ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}