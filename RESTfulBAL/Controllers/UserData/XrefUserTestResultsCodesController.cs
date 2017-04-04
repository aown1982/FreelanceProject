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
    public class XrefUserTestResultsCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserTestResultsCodes
        [Route("api/UserData/GetXrefUserTestResultsCodes")]
        public IQueryable<tXrefUserTestResultsCode> GettXrefUserTestResultsCodes()
        {
            return db.tXrefUserTestResultsCodes;
        }

        // GET: api/XrefUserTestResultsCodes/5
        [Route("api/UserData/GetXrefUserTestResultsCodes/{id}")]
        [ResponseType(typeof(tXrefUserTestResultsCode))]
        public async Task<IHttpActionResult> GettXrefUserTestResultsCode(int id)
        {
            tXrefUserTestResultsCode tXrefUserTestResultsCode = await db.tXrefUserTestResultsCodes.FindAsync(id);
            if (tXrefUserTestResultsCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserTestResultsCode);
        }

        // PUT: api/XrefUserTestResultsCodes/5
        [Route("api/UserData/UpdateXrefUserTestResultsCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserTestResultsCode(int id, tXrefUserTestResultsCode tXrefUserTestResultsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserTestResultsCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserTestResultsCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserTestResultsCodeExists(id))
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

        // POST: api/XrefUserTestResultsCodes
        [Route("api/UserData/PostXrefUserTestResultsCodes")]
        [ResponseType(typeof(tXrefUserTestResultsCode))]
        public async Task<IHttpActionResult> PosttXrefUserTestResultsCode(tXrefUserTestResultsCode tXrefUserTestResultsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserTestResultsCodes.Add(tXrefUserTestResultsCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserTestResultsCode.ID }, tXrefUserTestResultsCode);
        }

        // DELETE: api/XrefUserTestResultsCodes/5
        [Route("api/UserData/DeleteXrefUserTestResultsCodes/{id}")]
        [ResponseType(typeof(tXrefUserTestResultsCode))]
        public async Task<IHttpActionResult> DeletetXrefUserTestResultsCode(int id)
        {
            tXrefUserTestResultsCode tXrefUserTestResultsCode = await db.tXrefUserTestResultsCodes.FindAsync(id);
            if (tXrefUserTestResultsCode == null)
            {
                return NotFound();
            }

            db.tXrefUserTestResultsCodes.Remove(tXrefUserTestResultsCode);
            await db.SaveChangesAsync();

            return Ok(tXrefUserTestResultsCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserTestResultsCodeExists(int id)
        {
            return db.tXrefUserTestResultsCodes.Count(e => e.ID == id) > 0;
        }
    }
}