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
    public class SystemStatusController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/SystemStatus
        [Route("api/UserData/GetSystemStatuses")]
        public IQueryable<tSystemStatus> GettSystemStatuses()
        {
            return db.tSystemStatuses;
        }

        // GET: api/SystemStatus/5
        [Route("api/UserData/GetSystemStatuses/{id}")]
        [ResponseType(typeof(tSystemStatus))]
        public async Task<IHttpActionResult> GettSystemStatus(int id)
        {
            tSystemStatus tSystemStatus = await db.tSystemStatuses.FindAsync(id);
            if (tSystemStatus == null)
            {
                return NotFound();
            }

            return Ok(tSystemStatus);
        }

        // PUT: api/SystemStatus/5
        [Route("api/UserData/UpdateSystemStatuses/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttSystemStatus(int id, tSystemStatus tSystemStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tSystemStatus.ID)
            {
                return BadRequest();
            }

            db.Entry(tSystemStatus).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tSystemStatusExists(id))
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

        // POST: api/SystemStatus
        [Route("api/UserData/PostSystemStatuses")]
        [ResponseType(typeof(tSystemStatus))]
        public async Task<IHttpActionResult> PosttSystemStatus(tSystemStatus tSystemStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tSystemStatuses.Add(tSystemStatus);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tSystemStatus.ID }, tSystemStatus);
        }

        // DELETE: api/SystemStatus/5
        [Route("api/UserData/DeleteSystemStatuses/{id}")]
        [ResponseType(typeof(tSystemStatus))]
        public async Task<IHttpActionResult> DeletetSystemStatus(int id)
        {
            tSystemStatus tSystemStatus = await db.tSystemStatuses.FindAsync(id);
            if (tSystemStatus == null)
            {
                return NotFound();
            }

            db.tSystemStatuses.Remove(tSystemStatus);
            await db.SaveChangesAsync();

            return Ok(tSystemStatus);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tSystemStatusExists(int id)
        {
            return db.tSystemStatuses.Count(e => e.ID == id) > 0;
        }
    }
}