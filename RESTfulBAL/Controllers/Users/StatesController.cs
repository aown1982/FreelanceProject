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
    public class StatesController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/Users/States
        [Route("api/Users/States")]
        public IQueryable<tState> GettStates()
        {
            return db.tStates;
        }

        // GET: api/Users/States/5
        [Route("api/Users/States/{id}")]
        [ResponseType(typeof(tState))]
        public IHttpActionResult GettState(int id)
        {
            tState tState = db.tStates.Find(id);
            if (tState == null)
            {
                return NotFound();
            }

            return Ok(tState);
        }

        // GET: api/Users/States/ByCountry/5
        [Route("api/Users/States/ByCountry/{id}")]
        public IQueryable<tState> GettStateByCountryID(int id)
        {
            return db.tStates.Where<tState>(a => a.CountryID == id);
        }

        //// PUT: api/States/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PuttState(int id, tState tState)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tState.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tState).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tStateExists(id))
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

        //// POST: api/States
        //[ResponseType(typeof(tState))]
        //public IHttpActionResult PosttState(tState tState)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tStates.Add(tState);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = tState.Id }, tState);
        //}

        //// DELETE: api/States/5
        //[ResponseType(typeof(tState))]
        //public IHttpActionResult DeletetState(int id)
        //{
        //    tState tState = db.tStates.Find(id);
        //    if (tState == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tStates.Remove(tState);
        //    db.SaveChanges();

        //    return Ok(tState);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tStateExists(int id)
        {
            return db.tStates.Count(e => e.Id == id) > 0;
        }
    }
}