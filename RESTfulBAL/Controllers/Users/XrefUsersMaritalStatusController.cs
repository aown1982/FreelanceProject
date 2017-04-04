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
using DAL.Users;

namespace RESTfulBAL.Controllers.Users
{
    public class XrefUsersMaritalStatusController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/XrefUsersMaritalStatus
        [Route("api/WebApp/GetXrefUsersMaritalStatus")]
        public IQueryable<tXrefUsersMaritalStatu> GettXrefUsersMaritalStatus()
        {
            return db.tXrefUsersMaritalStatus;
        }

        // GET: api/XrefUsersMaritalStatus/5
        [Route("api/WebApp/GetXrefUsersMaritalStatus/{id}")]
        [ResponseType(typeof(tXrefUsersMaritalStatu))]
        public async Task<IHttpActionResult> GettXrefUsersMaritalStatu(int id)
        {
            tXrefUsersMaritalStatu tXrefUsersMaritalStatu = await db.tXrefUsersMaritalStatus.FindAsync(id);
            if (tXrefUsersMaritalStatu == null)
            {
                return NotFound();
            }

            return Ok(tXrefUsersMaritalStatu);
        }

        // PUT: api/XrefUsersMaritalStatus/5
        [Route("api/WebApp/EditXrefUsersMaritalStatus/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUsersMaritalStatu(int id, tXrefUsersMaritalStatu tXrefUsersMaritalStatu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUsersMaritalStatu.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUsersMaritalStatu).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUsersMaritalStatuExists(id))
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

        // POST: api/XrefUsersMaritalStatus
        [Route("api/WebApp/XrefUsersMaritalStatus/Add")]
        [ResponseType(typeof(tXrefUsersMaritalStatu))]
        public async Task<IHttpActionResult> PosttXrefUsersMaritalStatu(tXrefUsersMaritalStatu tXrefUsersMaritalStatu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUsersMaritalStatus.Add(tXrefUsersMaritalStatu);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUsersMaritalStatu.ID }, tXrefUsersMaritalStatu);
        }

        // DELETE: api/XrefUsersMaritalStatus/5
        [Route("api/WebApp/DeleteXrefUsersMaritalStatus/{id}")]
        [ResponseType(typeof(tXrefUsersMaritalStatu))]
        public async Task<IHttpActionResult> DeletetXrefUsersMaritalStatu(int id)
        {
            tXrefUsersMaritalStatu tXrefUsersMaritalStatu = await db.tXrefUsersMaritalStatus.FindAsync(id);
            if (tXrefUsersMaritalStatu == null)
            {
                return NotFound();
            }

            db.tXrefUsersMaritalStatus.Remove(tXrefUsersMaritalStatu);
            await db.SaveChangesAsync();

            return Ok(tXrefUsersMaritalStatu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUsersMaritalStatuExists(int id)
        {
            return db.tXrefUsersMaritalStatus.Count(e => e.ID == id) > 0;
        }
    }
}