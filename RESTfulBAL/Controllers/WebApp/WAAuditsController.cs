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
    public class WAAuditsController : ApiController
    {
        private WebApplicationEntities db = new WebApplicationEntities();

        // GET: api/WAAudits
        [Route("api/WebApp/GetWAAudits")]
        public IQueryable<tWAAudit> GettWAAudits()
        {
            return db.tWAAudits;
        }

        // GET: api/WAAudits/5
        [Route("api/WebApp/GetWAAudits/{id}")]
        [ResponseType(typeof(tWAAudit))]
        public async Task<IHttpActionResult> GettWAAudit(int id)
        {
            tWAAudit tWAAudit = await db.tWAAudits.FindAsync(id);
            if (tWAAudit == null)
            {
                return NotFound();
            }

            return Ok(tWAAudit);
        }

        //// PUT: api/WAAudits/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PuttWAAudit(int id, tWAAudit tWAAudit)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tWAAudit.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tWAAudit).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tWAAuditExists(id))
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

        // POST: api/WAAudits
        [Route("api/WebApp/WAAudits/Add")]
        [ResponseType(typeof(tWAAudit))]
        public async Task<IHttpActionResult> PosttWAAudit(tWAAudit tWAAudit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tWAAudits.Add(tWAAudit);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tWAAudit.Id }, tWAAudit);
        }

        //// DELETE: api/WAAudits/5
        //[ResponseType(typeof(tWAAudit))]
        //public async Task<IHttpActionResult> DeletetWAAudit(int id)
        //{
        //    tWAAudit tWAAudit = await db.tWAAudits.FindAsync(id);
        //    if (tWAAudit == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tWAAudits.Remove(tWAAudit);
        //    await db.SaveChangesAsync();

        //    return Ok(tWAAudit);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tWAAuditExists(int id)
        {
            return db.tWAAudits.Count(e => e.Id == id) > 0;
        }
    }
}