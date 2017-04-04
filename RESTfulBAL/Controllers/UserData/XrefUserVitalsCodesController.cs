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
    public class XrefUserVitalsCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserVitalsCodes
        [Route("api/UserData/GetXrefUserVitalsCodes")]
        public IQueryable<tXrefUserVitalsCode> GettXrefUserVitalsCodes()
        {
            return db.tXrefUserVitalsCodes;
        }

        // GET: api/XrefUserVitalsCodes/5
        [Route("api/UserData/GetXrefUserVitalsCodes/{id}")]
        [ResponseType(typeof(tXrefUserVitalsCode))]
        public async Task<IHttpActionResult> GettXrefUserVitalsCode(int id)
        {
            tXrefUserVitalsCode tXrefUserVitalsCode = await db.tXrefUserVitalsCodes.FindAsync(id);
            if (tXrefUserVitalsCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserVitalsCode);
        }

        // PUT: api/XrefUserVitalsCodes/5
        [Route("api/UserData/UpdateXrefUserVitalsCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserVitalsCode(int id, tXrefUserVitalsCode tXrefUserVitalsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserVitalsCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserVitalsCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserVitalsCodeExists(id))
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

        // POST: api/XrefUserVitalsCodes
        [Route("api/UserData/PostXrefUserVitalsCodes")]
        [ResponseType(typeof(tXrefUserVitalsCode))]
        public async Task<IHttpActionResult> PosttXrefUserVitalsCode(tXrefUserVitalsCode tXrefUserVitalsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserVitalsCodes.Add(tXrefUserVitalsCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserVitalsCode.ID }, tXrefUserVitalsCode);
        }

        // DELETE: api/XrefUserVitalsCodes/5
        [Route("api/UserData/DeleteXrefUserVitalsCodes/{id}")]
        [ResponseType(typeof(tXrefUserVitalsCode))]
        public async Task<IHttpActionResult> DeletetXrefUserVitalsCode(int id)
        {
            tXrefUserVitalsCode tXrefUserVitalsCode = await db.tXrefUserVitalsCodes.FindAsync(id);
            if (tXrefUserVitalsCode == null)
            {
                return NotFound();
            }

            db.tXrefUserVitalsCodes.Remove(tXrefUserVitalsCode);
            await db.SaveChangesAsync();

            return Ok(tXrefUserVitalsCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserVitalsCodeExists(int id)
        {
            return db.tXrefUserVitalsCodes.Count(e => e.ID == id) > 0;
        }
    }
}