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
    public class XrefUserSNPProbesSNPAvailabilitiesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserSNPProbesSNPAvailabilities
        [Route("api/UserData/GetXrefUserSNPProbesSNPAvailabilities")]
        public IQueryable<tXrefUserSNPProbesSNPAvailability> GettXrefUserSNPProbesSNPAvailabilities()
        {
            return db.tXrefUserSNPProbesSNPAvailabilities;
        }

        // GET: api/XrefUserSNPProbesSNPAvailabilities/5
        [Route("api/UserData/GetXrefUserSNPProbesSNPAvailabilities/{id}")]
        [ResponseType(typeof(tXrefUserSNPProbesSNPAvailability))]
        public async Task<IHttpActionResult> GettXrefUserSNPProbesSNPAvailability(int id)
        {
            tXrefUserSNPProbesSNPAvailability tXrefUserSNPProbesSNPAvailability = await db.tXrefUserSNPProbesSNPAvailabilities.FindAsync(id);
            if (tXrefUserSNPProbesSNPAvailability == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserSNPProbesSNPAvailability);
        }

        // PUT: api/XrefUserSNPProbesSNPAvailabilities/5
        [Route("api/UserData/UpdateXrefUserSNPProbesSNPAvailabilities/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserSNPProbesSNPAvailability(int id, tXrefUserSNPProbesSNPAvailability tXrefUserSNPProbesSNPAvailability)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserSNPProbesSNPAvailability.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserSNPProbesSNPAvailability).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserSNPProbesSNPAvailabilityExists(id))
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

        // POST: api/XrefUserSNPProbesSNPAvailabilities
        [Route("api/UserData/PostXrefUserSNPProbesSNPAvailabilities")]
        [ResponseType(typeof(tXrefUserSNPProbesSNPAvailability))]
        public async Task<IHttpActionResult> PosttXrefUserSNPProbesSNPAvailability(tXrefUserSNPProbesSNPAvailability tXrefUserSNPProbesSNPAvailability)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserSNPProbesSNPAvailabilities.Add(tXrefUserSNPProbesSNPAvailability);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserSNPProbesSNPAvailability.ID }, tXrefUserSNPProbesSNPAvailability);
        }

        // DELETE: api/XrefUserSNPProbesSNPAvailabilities/5
        [Route("api/UserData/DeleteXrefUserSNPProbesSNPAvailabilities/{id}")]
        [ResponseType(typeof(tXrefUserSNPProbesSNPAvailability))]
        public async Task<IHttpActionResult> DeletetXrefUserSNPProbesSNPAvailability(int id)
        {
            tXrefUserSNPProbesSNPAvailability tXrefUserSNPProbesSNPAvailability = await db.tXrefUserSNPProbesSNPAvailabilities.FindAsync(id);
            if (tXrefUserSNPProbesSNPAvailability == null)
            {
                return NotFound();
            }

            db.tXrefUserSNPProbesSNPAvailabilities.Remove(tXrefUserSNPProbesSNPAvailability);
            await db.SaveChangesAsync();

            return Ok(tXrefUserSNPProbesSNPAvailability);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserSNPProbesSNPAvailabilityExists(int id)
        {
            return db.tXrefUserSNPProbesSNPAvailabilities.Count(e => e.ID == id) > 0;
        }
    }
}