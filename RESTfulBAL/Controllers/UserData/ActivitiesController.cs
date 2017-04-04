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

namespace RESTfulBAL.Controllers.UserData
{
    public class ActivitiesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/Activities
        [Route("api/UserData/GetActivities")]
        public IQueryable<tActivity> GettActivities()
        {
            return db.tActivities;
        }

        // GET: api/Activities/5
        [Route("api/UserData/GetActivities/{id}")]
        [ResponseType(typeof(tActivity))]
        public async Task<IHttpActionResult> GettActivity(int id)
        {
            tActivity tActivity = await db.tActivities.FindAsync(id);
            if (tActivity == null)
            {
                return NotFound();
            }

            return Ok(tActivity);
        }

        // PUT: api/Activities/5
        [Route("api/UserData/UpdateActivities/{id}/Activity")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttActivity(int id, tActivity Activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Activity.ID)
            {
                return BadRequest();
            }

            db.Entry(Activity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tActivityExists(id))
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

        // POST: api/Activities
        [Route("api/UserData/PostActivities/Activity")]
        [ResponseType(typeof(tActivity))]
        public async Task<IHttpActionResult> PosttActivity(tActivity Activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tActivities.Add(Activity);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = Activity.ID }, Activity);
        }

        // DELETE: api/Activities/5
        [Route("api/UserData/DeleteActivity/{id}")]
        [ResponseType(typeof(tActivity))]
        public async Task<IHttpActionResult> DeletetActivity(int id)
        {
            tActivity tActivity = await db.tActivities.FindAsync(id);
            if (tActivity == null)
            {
                return NotFound();
            }

            db.tActivities.Remove(tActivity);
            await db.SaveChangesAsync();

            return Ok(tActivity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tActivityExists(int id)
        {
            return db.tActivities.Count(e => e.ID == id) > 0;
        }
    }
}