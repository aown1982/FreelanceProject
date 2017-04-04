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
    public class UserImmunizationsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserImmunizations
        [Route("api/UserData/GetUserImmunizations")]
        public IQueryable<tUserImmunization> GettUserImmunizations()
        {
            return db.tUserImmunizations;
        }

        // GET: api/UserImmunizations/5
        [Route("api/UserData/GetUserImmunizations/{id}")]
        [ResponseType(typeof(tUserImmunization))]
        public async Task<IHttpActionResult> GettUserImmunization(int id)
        {
            tUserImmunization tUserImmunization = await db.tUserImmunizations.FindAsync(id);
            if (tUserImmunization == null)
            {
                return NotFound();
            }

            return Ok(tUserImmunization);
        }

        // PUT: api/UserImmunizations/5
        [Route("api/UserData/UpdateUserImmunizations/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserImmunization(int id, tUserImmunization tUserImmunization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserImmunization.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserImmunization).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserImmunizationExists(id))
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

        // POST: api/UserImmunizations
        [Route("api/UserData/PostUserImmunizations")]
        [ResponseType(typeof(tUserImmunization))]
        public async Task<IHttpActionResult> PosttUserImmunization(tUserImmunization tUserImmunization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserImmunizations.Add(tUserImmunization);
            await db.SaveChangesAsync();

            return Ok(tUserImmunization);
        }

        // DELETE: api/UserImmunizations/5
        [Route("api/UserData/DeleteUserImmunizations/{id}")]
        [ResponseType(typeof(tUserImmunization))]
        public async Task<IHttpActionResult> DeletetUserImmunization(int id)
        {
            tUserImmunization tUserImmunization = await db.tUserImmunizations.FindAsync(id);
            if (tUserImmunization == null)
            {
                return NotFound();
            }

            db.tUserImmunizations.Remove(tUserImmunization);
            await db.SaveChangesAsync();

            return Ok(tUserImmunization);
        }

        // DELETE: api/SoftDeleteUserImmunization/5/2
        [Route("api/UserData/SoftDeleteUserImmunization/{id}/{status}")]
        [ResponseType(typeof(tUserImmunization))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserImmunization(int id, int status)
        {
            tUserImmunization tUserImmunization = await db.tUserImmunizations.FindAsync(id);
            if (tUserImmunization == null)
            {
                return NotFound();
            }

            tUserImmunization.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tUserImmunization);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserImmunizationExists(int id)
        {
            return db.tUserImmunizations.Count(e => e.ID == id) > 0;
        }
    }
}