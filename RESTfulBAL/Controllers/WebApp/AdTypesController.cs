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
    public class AdTypesController : ApiController
    {
        private WebApplicationEntities db = new WebApplicationEntities();

        // GET: api/AdTypes
        [Route("api/WebApp/GetAdTypes")]
        public IQueryable<tAdType> GettAdTypes()
        {
            return db.tAdTypes;
        }

        // GET: api/AdTypes/5
        [Route("api/WebApp/GetAdTypes/{id}")]
        [ResponseType(typeof(tAdType))]
        public async Task<IHttpActionResult> GettAdType(int id)
        {
            tAdType tAdType = await db.tAdTypes.FindAsync(id);
            if (tAdType == null)
            {
                return NotFound();
            }

            return Ok(tAdType);
        }

        //// PUT: api/AdTypes/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PuttAdType(int id, tAdType tAdType)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tAdType.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tAdType).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tAdTypeExists(id))
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

        //// POST: api/AdTypes
        //[ResponseType(typeof(tAdType))]
        //public async Task<IHttpActionResult> PosttAdType(tAdType tAdType)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tAdTypes.Add(tAdType);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = tAdType.ID }, tAdType);
        //}

        //// DELETE: api/AdTypes/5
        //[ResponseType(typeof(tAdType))]
        //public async Task<IHttpActionResult> DeletetAdType(int id)
        //{
        //    tAdType tAdType = await db.tAdTypes.FindAsync(id);
        //    if (tAdType == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tAdTypes.Remove(tAdType);
        //    await db.SaveChangesAsync();

        //    return Ok(tAdType);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tAdTypeExists(int id)
        {
            return db.tAdTypes.Count(e => e.ID == id) > 0;
        }
    }
}