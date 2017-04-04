using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;
using DAL.WebApplication;

namespace RESTfulBAL.Controllers.WebApp
{
    public class WAAuditObjectsController : ApiController
    {
        private WebApplicationEntities db = new WebApplicationEntities();

        // GET: api/WAAuditObjects
        [Route("api/WebApp/GetWAAuditObjects")]
        public IQueryable<tWAAuditObject> GettWAAuditObjects()
        {
            return db.tWAAuditObjects;
        }

        // GET: api/WAAuditObjects/5
        [Route("api/WebApp/GetWAAuditObjects/{id}")]
        [ResponseType(typeof(tWAAuditObject))]
        public async Task<IHttpActionResult> GettWAAuditObject(int id)
        {
            tWAAuditObject tWAAuditObject = await db.tWAAuditObjects.FindAsync(id);
            if (tWAAuditObject == null)
            {
                return NotFound();
            }

            return Ok(tWAAuditObject);
        }

        //// PUT: api/WAAuditObjects/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PuttWAAuditObject(int id, tWAAuditObject tWAAuditObject)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tWAAuditObject.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tWAAuditObject).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tWAAuditObjectExists(id))
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

        // POST: api/WAAuditObjects
        [Route("api/WebApp/WAAuditObjects/Add")]
        [ResponseType(typeof(tWAAuditObject))]
        public async Task<IHttpActionResult> PosttWAAuditObject(tWAAuditObject tWAAuditObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tWAAuditObjects.Add(tWAAuditObject);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tWAAuditObject.ID }, tWAAuditObject);
        }

        //// DELETE: api/WAAuditObjects/5
        //[ResponseType(typeof(tWAAuditObject))]
        //public async Task<IHttpActionResult> DeletetWAAuditObject(int id)
        //{
        //    tWAAuditObject tWAAuditObject = await db.tWAAuditObjects.FindAsync(id);
        //    if (tWAAuditObject == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tWAAuditObjects.Remove(tWAAuditObject);
        //    await db.SaveChangesAsync();

        //    return Ok(tWAAuditObject);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tWAAuditObjectExists(int id)
        {
            return db.tWAAuditObjects.Count(e => e.ID == id) > 0;
        }
    }
}