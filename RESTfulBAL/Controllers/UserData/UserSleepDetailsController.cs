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
    public class UserSleepDetailsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserSleepDetails
        [Route("api/UserData/GetUserSleepDetails")]
        public IQueryable<tUserSleepDetail> GettUserSleepDetails()
        {
            return db.tUserSleepDetails;
        }

        // GET: api/UserSleepDetails/5
        [Route("api/UserData/GetUserSleepDetails/{id}")]
        [ResponseType(typeof(tUserSleepDetail))]
        public async Task<IHttpActionResult> GettUserSleepDetail(int id)
        {
            tUserSleepDetail tUserSleepDetail = await db.tUserSleepDetails.FindAsync(id);
            if (tUserSleepDetail == null)
            {
                return NotFound();
            }

            return Ok(tUserSleepDetail);
        }

        // PUT: api/UserSleepDetails/5
        [Route("api/UserData/UpdateUserSleepDetails/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserSleepDetail(int id, tUserSleepDetail tUserSleepDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserSleepDetail.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserSleepDetail).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserSleepDetailExists(id))
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

        // POST: api/UserSleepDetails
        [Route("api/UserData/PostUserSleepDetails")]
        [ResponseType(typeof(tUserSleepDetail))]
        public async Task<IHttpActionResult> PosttUserSleepDetail(tUserSleepDetail tUserSleepDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserSleepDetails.Add(tUserSleepDetail);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserSleepDetail.ID }, tUserSleepDetail);
        }

        // DELETE: api/UserSleepDetails/5
        [Route("api/UserData/DeleteUserSleepDetails/{id}")]
        [ResponseType(typeof(tUserSleepDetail))]
        public async Task<IHttpActionResult> DeletetUserSleepDetail(int id)
        {
            tUserSleepDetail tUserSleepDetail = await db.tUserSleepDetails.FindAsync(id);
            if (tUserSleepDetail == null)
            {
                return NotFound();
            }

            db.tUserSleepDetails.Remove(tUserSleepDetail);
            await db.SaveChangesAsync();

            return Ok(tUserSleepDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserSleepDetailExists(int id)
        {
            return db.tUserSleepDetails.Count(e => e.ID == id) > 0;
        }
    }
}