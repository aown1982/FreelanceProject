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
    public class RacesController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/Users/Races
        [Route("api/Users/Races")]
        public IQueryable<tRace> GettRaces()
        {
            return db.traces;
        }

        // GET: api/Users/Races/5
        [Route("api/Users/Races/{id}")]
        [ResponseType(typeof(tRace))]
        public IHttpActionResult GettRace(int id)
        {
            tRace tRace = db.traces.Find(id);
            if (tRace == null)
            {
                return NotFound();
            }

            return Ok(tRace);
        }

        //// PUT: api/Races/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PuttRace(int id, tRace tRace)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tRace.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tRace).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tRaceExists(id))
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

        //// POST: api/Races
        //[ResponseType(typeof(tRace))]
        //public IHttpActionResult PosttRace(tRace tRace)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.traces.Add(tRace);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = tRace.Id }, tRace);
        //}

        //// DELETE: api/Races/5
        //[ResponseType(typeof(tRace))]
        //public IHttpActionResult DeletetRace(int id)
        //{
        //    tRace tRace = db.traces.Find(id);
        //    if (tRace == null)
        //    {
        //        return NotFound();
        //    }

        //    db.traces.Remove(tRace);
        //    db.SaveChanges();

        //    return Ok(tRace);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private bool tRaceExists(int id)
        {
            return db.traces.Count(e => e.Id == id) > 0;
        }
    }
}