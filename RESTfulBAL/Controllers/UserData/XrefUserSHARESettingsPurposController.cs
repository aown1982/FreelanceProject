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
    public class XrefUserSHARESettingsPurposController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserSHARESettingsPurpos
        [Route("api/UserData/GetXrefUserSHARESettingsPurposes")]
        public IQueryable<tXrefUserSHARESettingsPurpos> GettXrefUserSHARESettingsPurposes()
        {
            return db.tXrefUserSHARESettingsPurposes;
        }

        // GET: api/XrefUserSHARESettingsPurpos/5
        [Route("api/UserData/GetXrefUserSHARESettingsPurposes/{id}")]
        [ResponseType(typeof(tXrefUserSHARESettingsPurpos))]
        public async Task<IHttpActionResult> GettXrefUserSHARESettingsPurpos(int id)
        {
            tXrefUserSHARESettingsPurpos tXrefUserSHARESettingsPurpos = await db.tXrefUserSHARESettingsPurposes.FindAsync(id);
            if (tXrefUserSHARESettingsPurpos == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserSHARESettingsPurpos);
        }

        // PUT: api/XrefUserSHARESettingsPurpos/5
        [Route("api/UserData/UpdateXrefUserSHARESettingsPurposes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserSHARESettingsPurpos(int id, tXrefUserSHARESettingsPurpos tXrefUserSHARESettingsPurpos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserSHARESettingsPurpos.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserSHARESettingsPurpos).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserSHARESettingsPurposExists(id))
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

        // POST: api/XrefUserSHARESettingsPurpos
        [Route("api/UserData/PostXrefUserSHARESettingsPurposes")]
        [ResponseType(typeof(tXrefUserSHARESettingsPurpos))]
        public async Task<IHttpActionResult> PosttXrefUserSHARESettingsPurpos(tXrefUserSHARESettingsPurpos tXrefUserSHARESettingsPurpos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserSHARESettingsPurposes.Add(tXrefUserSHARESettingsPurpos);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserSHARESettingsPurpos.ID }, tXrefUserSHARESettingsPurpos);
        }

        // DELETE: api/XrefUserSHARESettingsPurpos/5
        [Route("api/UserData/DeleteXrefUserSHARESettingsPurposes/{id}")]
        [ResponseType(typeof(tXrefUserSHARESettingsPurpos))]
        public async Task<IHttpActionResult> DeletetXrefUserSHARESettingsPurpos(int id)
        {
            tXrefUserSHARESettingsPurpos tXrefUserSHARESettingsPurpos = await db.tXrefUserSHARESettingsPurposes.FindAsync(id);
            if (tXrefUserSHARESettingsPurpos == null)
            {
                return NotFound();
            }

            db.tXrefUserSHARESettingsPurposes.Remove(tXrefUserSHARESettingsPurpos);
            await db.SaveChangesAsync();

            return Ok(tXrefUserSHARESettingsPurpos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserSHARESettingsPurposExists(int id)
        {
            return db.tXrefUserSHARESettingsPurposes.Count(e => e.ID == id) > 0;
        }
    }
}