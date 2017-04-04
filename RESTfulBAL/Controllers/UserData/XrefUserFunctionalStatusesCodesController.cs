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
    public class XrefUserFunctionalStatusesCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserFunctionalStatusesCodes
        [Route("api/UserData/GetXrefUserFunctionalStatusesCodes")]
        public IQueryable<tXrefUserFunctionalStatusesCode> GettXrefUserFunctionalStatusesCodes()
        {
            return db.tXrefUserFunctionalStatusesCodes;
        }

        // GET: api/XrefUserFunctionalStatusesCodes/5
        [Route("api/UserData/GetXrefUserFunctionalStatusesCodes/{id}")]    
        [ResponseType(typeof(tXrefUserFunctionalStatusesCode))]
        public async Task<IHttpActionResult> GettXrefUserFunctionalStatusesCode(int id)
        {
            tXrefUserFunctionalStatusesCode tXrefUserFunctionalStatusesCode = await db.tXrefUserFunctionalStatusesCodes.FindAsync(id);
            if (tXrefUserFunctionalStatusesCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserFunctionalStatusesCode);
        }

        // PUT: api/XrefUserFunctionalStatusesCodes/5
        [Route("api/UserData/UpdateXrefUserFunctionalStatusesCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserFunctionalStatusesCode(int id, tXrefUserFunctionalStatusesCode tXrefUserFunctionalStatusesCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserFunctionalStatusesCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserFunctionalStatusesCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserFunctionalStatusesCodeExists(id))
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

        // POST: api/XrefUserFunctionalStatusesCodes
        [Route("api/UserData/PostXrefUserFunctionalStatusesCodes")]
        [ResponseType(typeof(tXrefUserFunctionalStatusesCode))]
        public async Task<IHttpActionResult> PosttXrefUserFunctionalStatusesCode(tXrefUserFunctionalStatusesCode tXrefUserFunctionalStatusesCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserFunctionalStatusesCodes.Add(tXrefUserFunctionalStatusesCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserFunctionalStatusesCode.ID }, tXrefUserFunctionalStatusesCode);
        }

        // DELETE: api/XrefUserFunctionalStatusesCodes/5
        [Route("api/UserData/DeleteXrefUserFunctionalStatusesCodes/{id}")]
        [ResponseType(typeof(tXrefUserFunctionalStatusesCode))]
        public async Task<IHttpActionResult> DeletetXrefUserFunctionalStatusesCode(int id)
        {
            tXrefUserFunctionalStatusesCode tXrefUserFunctionalStatusesCode = await db.tXrefUserFunctionalStatusesCodes.FindAsync(id);
            if (tXrefUserFunctionalStatusesCode == null)
            {
                return NotFound();
            }

            db.tXrefUserFunctionalStatusesCodes.Remove(tXrefUserFunctionalStatusesCode);
            await db.SaveChangesAsync();

            return Ok(tXrefUserFunctionalStatusesCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserFunctionalStatusesCodeExists(int id)
        {
            return db.tXrefUserFunctionalStatusesCodes.Count(e => e.ID == id) > 0;
        }
    }
}