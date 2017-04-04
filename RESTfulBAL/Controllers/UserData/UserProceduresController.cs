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
    public class UserProceduresController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserProcedures
        [Route("api/UserData/GetUserProcedures")]
        public IQueryable<tUserProcedure> GettUserProcedures()
        {
            return db.tUserProcedures;
        }

        // GET: api/UserProcedures/5
        [Route("api/UserData/GetUserProcedures/{id}")]
        [ResponseType(typeof(tUserProcedure))]
        public async Task<IHttpActionResult> GettUserProcedure(int id)
        {
            tUserProcedure tUserProcedure = await db.tUserProcedures.FindAsync(id);
            if (tUserProcedure == null)
            {
                return NotFound();
            }

            return Ok(tUserProcedure);
        }

        // PUT: api/UserProcedures/5
        [Route("api/UserData/UpdateUserProcedures/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserProcedure(int id, tUserProcedure tUserProcedure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserProcedure.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserProcedure).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserProcedureExists(id))
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

        // POST: api/UserProcedures
        [Route("api/UserData/PostUserProcedures")]
        [ResponseType(typeof(tUserProcedure))]
        public async Task<IHttpActionResult> PosttUserProcedure(tUserProcedure tUserProcedure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserProcedures.Add(tUserProcedure);
            await db.SaveChangesAsync();

            return Ok(tUserProcedure);
        }

        // DELETE: api/UserProcedures/5
        [Route("api/UserData/DeleteUserProcedures/{id}")]
        [ResponseType(typeof(tUserProcedure))]
        public async Task<IHttpActionResult> DeletetUserProcedure(int id)
        {
            tUserProcedure tUserProcedure = await db.tUserProcedures.FindAsync(id);
            if (tUserProcedure == null)
            {
                return NotFound();
            }

            db.tUserProcedures.Remove(tUserProcedure);
            await db.SaveChangesAsync();

            return Ok(tUserProcedure);
        }
        // DELETE: api/SoftDeleteUserProcedure/5/2
        [Route("api/UserData/SoftDeleteUserProcedure/{id}/{status}")]
        [ResponseType(typeof(tUserProcedure))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserProcedure(int id, int status)
        {
            tUserProcedure tUserProcedure = await db.tUserProcedures.FindAsync(id);
            if (tUserProcedure == null)
            {
                return NotFound();
            }
            tUserProcedure.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tUserProcedure);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserProcedureExists(int id)
        {
            return db.tUserProcedures.Count(e => e.ID == id) > 0;
        }
    }
}