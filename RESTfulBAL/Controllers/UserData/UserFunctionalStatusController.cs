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
    public class UserFunctionalStatusController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserFunctionalStatus
        [Route("api/UserData/GetUserFunctionalStatuses")]
        public IQueryable<tUserFunctionalStatus> GettUserFunctionalStatuses()
        {
            return db.tUserFunctionalStatuses;
        }

        // GET: api/UserFunctionalStatus/5
        [Route("api/UserData/GetUserFunctionalStatuses/{id}")]
        [ResponseType(typeof(tUserFunctionalStatus))]
        public async Task<IHttpActionResult> GettUserFunctionalStatus(int id)
        {
            tUserFunctionalStatus tUserFunctionalStatus = await db.tUserFunctionalStatuses.FindAsync(id);
            if (tUserFunctionalStatus == null)
            {
                return NotFound();
            }

            return Ok(tUserFunctionalStatus);
        }

        // PUT: api/UserFunctionalStatus/5
        [Route("api/UserData/UpdateUserFunctionalStatuses/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserFunctionalStatus(int id, tUserFunctionalStatus tUserFunctionalStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserFunctionalStatus.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserFunctionalStatus).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserFunctionalStatusExists(id))
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

        // POST: api/UserFunctionalStatus
        [Route("api/UserData/PostUserFunctionalStatuses")]
        [ResponseType(typeof(tUserFunctionalStatus))]
        public async Task<IHttpActionResult> PosttUserFunctionalStatus(tUserFunctionalStatus tUserFunctionalStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserFunctionalStatuses.Add(tUserFunctionalStatus);
            await db.SaveChangesAsync();

            return Ok(tUserFunctionalStatus);
        }

        // DELETE: api/UserFunctionalStatus/5
        [Route("api/UserData/DeleteUserFunctionalStatuses/{id}")]
        [ResponseType(typeof(tUserFunctionalStatus))]
        public async Task<IHttpActionResult> DeletetUserFunctionalStatus(int id)
        {
            tUserFunctionalStatus tUserFunctionalStatus = await db.tUserFunctionalStatuses.FindAsync(id);
            if (tUserFunctionalStatus == null)
            {
                return NotFound();
            }

            db.tUserFunctionalStatuses.Remove(tUserFunctionalStatus);
            await db.SaveChangesAsync();

            return Ok(tUserFunctionalStatus);
        }

        // DELETE: api/SoftDeleteUserFunctionalStatus/5/2
        [Route("api/UserData/SoftDeleteUserFunctionalStatus/{id}/{status}")]
        [ResponseType(typeof(tUserFunctionalStatus))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserFunctionalStatus(int id, int status)
        {
            tUserFunctionalStatus tUserFunctionalStatus = await db.tUserFunctionalStatuses.FindAsync(id);
            if (tUserFunctionalStatus == null)
            {
                return NotFound();
            }

            tUserFunctionalStatus.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tUserFunctionalStatus);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserFunctionalStatusExists(int id)
        {
            return db.tUserFunctionalStatuses.Count(e => e.ID == id) > 0;
        }
    }
}