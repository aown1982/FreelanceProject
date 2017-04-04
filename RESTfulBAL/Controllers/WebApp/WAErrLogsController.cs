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
    public class WAErrLogsController : ApiController
    {
        private WebApplicationEntities db = new WebApplicationEntities();

        // GET: api/WAErrLogs
        [Route("api/WebApp/GetWAErrLogs")]
        public IQueryable<tWAErrLog> GettWAErrLogs()
        {
            return db.tWAErrLogs;
        }

        // GET: api/WAErrLogs/5
        [Route("api/WebApp/GetWAErrLogs/{id}")]
        [ResponseType(typeof(tWAErrLog))]
        public async Task<IHttpActionResult> GettWAErrLog(int id)
        {
            tWAErrLog tWAErrLog = await db.tWAErrLogs.FindAsync(id);
            if (tWAErrLog == null)
            {
                return NotFound();
            }

            return Ok(tWAErrLog);
        }

        //// PUT: api/WAErrLogs/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PuttWAErrLog(int id, tWAErrLog tWAErrLog)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tWAErrLog.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tWAErrLog).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tWAErrLogExists(id))
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

        // POST: api/WAErrLogs
        [Route("api/WebApp/WAErrLogs/Add")]
        [ResponseType(typeof(tWAErrLog))]
        public async Task<IHttpActionResult> PosttWAErrLog(tWAErrLog tWAErrLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tWAErrLogs.Add(tWAErrLog);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tWAErrLog.Id }, tWAErrLog);
        }

        //// DELETE: api/WAErrLogs/5
        //[ResponseType(typeof(tWAErrLog))]
        //public async Task<IHttpActionResult> DeletetWAErrLog(int id)
        //{
        //    tWAErrLog tWAErrLog = await db.tWAErrLogs.FindAsync(id);
        //    if (tWAErrLog == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tWAErrLogs.Remove(tWAErrLog);
        //    await db.SaveChangesAsync();

        //    return Ok(tWAErrLog);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tWAErrLogExists(int id)
        {
            return db.tWAErrLogs.Count(e => e.Id == id) > 0;
        }
    }
}