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

namespace RESTfulBAL.Controllers.Users
{
    public class UsersErrLogsController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/Users/UsersErrLogs
        [Route("api/Users/UsersErrLogs")]
        public IQueryable<tUsersErrLog> GettUsersErrLogs()
        {
            return db.tUsersErrLogs;
        }

        // GET: api/Users/UsersErrLogs/5
        [Route("api/Users/UsersErrLogs/{id}")]
        [ResponseType(typeof(tUsersErrLog))]
        public IHttpActionResult GettUsersErrLog(int id)
        {
            tUsersErrLog tUsersErrLog = db.tUsersErrLogs.Find(id);
            if (tUsersErrLog == null)
            {
                return NotFound();
            }

            return Ok(tUsersErrLog);
        }

        // GET: api/Users/UsersErrLogs/ByUser/5
        [Route("api/Users/UsersErrLogs/ByUser/{id}")]
        public IQueryable<tUsersErrLog> GettUsersErrLogByUserID(int id)
        {
            return db.tUsersErrLogs.Where<tUsersErrLog>(a => a.UserID == id);
        }

        // PUT: api/Users/UsersErrLogs/5
        [Route("api/Users/UsersErrLogs/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttUsersErrLog(int id, tUsersErrLog tUsersErrLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUsersErrLog.Id)
            {
                return BadRequest();
            }

            db.Entry(tUsersErrLog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUsersErrLogExists(id))
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

        // POST: api/Users/UsersErrLogs
        [Route("api/Users/UsersErrLogs")]
        [ResponseType(typeof(tUsersErrLog))]
        public IHttpActionResult PosttUsersErrLog(tUsersErrLog tUsersErrLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUsersErrLogs.Add(tUsersErrLog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tUsersErrLog.Id }, tUsersErrLog);
        }

        // DELETE: api/Users/UsersErrLogs/5
        [Route("api/Users/UsersErrLogs/{id}")]
        [ResponseType(typeof(tUsersErrLog))]
        public IHttpActionResult DeletetUsersErrLog(int id)
        {
            tUsersErrLog tUsersErrLog = db.tUsersErrLogs.Find(id);
            if (tUsersErrLog == null)
            {
                return NotFound();
            }

            db.tUsersErrLogs.Remove(tUsersErrLog);
            db.SaveChanges();

            return Ok(tUsersErrLog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUsersErrLogExists(int id)
        {
            return db.tUsersErrLogs.Count(e => e.Id == id) > 0;
        }
    }
}