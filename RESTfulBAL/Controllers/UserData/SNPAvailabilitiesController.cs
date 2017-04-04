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
    public class SNPAvailabilitiesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/SNPAvailabilities
        [Route("api/UserData/GetSNPAvailabilities")]
        public IQueryable<tSNPAvailability> GettSNPAvailabilities()
        {
            return db.tSNPAvailabilities;
        }

        // GET: api/SNPAvailabilities/5
        [Route("api/UserData/GetSNPAvailabilities/{id}")]
        [ResponseType(typeof(tSNPAvailability))]
        public async Task<IHttpActionResult> GettSNPAvailability(int id)
        {
            tSNPAvailability tSNPAvailability = await db.tSNPAvailabilities.FindAsync(id);
            if (tSNPAvailability == null)
            {
                return NotFound();
            }

            return Ok(tSNPAvailability);
        }

        // PUT: api/SNPAvailabilities/5
        [Route("api/UserData/UpdateSNPAvailabilities/{id}/SNPAvailability")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttSNPAvailability(int id, tSNPAvailability SNPAvailability)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != SNPAvailability.ID)
            {
                return BadRequest();
            }

            db.Entry(SNPAvailability).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tSNPAvailabilityExists(id))
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

        // POST: api/SNPAvailabilities
        [Route("api/UserData/PostSNPAvailabilities/SNPAvailability")]
        [ResponseType(typeof(tSNPAvailability))]
        public async Task<IHttpActionResult> PosttSNPAvailability(tSNPAvailability SNPAvailability)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tSNPAvailabilities.Add(SNPAvailability);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = SNPAvailability.ID }, SNPAvailability);
        }

        // DELETE: api/SNPAvailabilities/5
        [Route("api/UserData/DeleteSNPAvailabilities/{id}")]
        [ResponseType(typeof(tSNPAvailability))]
        public async Task<IHttpActionResult> DeletetSNPAvailability(int id)
        {
            tSNPAvailability tSNPAvailability = await db.tSNPAvailabilities.FindAsync(id);
            if (tSNPAvailability == null)
            {
                return NotFound();
            }

            db.tSNPAvailabilities.Remove(tSNPAvailability);
            await db.SaveChangesAsync();

            return Ok(tSNPAvailability);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tSNPAvailabilityExists(int id)
        {
            return db.tSNPAvailabilities.Count(e => e.ID == id) > 0;
        }
    }
}