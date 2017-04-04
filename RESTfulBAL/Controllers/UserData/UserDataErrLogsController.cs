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
using DAL.UserData;

namespace RESTfulBAL.Controllers.UserData
{
    public class UserDataErrLogsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserDataErrLogs
        [Route("api/UserData/GetUserDataErrLogs")]
        public IQueryable<tUserDataErrLog> GettUserDataErrLogs()
        {
            return db.tUserDataErrLogs;
        }

        // GET: api/UserDataErrLogs/5
        [Route("api/UserData/GetUserDataErrLogs/{id}")]
        [ResponseType(typeof(tUserDataErrLog))]
        public async Task<IHttpActionResult> GettUserDataErrLog(int id)
        {
            tUserDataErrLog tUserDataErrLog = await db.tUserDataErrLogs.FindAsync(id);
            if (tUserDataErrLog == null)
            {
                return NotFound();
            }

            return Ok(tUserDataErrLog);
        }

        // PUT: api/UserDataErrLogs/5
        [Route("api/UserData/UpdateUserDataErrLogs/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserDataErrLog(int id, tUserDataErrLog tUserDataErrLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserDataErrLog.Id)
            {
                return BadRequest();
            }

            db.Entry(tUserDataErrLog).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserDataErrLogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UserDataErrLogs
        [Route("api/UserData/PostUserDataErrLogs")]
        [ResponseType(typeof(tUserDataErrLog))]
        public async Task<IHttpActionResult> PosttUserDataErrLog(tUserDataErrLog tUserDataErrLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserDataErrLogs.Add(tUserDataErrLog);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserDataErrLog.Id }, tUserDataErrLog);
        }

        // DELETE: api/UserDataErrLogs/5
        [Route("api/UserData/DeleteUserDataErrLogs/{id}")]
        [ResponseType(typeof(tUserDataErrLog))]
        public async Task<IHttpActionResult> DeletetUserDataErrLog(int id)
        {
            tUserDataErrLog tUserDataErrLog = await db.tUserDataErrLogs.FindAsync(id);
            if (tUserDataErrLog == null)
            {
                return NotFound();
            }

            db.tUserDataErrLogs.Remove(tUserDataErrLog);
            await db.SaveChangesAsync();

            return Ok(tUserDataErrLog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserDataErrLogExists(int id)
        {
            return db.tUserDataErrLogs.Count(e => e.Id == id) > 0;
        }
    }
}