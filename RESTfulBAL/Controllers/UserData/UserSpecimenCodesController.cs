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
    public class UserSpecimenCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserSpecimenCodes
        [Route("api/UserData/GetUserSpecimenCodes")]
        public IQueryable<tUserSpecimenCode> GettUserSpecimenCodes()
        {
            return db.tUserSpecimenCodes;
        }

        // GET: api/UserSpecimenCodes/5
        [Route("api/UserData/GetUserSpecimenCodes/{id}")]
        [ResponseType(typeof(tUserSpecimenCode))]
        public async Task<IHttpActionResult> GettUserSpecimenCode(int id)
        {
            tUserSpecimenCode tUserSpecimenCode = await db.tUserSpecimenCodes.FindAsync(id);
            if (tUserSpecimenCode == null)
            {
                return NotFound();
            }

            return Ok(tUserSpecimenCode);
        }

        // PUT: api/UserSpecimenCodes/5
        [Route("api/UserData/UpdateUserSpecimenCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserSpecimenCode(int id, tUserSpecimenCode tUserSpecimenCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserSpecimenCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserSpecimenCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserSpecimenCodeExists(id))
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

        // POST: api/UserSpecimenCodes
        [Route("api/UserData/PostUserSpecimenCodes")]
        [ResponseType(typeof(tUserSpecimenCode))]
        public async Task<IHttpActionResult> PosttUserSpecimenCode(tUserSpecimenCode tUserSpecimenCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserSpecimenCodes.Add(tUserSpecimenCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserSpecimenCode.ID }, tUserSpecimenCode);
        }

        // DELETE: api/UserSpecimenCodes/5
        [Route("api/UserData/DeleteUserSpecimenCodes/{id}")]
        [ResponseType(typeof(tUserSpecimenCode))]
        public async Task<IHttpActionResult> DeletetUserSpecimenCode(int id)
        {
            tUserSpecimenCode tUserSpecimenCode = await db.tUserSpecimenCodes.FindAsync(id);
            if (tUserSpecimenCode == null)
            {
                return NotFound();
            }

            db.tUserSpecimenCodes.Remove(tUserSpecimenCode);
            await db.SaveChangesAsync();

            return Ok(tUserSpecimenCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserSpecimenCodeExists(int id)
        {
            return db.tUserSpecimenCodes.Count(e => e.ID == id) > 0;
        }
    }
}