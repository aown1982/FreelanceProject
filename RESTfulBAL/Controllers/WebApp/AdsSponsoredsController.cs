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
using RESTfulBAL.Common;

namespace RESTfulBAL.Controllers.WebApp
{
    public class AdsSponsoredsController : ApiController
    {
        private WebApplicationEntities db = new WebApplicationEntities();

        // GET: api/AdsSponsoreds
        [Route("api/WebApp/GetAdsSponsoreds")]
        public IQueryable<tAdsSponsored> GettAdsSponsoreds()
        {
            return db.tAdsSponsoreds;
        }

        // GET: api/AdsSponsoreds/5
        [Route("api/WebApp/GetAdsSponsoreds/{id}")]
        [ResponseType(typeof(tAdsSponsored))]
        public async Task<IHttpActionResult> GettAdsSponsored(int id)
        {
            tAdsSponsored tAdsSponsored = await db.tAdsSponsoreds.FindAsync(id);
            if (tAdsSponsored == null)
            {
                return NotFound();
            }

            return Ok(tAdsSponsored);
        }

        //// PUT: api/AdsSponsoreds/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PuttAdsSponsored(int id, tAdsSponsored tAdsSponsored)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tAdsSponsored.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tAdsSponsored).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tAdsSponsoredExists(id))
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

        //// POST: api/AdsSponsoreds
        //[ResponseType(typeof(tAdsSponsored))]
        //public async Task<IHttpActionResult> PosttAdsSponsored(tAdsSponsored tAdsSponsored)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tAdsSponsoreds.Add(tAdsSponsored);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = tAdsSponsored.ID }, tAdsSponsored);
        //}

        //// DELETE: api/AdsSponsoreds/5
        //[ResponseType(typeof(tAdsSponsored))]
        //public async Task<IHttpActionResult> DeletetAdsSponsored(int id)
        //{
        //    tAdsSponsored tAdsSponsored = await db.tAdsSponsoreds.FindAsync(id);
        //    if (tAdsSponsored == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tAdsSponsoreds.Remove(tAdsSponsored);
        //    await db.SaveChangesAsync();

        //    return Ok(tAdsSponsored);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tAdsSponsoredExists(int id)
        {
            return db.tAdsSponsoreds.Count(e => e.ID == id) > 0;
        }


        [Route("api/WebApp/GetAdsByUserId/{userId}")]
        [ResponseType(typeof(List<tAdsSponsored>))]
        public async Task<IHttpActionResult> GettAdsSponsoreds(int userId)
        {
            var list = new List<tAdsSponsored>();
            var allResults = db.tAdsSponsoreds;
            foreach (var item in allResults)
            {
                var isExists = item.UserFilterID.HasValue ? CommonMethods.IsUserExistsInUserFilters(item.UserFilterID.Value, userId) : true;
                if (isExists)
                    list.Add(item);
            }

            return Ok(list);
        }
    }
}