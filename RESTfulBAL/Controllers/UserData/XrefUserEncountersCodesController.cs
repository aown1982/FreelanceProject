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
    public class XrefUserEncountersCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserEncountersCodes
        [Route("api/UserData/GetXrefUserEncountersCodes")]
        public IQueryable<tXrefUserEncountersCode> GettXrefUserEncountersCodes()
        {
            return db.tXrefUserEncountersCodes;
        }

        // GET: api/XrefUserEncountersCodes/5
        [Route("api/UserData/GetXrefUserEncountersCodes/{id}")]
        [ResponseType(typeof(tXrefUserEncountersCode))]
        public async Task<IHttpActionResult> GettXrefUserEncountersCode(int id)
        {
            tXrefUserEncountersCode tXrefUserEncountersCode = await db.tXrefUserEncountersCodes.FindAsync(id);
            if (tXrefUserEncountersCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserEncountersCode);
        }

        // PUT: api/XrefUserEncountersCodes/5
        [Route("api/UserData/UpdateXrefUserEncountersCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserEncountersCode(int id, tXrefUserEncountersCode tXrefUserEncountersCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserEncountersCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserEncountersCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserEncountersCodeExists(id))
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

        // POST: api/XrefUserEncountersCodes
        [Route("api/UserData/PostXrefUserEncountersCodes")]
        [ResponseType(typeof(tXrefUserEncountersCode))]
        public async Task<IHttpActionResult> PosttXrefUserEncountersCode(tXrefUserEncountersCode tXrefUserEncountersCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserEncountersCodes.Add(tXrefUserEncountersCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserEncountersCode.ID }, tXrefUserEncountersCode);
        }

        // DELETE: api/XrefUserEncountersCodes/5
        [Route("api/UserData/DeleteXrefUserEncountersCodes/{id}")]
        [ResponseType(typeof(tXrefUserEncountersCode))]
        public async Task<IHttpActionResult> DeletetXrefUserEncountersCode(int id)
        {
            tXrefUserEncountersCode tXrefUserEncountersCode = await db.tXrefUserEncountersCodes.FindAsync(id);
            if (tXrefUserEncountersCode == null)
            {
                return NotFound();
            }

            db.tXrefUserEncountersCodes.Remove(tXrefUserEncountersCode);
            await db.SaveChangesAsync();

            return Ok(tXrefUserEncountersCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserEncountersCodeExists(int id)
        {
            return db.tXrefUserEncountersCodes.Count(e => e.ID == id) > 0;
        }
    }
}