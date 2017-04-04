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
    public class ConsentController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/Consent
        [Route("api/UserData/GettConsents")]
        public IQueryable<tConsent> GettConsents()
        {
            return db.tConsents;
        }

        // GET: api/Consent/5
        [Route("api/UserData/GettConsent")]
        [ResponseType(typeof(tConsent))]
        public async Task<IHttpActionResult> GettConsent(int id)
        {
            tConsent tConsent = await db.tConsents.FindAsync(id);
            if (tConsent == null)
            {
                return NotFound();
            }

            return Ok(tConsent);
        }

        // PUT: api/Consent/5
        [Route("api/UserData/PuttConsent")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttConsent(int id, tConsent tConsent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tConsent.ID)
            {
                return BadRequest();
            }

            db.Entry(tConsent).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tConsentExists(id))
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

        // POST: api/Consent
        [Route("api/UserData/PosttConsent")]
        [ResponseType(typeof(tConsent))]
        public async Task<IHttpActionResult> PosttConsent(tConsent tConsent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tConsents.Add(tConsent);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tConsent.ID }, tConsent);
        }

        // DELETE: api/Consent/5
        [Route("api/UserData/DeletetConsent")]
        [ResponseType(typeof(tConsent))]
        public async Task<IHttpActionResult> DeletetConsent(int id)
        {
            tConsent tConsent = await db.tConsents.FindAsync(id);
            if (tConsent == null)
            {
                return NotFound();
            }

            db.tConsents.Remove(tConsent);
            await db.SaveChangesAsync();

            return Ok(tConsent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tConsentExists(int id)
        {
            return db.tConsents.Count(e => e.ID == id) > 0;
        }
    }
}