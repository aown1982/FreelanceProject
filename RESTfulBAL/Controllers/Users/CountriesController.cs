using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;
using DAL.Users;

namespace RESTfulBAL.Controllers.Users
{
    public class CountriesController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/Users/Countries
        [Route("api/Users/Countries")]
        public IQueryable<tCountry> GettCountries()
        {
            return db.tCountries.Include("tStates");
        }

        // GET: api/Users/Countries/5
        [Route("api/Users/Countries/{id}")]
        [ResponseType(typeof(tCountry))]
        public IHttpActionResult GettCountry(int id)
        {
            tCountry tCountry = db.tCountries
                                            .Include("tStates")
                                            .SingleOrDefault(x => x.Id == id);
            if (tCountry == null)
            {
                return NotFound();
            }

            return Ok(tCountry);
        }

        //// PUT: api/Users/Countries/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PuttCountry(int id, tCountry tCountry)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tCountry.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tCountry).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tCountryExists(id))
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

        //// POST: api/Users/Countries
        //[ResponseType(typeof(tCountry))]
        //public IHttpActionResult PosttCountry(tCountry tCountry)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tCountries.Add(tCountry);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = tCountry.Id }, tCountry);
        //}

        //// DELETE: api/Users/Countries/5
        //[ResponseType(typeof(tCountry))]
        //public IHttpActionResult DeletetCountry(int id)
        //{
        //    tCountry tCountry = db.tCountries.Find(id);
        //    if (tCountry == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tCountries.Remove(tCountry);
        //    db.SaveChanges();

        //    return Ok(tCountry);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tCountryExists(int id)
        {
            return db.tCountries.Count(e => e.Id == id) > 0;
        }
    }
}