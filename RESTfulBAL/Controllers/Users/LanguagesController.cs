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
    public class LanguagesController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/Users/Languages
        [Route("api/Users/Languages")]
        public IQueryable<tLanguage> GettLanguages()
        {
            return db.tLanguages;
        }

        // GET: api/Languages/5
        [Route("api/Users/Languages/{id}")]
        [ResponseType(typeof(tLanguage))]
        public IHttpActionResult GettLanguage(int id)
        {
            tLanguage tLanguage = db.tLanguages.Find(id);
            if (tLanguage == null)
            {
                return NotFound();
            }

            return Ok(tLanguage);
        }

        //// PUT: api/Languages/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PuttLanguage(int id, tLanguage tLanguage)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tLanguage.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tLanguage).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tLanguageExists(id))
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

        //// POST: api/Languages
        //[ResponseType(typeof(tLanguage))]
        //public IHttpActionResult PosttLanguage(tLanguage tLanguage)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tLanguages.Add(tLanguage);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = tLanguage.Id }, tLanguage);
        //}

        //// DELETE: api/Languages/5
        //[ResponseType(typeof(tLanguage))]
        //public IHttpActionResult DeletetLanguage(int id)
        //{
        //    tLanguage tLanguage = db.tLanguages.Find(id);
        //    if (tLanguage == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tLanguages.Remove(tLanguage);
        //    db.SaveChanges();

        //    return Ok(tLanguage);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tLanguageExists(int id)
        {
            return db.tLanguages.Count(e => e.Id == id) > 0;
        }
    }
}