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
    public class MaritalStatusController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/Users/MaritalStatus
        [Route("api/Users/MaritalStatus")]
        public IQueryable<tMaritalStatus> GettMaritalStatuses()
        {
            return db.tMaritalStatuses;
        }

        // GET: api/Users/MaritalStatus/5
        [Route("api/Users/MaritalStatus/{id}")]
        [ResponseType(typeof(tMaritalStatus))]
        public IHttpActionResult GettMaritalStatus(int id)
        {
            tMaritalStatus tMaritalStatus = db.tMaritalStatuses.Find(id);
            if (tMaritalStatus == null)
            {
                return NotFound();
            }

            return Ok(tMaritalStatus);
        }

        //// PUT: api/MaritalStatus/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PuttMaritalStatus(int id, tMaritalStatus tMaritalStatus)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tMaritalStatus.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tMaritalStatus).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tMaritalStatusExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/MaritalStatus
        //[ResponseType(typeof(tMaritalStatus))]
        //public IHttpActionResult PosttMaritalStatus(tMaritalStatus tMaritalStatus)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tMaritalStatuses.Add(tMaritalStatus);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = tMaritalStatus.Id }, tMaritalStatus);
        //}

        //// DELETE: api/MaritalStatus/5
        //[ResponseType(typeof(tMaritalStatus))]
        //public IHttpActionResult DeletetMaritalStatus(int id)
        //{
        //    tMaritalStatus tMaritalStatus = db.tMaritalStatuses.Find(id);
        //    if (tMaritalStatus == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tMaritalStatuses.Remove(tMaritalStatus);
        //    db.SaveChanges();

        //    return Ok(tMaritalStatus);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tMaritalStatusExists(int id)
        {
            return db.tMaritalStatuses.Count(e => e.Id == id) > 0;
        }
    }
}