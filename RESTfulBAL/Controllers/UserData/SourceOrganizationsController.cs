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
    public class SourceOrganizationsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/SourceOrganizations
        [Route("api/UserData/GetSourceOrganizations")]
        public IQueryable<tSourceOrganization> GettSourceOrganizations()
        {
            return db.tSourceOrganizations.Include("tOrganization");
        }

        // GET: api/SourceOrganizations/5
        [Route("api/UserData/GetSourceOrganizations/{id}")]
        [ResponseType(typeof(tSourceOrganization))]
        public async Task<IHttpActionResult> GettSourceOrganization(int id)
        {
            tSourceOrganization tSourceOrganization = await db.tSourceOrganizations.FindAsync(id);
            if (tSourceOrganization == null)
            {
                return NotFound();
            }

            return Ok(tSourceOrganization);
        }

        // PUT: api/SourceOrganizations/5
        [Route("api/UserData/UpdateSourceOrganizations/{id}/SourceOrganization")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttSourceOrganization(int id, tSourceOrganization SourceOrganization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != SourceOrganization.ID)
            {
                return BadRequest();
            }

            db.Entry(SourceOrganization).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tSourceOrganizationExists(id))
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

        // POST: api/SourceOrganizations
        [Route("api/UserData/PostSourceOrganizations/SourceOrganization")]
        [ResponseType(typeof(tSourceOrganization))]
        public async Task<IHttpActionResult> PosttSourceOrganization(tSourceOrganization SourceOrganization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tSourceOrganizations.Add(SourceOrganization);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = SourceOrganization.ID }, SourceOrganization);
        }

        // DELETE: api/SourceOrganizations/5
        [Route("api/UserData/DeleteSourceOrganizations/{id}")]
        [ResponseType(typeof(tSourceOrganization))]
        public async Task<IHttpActionResult> DeletetSourceOrganization(int id)
        {
            tSourceOrganization tSourceOrganization = await db.tSourceOrganizations.FindAsync(id);
            if (tSourceOrganization == null)
            {
                return NotFound();
            }

            db.tSourceOrganizations.Remove(tSourceOrganization);
            await db.SaveChangesAsync();

            return Ok(tSourceOrganization);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tSourceOrganizationExists(int id)
        {
            return db.tSourceOrganizations.Count(e => e.ID == id) > 0;
        }
    }
}