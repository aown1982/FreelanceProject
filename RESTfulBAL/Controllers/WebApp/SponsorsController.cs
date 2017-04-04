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
    public class SponsorsController : ApiController
    {
        private WebApplicationEntities db = new WebApplicationEntities();

        // GET: api/Sponsors
        [Route("api/WebApp/GetSponsors")]
        public IQueryable<tSponsor> GettSponsors()
        {
            return db.tSponsors;
        }

        // GET: api/Sponsors/5
        [Route("api/WebApp/GetSponsors/{id}")]
        [ResponseType(typeof(tSponsor))]
        public async Task<IHttpActionResult> GettSponsor(int id)
        {
            tSponsor tSponsor = await db.tSponsors.FindAsync(id);
            if (tSponsor == null)
            {
                return NotFound();
            }

            return Ok(tSponsor);
        }

        //// PUT: api/Sponsors/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PuttSponsor(int id, tSponsor tSponsor)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tSponsor.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tSponsor).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tSponsorExists(id))
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

        //// POST: api/Sponsors
        //[ResponseType(typeof(tSponsor))]
        //public async Task<IHttpActionResult> PosttSponsor(tSponsor tSponsor)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tSponsors.Add(tSponsor);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = tSponsor.ID }, tSponsor);
        //}

        //// DELETE: api/Sponsors/5
        //[ResponseType(typeof(tSponsor))]
        //public async Task<IHttpActionResult> DeletetSponsor(int id)
        //{
        //    tSponsor tSponsor = await db.tSponsors.FindAsync(id);
        //    if (tSponsor == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tSponsors.Remove(tSponsor);
        //    await db.SaveChangesAsync();

        //    return Ok(tSponsor);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tSponsorExists(int id)
        {
            return db.tSponsors.Count(e => e.ID == id) > 0;
        }
    }
}