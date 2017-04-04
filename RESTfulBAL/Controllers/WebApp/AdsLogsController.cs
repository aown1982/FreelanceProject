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
    public class AdsLogsController : ApiController
    {
        private WebApplicationEntities db = new WebApplicationEntities();

        // GET: api/AdsLogs
        [Route("api/WebApp/GetAdsLogs")]
        public IQueryable<tAdsLog> GettAdsLogs()
        {
            return db.tAdsLogs;
        }

        // GET: api/AdsLogs/5
        [Route("api/WebApp/GetAdsLogs/{id}")]
        [ResponseType(typeof(tAdsLog))]
        public async Task<IHttpActionResult> GettAdsLog(int id)
        {
            tAdsLog tAdsLog = await db.tAdsLogs.FindAsync(id);
            if (tAdsLog == null)
            {
                return NotFound();
            }

            return Ok(tAdsLog);
        }

        //// PUT: api/AdsLogs/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PuttAdsLog(int id, tAdsLog tAdsLog)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tAdsLog.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tAdsLog).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tAdsLogExists(id))
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

        //// POST: api/AdsLogs
        //[ResponseType(typeof(tAdsLog))]
        //public async Task<IHttpActionResult> PosttAdsLog(tAdsLog tAdsLog)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tAdsLogs.Add(tAdsLog);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = tAdsLog.ID }, tAdsLog);
        //}

        //// DELETE: api/AdsLogs/5
        //[ResponseType(typeof(tAdsLog))]
        //public async Task<IHttpActionResult> DeletetAdsLog(int id)
        //{
        //    tAdsLog tAdsLog = await db.tAdsLogs.FindAsync(id);
        //    if (tAdsLog == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tAdsLogs.Remove(tAdsLog);
        //    await db.SaveChangesAsync();

        //    return Ok(tAdsLog);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tAdsLogExists(int id)
        {
            return db.tAdsLogs.Count(e => e.ID == id) > 0;
        }


        [Route("api/WebApp/PostAdsLog")]
        [ResponseType(typeof(tAdsLog))]
        public async Task<IHttpActionResult> PostAdsLog(tAdsLog tAdsResult)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            db.tAdsLogs.Add(tAdsResult);
            var result = await db.SaveChangesAsync();
            return Ok(result);
        }



        }
}