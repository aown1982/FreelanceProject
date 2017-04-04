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
    public class AllergySeveritiesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/AllergySeverities
        [Route("api/UserData/GetAllergySeverity")]
        public IQueryable<tAllergySeverity> GettAllergySeverities()
        {
            return db.tAllergySeverities
                        .Where(allergySev => allergySev.ParentID == null)
                        .OrderBy(allergySev => allergySev.Severity);
        }

        // GET: api/AllergySeverities/5
        [Route("api/UserData/GetAllergySeverity/{id}")]
        [ResponseType(typeof(tAllergySeverity))]
        public async Task<IHttpActionResult> GettAllergySeverity(int id)
        {
            tAllergySeverity tAllergySeverity = await db.tAllergySeverities.FindAsync(id);
            if (tAllergySeverity == null)
            {
                return NotFound();
            }

            return Ok(tAllergySeverity);
        }

        // PUT: api/AllergySeverities/5
        [Route("api/UserData/UpdateAllergySeverity/{id}/AllergySeverity")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttAllergySeverity(int id, tAllergySeverity AllergySeverity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != AllergySeverity.ID)
            {
                return BadRequest();
            }

            db.Entry(AllergySeverity).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tAllergySeverityExists(id))
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

        // POST: api/AllergySeverities
        [Route("api/UserData/PostAllergySeverity/AllergySeverity")]
        [ResponseType(typeof(tAllergySeverity))]
        public async Task<IHttpActionResult> PosttAllergySeverity(tAllergySeverity AllergySeverity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tAllergySeverities.Add(AllergySeverity);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = AllergySeverity.ID }, AllergySeverity);
        }

        // DELETE: api/AllergySeverities/5
        [Route("api/UserData/DeleteAllergySeverity/{id}")]
        [ResponseType(typeof(tAllergySeverity))]
        public async Task<IHttpActionResult> DeletetAllergySeverity(int id)
        {
            tAllergySeverity tAllergySeverity = await db.tAllergySeverities.FindAsync(id);
            if (tAllergySeverity == null)
            {
                return NotFound();
            }

            db.tAllergySeverities.Remove(tAllergySeverity);
            await db.SaveChangesAsync();

            return Ok(tAllergySeverity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tAllergySeverityExists(int id)
        {
            return db.tAllergySeverities.Count(e => e.ID == id) > 0;
        }
    }
}