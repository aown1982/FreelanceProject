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
    public class SHARESettingsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/SHARESettings
        [Route("api/UserData/GetSHARESettings")]
        public IQueryable<tSHARESetting> GettSHARESettings()
        {
            return db.tSHARESettings;
        }

        // GET: api/SHARESettings/5
        [Route("api/UserData/GetSHARESettings/{id}")]
        [ResponseType(typeof(tSHARESetting))]
        public async Task<IHttpActionResult> GettSHARESetting(int id)
        {
            tSHARESetting tSHARESetting = await db.tSHARESettings.FindAsync(id);
            if (tSHARESetting == null)
            {
                return NotFound();
            }

            return Ok(tSHARESetting);
        }

        // PUT: api/SHARESettings/5
        [Route("api/UserData/UpdateSHARESettings/{id}/SHARESetting")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttSHARESetting(int id, tSHARESetting SHARESetting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != SHARESetting.ID)
            {
                return BadRequest();
            }

            db.Entry(SHARESetting).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tSHARESettingExists(id))
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

        // POST: api/SHARESettings
        [Route("api/UserData/PostSHARESettings/SHARESetting")]
        [ResponseType(typeof(tSHARESetting))]
        public async Task<IHttpActionResult> PosttSHARESetting(tSHARESetting SHARESetting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tSHARESettings.Add(SHARESetting);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = SHARESetting.ID }, SHARESetting);
        }

        // DELETE: api/SHARESettings/5
        [Route("api/UserData/DeleteSHARESettings/{id}")]
        [ResponseType(typeof(tSHARESetting))]
        public async Task<IHttpActionResult> DeletetSHARESetting(int id)
        {
            tSHARESetting tSHARESetting = await db.tSHARESettings.FindAsync(id);
            if (tSHARESetting == null)
            {
                return NotFound();
            }

            db.tSHARESettings.Remove(tSHARESetting);
            await db.SaveChangesAsync();

            return Ok(tSHARESetting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tSHARESettingExists(int id)
        {
            return db.tSHARESettings.Count(e => e.ID == id) > 0;
        }
    }
}