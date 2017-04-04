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
    public class GendersController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/Users/Genders
        [Route("api/Users/Genders")]
        public IQueryable<tGender> GettGenders()
        {
            return db.tGenders;
        }

        // GET: api/Users/Genders/5
        [Route("api/Users/Genders/{id}")]
        [ResponseType(typeof(tGender))]
        public IHttpActionResult GettGender(int id)
        {
            tGender tGender = db.tGenders.Find(id);
            if (tGender == null)
            {
                return NotFound();
            }

            return Ok(tGender);
        }

        //// PUT: api/Genders/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PuttGender(int id, tGender tGender)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tGender.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tGender).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tGenderExists(id))
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

        //// POST: api/Genders
        //[ResponseType(typeof(tGender))]
        //public IHttpActionResult PosttGender(tGender tGender)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tGenders.Add(tGender);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = tGender.Id }, tGender);
        //}

        //// DELETE: api/Genders/5
        //[ResponseType(typeof(tGender))]
        //public IHttpActionResult DeletetGender(int id)
        //{
        //    tGender tGender = db.tGenders.Find(id);
        //    if (tGender == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tGenders.Remove(tGender);
        //    db.SaveChanges();

        //    return Ok(tGender);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tGenderExists(int id)
        {
            return db.tGenders.Count(e => e.Id == id) > 0;
        }
    }
}