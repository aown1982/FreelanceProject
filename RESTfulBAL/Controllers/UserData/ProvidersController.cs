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
    public class ProvidersController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/Providers
        [Route("api/UserData/GetProviders")]
        public IQueryable<tProvider> GettProviders()
        {
            return db.tProviders;
        }

        // GET: api/Providers/5
        [Route("api/UserData/GetProviders/{id}")]
        [ResponseType(typeof(tProvider))]
        public async Task<IHttpActionResult> GettProvider(int id)
        {
            tProvider tProvider = await db.tProviders.FindAsync(id);
            if (tProvider == null)
            {
                return NotFound();
            }

            return Ok(tProvider);
        }

        // PUT: api/Providers/5
        [Route("api/UserData/UpdateProviders/{id}/Provider")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttProvider(int id, tProvider Provider)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Provider.ID)
            {
                return BadRequest();
            }

            db.Entry(Provider).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tProviderExists(id))
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

        // POST: api/Providers
        [Route("api/UserData/PostProviders/Provider")]
        [ResponseType(typeof(tProvider))]
        public async Task<IHttpActionResult> PosttProvider(tProvider Provider)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tProviders.Add(Provider);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = Provider.ID }, Provider);
        }

        // DELETE: api/Providers/5
        [Route("api/UserData/DeleteProviders/{id}")]
        [ResponseType(typeof(tProvider))]
        public async Task<IHttpActionResult> DeletetProvider(int id)
        {
            tProvider tProvider = await db.tProviders.FindAsync(id);
            if (tProvider == null)
            {
                return NotFound();
            }

            db.tProviders.Remove(tProvider);
            await db.SaveChangesAsync();

            return Ok(tProvider);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tProviderExists(int id)
        {
            return db.tProviders.Count(e => e.ID == id) > 0;
        }
    }
}