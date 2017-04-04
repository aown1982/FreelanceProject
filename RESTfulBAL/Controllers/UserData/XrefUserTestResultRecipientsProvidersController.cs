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
    public class XrefUserTestResultRecipientsProvidersController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserTestResultRecipientsProviders
        [Route("api/UserData/GetXrefUserTestResultRecipientsProviders")]
        public IQueryable<tXrefUserTestResultRecipientsProvider> GettXrefUserTestResultRecipientsProviders()
        {
            return db.tXrefUserTestResultRecipientsProviders;
        }

        // GET: api/XrefUserTestResultRecipientsProviders/5
        [Route("api/UserData/GetXrefUserTestResultRecipientsProviders/{id}")]
        [ResponseType(typeof(tXrefUserTestResultRecipientsProvider))]
        public async Task<IHttpActionResult> GettXrefUserTestResultRecipientsProvider(int id)
        {
            tXrefUserTestResultRecipientsProvider tXrefUserTestResultRecipientsProvider = await db.tXrefUserTestResultRecipientsProviders.FindAsync(id);
            if (tXrefUserTestResultRecipientsProvider == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserTestResultRecipientsProvider);
        }

        // PUT: api/XrefUserTestResultRecipientsProviders/5
        [Route("api/UserData/UpdateXrefUserTestResultRecipientsProviders/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserTestResultRecipientsProvider(int id, tXrefUserTestResultRecipientsProvider tXrefUserTestResultRecipientsProvider)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserTestResultRecipientsProvider.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserTestResultRecipientsProvider).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserTestResultRecipientsProviderExists(id))
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

        // POST: api/XrefUserTestResultRecipientsProviders
        [Route("api/UserData/PostXrefUserTestResultRecipientsProviders")]
        [ResponseType(typeof(tXrefUserTestResultRecipientsProvider))]
        public async Task<IHttpActionResult> PosttXrefUserTestResultRecipientsProvider(tXrefUserTestResultRecipientsProvider tXrefUserTestResultRecipientsProvider)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserTestResultRecipientsProviders.Add(tXrefUserTestResultRecipientsProvider);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserTestResultRecipientsProvider.ID }, tXrefUserTestResultRecipientsProvider);
        }

        // DELETE: api/XrefUserTestResultRecipientsProviders/5
        [Route("api/UserData/DeleteXrefUserTestResultRecipientsProviders/{id}")]
        [ResponseType(typeof(tXrefUserTestResultRecipientsProvider))]
        public async Task<IHttpActionResult> DeletetXrefUserTestResultRecipientsProvider(int id)
        {
            tXrefUserTestResultRecipientsProvider tXrefUserTestResultRecipientsProvider = await db.tXrefUserTestResultRecipientsProviders.FindAsync(id);
            if (tXrefUserTestResultRecipientsProvider == null)
            {
                return NotFound();
            }

            db.tXrefUserTestResultRecipientsProviders.Remove(tXrefUserTestResultRecipientsProvider);
            await db.SaveChangesAsync();

            return Ok(tXrefUserTestResultRecipientsProvider);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserTestResultRecipientsProviderExists(int id)
        {
            return db.tXrefUserTestResultRecipientsProviders.Count(e => e.ID == id) > 0;
        }
    }
}