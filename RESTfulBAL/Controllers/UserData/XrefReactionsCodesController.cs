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
    public class XrefReactionsCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefReactionsCodes
        [Route("api/UserData/GetXrefReactionsCodes")]
        public IQueryable<tXrefReactionsCode> GettXrefReactionsCodes()
        {
            return db.tXrefReactionsCodes;
        }

        // GET: api/XrefReactionsCodes/5
        [Route("api/UserData/GetXrefReactionsCodes/{id}")]
        [ResponseType(typeof(tXrefReactionsCode))]
        public async Task<IHttpActionResult> GettXrefReactionsCode(int id)
        {
            tXrefReactionsCode tXrefReactionsCode = await db.tXrefReactionsCodes.FindAsync(id);
            if (tXrefReactionsCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefReactionsCode);
        }

        // PUT: api/XrefReactionsCodes/5
        [Route("api/UserData/UpdateXrefReactionsCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefReactionsCode(int id, tXrefReactionsCode tXrefReactionsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefReactionsCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefReactionsCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefReactionsCodeExists(id))
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

        // POST: api/XrefReactionsCodes
        [Route("api/UserData/PostXrefReactionsCodes")]
        [ResponseType(typeof(tXrefReactionsCode))]
        public async Task<IHttpActionResult> PosttXrefReactionsCode(tXrefReactionsCode tXrefReactionsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefReactionsCodes.Add(tXrefReactionsCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefReactionsCode.ID }, tXrefReactionsCode);
        }

        // DELETE: api/XrefReactionsCodes/5
        [Route("api/UserData/DeleteXrefReactionsCodes/{id}")]
        [ResponseType(typeof(tXrefReactionsCode))]
        public async Task<IHttpActionResult> DeletetXrefReactionsCode(int id)
        {
            tXrefReactionsCode tXrefReactionsCode = await db.tXrefReactionsCodes.FindAsync(id);
            if (tXrefReactionsCode == null)
            {
                return NotFound();
            }

            db.tXrefReactionsCodes.Remove(tXrefReactionsCode);
            await db.SaveChangesAsync();

            return Ok(tXrefReactionsCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefReactionsCodeExists(int id)
        {
            return db.tXrefReactionsCodes.Count(e => e.ID == id) > 0;
        }
    }
}