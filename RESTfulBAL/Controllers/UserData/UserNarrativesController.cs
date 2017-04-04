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
    public class UserNarrativesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserNarratives
        [Route("api/UserData/GetUserNarratives")]
        public IQueryable<tUserNarrative> GettUserNarratives()
        {
            return db.tUserNarratives;
        }

        // GET: api/UserNarratives/5
        [Route("api/UserData/GetUserNarratives/{id}")]
        [ResponseType(typeof(tUserNarrative))]
        public async Task<IHttpActionResult> GettUserNarrative(int id)
        {
            tUserNarrative tUserNarrative = await db.tUserNarratives.FindAsync(id);
            if (tUserNarrative == null)
            {
                return NotFound();
            }

            return Ok(tUserNarrative);
        }

        // PUT: api/UserNarratives/5
        [Route("api/UserData/UpdateUserNarratives/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserNarrative(int id, tUserNarrative tUserNarrative)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserNarrative.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserNarrative).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserNarrativeExists(id))
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

        // POST: api/UserNarratives
        [Route("api/UserData/PostUserNarrative")]
        [ResponseType(typeof(tUserNarrative))]
        public async Task<IHttpActionResult> PosttUserNarrative(tUserNarrative tUserNarrative)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserNarratives.Add(tUserNarrative);
            await db.SaveChangesAsync();

            return Ok(tUserNarrative);
        }

        // DELETE: api/UserNarratives/5
        [Route("api/UserData/DeleteUserNarratives/{id}")]
        [ResponseType(typeof(tUserNarrative))]
        public async Task<IHttpActionResult> DeletetUserNarrative(int id)
        {
            tUserNarrative tUserNarrative = await db.tUserNarratives.FindAsync(id);
            if (tUserNarrative == null)
            {
                return NotFound();
            }

            db.tUserNarratives.Remove(tUserNarrative);
            await db.SaveChangesAsync();

            return Ok(tUserNarrative);
        }
        // DELETE: api/SoftDeleteUserNarrative/5/2
        [Route("api/UserData/SoftDeleteUserNarrative/{id}/{status}")]
        [ResponseType(typeof(tUserNarrative))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserNarrative(int id, int status)
        {
            tUserNarrative tUserNarrative = await db.tUserNarratives.FindAsync(id);
            if (tUserNarrative == null)
            {
                return NotFound();
            }
            tUserNarrative.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tUserNarrative);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserNarrativeExists(int id)
        {
            return db.tUserNarratives.Count(e => e.ID == id) > 0;
        }
    }
}