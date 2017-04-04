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
    public class UserSNPProbesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserSNPProbes
        [Route("api/UserData/GetUserSNPProbes")]
        public IQueryable<tUserSNPProbe> GettUserSNPProbes()
        {
            return db.tUserSNPProbes;
        }

        // GET: api/UserSNPProbes/5
        [Route("api/UserData/GetUserSNPProbes/{id}")]
        [ResponseType(typeof(tUserSNPProbe))]
        public async Task<IHttpActionResult> GettUserSNPProbe(int id)
        {
            tUserSNPProbe tUserSNPProbe = await db.tUserSNPProbes.FindAsync(id);
            if (tUserSNPProbe == null)
            {
                return NotFound();
            }

            return Ok(tUserSNPProbe);
        }

        // PUT: api/UserSNPProbes/5
        [Route("api/UserData/UpdateUserSNPProbes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserSNPProbe(int id, tUserSNPProbe tUserSNPProbe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserSNPProbe.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserSNPProbe).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserSNPProbeExists(id))
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

        // POST: api/UserSNPProbes
        [Route("api/UserData/PostUserSNPProbes")]
        [ResponseType(typeof(tUserSNPProbe))]
        public async Task<IHttpActionResult> PosttUserSNPProbe(tUserSNPProbe tUserSNPProbe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserSNPProbes.Add(tUserSNPProbe);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserSNPProbe.ID }, tUserSNPProbe);
        }

        // DELETE: api/UserSNPProbes/5
        [Route("api/UserData/DeleteUserSNPProbes/{id}")]
        [ResponseType(typeof(tUserSNPProbe))]
        public async Task<IHttpActionResult> DeletetUserSNPProbe(int id)
        {
            tUserSNPProbe tUserSNPProbe = await db.tUserSNPProbes.FindAsync(id);
            if (tUserSNPProbe == null)
            {
                return NotFound();
            }

            db.tUserSNPProbes.Remove(tUserSNPProbe);
            await db.SaveChangesAsync();

            return Ok(tUserSNPProbe);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserSNPProbeExists(int id)
        {
            return db.tUserSNPProbes.Count(e => e.ID == id) > 0;
        }
    }
}