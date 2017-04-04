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
    public class XrefUserEncountersPrescriptionsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserEncountersPrescriptions
        [Route("api/UserData/GetXrefUserEncountersPrescriptions")]
        public IQueryable<tXrefUserEncountersPrescription> GettXrefUserEncountersPrescriptions()
        {
            return db.tXrefUserEncountersPrescriptions;
        }

        // GET: api/XrefUserEncountersPrescriptions/5
        [Route("api/UserData/GetXrefUserEncountersPrescriptions/{id}")]
        [ResponseType(typeof(tXrefUserEncountersPrescription))]
        public async Task<IHttpActionResult> GettXrefUserEncountersPrescription(int id)
        {
            tXrefUserEncountersPrescription tXrefUserEncountersPrescription = await db.tXrefUserEncountersPrescriptions.FindAsync(id);
            if (tXrefUserEncountersPrescription == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserEncountersPrescription);
        }

        // PUT: api/XrefUserEncountersPrescriptions/5
        [Route("api/UserData/UpdateXrefUserEncountersPrescriptions/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserEncountersPrescription(int id, tXrefUserEncountersPrescription tXrefUserEncountersPrescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserEncountersPrescription.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserEncountersPrescription).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserEncountersPrescriptionExists(id))
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

        // POST: api/XrefUserEncountersPrescriptions
        [Route("api/UserData/PostXrefUserEncountersPrescriptions")]
        [ResponseType(typeof(tXrefUserEncountersPrescription))]
        public async Task<IHttpActionResult> PosttXrefUserEncountersPrescription(tXrefUserEncountersPrescription tXrefUserEncountersPrescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserEncountersPrescriptions.Add(tXrefUserEncountersPrescription);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserEncountersPrescription.ID }, tXrefUserEncountersPrescription);
        }

        // DELETE: api/XrefUserEncountersPrescriptions/5
        [Route("api/UserData/DeleteXrefUserEncountersPrescriptions/{id}")]
        [ResponseType(typeof(tXrefUserEncountersPrescription))]
        public async Task<IHttpActionResult> DeletetXrefUserEncountersPrescription(int id)
        {
            tXrefUserEncountersPrescription tXrefUserEncountersPrescription = await db.tXrefUserEncountersPrescriptions.FindAsync(id);
            if (tXrefUserEncountersPrescription == null)
            {
                return NotFound();
            }

            db.tXrefUserEncountersPrescriptions.Remove(tXrefUserEncountersPrescription);
            await db.SaveChangesAsync();

            return Ok(tXrefUserEncountersPrescription);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserEncountersPrescriptionExists(int id)
        {
            return db.tXrefUserEncountersPrescriptions.Count(e => e.ID == id) > 0;
        }
    }
}