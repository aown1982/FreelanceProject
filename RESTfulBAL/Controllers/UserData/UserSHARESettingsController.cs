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
    public class UserSHARESettingsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        /*// GET: api/UserSHARESettings
        [Route("api/UserData/GetUserSHARESettings")]
        public IQueryable<tUserSHARESetting> GettUserSHARESettings()
        {
            return db.tUserSHARESettings;
        }*/

        // GET: api/UserSHARESettings/5
        [Route("api/UserData/GetUserSHARESettings/{id}")]
        [ResponseType(typeof(tUserSHARESetting))]
        public async Task<IHttpActionResult> GettUserSHARESetting(int id)
        {
            tUserSHARESetting tUserSHARESetting = await db.tUserSHARESettings.FindAsync(id);
            if (tUserSHARESetting == null)
            {
                return NotFound();
            }

            return Ok(tUserSHARESetting);
        }

        //GET: api/GetUserSHAREAllowsByUserID/5
        [Route("api/UserData/GetUserSHAREAllowsByUserID/{userID}")]
        [ResponseType(typeof(tUserSHARESetting))]
        public async Task<IHttpActionResult> GetUserSHAREAllowsByUserID(int userID)
        {
            var tUserSHARESetting = db.spGetUserSHARESettings(userID, 1);
            if (tUserSHARESetting == null)
            {
                return NotFound();
            }

            return Ok(tUserSHARESetting);
        }


        // GET: api/GetUserSHAREDeniesByUserID/5
        [Route("api/UserData/GetUserSHAREDeniesByUserID/{userID}")]
        [ResponseType(typeof(tUserSHARESetting))]
        public async Task<IHttpActionResult> GetUserSHAREDeniesByUserID(int userID)
        {
            var tUserSHARESetting = db.spGetUserSHARESettings(userID, 2);
            if (tUserSHARESetting == null)
            {
                return NotFound();
            }

            return Ok(tUserSHARESetting);
        }
        
        // PUT: api/UserSHARESettings/5
        [Route("api/UserData/UpdateUserSHARESettings/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserSHARESetting(int id, tUserSHARESetting tUserSHARESetting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserSHARESetting.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserSHARESetting).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserSHARESettingExists(id))
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

        // POST: api/UserSHARESettings
        [Route("api/UserData/PostUserSHARESettings")]
        [ResponseType(typeof(tUserSHARESetting))]
        public async Task<IHttpActionResult> PosttUserSHARESetting(tUserSHARESetting tUserSHARESetting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserSHARESettings.Add(tUserSHARESetting);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserSHARESetting.ID }, tUserSHARESetting);
        }

        /*// DELETE: api/UserSHARESettings/5
        [Route("api/UserData/DeleteUserSHARESettings/{id}")]
        [ResponseType(typeof(tUserSHARESetting))]
        public async Task<IHttpActionResult> DeletetUserSHARESetting(int id)
        {
            tUserSHARESetting tUserSHARESetting = await db.tUserSHARESettings.FindAsync(id);
            if (tUserSHARESetting == null)
            {
                return NotFound();
            }

            db.tUserSHARESettings.Remove(tUserSHARESetting);
            await db.SaveChangesAsync();

            return Ok(tUserSHARESetting);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserSHARESettingExists(int id)
        {
            return db.tUserSHARESettings.Count(e => e.ID == id) > 0;
        }
    }
}