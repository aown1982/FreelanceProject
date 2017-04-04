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
    public class UserIDsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserIDs
        [Route("api/UserData/GetUserIDs")]
        public IQueryable<tUserID> GettUserIDs()
        {
            return db.tUserIDs;
        }

        // GET: api/UserIDs/5
        [Route("api/UserData/GetUserIDs/{id}")]
        [ResponseType(typeof(tUserID))]
        public async Task<IHttpActionResult> GettUserID(int id)
        {
            tUserID tUserID = await db.tUserIDs.FindAsync(id);
            if (tUserID == null)
            {
                return NotFound();
            }

            return Ok(tUserID);
        }

        // PUT: api/UserIDs/5
        [Route("api/UserData/UpdateUserIDs/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserID(int id, tUserID tUserID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserID.UserID)
            {
                return BadRequest();
            }

            db.Entry(tUserID).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserIDExists(id))
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

        // POST: api/UserIDs
        [Route("api/UserData/PostUserID")]
        [ResponseType(typeof(tUserID))]
        public async Task<IHttpActionResult> PosttUserID(tUserID tUserID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserIDs.Add(tUserID);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tUserIDExists(tUserID.UserID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tUserID.UserID }, tUserID);
        }

        // DELETE: api/UserIDs/5
        [Route("api/UserData/DeleteUserIDs/{id}")]
        [ResponseType(typeof(tUserID))]
        public async Task<IHttpActionResult> DeletetUserID(int id)
        {
            tUserID tUserID = await db.tUserIDs.FindAsync(id);
            if (tUserID == null)
            {
                return NotFound();
            }

            db.tUserIDs.Remove(tUserID);
            await db.SaveChangesAsync();

            return Ok(tUserID);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserIDExists(int id)
        {
            return db.tUserIDs.Count(e => e.UserID == id) > 0;
        }
    }
}