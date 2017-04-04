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
    public class CodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/Codes
        [Route("api/UserData/GetCodes")]
        public IQueryable<tCode> GettCodes()
        {
            return db.tCodes;
        }

        // GET: api/Codes/5
        [Route("api/UserData/GetCodes/{id}")]
        [ResponseType(typeof(tCode))]
        public async Task<IHttpActionResult> GettCode(int id)
        {
            tCode tCode = await db.tCodes.FindAsync(id);
            if (tCode == null)
            {
                return NotFound();
            }

            return Ok(tCode);
        }

        // PUT: api/Codes/5
        [Route("api/UserData/UpdateCode/{id}/Code")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttCode(int id, tCode Code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Code.ID)
            {
                return BadRequest();
            }

            db.Entry(Code).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tCodeExists(id))
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

        // POST: api/Codes
        [Route("api/UserData/PostCode/Code")]
        [ResponseType(typeof(tCode))]
        public async Task<IHttpActionResult> PosttCode(tCode Code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tCodes.Add(Code);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = Code.ID }, Code);
        }

        // DELETE: api/Codes/5
        [Route("api/UserData/DeleteCode/{id}")]
        [ResponseType(typeof(tCode))]
        public async Task<IHttpActionResult> DeletetCode(int id)
        {
            tCode tCode = await db.tCodes.FindAsync(id);
            if (tCode == null)
            {
                return NotFound();
            }

            db.tCodes.Remove(tCode);
            await db.SaveChangesAsync();

            return Ok(tCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tCodeExists(int id)
        {
            return db.tCodes.Count(e => e.ID == id) > 0;
        }
    }
}