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
    public class UserSourceServiceStatusController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserSourceServiceStatus
        [Route("api/UserData/GetUserSourceServiceStatuses")]
        public IQueryable<tUserSourceServiceStatus> GettUserSourceServiceStatuses()
        {
            return db.tUserSourceServiceStatuses;
        }

        // GET: api/UserSourceServiceStatus/5
        [Route("api/UserData/GetUserSourceServiceStatuses/{id}")]
        [ResponseType(typeof(tUserSourceServiceStatus))]
        public async Task<IHttpActionResult> GettUserSourceServiceStatus(int id)
        {
            tUserSourceServiceStatus tUserSourceServiceStatus = await db.tUserSourceServiceStatuses.FindAsync(id);
            if (tUserSourceServiceStatus == null)
            {
                return NotFound();
            }

            return Ok(tUserSourceServiceStatus);
        }

        // PUT: api/UserSourceServiceStatus/5
        [Route("api/UserData/UpdateUserSourceServiceStatuses/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserSourceServiceStatus(int id, tUserSourceServiceStatus tUserSourceServiceStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserSourceServiceStatus.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserSourceServiceStatus).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserSourceServiceStatusExists(id))
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

        // POST: api/UserSourceServiceStatus
        [Route("api/UserData/PostUserSourceServiceStatuses")]
        [ResponseType(typeof(tUserSourceServiceStatus))]
        public async Task<IHttpActionResult> PosttUserSourceServiceStatus(tUserSourceServiceStatus tUserSourceServiceStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserSourceServiceStatuses.Add(tUserSourceServiceStatus);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserSourceServiceStatus.ID }, tUserSourceServiceStatus);
        }

        // DELETE: api/UserSourceServiceStatus/5
        [Route("api/UserData/DeleteUserSourceServiceStatuses/{id}")]
        [ResponseType(typeof(tUserSourceServiceStatus))]
        public async Task<IHttpActionResult> DeletetUserSourceServiceStatus(int id)
        {
            tUserSourceServiceStatus tUserSourceServiceStatus = await db.tUserSourceServiceStatuses.FindAsync(id);
            if (tUserSourceServiceStatus == null)
            {
                return NotFound();
            }

            db.tUserSourceServiceStatuses.Remove(tUserSourceServiceStatus);
            await db.SaveChangesAsync();

            return Ok(tUserSourceServiceStatus);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserSourceServiceStatusExists(int id)
        {
            return db.tUserSourceServiceStatuses.Count(e => e.ID == id) > 0;
        }
    }
}