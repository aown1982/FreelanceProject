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
    public class UserDataAuditsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserDataAudits
        [Route("api/UserData/GetUserDataAudits")]
        public IQueryable<tUserDataAudit> GettUserDataAudits()
        {
            return db.tUserDataAudits;
        }

        // GET: api/UserDataAudits/5
        [Route("api/UserData/GetUserDataAudits/{id}")]
        [ResponseType(typeof(tUserDataAudit))]
        public async Task<IHttpActionResult> GettUserDataAudit(int id)
        {
            tUserDataAudit tUserDataAudit = await db.tUserDataAudits.FindAsync(id);
            if (tUserDataAudit == null)
            {
                return NotFound();
            }

            return Ok(tUserDataAudit);
        }

        // PUT: api/UserDataAudits/5
        [Route("api/UserData/UpdateUserDataAudits/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserDataAudit(int id, tUserDataAudit tUserDataAudit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserDataAudit.Id)
            {
                return BadRequest();
            }

            db.Entry(tUserDataAudit).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserDataAuditExists(id))
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

        // POST: api/UserDataAudits
        [Route("api/UserData/PostUserDataAudits")]
        [ResponseType(typeof(tUserDataAudit))]
        public async Task<IHttpActionResult> PosttUserDataAudit(tUserDataAudit tUserDataAudit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserDataAudits.Add(tUserDataAudit);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserDataAudit.Id }, tUserDataAudit);
        }

        // DELETE: api/UserDataAudits/5
        [Route("api/UserData/DeleteUserDataAudits/{id}")]
        [ResponseType(typeof(tUserDataAudit))]
        public async Task<IHttpActionResult> DeletetUserDataAudit(int id)
        {
            tUserDataAudit tUserDataAudit = await db.tUserDataAudits.FindAsync(id);
            if (tUserDataAudit == null)
            {
                return NotFound();
            }

            db.tUserDataAudits.Remove(tUserDataAudit);
            await db.SaveChangesAsync();

            return Ok(tUserDataAudit);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserDataAuditExists(int id)
        {
            return db.tUserDataAudits.Count(e => e.Id == id) > 0;
        }
    }
}