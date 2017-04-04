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
    public class EthnicitiesController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/Users/Ethnicities
        [Route("api/Users/Ethnicities")]
        public IQueryable<tEthnicity> GettEthnicities()
        {
            return db.tEthnicities;
        }

        // GET: api/Users/Ethnicities/5
        [Route("api/Users/Ethnicities/{id}")]
        [ResponseType(typeof(tEthnicity))]
        public IHttpActionResult GettEthnicity(int id)
        {
            tEthnicity tEthnicity = db.tEthnicities.Find(id);
            if (tEthnicity == null)
            {
                return NotFound();
            }

            return Ok(tEthnicity);
        }

        //// PUT: api/Users/Ethnicities/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PuttEthnicity(int id, tEthnicity tEthnicity)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tEthnicity.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tEthnicity).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tEthnicityExists(id))
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

        //// POST: api/Users/Ethnicities
        //[ResponseType(typeof(tEthnicity))]
        //public IHttpActionResult PosttEthnicity(tEthnicity tEthnicity)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tEthnicities.Add(tEthnicity);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = tEthnicity.Id }, tEthnicity);
        //}

        //// DELETE: api/Users/Ethnicities/5
        //[ResponseType(typeof(tEthnicity))]
        //public IHttpActionResult DeletetEthnicity(int id)
        //{
        //    tEthnicity tEthnicity = db.tEthnicities.Find(id);
        //    if (tEthnicity == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tEthnicities.Remove(tEthnicity);
        //    db.SaveChanges();

        //    return Ok(tEthnicity);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tEthnicityExists(int id)
        {
            return db.tEthnicities.Count(e => e.Id == id) > 0;
        }
    }
}