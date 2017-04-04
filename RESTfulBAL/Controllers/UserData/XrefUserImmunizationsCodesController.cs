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
    public class XrefUserImmunizationsCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserImmunizationsCodes
        [Route("api/UserData/GetXrefUserImmunizationsCodes")]
        public IQueryable<tXrefUserImmunizationsCode> GettXrefUserImmunizationsCodes()
        {
            return db.tXrefUserImmunizationsCodes;
        }

        // GET: api/XrefUserImmunizationsCodes/5
        [Route("api/UserData/GetXrefUserImmunizationsCodes/{id}")]
        [ResponseType(typeof(tXrefUserImmunizationsCode))]
        public async Task<IHttpActionResult> GettXrefUserImmunizationsCode(int id)
        {
            tXrefUserImmunizationsCode tXrefUserImmunizationsCode = await db.tXrefUserImmunizationsCodes.FindAsync(id);
            if (tXrefUserImmunizationsCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserImmunizationsCode);
        }

        // PUT: api/XrefUserImmunizationsCodes/5
        [Route("api/UserData/UpdateXrefUserImmunizationsCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserImmunizationsCode(int id, tXrefUserImmunizationsCode tXrefUserImmunizationsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserImmunizationsCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserImmunizationsCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserImmunizationsCodeExists(id))
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

        // POST: api/XrefUserImmunizationsCodes
        [Route("api/UserData/PostXrefUserImmunizationsCodes")]
        [ResponseType(typeof(tXrefUserImmunizationsCode))]
        public async Task<IHttpActionResult> PosttXrefUserImmunizationsCode(tXrefUserImmunizationsCode tXrefUserImmunizationsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserImmunizationsCodes.Add(tXrefUserImmunizationsCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserImmunizationsCode.ID }, tXrefUserImmunizationsCode);
        }

        // DELETE: api/XrefUserImmunizationsCodes/5
        [Route("api/UserData/DeleteXrefUserImmunizationsCodes/{id}")]
        [ResponseType(typeof(tXrefUserImmunizationsCode))]
        public async Task<IHttpActionResult> DeletetXrefUserImmunizationsCode(int id)
        {
            tXrefUserImmunizationsCode tXrefUserImmunizationsCode = await db.tXrefUserImmunizationsCodes.FindAsync(id);
            if (tXrefUserImmunizationsCode == null)
            {
                return NotFound();
            }

            db.tXrefUserImmunizationsCodes.Remove(tXrefUserImmunizationsCode);
            await db.SaveChangesAsync();

            return Ok(tXrefUserImmunizationsCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserImmunizationsCodeExists(int id)
        {
            return db.tXrefUserImmunizationsCodes.Count(e => e.ID == id) > 0;
        }
    }
}