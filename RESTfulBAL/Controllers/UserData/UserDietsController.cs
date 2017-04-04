using System;
using System.Collections;
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

namespace RESTfulBAL.Controllers.UserData
{
    public class UserDietsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserDiets
        [Route("api/UserData/GetUserDiets")]
        public IQueryable<tUserDiet> GettUserDiets()
        {
            return db.tUserDiets;
        }

        // GET: api/UserDiets/5
        [Route("api/UserData/GetUserDiets/{id}")]
        [ResponseType(typeof(tUserDiet))]
        public async Task<IHttpActionResult> GettUserDiet(int id)
        {
            tUserDiet tUserDiet = await db.tUserDiets.FindAsync(id);
            if (tUserDiet == null)
            {
                return NotFound();
            }

            return Ok(tUserDiet);
        }

        // PUT: api/UserDiets/5
        [Route("api/UserData/UpdateUserDiets/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserDiet(int id, tUserDiet tUserDiet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserDiet.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserDiet).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserDietExists(id))
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

        // POST: api/UserDiets
        [Route("api/UserData/PostUserDiets")]
        [ResponseType(typeof(tUserDiet))]
        public async Task<IHttpActionResult> PosttUserDiet(tUserDiet tUserDiet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserDiets.Add(tUserDiet);
            await db.SaveChangesAsync();

            //  return CreatedAtRoute("DefaultApi", new { id = tUserDiet.ID }, tUserDiet);
            return Ok(tUserDiet);
        }

        // DELETE: api/UserDiets/5
        [Route("api/UserData/DeleteUserDiets/{id}")]
        [ResponseType(typeof(tUserDiet))]
        public async Task<IHttpActionResult> DeletetUserDiet(int id)
        {
            tUserDiet tUserDiet = await db.tUserDiets.FindAsync(id);
            if (tUserDiet == null)
            {
                return NotFound();
            }

            db.tUserDiets.Remove(tUserDiet);
            await db.SaveChangesAsync();

            return Ok(tUserDiet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserDietExists(int id)
        {
            return db.tUserDiets.Count(e => e.ID == id) > 0;
        }

        // GET: api/UserData/GetUserLast5DietVitals
        [Route("api/UserData/GetUserLast5DietVitals")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserLast5DietVitals(HealthGoalModel healthGoalModel)
        {

            //db.tUserDiets
            //  .Include(d=>d.tXrefUserDietNutrients)
            //  .OrderBy(u => u.EnteredDateTime)
            //  .Where(u => u.UserID == healthGoalModel.UserId && u.EnteredDateTime >= healthGoalModel.StartDate &&
            //              u.EnteredDateTime <= healthGoalModel.EndDate).ToList();
            var userModel = await db.tUserDiets
                .Join(db.tXrefUserDietNutrients, d => d.ID, x => x.UserDietID, (d, x) => new { d, x })
                .Where(w => w.x.NutrientID == 170 && 
                            w.d.UserID == healthGoalModel.UserId &&
                            w.d.SystemStatusID == 1)
                .GroupBy(y => new { y.d.EnteredDateTime })
                .Select(g => new UserDietModel { EnteredDateTime = g.Key.EnteredDateTime, Value = g.Sum(a => a.x.Value) })
                .OrderByDescending(u => u.EnteredDateTime)
                .Take(5)
                .ToListAsync();

            return Ok(userModel);

        }

        // GET: api/UserVitals
        [Route("api/UserData/GetUserDietVitals")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserDietVitals(HealthGoalModel healthGoalModel)
        {
            try
            {
                if (healthGoalModel.Last5 != null && healthGoalModel.Last5 == true)
                {

                    var userModel = await db.tUserDiets
                                        .Join(db.tXrefUserDietNutrients, d => d.ID, x => x.UserDietID, (d, x) => new { d, x })
                                        .Where(w => w.x.NutrientID == 170 && 
                                                    w.d.UserID == healthGoalModel.UserId && 
                                                    w.d.SystemStatusID == 1)
                                        .GroupBy(y => new { y.d.EnteredDateTime })
                                        .Select(g => new UserDietModel { EnteredDateTime = g.Key.EnteredDateTime, Value = g.Sum(a => a.x.Value) })
                                        .OrderByDescending(u => u.EnteredDateTime)
                                        .Take(5)
                                        .ToListAsync();

                    return Ok(userModel);
                }
                else
                {
                    var userModel = await db.tUserDiets
                                        .Join(db.tXrefUserDietNutrients, d => d.ID, x => x.UserDietID, (d, x) => new { d, x })
                                        .Where(w => w.x.NutrientID == 170 &&
                                                    w.d.UserID == healthGoalModel.UserId &&
                                                    w.d.EnteredDateTime >= healthGoalModel.StartDate &&
                                                    w.d.EnteredDateTime <= healthGoalModel.EndDate &&
                                                    w.d.SystemStatusID == 1)
                                        .GroupBy(y => new { y.d.EnteredDateTime })
                                        .Select(g => new UserDietModel { EnteredDateTime = g.Key.EnteredDateTime, Value = g.Sum(a => a.x.Value) }).ToListAsync();

                    return Ok(userModel);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [Route("api/UserData/GetUserDietGoalVital/{id}")]
        public async Task<IHttpActionResult> GetUserDietGoalVital(int id)
        {
            //var tUserDiet =
            //    db.tUserDiets
            //    .Include(d => d.tXrefUserDietNutrients)
            //    .Where(u => u.UserID == id && u.EnteredDateTime <= DateTimeOffset.Now && u.SystemStatusID == 1 )
            //        .OrderByDescending(u => u.EnteredDateTime)
            //        .FirstOrDefault() ?? new tUserDiet();

            var userModel = await db.tUserDiets
               .Join(db.tXrefUserDietNutrients, d => d.ID, x => x.UserDietID, (d, x) => new { d, x })
               .Where(w => w.x.NutrientID == 170 && w.d.UserID == id && w.d.EnteredDateTime <= DateTimeOffset.Now && w.d.SystemStatusID == 1)
               .OrderByDescending(o=>o.d.EnteredDateTime)
               .Select(g => new UserDietModel { EnteredDateTime = g.d.EnteredDateTime, Value = g.x.Value,  }).FirstOrDefaultAsync() ?? new UserDietModel();


            var goal = await db.tUserHealthGoals
                .Where(g => g.GoalTypeID == 2 && g.SystemStatusID == 1 && g.UserID == id)
                .OrderByDescending(g => g.CreateDateTime)
                .FirstOrDefaultAsync() ?? new tUserHealthGoal();

            return Ok(new
            {
                EnteredDateTime = userModel.EnteredDateTime,
                Value = userModel.Value??0,
                Goal = goal.Value
            });
        }

        // GET: api/UserVitals
        [Route("api/UserData/SetUserDietGoal")]
        [HttpPost]
        public async Task<IHttpActionResult> SetUserDietGoal(tUserHealthGoal healthGoal)
        {
            var tHealthGoal =  await db.tUserHealthGoals.Where(u => u.UserID == healthGoal.UserID && u.SystemStatusID == 1 && u.GoalTypeID == 2).SingleOrDefaultAsync();

            if (tHealthGoal != null)
            {
                tHealthGoal.SystemStatusID = 2;
                tHealthGoal.LastUpdatedDateTime = DateTime.Now;
            }

            healthGoal.GoalTypeID = 2;
            healthGoal.CreateDateTime = DateTime.Now;
            healthGoal.ObjectID = new Guid();
            healthGoal.SystemStatusID = 1;

            db.tUserHealthGoals.Add(healthGoal);
           await db.SaveChangesAsync();
            return Ok();
        }

        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendDietAccounts/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserTrendDietAccounts(int id)
        {
            try
            {

                var list = await (from tr in db.tUserDiets
                                  join uss in db.tUserSourceServices on new { p1 = tr.UserSourceServiceID, p2 = tr.UserID } equals new { p1 = (int?)uss.ID, p2 = uss.UserID }
                                  join so in db.tSourceOrganizations on uss.SourceServiceID equals so.SourceServiceID
                                  join o in db.tOrganizations on so.OrganizationID equals o.ID
                                  where uss.SystemStatusID == 1 && tr.UserID == id
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

        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendDiet")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserTrendDiet(HealthGoalModel healthGoalModel)
        {
            try
            {


                var result = await
                            db.tUserDiets
                            .Include(u=>u.tDietCategory)
                            .Include(u => u.tUnitsOfMeasure)
                            .Include(u => u.tUserSourceService.tSourceService)
                            .Where(u => u.SystemStatusID == 1 && u.UserID == healthGoalModel.UserId && u.EnteredDateTime >=
                            healthGoalModel.StartDate && u.EnteredDateTime <= healthGoalModel.EndDate)
                            .Select(t => new {
                                t.UserID,
                                t.EnteredDateTime,
                                t.ID,
                                tUserSourceService = t.tUserSourceService.tSourceService,
                                t.Name,
                                t.ObjectID,
                                t.SourceObjectID,
                                t.UserSourceServiceID,
                                t.Servings,
                                t.ServingUOMID,
                                t.SystemStatusID,
                                t.tUnitsOfMeasure.UnitOfMeasure,
                                t.DietCategoryID,
                                Category = t.tDietCategory.Name,
                                t.CreateDateTime,
                                Note = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == t.ObjectID)
                            }).OrderBy(u => u.EnteredDateTime)
                            .ToListAsync();


                var retVal = new List<Models.UserData.UserDiet>();
                
                foreach (var ts in result)
                {
                    var uditem = new Models.UserData.UserDiet
                    {
                        NoteID = ts.Note?.ID ?? 0,
                        ID = ts.ID,
                        Note = ts.Note?.Note ?? "",
                        EnteredDateTime = ts.EnteredDateTime,
                        Source = ts.tUserSourceService != null ?  ts.tUserSourceService.ServiceName : "",
                        ServingUOMID = ts.ServingUOMID,
                        Servings = ts.Servings,
                        UserID = ts.UserID,
                        SourceObjectID = ts.SourceObjectID,
                        UserSourceServiceID = ts.UserSourceServiceID,
                        SystemStatusID = ts.SystemStatusID,
                        UnitOfMeasure = ts.UnitOfMeasure,
                        Name = ts.Name,
                        DietCategoryID = ts.DietCategoryID,
                        Category = ts.Category,
                        CreateDateTime = ts.CreateDateTime       
                    };
                    retVal.Add(uditem);
                }

                return Ok(retVal);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // DELETE: api/UserTest/5
        [Route("api/UserData/SoftDeleteUserDiet/{id}/{status}")]
        [ResponseType(typeof(tUserDiet))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserDiet(int id, int status)
        {
            tUserDiet tUserDiet = await db.tUserDiets.FindAsync(id);
            if (tUserDiet == null)
            {
                return NotFound();
            }
            tUserDiet.SystemStatusID = status;
            tUserDiet.LastUpdatedDateTime = DateTime.Now;
            await db.SaveChangesAsync();
            return Ok(tUserDiet);
        }


        // POST: api/UserDiets
        [Route("api/UserData/AddUserDiets")]
        [ResponseType(typeof(tUserDiet))]
        public async Task<IHttpActionResult> AddUserDiets(tUserDiet tUserDiet)
        {

            try
            {

            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var tNutrient = await db.tXrefUserDietNutrients.Where(u => u.UserDietID == tUserDiet.ID).ToListAsync();
        //    tUserDiet.ID = 0;
         //   db.tUserDiets.Add(tUserDiet);

            foreach (var item in tNutrient)
            {
                tUserDiet.tXrefUserDietNutrients.Add(new tXrefUserDietNutrient
                {
                    CreateDateTime = DateTime.Now,
                    NutrientID = item.NutrientID,
                    SystemStatusID = 1,
                    UOMID = item.UOMID,
                    Value = item.Value
                });
            }

            db.tUserDiets.Add(tUserDiet);

            await db.SaveChangesAsync();

            //  return CreatedAtRoute("DefaultApi", new { id = tUserDiet.ID }, tUserDiet);
            return Ok(tUserDiet);
            }
            catch (Exception ex)
            {

                throw;
            }

        }


    }
}