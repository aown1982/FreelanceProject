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
    public class XrefUserProblemsCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserProblemsCodes
        [Route("api/UserData/GetXrefUserProblemsCodes")]
        public IQueryable<tXrefUserProblemsCode> GettXrefUserProblemsCodes()
        {
            return db.tXrefUserProblemsCodes;
        }

        // GET: api/XrefUserProblemsCodes/5
        [Route("api/UserData/GetXrefUserProblemsCodes/{id}")]
        [ResponseType(typeof(tXrefUserProblemsCode))]
        public async Task<IHttpActionResult> GettXrefUserProblemsCode(int id)
        {
            tXrefUserProblemsCode tXrefUserProblemsCode = await db.tXrefUserProblemsCodes.FindAsync(id);
            if (tXrefUserProblemsCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserProblemsCode);
        }

        // PUT: api/XrefUserProblemsCodes/5
        [Route("api/UserData/UpdateXrefUserProblemsCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserProblemsCode(int id, tXrefUserProblemsCode tXrefUserProblemsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserProblemsCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserProblemsCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserProblemsCodeExists(id))
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

        // POST: api/XrefUserProblemsCodes
        [Route("api/UserData/PostXrefUserProblemsCodes")]
        [ResponseType(typeof(tXrefUserProblemsCode))]
        public async Task<IHttpActionResult> PosttXrefUserProblemsCode(tXrefUserProblemsCode tXrefUserProblemsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserProblemsCodes.Add(tXrefUserProblemsCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserProblemsCode.ID }, tXrefUserProblemsCode);
        }

        // DELETE: api/XrefUserProblemsCodes/5
        [Route("api/UserData/DeleteXrefUserProblemsCodes/{id}")]
        [ResponseType(typeof(tXrefUserProblemsCode))]
        public async Task<IHttpActionResult> DeletetXrefUserProblemsCode(int id)
        {
            tXrefUserProblemsCode tXrefUserProblemsCode = await db.tXrefUserProblemsCodes.FindAsync(id);
            if (tXrefUserProblemsCode == null)
            {
                return NotFound();
            }

            db.tXrefUserProblemsCodes.Remove(tXrefUserProblemsCode);
            await db.SaveChangesAsync();

            return Ok(tXrefUserProblemsCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserProblemsCodeExists(int id)
        {
            return db.tXrefUserProblemsCodes.Count(e => e.ID == id) > 0;
        }
    }
}