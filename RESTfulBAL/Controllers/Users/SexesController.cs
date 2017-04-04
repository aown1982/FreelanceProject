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
    public class SexesController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/Users/Sexes
        [Route("api/Users/Sexes")]
        public IQueryable<tSex> GettSexes()
        {
            return db.tSexes;
        }

        // GET: api/Users/Sexes/5
        [Route("api/Users/Sexes/{id}")]
        [ResponseType(typeof(tSex))]
        public IHttpActionResult GettSex(int id)
        {
            tSex tSex = db.tSexes.Find(id);
            if (tSex == null)
            {
                return NotFound();
            }

            return Ok(tSex);
        }

        //// PUT: api/Sexes/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PuttSex(int id, tSex tSex)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tSex.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tSex).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tSexExists(id))
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

        //// POST: api/Sexes
        //[ResponseType(typeof(tSex))]
        //public IHttpActionResult PosttSex(tSex tSex)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tSexes.Add(tSex);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = tSex.Id }, tSex);
        //}

        //// DELETE: api/Sexes/5
        //[ResponseType(typeof(tSex))]
        //public IHttpActionResult DeletetSex(int id)
        //{
        //    tSex tSex = db.tSexes.Find(id);
        //    if (tSex == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tSexes.Remove(tSex);
        //    db.SaveChanges();

        //    return Ok(tSex);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tSexExists(int id)
        {
            return db.tSexes.Count(e => e.Id == id) > 0;
        }
    }
}