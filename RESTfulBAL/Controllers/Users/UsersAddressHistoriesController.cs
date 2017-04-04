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
    public class UsersAddressHistoriesController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/Users/UsersAddressHistories
        [Route("api/Users/UsersAddressHistories")]
        public IQueryable<tUsersAddressHistory> GettUsersAddressHistories()
        {
            return db.tUsersAddressHistories;
        }

        // GET: api/Users/UsersAddressHistories/5
        [Route("api/Users/UsersAddressHistories/{id}")]
        [ResponseType(typeof(tUsersAddressHistory))]
        public IHttpActionResult GettUsersAddressHistory(int id)
        {
            tUsersAddressHistory tUsersAddressHistory = db.tUsersAddressHistories.Find(id);
            if (tUsersAddressHistory == null)
            {
                return NotFound();
            }

            return Ok(tUsersAddressHistory);
        }

        // GET: api/Users/UsersAddressHistories/ByUser/5
        [Route("api/Users/UsersAddressHistories/ByUser/{id}")]
        public IQueryable<tUsersAddressHistory> GettUsersAddressHistoryByUserID(int id)
        {
            return db.tUsersAddressHistories.Where<tUsersAddressHistory>(a => a.UserID == id);
        }

        // PUT: api/Users/UsersAddressHistories/5
        [ResponseType(typeof(void))]
        [Route("api/Users/UsersAddressHistories/{id}")]
        public IHttpActionResult PuttUsersAddressHistory(int id, tUsersAddressHistory tUsersAddressHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUsersAddressHistory.ID)
            {
                return BadRequest();
            }

            db.Entry(tUsersAddressHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUsersAddressHistoryExists(id))
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

        // POST: api/Users/UsersAddressHistories
        [Route("api/Users/UsersAddressHistories")]
        [ResponseType(typeof(tUsersAddressHistory))]
        public IHttpActionResult PosttUsersAddressHistory(tUsersAddressHistory tUsersAddressHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUsersAddressHistories.Add(tUsersAddressHistory);
            db.SaveChanges();

            return CreatedAtRoute("UsersAPI", new { id = tUsersAddressHistory.ID }, tUsersAddressHistory);
        }

        // DELETE: api/Users/UsersAddressHistories/5
        [Route("api/Users/UsersAddressHistories/{id}")]
        [ResponseType(typeof(tUsersAddressHistory))]
        public IHttpActionResult DeletetUsersAddressHistory(int id)
        {
            tUsersAddressHistory tUsersAddressHistory = db.tUsersAddressHistories.Find(id);
            if (tUsersAddressHistory == null)
            {
                return NotFound();
            }

            db.tUsersAddressHistories.Remove(tUsersAddressHistory);
            db.SaveChanges();

            return Ok(tUsersAddressHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUsersAddressHistoryExists(int id)
        {
            return db.tUsersAddressHistories.Count(e => e.ID == id) > 0;
        }
    }
}