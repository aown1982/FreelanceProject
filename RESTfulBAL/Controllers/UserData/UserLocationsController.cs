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
    public class UserLocationsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserLocations
        [Route("api/UserData/GetUserLocations")]
        public IQueryable<tUserLocation> GettUserLocations()
        {
            return db.tUserLocations;
        }

        // GET: api/UserLocations/5
        [Route("api/UserData/GetUserLocations/{id}")]
        [ResponseType(typeof(tUserLocation))]
        public async Task<IHttpActionResult> GettUserLocation(int id)
        {
            tUserLocation tUserLocation = await db.tUserLocations.FindAsync(id);
            if (tUserLocation == null)
            {
                return NotFound();
            }

            return Ok(tUserLocation);
        }

        // PUT: api/UserLocations/5
        [Route("api/UserData/UpdateUserLocations/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserLocation(int id, tUserLocation tUserLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserLocation.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserLocation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserLocationExists(id))
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

        // POST: api/UserLocations
        [Route("api/UserData/PostUserLocations")]
        [ResponseType(typeof(tUserLocation))]
        public async Task<IHttpActionResult> PosttUserLocation(tUserLocation tUserLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserLocations.Add(tUserLocation);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserLocation.ID }, tUserLocation);
        }

        // DELETE: api/UserLocations/5
        [Route("api/UserData/DeleteUserLocations/{id}")]
        [ResponseType(typeof(tUserLocation))]
        public async Task<IHttpActionResult> DeletetUserLocation(int id)
        {
            tUserLocation tUserLocation = await db.tUserLocations.FindAsync(id);
            if (tUserLocation == null)
            {
                return NotFound();
            }

            db.tUserLocations.Remove(tUserLocation);
            await db.SaveChangesAsync();

            return Ok(tUserLocation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserLocationExists(int id)
        {
            return db.tUserLocations.Count(e => e.ID == id) > 0;
        }
    }
}