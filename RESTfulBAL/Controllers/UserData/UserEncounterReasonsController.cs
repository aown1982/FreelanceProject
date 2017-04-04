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
    public class UserEncounterReasonsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserEncounterReasons
        [Route("api/UserData/GetUserEncounterReasons")]
        public IQueryable<tUserEncounterReason> GettUserEncounterReasons()
        {
            return db.tUserEncounterReasons;
        }

        // GET: api/UserEncounterReasons/5
        [Route("api/UserData/GetUserEncounterReasons/{id}")]
        [ResponseType(typeof(tUserEncounterReason))]
        public async Task<IHttpActionResult> GettUserEncounterReason(int id)
        {
            tUserEncounterReason tUserEncounterReason = await db.tUserEncounterReasons.FindAsync(id);
            if (tUserEncounterReason == null)
            {
                return NotFound();
            }

            return Ok(tUserEncounterReason);
        }

        // PUT: api/UserEncounterReasons/5
        [Route("api/UserData/UpdateUserEncounterReasons/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserEncounterReason(int id, tUserEncounterReason tUserEncounterReason)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserEncounterReason.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserEncounterReason).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserEncounterReasonExists(id))
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

        // POST: api/UserEncounterReasons
        [Route("api/UserData/PostUserEncounterReasons")]
        [ResponseType(typeof(tUserEncounterReason))]
        public async Task<IHttpActionResult> PosttUserEncounterReason(tUserEncounterReason tUserEncounterReason)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserEncounterReasons.Add(tUserEncounterReason);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserEncounterReason.ID }, tUserEncounterReason);
        }

        // DELETE: api/UserEncounterReasons/5
        [Route("api/UserData/DeleteUserEncounterReasons/{id}")]
        [ResponseType(typeof(tUserEncounterReason))]
        public async Task<IHttpActionResult> DeletetUserEncounterReason(int id)
        {
            tUserEncounterReason tUserEncounterReason = await db.tUserEncounterReasons.FindAsync(id);
            if (tUserEncounterReason == null)
            {
                return NotFound();
            }

            db.tUserEncounterReasons.Remove(tUserEncounterReason);
            await db.SaveChangesAsync();

            return Ok(tUserEncounterReason);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserEncounterReasonExists(int id)
        {
            return db.tUserEncounterReasons.Count(e => e.ID == id) > 0;
        }
    }
}