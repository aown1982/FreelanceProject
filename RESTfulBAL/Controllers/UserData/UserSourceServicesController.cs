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
using DAL.Users;
using RESTfulBAL.Models.UserData;

namespace RESTfulBAL.Controllers.UserData
{
    public class UserSourceServicesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UsersEntities dbUsers = new UsersEntities();


        // GET: api/UserSourceServices
        [Route("api/UserData/GetUserSourceServices")]
        public IQueryable<tUserSourceService> GettUserSourceServices()
        {
            return db.tUserSourceServices;
        }

        // GET: api/UserSourceServices/5
        [Route("api/UserData/GetUserSourceServices/{id}")]
        [ResponseType(typeof(tUserSourceService))]
        public async Task<IHttpActionResult> GettUserSourceService(int id)
        {
            tUserSourceService tUserSourceService = await db.tUserSourceServices.FindAsync(id);
            if (tUserSourceService == null)
            {
                return NotFound();
            }

            return Ok(tUserSourceService);
        }

        // PUT: api/UserSourceServices/5
        [Route("api/UserData/UpdateUserSourceServices/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserSourceService(int id, tUserSourceService tUserSourceService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserSourceService.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserSourceService).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserSourceServiceExists(id))
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

        // POST: api/UserSourceServices
        [Route("api/UserData/PostUserSourceServices")]
        [ResponseType(typeof(tUserSourceService))]
        public async Task<IHttpActionResult> PosttUserSourceService(tUserSourceService tUserSourceService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserSourceServices.Add(tUserSourceService);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserSourceService.ID }, tUserSourceService);
        }


        [Route("api/UserData/GetUserSourceServiceByUserIdAndCategory/{userId}/{category}")]
        [ResponseType(typeof(tUserSourceService))]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserSourceServiceByUserIdAndCategory(int userId, string category)
        {
            tUserSourceService _item = null;
            switch (category)
            {
                case "Allergies":
                    _item = db.tUserSourceServices.Where(a => a.UserID == userId & a.SourceServiceID == 12).ToList().FirstOrDefault();
                    if (_item == null)
                    {
                        _item = new tUserSourceService();
                        _item.SourceServiceID = 12;
                        _item.UserID = userId;
                        _item.ObjectID = Guid.NewGuid();
                        _item.StatusID = 1;
                        db.tUserSourceServices.Add(_item);
                        await db.SaveChangesAsync();
                    }
                    break;
                case "Care Plans":
                    _item = db.tUserSourceServices.Where(a => a.UserID == userId & a.SourceServiceID == 12).ToList().FirstOrDefault();
                    if (_item == null)
                    {
                        _item = new tUserSourceService();
                        _item.SourceServiceID = 12;
                        _item.UserID = userId;
                        _item.ObjectID = Guid.NewGuid();
                        _item.StatusID = 1;
                        db.tUserSourceServices.Add(_item);
                        await db.SaveChangesAsync();
                    }
                    break;
                case "Encounters":
                    _item = db.tUserSourceServices.Where(a => a.UserID == userId & a.SourceServiceID == 12).ToList().FirstOrDefault();
                    if (_item == null)
                    {
                        _item = new tUserSourceService();
                        _item.SourceServiceID = 12;
                        _item.UserID = userId;
                        _item.ObjectID = Guid.NewGuid();
                        _item.StatusID = 1;
                        db.tUserSourceServices.Add(_item);
                        await db.SaveChangesAsync();
                    }
                    break;
                case "Functional Statuses":
                    _item = db.tUserSourceServices.Where(a => a.UserID == userId & a.SourceServiceID == 12).ToList().FirstOrDefault();
                    if (_item == null)
                    {
                        _item = new tUserSourceService();
                        _item.SourceServiceID = 12;
                        _item.UserID = userId;
                        _item.ObjectID = Guid.NewGuid();
                        _item.StatusID = 1;
                        db.tUserSourceServices.Add(_item);
                        await db.SaveChangesAsync();
                    }
                    break;
                case "Immunizations":
                    _item = db.tUserSourceServices.Where(a => a.UserID == userId & a.SourceServiceID == 12).ToList().FirstOrDefault();
                    if (_item == null)
                    {
                        _item = new tUserSourceService();
                        _item.SourceServiceID = 12;
                        _item.UserID = userId;
                        _item.ObjectID = Guid.NewGuid();
                        _item.StatusID = 1;
                        db.tUserSourceServices.Add(_item);
                        await db.SaveChangesAsync();
                    }
                    break;
                case "Instructions":
                    _item = db.tUserSourceServices.Where(a => a.UserID == userId & a.SourceServiceID == 12).ToList().FirstOrDefault();
                    if (_item == null)
                    {
                        _item = new tUserSourceService();
                        _item.SourceServiceID = 12;
                        _item.UserID = userId;
                        _item.ObjectID = Guid.NewGuid();
                        _item.StatusID = 1;
                        db.tUserSourceServices.Add(_item);
                        await db.SaveChangesAsync();
                    }
                    break;
                case "Narratives":
                    _item = db.tUserSourceServices.Where(a => a.UserID == userId & a.SourceServiceID == 12).ToList().FirstOrDefault();
                    if (_item == null)
                    {
                        _item = new tUserSourceService();
                        _item.SourceServiceID = 12;
                        _item.UserID = userId;
                        _item.ObjectID = Guid.NewGuid();
                        _item.StatusID = 1;
                        db.tUserSourceServices.Add(_item);
                        await db.SaveChangesAsync();
                    }
                    break;
                case "Prescriptions":
                    _item = db.tUserSourceServices.Where(a => a.UserID == userId & a.SourceServiceID == 12).ToList().FirstOrDefault();
                    if (_item == null)
                    {
                        _item = new tUserSourceService();
                        _item.SourceServiceID = 12;
                        _item.UserID = userId;
                        _item.ObjectID = Guid.NewGuid();
                        _item.StatusID = 1;
                        db.tUserSourceServices.Add(_item);
                        await db.SaveChangesAsync();
                    }
                    break;
                case "Problems":
                    _item = db.tUserSourceServices.Where(a => a.UserID == userId & a.SourceServiceID == 12).ToList().FirstOrDefault();
                    if (_item == null)
                    {
                        _item = new tUserSourceService();
                        _item.SourceServiceID = 12;
                        _item.UserID = userId;
                        _item.ObjectID = Guid.NewGuid();
                        _item.StatusID = 1;
                        db.tUserSourceServices.Add(_item);
                        await db.SaveChangesAsync();
                    }
                    break;
                case "Procedures":
                    _item = db.tUserSourceServices.Where(a => a.UserID == userId & a.SourceServiceID == 12).ToList().FirstOrDefault();
                    if (_item == null)
                    {
                        _item = new tUserSourceService();
                        _item.SourceServiceID = 12;
                        _item.UserID = userId;
                        _item.ObjectID = Guid.NewGuid();
                        _item.StatusID = 1;
                        db.tUserSourceServices.Add(_item);
                        await db.SaveChangesAsync();
                    }
                    break;
                case "Test Results":
                    _item = db.tUserSourceServices.Where(a => a.UserID == userId & a.SourceServiceID == 12).ToList().FirstOrDefault();
                    if (_item == null)
                    {
                        _item = new tUserSourceService();
                        _item.SourceServiceID = 12;
                        _item.UserID = userId;
                        _item.ObjectID = Guid.NewGuid();
                        _item.StatusID = 1;
                        db.tUserSourceServices.Add(_item);
                        await db.SaveChangesAsync();
                    }
                    break;
                case "Vitals":
                    _item = db.tUserSourceServices.Where(a => a.UserID == userId & a.SourceServiceID == 12).ToList().FirstOrDefault();
                    if (_item == null)
                    {
                        _item = new tUserSourceService();
                        _item.SourceServiceID = 12;
                        _item.UserID = userId;
                        _item.ObjectID = Guid.NewGuid();
                        _item.StatusID = 1;
                        db.tUserSourceServices.Add(_item);
                        await db.SaveChangesAsync();
                    }
                    break;

                case "BloodGlucose":
                    try
                    {
                        _item = db.tUserSourceServices.Where(a => a.UserID == userId & a.SourceServiceID == 12 && a.SystemStatusID == 1).ToList().FirstOrDefault();
                        if (_item == null)
                        {
                            var credentials = db.tCredentials.FirstOrDefault(u => u.UserID == userId);

                            _item = new tUserSourceService();
                            _item.SourceServiceID = 12;
                            _item.UserID = userId;
                            _item.ObjectID = Guid.NewGuid();
                            _item.StatusID = 1;
                            _item.SystemStatusID = 1;
                            if (credentials != null) _item.CredentialID = credentials.ID;

                            db.tUserSourceServices.Add(_item);
                            await db.SaveChangesAsync();
                        }
                        break;

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                default:
                    break;
            }
            return Ok(_item);
        }


        // DELETE: api/UserSourceServices/5
        [Route("api/UserData/DeleteUserSourceServices/{id}")]
        [ResponseType(typeof(tUserSourceService))]
        public async Task<IHttpActionResult> DeletetUserSourceService(int id)
        {
            tUserSourceService tUserSourceService = await db.tUserSourceServices.FindAsync(id);
            if (tUserSourceService == null)
            {
                return NotFound();
            }

            db.tUserSourceServices.Remove(tUserSourceService);
            await db.SaveChangesAsync();

            return Ok(tUserSourceService);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserSourceServiceExists(int id)
        {
            return db.tUserSourceServices.Count(e => e.ID == id) > 0;
        }

        [Route("api/UserData/GetListOfAuditTypesByUser/{userId}/{recordsCount}/{currentPage}")]
        public async Task<IEnumerable<UserAuditLogsViewModel>> GetListOfAuditTypesByUser(int userId, int recordsCount, int currentPage)
        {
            var auditTypes = db.tUserDataAuditTypes.Where(a => a.IsDisplayed && a.SystemStatusID == 1);
            var loginAuditType = auditTypes.Where(a => a.ID == 1).Select(a1 => a1.DisplayDescription).FirstOrDefault();

            var mainList = await dbUsers.tUsersAudits.Where(u => u.UserID == userId).OrderByDescending(a => a.DateTimeStamp).Select(a => new UserAuditLogsViewModel
            {
                DateTimeStamp = a.DateTimeStamp.Value,
                Name = loginAuditType + " " + a.DateTimeStamp,
                Id = a.Id
            }).ToListAsync();

            var objectIds = db.tUserDataAudits.Select(a => a.NewObjectID.Value).Select(s => s.ToString());
            var list = new List<int>();
            var list2 = new List<string>();
            var tableNames = new[] { "[dbo].[tUserActivities]", "[dbo].[tUserAllergies]", "[dbo].[tUserCarePlans]", "[dbo].[tUserEncounters]"
                , "[dbo].[tUserFunctionalStatuses]","[dbo].[tUserImmunizations]","[dbo].[tUserInstructions]","[dbo].[tUserNarratives]","[dbo].[tUserPrescriptions]"
                ,"[dbo].[tUserProblems]","[dbo].[tUserProcedures]","[dbo].[tUserTestResults]","[dbo].[tUserVitals]","[dbo].[tUserDiet]","[dbo].[tUserEducation]"
                ,"[dbo].[tUserGeneticTraits]","[dbo].[tUserLocations]","[dbo].[tUserSleep]","[tUserSNPProbes]","[dbo].[tUserEmployment]","[dbo].[tUserOrders]"
                ,"[dbo].[tUserPolitics]","[dbo].[tUserRelationships]","[dbo].[tUserReligion]" };

            /*
             * Tables without UserSourceServiceID: tUserSpecimens,tUserHealthGoals
             */
            var userSourceServiceIDs = new List<int>();
            var query = string.Empty;
            foreach (var tn in tableNames)
            {
                var subquery = string.Format("Select UserSourceServiceID From {0} (NOLOCK) Where ObjectID IN ('{1}')", tn, string.Join("','", objectIds));

                if (tn.Contains("tUserOrders"))
                    subquery = string.Format("Select UserSourceServiceID From tUserEncounters (NOLOCK) Where ObjectID IN ('{1}')", tn, string.Join("','", objectIds));

                query += string.IsNullOrEmpty(query) ? subquery : (" UNION " + subquery);
            }

            var result = await db.Database.SqlQuery<int>(query).ToListAsync();
            if (result.Any())
                userSourceServiceIDs.AddRange(result);

            var fSQL = "SELECT (Case ISNULL(torg.Name,'') When '' THEN tss.ServiceName Else torg.Name END) AS Name" +
                   " ,(Case ISNULL(torg.ID,'') When '' THEN tss.CreateDateTime Else torg.CreateDateTime END) As DateTimeStamp" +
                   " ,(Case ISNULL(torg.ID,'') When '' THEN tss.ID Else torg.ID END) As ID" +
                   " ,(Case ISNULL(torg.ID,'') When '' THEN 7 Else 4 END) As TypeId" +
                   " FROM tSourceServices AS tss LEFT OUTER JOIN" +
                   " tSourceOrganizations AS tso ON tso.SourceServiceID = tss.ID LEFT OUTER JOIN" +
                   " tOrganizations AS torg ON torg.ID = tso.OrganizationID RIGHT OUTER JOIN" +
                   " tUserSourceServices AS tus ON tus.SourceServiceID = tss.ID" +
                   " WHERE(tus.ID IN (" + string.Join(",", userSourceServiceIDs) + "))";

            var result1 = db.Database.SqlQuery<UserAuditLogsViewModel>(fSQL).ToList();
            if (result1.Any())
            {
                result1.ForEach(a =>
                {
                    var prefix = auditTypes.Where(a1 => a1.ID == a.TypeId).Select(s => s.DisplayDescription).FirstOrDefault();
                    a.Name = string.Format("{0} {1}", prefix, a.Name);
                });
                mainList.AddRange(result1);
            }
            int startIndex = (currentPage - 1) * recordsCount;
            var totalRecords = mainList.OrderByDescending(a => a.DateTimeStamp);
            mainList = totalRecords.Skip(startIndex).Take(recordsCount).ToList();
            if (mainList.Any())
                mainList[0].TotalCount = totalRecords.Count();
            return mainList;
        }
    }
}