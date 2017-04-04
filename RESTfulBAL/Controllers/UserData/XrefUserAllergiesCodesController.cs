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
    public class XrefUserAllergiesCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserAllergiesCodes
        [Route("api/UserData/GetXrefUserAllergiesCodes")]
        public IQueryable<tXrefUserAllergiesCode> GettXrefUserAllergiesCodes()
        {
            return db.tXrefUserAllergiesCodes;
        }

        // GET: api/XrefUserAllergiesCodes/5
        [Route("api/UserData/GetXrefUserAllergiesCodes/{id}")]
        [ResponseType(typeof(tXrefUserAllergiesCode))]
        public async Task<IHttpActionResult> GettXrefUserAllergiesCode(int id)
        {
            tXrefUserAllergiesCode tXrefUserAllergiesCode = await db.tXrefUserAllergiesCodes.FindAsync(id);
            if (tXrefUserAllergiesCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserAllergiesCode);
        }

        // PUT: api/XrefUserAllergiesCodes/5
        [Route("api/UserData/UpdateXrefUserAllergiesCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserAllergiesCode(int id, tXrefUserAllergiesCode tXrefUserAllergiesCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserAllergiesCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserAllergiesCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserAllergiesCodeExists(id))
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

        // POST: api/XrefUserAllergiesCodes
        [Route("api/UserData/PostXrefUserAllergiesCodes")]
        [ResponseType(typeof(tXrefUserAllergiesCode))]
        public async Task<IHttpActionResult> PosttXrefUserAllergiesCode(tXrefUserAllergiesCode tXrefUserAllergiesCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserAllergiesCodes.Add(tXrefUserAllergiesCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserAllergiesCode.ID }, tXrefUserAllergiesCode);
        }

        // DELETE: api/XrefUserAllergiesCodes/5
        [Route("api/UserData/DeleteXrefUserAllergiesCodes/{id}")]
        [ResponseType(typeof(tXrefUserAllergiesCode))]
        public async Task<IHttpActionResult> DeletetXrefUserAllergiesCode(int id)
        {
            tXrefUserAllergiesCode tXrefUserAllergiesCode = await db.tXrefUserAllergiesCodes.FindAsync(id);
            if (tXrefUserAllergiesCode == null)
            {
                return NotFound();
            }

            db.tXrefUserAllergiesCodes.Remove(tXrefUserAllergiesCode);
            await db.SaveChangesAsync();

            return Ok(tXrefUserAllergiesCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserAllergiesCodeExists(int id)
        {
            return db.tXrefUserAllergiesCodes.Count(e => e.ID == id) > 0;
        }
    }
}