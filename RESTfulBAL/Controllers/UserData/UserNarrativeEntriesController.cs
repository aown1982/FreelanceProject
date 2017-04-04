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
    public class UserNarrativeEntriesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserNarrativeEntries
        [Route("api/UserData/GetUserNarrativeEntries")]
        public IQueryable<tUserNarrativeEntry> GettUserNarrativeEntries()
        {
            return db.tUserNarrativeEntries;
        }

        // GET: api/UserNarrativeEntries/5
        [Route("api/UserData/GetUserNarrativeEntries/{id}")]
        [ResponseType(typeof(tUserNarrativeEntry))]
        public async Task<IHttpActionResult> GettUserNarrativeEntry(int id)
        {
            tUserNarrativeEntry tUserNarrativeEntry = await db.tUserNarrativeEntries.FindAsync(id);
            if (tUserNarrativeEntry == null)
            {
                return NotFound();
            }

            return Ok(tUserNarrativeEntry);
        }

        // PUT: api/UserNarrativeEntries/5
        [Route("api/UserData/UpdateUserNarrativeEntries/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserNarrativeEntry(int id, tUserNarrativeEntry tUserNarrativeEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserNarrativeEntry.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserNarrativeEntry).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserNarrativeEntryExists(id))
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

        // POST: api/UserNarrativeEntries
        [Route("api/UserData/PostUserNarrativeEntries")]
        [ResponseType(typeof(tUserNarrativeEntry))]
        public async Task<IHttpActionResult> PosttUserNarrativeEntry(tUserNarrativeEntry tUserNarrativeEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserNarrativeEntries.Add(tUserNarrativeEntry);
            await db.SaveChangesAsync();

            return Ok( tUserNarrativeEntry);
        }

        // DELETE: api/UserNarrativeEntries/5
        [Route("api/UserData/DeleteUserNarrativeEntries/{id}")]
        [ResponseType(typeof(tUserNarrativeEntry))]
        public async Task<IHttpActionResult> DeletetUserNarrativeEntry(int id)
        {
            tUserNarrativeEntry tUserNarrativeEntry = await db.tUserNarrativeEntries.FindAsync(id);
            if (tUserNarrativeEntry == null)
            {
                return NotFound();
            }

            db.tUserNarrativeEntries.Remove(tUserNarrativeEntry);
            await db.SaveChangesAsync();

            return Ok(tUserNarrativeEntry);
        }
        // DELETE: api/SoftDeleteUserNarrativeEntry/5/2
        [Route("api/UserData/SoftDeleteUserNarrativeEntry/{id}/{status}")]
        [ResponseType(typeof(tUserNarrativeEntry))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserNarrativeEntry(int id, int status)
        {
            tUserNarrativeEntry tUserNarrativeEntry = await db.tUserNarrativeEntries.FindAsync(id);
            if (tUserNarrativeEntry == null)
            {
                return NotFound();
            }
            tUserNarrativeEntry.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tUserNarrativeEntry);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserNarrativeEntryExists(int id)
        {
            return db.tUserNarrativeEntries.Count(e => e.ID == id) > 0;
        }
    }
}