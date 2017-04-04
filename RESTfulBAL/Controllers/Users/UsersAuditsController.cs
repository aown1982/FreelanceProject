using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;
using DAL.Users;
using DAL.UserData;

namespace RESTfulBAL.Controllers.Users
{
    public class UsersAuditsController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/Users/UsersAudits
        [Route("api/Users/UsersAudits")]
        public IQueryable<tUsersAudit> GettUsersAudits()
        {
            return db.tUsersAudits.Include("tUsersAuditObject");
        }

        // GET: api/Users/UsersAudits/5
        [Route("api/Users/UsersAudits/{id}")]
        [ResponseType(typeof(tUsersAudit))]
        public IHttpActionResult GettUsersAudit(int id)
        {
            tUsersAudit tUsersAudit = db.tUsersAudits.Find(id);
            if (tUsersAudit == null)
            {
                return NotFound();
            }

            return Ok(tUsersAudit);
        }

        // GET: api/Users/UsersAudits/ByUser/5
        [Route("api/Users/UsersAudits/ByUser/{id}")]
        public IQueryable<tUsersAudit> GettUsersAuditByUserID(int id)
        {
            return db.tUsersAudits.Include("tUsersAuditObject").Where<tUsersAudit>(a => a.UserID == id);
        }

        // PUT: api/Users/UsersAudits/5
        [Route("api/Users/UsersAudits/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttUsersAudit(int id, tUsersAudit tUsersAudit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUsersAudit.Id)
            {
                return BadRequest();
            }

            db.Entry(tUsersAudit).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUsersAuditExists(id))
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

        // POST: api/Users/UsersAudits
        [Route("api/Users/UsersAudits")]
        [ResponseType(typeof(tUsersAudit))]
        public IHttpActionResult PosttUsersAudit(tUsersAudit tUsersAudit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUsersAudits.Add(tUsersAudit);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tUsersAudit.Id }, tUsersAudit);
        }

        // DELETE: api/Users/UsersAudits/5
        [Route("api/Users/UsersAudits/{id}")]
        [ResponseType(typeof(tUsersAudit))]
        public IHttpActionResult DeletetUsersAudit(int id)
        {
            tUsersAudit tUsersAudit = db.tUsersAudits.Find(id);
            if (tUsersAudit == null)
            {
                return NotFound();
            }

            db.tUsersAudits.Remove(tUsersAudit);
            db.SaveChanges();

            return Ok(tUsersAudit);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUsersAuditExists(int id)
        {
            return db.tUsersAudits.Count(e => e.Id == id) > 0;
        }

        [Route("api/Users/GetUsersAuditTypes/{userId}/{recordsCount}/{pageNumber}")]
        public IQueryable<tUsersAuditType> GettUsersAudit(int userId, int recordsCount, int pageNumber)
        {
            int startIndex = (pageNumber - 1) * recordsCount;
            var qList = db.tUsersAuditTypes.Where(t => t.IsDisplayed).OrderByDescending(a => a.ID).Skip(startIndex).Take(recordsCount);
            return qList;
        }
    }
}