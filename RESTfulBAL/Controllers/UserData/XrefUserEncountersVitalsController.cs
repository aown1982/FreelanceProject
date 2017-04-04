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
    public class XrefUserEncountersVitalsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserEncountersVitals
        [Route("api/UserData/GetXrefUserEncountersVitals")]
        public IQueryable<tXrefUserEncountersVital> GettXrefUserEncountersVitals()
        {
            return db.tXrefUserEncountersVitals;
        }

        // GET: api/XrefUserEncountersVitals/5
        [Route("api/UserData/GetXrefUserEncountersVitals/{id}")]
        [ResponseType(typeof(tXrefUserEncountersVital))]
        public async Task<IHttpActionResult> GettXrefUserEncountersVital(int id)
        {
            tXrefUserEncountersVital tXrefUserEncountersVital = await db.tXrefUserEncountersVitals.FindAsync(id);
            if (tXrefUserEncountersVital == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserEncountersVital);
        }

        // PUT: api/XrefUserEncountersVitals/5
        [Route("api/UserData/UpdateXrefUserEncountersVitals/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserEncountersVital(int id, tXrefUserEncountersVital tXrefUserEncountersVital)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserEncountersVital.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserEncountersVital).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserEncountersVitalExists(id))
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

        // POST: api/XrefUserEncountersVitals
        [Route("api/UserData/PostXrefUserEncountersVitals")]
        [ResponseType(typeof(tXrefUserEncountersVital))]
        public async Task<IHttpActionResult> PosttXrefUserEncountersVital(tXrefUserEncountersVital tXrefUserEncountersVital)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserEncountersVitals.Add(tXrefUserEncountersVital);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserEncountersVital.ID }, tXrefUserEncountersVital);
        }

        // DELETE: api/XrefUserEncountersVitals/5
        [Route("api/UserData/DeleteXrefUserEncountersVitals/{id}")]
        [ResponseType(typeof(tXrefUserEncountersVital))]
        public async Task<IHttpActionResult> DeletetXrefUserEncountersVital(int id)
        {
            tXrefUserEncountersVital tXrefUserEncountersVital = await db.tXrefUserEncountersVitals.FindAsync(id);
            if (tXrefUserEncountersVital == null)
            {
                return NotFound();
            }

            db.tXrefUserEncountersVitals.Remove(tXrefUserEncountersVital);
            await db.SaveChangesAsync();

            return Ok(tXrefUserEncountersVital);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserEncountersVitalExists(int id)
        {
            return db.tXrefUserEncountersVitals.Count(e => e.ID == id) > 0;
        }
    }
}