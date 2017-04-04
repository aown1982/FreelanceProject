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
    public class OrganizationsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/Organizations
        [Route("api/UserData/GetOrganizations")]
        public IQueryable<tOrganization> GettOrganizations()
        {
            return db.tOrganizations;
        }

        // GET: api/Organizations/5
        [Route("api/UserData/GetOrganizations/{id}")]
        [ResponseType(typeof(tOrganization))]
        public async Task<IHttpActionResult> GettOrganization(int id)
        {
            tOrganization tOrganization = await db.tOrganizations.FindAsync(id);
            if (tOrganization == null)
            {
                return NotFound();
            }

            return Ok(tOrganization);
        }

        // PUT: api/Organizations/5
        [Route("api/UserData/UpdateOrganizations/{id}/Organization")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttOrganization(int id, tOrganization Organization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Organization.ID)
            {
                return BadRequest();
            }

            db.Entry(Organization).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tOrganizationExists(id))
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

        // POST: api/Organizations
        [Route("api/UserData/PostOrganizations/Organization")]
        [ResponseType(typeof(tOrganization))]
        public async Task<IHttpActionResult> PosttOrganization(tOrganization Organization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tOrganizations.Add(Organization);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = Organization.ID }, Organization);
        }

        // DELETE: api/Organizations/5
        [Route("api/UserData/DeleteOrganizations/{id}")]
        [ResponseType(typeof(tOrganization))]
        public async Task<IHttpActionResult> DeletetOrganization(int id)
        {
            tOrganization tOrganization = await db.tOrganizations.FindAsync(id);
            if (tOrganization == null)
            {
                return NotFound();
            }

            db.tOrganizations.Remove(tOrganization);
            await db.SaveChangesAsync();

            return Ok(tOrganization);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tOrganizationExists(int id)
        {
            return db.tOrganizations.Count(e => e.ID == id) > 0;
        }
    }
}