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
    public class UserSleepsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserSleeps
        [Route("api/UserData/GetUserSleeps")]
        public IQueryable<tUserSleep> GettUserSleeps()
        {
            return db.tUserSleeps;
        }

        // GET: api/UserSleeps/5
        [Route("api/UserData/GetUserSleeps/{id}")]
        [ResponseType(typeof(tUserSleep))]
        public async Task<IHttpActionResult> GettUserSleep(int id)
        {
            tUserSleep tUserSleep = await db.tUserSleeps.FindAsync(id);
            if (tUserSleep == null)
            {
                return NotFound();
            }

            return Ok(tUserSleep);
        }

        // PUT: api/UserSleeps/5
        [Route("api/UserData/UpdateUserSleeps/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserSleep(int id, tUserSleep tUserSleep)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserSleep.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserSleep).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserSleepExists(id))
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

        // POST: api/UserSleeps
        [Route("api/UserData/PostUserSleeps")]
        [ResponseType(typeof(tUserSleep))]
        public async Task<IHttpActionResult> PosttUserSleep(tUserSleep tUserSleep)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserSleeps.Add(tUserSleep);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserSleep.ID }, tUserSleep);
        }

        // DELETE: api/UserSleeps/5
        [Route("api/UserData/DeleteUserSleeps/{id}")]
        [ResponseType(typeof(tUserSleep))]
        public async Task<IHttpActionResult> DeletetUserSleep(int id)
        {
            tUserSleep tUserSleep = await db.tUserSleeps.FindAsync(id);
            if (tUserSleep == null)
            {
                return NotFound();
            }

            db.tUserSleeps.Remove(tUserSleep);
            await db.SaveChangesAsync();

            return Ok(tUserSleep);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserSleepExists(int id)
        {
            return db.tUserSleeps.Count(e => e.ID == id) > 0;
        }

        // GET: api/UserVitals
        [Route("api/UserData/GetUserSleepVitals")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserSleepVitals(HealthGoalModel healthGoalModel)
        {

            var sleeps = await db.tUserSleeps
                .OrderBy(u => u.StartDateTime)
                .Where(u => u.UserID == healthGoalModel.UserId && u.StartDateTime >= healthGoalModel.StartDate &&
                            u.StartDateTime <= healthGoalModel.EndDate && u.SystemStatusID == 1).Select(s => new UserSleepModel { StartDateTime = s.StartDateTime, TimeAsleep = s.TimeAsleep })
                .ToListAsync();
            return Ok(sleeps);
        }

        // GET: api/UserData/GetUserLast5SleepVitals
        [Route("api/UserData/GetUserLast5SleepVitals")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserLast5SleepVitals(HealthGoalModel healthGoalModel)
        {

            var sleeps = await db.tUserSleeps
                .OrderBy(u => u.StartDateTime)
                .Where(u => u.UserID == healthGoalModel.UserId && 
                            u.SystemStatusID == 1)
                .Select(s=> new UserSleepModel { StartDateTime = s.StartDateTime, TimeAsleep = s.TimeAsleep})
                .OrderByDescending(u => u.StartDateTime)
                .Take(5)
                .ToListAsync();
            return Ok(sleeps);
        }

        [Route("api/UserData/GetUserSleepGoalVital/{id}")]
        public async Task<IHttpActionResult> GetUserSleepGoalVital(int id)
        {
            var userSleepModel =
                await db.tUserSleeps.Where(u => u.UserID == id && u.StartDateTime <= DateTimeOffset.Now && u.SystemStatusID == 1 && u.TimeAsleep != null)
                    .OrderByDescending(u => u.StartDateTime).Select(s=> new UserSleepModel {StartDateTime = s.StartDateTime, TimeAsleep = s.TimeAsleep})
                    .FirstOrDefaultAsync() ?? new UserSleepModel();

            var goal = await db.tUserHealthGoals
                .Where(g => g.GoalTypeID == 4 && g.SystemStatusID == 1 && g.UserID == id)
                .OrderByDescending(g => g.CreateDateTime)
                .FirstOrDefaultAsync() ?? new tUserHealthGoal();

            return Ok(new
            {
                StartDateTime = userSleepModel.StartDateTime,
                TimeAsleep = userSleepModel.TimeAsleep ?? 0,
                Goal = goal.Value
            });
        }

        // GET: api/UserVitals
        [Route("api/UserData/SetUserSleepGoal")]
        [HttpPost]
        public async Task<IHttpActionResult> SetUserSleepGoal(tUserHealthGoal healthGoal)
        {
            var tHealthGoal = await db.tUserHealthGoals.Where(u => u.UserID == healthGoal.UserID && u.SystemStatusID == 1 && u.GoalTypeID == 4).SingleOrDefaultAsync();

            if (tHealthGoal != null)
            {
                tHealthGoal.SystemStatusID = 2;
                tHealthGoal.LastUpdatedDateTime = DateTime.Now;
            }

            healthGoal.GoalTypeID = 4;
            healthGoal.CreateDateTime = DateTime.Now;
            healthGoal.ObjectID = new Guid();
            healthGoal.SystemStatusID = 1;

            db.tUserHealthGoals.Add(healthGoal);
            await db.SaveChangesAsync();
            return Ok();
        }

    }
}