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
    public class TimeZonesController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/Users/TimeZones
        [Route("api/Users/TimeZones")]
        public IQueryable<tTimeZone> GettTimeZones()
        {
            return db.tTimeZones;
        }

        // GET: api/Users/TimeZones/5
        [Route("api/Users/TimeZones/{id}")]
        [ResponseType(typeof(tTimeZone))]
        public IHttpActionResult GettTimeZone(int id)
        {
            tTimeZone tTimeZone = db.tTimeZones.Find(id);
            if (tTimeZone == null)
            {
                return NotFound();
            }

            return Ok(tTimeZone);
        }

        //// PUT: api/TimeZones/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PuttTimeZone(int id, tTimeZone tTimeZone)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tTimeZone.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tTimeZone).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tTimeZoneExists(id))
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

        //// POST: api/TimeZones
        //[ResponseType(typeof(tTimeZone))]
        //public IHttpActionResult PosttTimeZone(tTimeZone tTimeZone)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tTimeZones.Add(tTimeZone);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = tTimeZone.Id }, tTimeZone);
        //}

        //// DELETE: api/TimeZones/5
        //[ResponseType(typeof(tTimeZone))]
        //public IHttpActionResult DeletetTimeZone(int id)
        //{
        //    tTimeZone tTimeZone = db.tTimeZones.Find(id);
        //    if (tTimeZone == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tTimeZones.Remove(tTimeZone);
        //    db.SaveChanges();

        //    return Ok(tTimeZone);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tTimeZoneExists(int id)
        {
            return db.tTimeZones.Count(e => e.Id == id) > 0;
        }
    }
}