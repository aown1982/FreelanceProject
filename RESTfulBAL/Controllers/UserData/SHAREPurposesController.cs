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
    public class SHAREPurposesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/SHAREPurposes
        [Route("api/UserData/GetSHAREPurposes")]
        public IQueryable<tSHAREPurpose> GettSHAREPurposes()
        {
            return db.tSHAREPurposes;
        }

        // GET: api/SHAREPurposes/5
        [Route("api/UserData/GetSHAREPurposes/{id}")]
        [ResponseType(typeof(tSHAREPurpose))]
        public async Task<IHttpActionResult> GettSHAREPurpose(int id)
        {
            tSHAREPurpose tSHAREPurpose = await db.tSHAREPurposes.FindAsync(id);
            if (tSHAREPurpose == null)
            {
                return NotFound();
            }

            return Ok(tSHAREPurpose);
        }

        // PUT: api/SHAREPurposes/5
        [Route("api/UserData/UpdateSHAREPurposes/{id}/SHAREPurpose")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttSHAREPurpose(int id, tSHAREPurpose SHAREPurpose)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != SHAREPurpose.ID)
            {
                return BadRequest();
            }

            db.Entry(SHAREPurpose).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tSHAREPurposeExists(id))
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

        // POST: api/SHAREPurposes
        [Route("api/UserData/PostSHAREPurposes/SHAREPurpose")]
        [ResponseType(typeof(tSHAREPurpose))]
        public async Task<IHttpActionResult> PosttSHAREPurpose(tSHAREPurpose SHAREPurpose)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tSHAREPurposes.Add(SHAREPurpose);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = SHAREPurpose.ID }, SHAREPurpose);
        }

        // DELETE: api/SHAREPurposes/5
        [Route("api/UserData/DeleteSHAREPurposes/{id}")]
        [ResponseType(typeof(tSHAREPurpose))]
        public async Task<IHttpActionResult> DeletetSHAREPurpose(int id)
        {
            tSHAREPurpose tSHAREPurpose = await db.tSHAREPurposes.FindAsync(id);
            if (tSHAREPurpose == null)
            {
                return NotFound();
            }

            db.tSHAREPurposes.Remove(tSHAREPurpose);
            await db.SaveChangesAsync();

            return Ok(tSHAREPurpose);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tSHAREPurposeExists(int id)
        {
            return db.tSHAREPurposes.Count(e => e.ID == id) > 0;
        }
    }
}