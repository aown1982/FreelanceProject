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

namespace RESTfulBAL.Controllers.UserData
{
    public class UserActivitiesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserActivities
        [Route("api/UserData/GetUserActivities")]
        public IQueryable<tUserActivity> GettUserActivities()
        {
            return db.tUserActivities;
        }

        // GET: api/UserActivities/5
        [Route("api/UserData/GetUserActivities/{id}")]
        [ResponseType(typeof(tUserActivity))]
        public async Task<IHttpActionResult> GettUserActivity(int id)
        {
            tUserActivity tUserActivity = await db.tUserActivities.FindAsync(id);
            if (tUserActivity == null)
            {
                return NotFound();
            }

            return Ok(tUserActivity);
        }

        // PUT: api/UserActivities/5
        [Route("api/UserData/UpdateUserActivities/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserActivity(int id, tUserActivity tUserActivity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserActivity.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserActivity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserActivityExists(id))
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

        // POST: api/UserActivities
        [Route("api/UserData/PostUserActivities")]
        [ResponseType(typeof(tUserActivity))]
        public async Task<IHttpActionResult> PosttUserActivity(tUserActivity tUserActivity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserActivities.Add(tUserActivity);
            await db.SaveChangesAsync();

            //  return CreatedAtRoute("DefaultApi", new { id = tUserActivity.ID }, tUserActivity);
         return   Ok(tUserActivity);
        }

        // DELETE: api/UserActivities/5
        [Route("api/UserData/DeleteUserActivities/{id}")]
        [ResponseType(typeof(tUserActivity))]
        public async Task<IHttpActionResult> DeletetUserActivity(int id)
        {
            tUserActivity tUserActivity = await db.tUserActivities.FindAsync(id);
            if (tUserActivity == null)
            {
                return NotFound();
            }

            db.tUserActivities.Remove(tUserActivity);
            await db.SaveChangesAsync();

            return Ok(tUserActivity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserActivityExists(int id)
        {
            return db.tUserActivities.Count(e => e.ID == id) > 0;
        }

        // GET: api/UserData/GetUserLast5ExerciseVitals
        [Route("api/UserData/GetUserLast5ExerciseVitals")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserLast5ExerciseVitals(HealthGoalModel healthGoalModel)
        {

            var activities = await db.tUserActivities
                .OrderByDescending(u => u.StartDateTime)
                .Where(u => u.UserID == healthGoalModel.UserId && 
                            u.SystemStatusID == 1)
                .Take(5)
                .ToListAsync();

            return Ok(activities);
        }

        // GET: api/UserVitals
        [Route("api/UserData/GetUserExerciseVitals")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserExerciseVitals(HealthGoalModel healthGoalModel)
        {
            try
            {
                if (healthGoalModel.Last5 != null && healthGoalModel.Last5 == true)
                {
                    var activities = await db.tUserActivities
                        .OrderByDescending(u => u.StartDateTime)
                        .Where(u => u.UserID == healthGoalModel.UserId &&  
                                    u.Steps != null &&
                                    u.SystemStatusID == 1)
                        .Take(5)
                        .ToListAsync();

                    return Ok(activities);
                }
                else
                {
                    var activities = await db.tUserActivities
                        .OrderBy(u => u.StartDateTime)
                        .Where(u => u.UserID == healthGoalModel.UserId &&
                                    u.StartDateTime >= healthGoalModel.StartDate &&
                                    u.StartDateTime <= healthGoalModel.EndDate &&
                                    u.Steps != null &&
                                    u.SystemStatusID == 1)
                        .ToListAsync();

                    return Ok(activities);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [Route("api/UserData/GetUserExerciseGoalVital/{id}")]
        public async Task<IHttpActionResult> GetUserExerciseGoalVital(int id)
        {   
            var tUserActivity =
              await  db.tUserActivities.Where(u => u.UserID == id && u.StartDateTime <= DateTimeOffset.Now && u.SystemStatusID == 1 && u.Duration != null)
                    .OrderByDescending(u => u.StartDateTime)
                    .FirstOrDefaultAsync() ?? new tUserActivity();

            var goal = await db.tUserHealthGoals
                .Where(g => g.GoalTypeID == 3 && g.SystemStatusID == 1 && g.UserID == id)
                .OrderByDescending(g => g.CreateDateTime)
                .FirstOrDefaultAsync() ?? new tUserHealthGoal();

            return Ok(new
            {
                StartDateTime = tUserActivity.StartDateTime,
                Calories = tUserActivity.Calories ?? 0,
                Duration = tUserActivity.Duration ?? 0,
                Goal = goal.Value
            });
        }

        // GET: api/UserVitals
        [Route("api/UserData/SetUserExerciseGoal")]
        [HttpPost]
        public async Task<IHttpActionResult> SetUserExerciseGoal(tUserHealthGoal healthGoal)
        {
            var tHealthGoal = await db.tUserHealthGoals.Where(u => u.UserID == healthGoal.UserID && u.SystemStatusID == 1 && u.GoalTypeID == 3).SingleOrDefaultAsync();

            if (tHealthGoal != null)
            {
                tHealthGoal.SystemStatusID = 2;
                tHealthGoal.LastUpdatedDateTime = DateTime.Now;
            }

            healthGoal.GoalTypeID = 3;
            healthGoal.CreateDateTime = DateTime.Now;
            healthGoal.ObjectID = new Guid();
            healthGoal.SystemStatusID = 1;

            db.tUserHealthGoals.Add(healthGoal);
          await  db.SaveChangesAsync();
            return Ok();
        }


        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendActivity")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserTrendActivity(HealthGoalModel healthGoalModel)
        {

            var activities = await db.tUserActivities.Include(u => u.tActivity)
                .Include(u => u.tUnitsOfMeasure)
                .Include(u => u.tUnitsOfMeasure1)
                .Include(u => u.tUserSourceService.tSourceService)
                .Where(u => u.UserID == healthGoalModel.UserId && u.StartDateTime >= healthGoalModel.StartDate &&
                            u.StartDateTime <= healthGoalModel.EndDate && u.SystemStatusID == 1).Select(u => new Models.UserData.UserAcitivityModel
                            {
                                ActivityID = u.ActivityID,
                                ID = u.ID,
                                SourceObjectID = u.SourceObjectID,
                                UserID = u.UserID,
                                UserSourceServiceID = u.UserSourceServiceID,
                                Duration = u.Duration,
                                Distance = u.Distance,
                                DurationUOMID = u.tUnitsOfMeasure.ID,
                                DistanceUOMID = u.tUnitsOfMeasure1.ID,
                                DurationUOM = u.tUnitsOfMeasure.UnitOfMeasure,
                                DistanceUOM = u.tUnitsOfMeasure1.UnitOfMeasure,
                                Steps = u.Steps,
                                Calories = u.Calories,
                                LightActivityMin = u.LightActivityMin,
                                ModerateActivityMin = u.ModerateActivityMin,
                                VigorousActivityMin = u.VigorousActivityMin,
                                SedentaryActivityMin = u.SedentaryActivityMin,
                                StartDateTime = u.StartDateTime,
                                EndDateTime = u.EndDateTime,
                                SystemStatusID = u.SystemStatusID,
                                CreateDateTime = u.CreateDateTime,
                                LastUpdatedDateTime = u.LastUpdatedDateTime,
                                UserDerivedNote = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == u.ObjectID),
                                Activity = u.tActivity.Name,
                                Source = u.tUserSourceService.tSourceService.ServiceName
                            }).OrderBy(u=>u.StartDateTime).
                            ToListAsync();


            return Ok(activities);
        }


        // DELETE: api/UserTest/5
        [Route("api/UserData/SoftDeleteUserActivity/{id}/{status}")]
        [ResponseType(typeof(tUserTestResult))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserActivity(int id, int status)
        {
            tUserActivity tUserActivity = await db.tUserActivities.FindAsync(id);
            if (tUserActivity == null)
            {
                return NotFound();
            }
            tUserActivity.SystemStatusID = status;
            tUserActivity.LastUpdatedDateTime = DateTime.Now;
            await db.SaveChangesAsync();
            return Ok(tUserActivity);
        }

        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendActivityAccounts/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserTrendActivityAccounts(int id)
        {
            try
            {

                var list = await (from tr in db.tUserActivities
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



    }
}