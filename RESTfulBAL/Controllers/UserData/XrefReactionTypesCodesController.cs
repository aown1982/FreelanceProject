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
    public class XrefReactionTypesCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefReactionTypesCodes
        [Route("api/UserData/GetXrefReactionTypesCodes")]
        public IQueryable<tXrefReactionTypesCode> GettXrefReactionTypesCodes()
        {
            return db.tXrefReactionTypesCodes;
        }

        // GET: api/XrefReactionTypesCodes/5
        [Route("api/UserData/GetXrefReactionTypesCodes/{id}")]
        [ResponseType(typeof(tXrefReactionTypesCode))]
        public async Task<IHttpActionResult> GettXrefReactionTypesCode(int id)
        {
            tXrefReactionTypesCode tXrefReactionTypesCode = await db.tXrefReactionTypesCodes.FindAsync(id);
            if (tXrefReactionTypesCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefReactionTypesCode);
        }

        // PUT: api/XrefReactionTypesCodes/5
        [Route("api/UserData/UpdateXrefReactionTypesCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefReactionTypesCode(int id, tXrefReactionTypesCode tXrefReactionTypesCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefReactionTypesCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefReactionTypesCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefReactionTypesCodeExists(id))
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

        // POST: api/XrefReactionTypesCodes
        [Route("api/UserData/PostXrefReactionTypesCodes")]
        [ResponseType(typeof(tXrefReactionTypesCode))]
        public async Task<IHttpActionResult> PosttXrefReactionTypesCode(tXrefReactionTypesCode tXrefReactionTypesCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefReactionTypesCodes.Add(tXrefReactionTypesCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefReactionTypesCode.ID }, tXrefReactionTypesCode);
        }

        // DELETE: api/XrefReactionTypesCodes/5
        [Route("api/UserData/DeleteXrefReactionTypesCodes/{id}")]
        [ResponseType(typeof(tXrefReactionTypesCode))]
        public async Task<IHttpActionResult> DeletetXrefReactionTypesCode(int id)
        {
            tXrefReactionTypesCode tXrefReactionTypesCode = await db.tXrefReactionTypesCodes.FindAsync(id);
            if (tXrefReactionTypesCode == null)
            {
                return NotFound();
            }

            db.tXrefReactionTypesCodes.Remove(tXrefReactionTypesCode);
            await db.SaveChangesAsync();

            return Ok(tXrefReactionTypesCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefReactionTypesCodeExists(int id)
        {
            return db.tXrefReactionTypesCodes.Count(e => e.ID == id) > 0;
        }
    }
}