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
    public class XrefAllergensCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefAllergensCodes
        [Route("api/UserData/GetXrefAllergensCodes")]
        public IQueryable<tXrefAllergensCode> GettXrefAllergensCodes()
        {
            return db.tXrefAllergensCodes;
        }

        // GET: api/XrefAllergensCodes/5
        [Route("api/UserData/GetXrefAllergensCodes/{id}")]
        [ResponseType(typeof(tXrefAllergensCode))]
        public async Task<IHttpActionResult> GettXrefAllergensCode(int id)
        {
            tXrefAllergensCode tXrefAllergensCode = await db.tXrefAllergensCodes.FindAsync(id);
            if (tXrefAllergensCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefAllergensCode);
        }

        // PUT: api/XrefAllergensCodes/5
        [Route("api/UserData/UpdateXrefAllergensCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefAllergensCode(int id, tXrefAllergensCode tXrefAllergensCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefAllergensCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefAllergensCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefAllergensCodeExists(id))
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

        // POST: api/XrefAllergensCodes
        [Route("api/UserData/PostXrefAllergensCodes")]
        [ResponseType(typeof(tXrefAllergensCode))]
        public async Task<IHttpActionResult> PosttXrefAllergensCode(tXrefAllergensCode tXrefAllergensCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefAllergensCodes.Add(tXrefAllergensCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefAllergensCode.ID }, tXrefAllergensCode);
        }

        // DELETE: api/XrefAllergensCodes/5
        [Route("api/UserData/DeleteXrefAllergensCodes/{id}")]
        [ResponseType(typeof(tXrefAllergensCode))]
        public async Task<IHttpActionResult> DeletetXrefAllergensCode(int id)
        {
            tXrefAllergensCode tXrefAllergensCode = await db.tXrefAllergensCodes.FindAsync(id);
            if (tXrefAllergensCode == null)
            {
                return NotFound();
            }

            db.tXrefAllergensCodes.Remove(tXrefAllergensCode);
            await db.SaveChangesAsync();

            return Ok(tXrefAllergensCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefAllergensCodeExists(int id)
        {
            return db.tXrefAllergensCodes.Count(e => e.ID == id) > 0;
        }
    }
}