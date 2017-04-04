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
    public class UserEventsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserEvents
        [Route("api/UserData/GetUserEvents")]
        public IQueryable<tUserEvent> GettUserEvents()
        {
            return db.tUserEvents;
        }

        // GET: api/UserEvents/5
        [Route("api/UserData/GetUserEvents/{id}")]
        [ResponseType(typeof(tUserEvent))]
        public async Task<IHttpActionResult> GettUserEvent(int id)
        {
            tUserEvent tUserEvent = await db.tUserEvents.FindAsync(id);
            if (tUserEvent == null)
            {
                return NotFound();
            }

            return Ok(tUserEvent);
        }

        // PUT: api/UserEvents/5
        [Route("api/UserData/UpdateUserEvents/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserEvent(int id, tUserEvent tUserEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserEvent.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserEvent).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserEventExists(id))
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

        // POST: api/UserEvents
        [Route("api/UserData/PostUserEvents")]
        [ResponseType(typeof(tUserEvent))]
        public async Task<IHttpActionResult> PosttUserEvent(tUserEvent tUserEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserEvents.Add(tUserEvent);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserEvent.ID }, tUserEvent);
        }

        // DELETE: api/UserEvents/5
        [Route("api/UserData/DeleteUserEvents/{id}")]
        [ResponseType(typeof(tUserEvent))]
        public async Task<IHttpActionResult> DeletetUserEvent(int id)
        {
            tUserEvent tUserEvent = await db.tUserEvents.FindAsync(id);
            if (tUserEvent == null)
            {
                return NotFound();
            }

            db.tUserEvents.Remove(tUserEvent);
            await db.SaveChangesAsync();

            return Ok(tUserEvent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserEventExists(int id)
        {
            return db.tUserEvents.Count(e => e.ID == id) > 0;
        }
    }
}