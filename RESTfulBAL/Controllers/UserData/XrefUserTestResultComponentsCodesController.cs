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
    public class XrefUserTestResultComponentsCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserTestResultComponentsCodes
        [Route("api/UserData/GetXrefUserTestResultComponentsCodes")]
        public IQueryable<tXrefUserTestResultComponentsCode> GettXrefUserTestResultComponentsCodes()
        {
            return db.tXrefUserTestResultComponentsCodes;
        }

        // GET: api/XrefUserTestResultComponentsCodes/5
        [Route("api/UserData/GetXrefUserTestResultComponentsCodes/{id}")]
        [ResponseType(typeof(tXrefUserTestResultComponentsCode))]
        public async Task<IHttpActionResult> GettXrefUserTestResultComponentsCode(int id)
        {
            tXrefUserTestResultComponentsCode tXrefUserTestResultComponentsCode = await db.tXrefUserTestResultComponentsCodes.FindAsync(id);
            if (tXrefUserTestResultComponentsCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserTestResultComponentsCode);
        }

        // PUT: api/XrefUserTestResultComponentsCodes/5
        [Route("api/UserData/UpdateXrefUserTestResultComponentsCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserTestResultComponentsCode(int id, tXrefUserTestResultComponentsCode tXrefUserTestResultComponentsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserTestResultComponentsCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserTestResultComponentsCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserTestResultComponentsCodeExists(id))
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

        // POST: api/XrefUserTestResultComponentsCodes
        [Route("api/UserData/PostXrefUserTestResultComponentsCodes")]
        [ResponseType(typeof(tXrefUserTestResultComponentsCode))]
        public async Task<IHttpActionResult> PosttXrefUserTestResultComponentsCode(tXrefUserTestResultComponentsCode tXrefUserTestResultComponentsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserTestResultComponentsCodes.Add(tXrefUserTestResultComponentsCode);
            await db.SaveChangesAsync();

            //   return CreatedAtRoute("DefaultApi", new { id = tXrefUserTestResultComponentsCode.ID }, tXrefUserTestResultComponentsCode);
            return Ok(tXrefUserTestResultComponentsCode);
        }

        // DELETE: api/XrefUserTestResultComponentsCodes/5
        [Route("api/UserData/DeleteXrefUserTestResultComponentsCodes/{id}")]
        [ResponseType(typeof(tXrefUserTestResultComponentsCode))]
        public async Task<IHttpActionResult> DeletetXrefUserTestResultComponentsCode(int id)
        {
            tXrefUserTestResultComponentsCode tXrefUserTestResultComponentsCode = await db.tXrefUserTestResultComponentsCodes.FindAsync(id);
            if (tXrefUserTestResultComponentsCode == null)
            {
                return NotFound();
            }

            db.tXrefUserTestResultComponentsCodes.Remove(tXrefUserTestResultComponentsCode);
            await db.SaveChangesAsync();

            return Ok(tXrefUserTestResultComponentsCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserTestResultComponentsCodeExists(int id)
        {
            return db.tXrefUserTestResultComponentsCodes.Count(e => e.ID == id) > 0;
        }
    }
}