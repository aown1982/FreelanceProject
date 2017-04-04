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
    public class XrefUserProceduresCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserProceduresCodes
        [Route("api/UserData/GetXrefUserProceduresCodes")]
        public IQueryable<tXrefUserProceduresCode> GettXrefUserProceduresCodes()
        {
            return db.tXrefUserProceduresCodes;
        }

        // GET: api/XrefUserProceduresCodes/5
        [Route("api/UserData/GetXrefUserProceduresCodes/{id}")]
        [ResponseType(typeof(tXrefUserProceduresCode))]
        public async Task<IHttpActionResult> GettXrefUserProceduresCode(int id)
        {
            tXrefUserProceduresCode tXrefUserProceduresCode = await db.tXrefUserProceduresCodes.FindAsync(id);
            if (tXrefUserProceduresCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserProceduresCode);
        }

        // PUT: api/XrefUserProceduresCodes/5
        [Route("api/UserData/UpdateXrefUserProceduresCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserProceduresCode(int id, tXrefUserProceduresCode tXrefUserProceduresCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserProceduresCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserProceduresCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserProceduresCodeExists(id))
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

        // POST: api/XrefUserProceduresCodes
        [Route("api/UserData/PostXrefUserProceduresCodes")]
        [ResponseType(typeof(tXrefUserProceduresCode))]
        public async Task<IHttpActionResult> PosttXrefUserProceduresCode(tXrefUserProceduresCode tXrefUserProceduresCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserProceduresCodes.Add(tXrefUserProceduresCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserProceduresCode.ID }, tXrefUserProceduresCode);
        }

        // DELETE: api/XrefUserProceduresCodes/5
        [Route("api/UserData/DeleteXrefUserProceduresCodes/{id}")]
        [ResponseType(typeof(tXrefUserProceduresCode))]
        public async Task<IHttpActionResult> DeletetXrefUserProceduresCode(int id)
        {
            tXrefUserProceduresCode tXrefUserProceduresCode = await db.tXrefUserProceduresCodes.FindAsync(id);
            if (tXrefUserProceduresCode == null)
            {
                return NotFound();
            }

            db.tXrefUserProceduresCodes.Remove(tXrefUserProceduresCode);
            await db.SaveChangesAsync();

            return Ok(tXrefUserProceduresCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserProceduresCodeExists(int id)
        {
            return db.tXrefUserProceduresCodes.Count(e => e.ID == id) > 0;
        }
    }
}