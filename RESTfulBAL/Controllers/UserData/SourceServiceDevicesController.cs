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
    public class SourceServiceDevicesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/SourceServiceDevices
        [Route("api/UserData/GetSourceServiceDevices")]
        public IQueryable<tSourceServiceDevice> GettSourceServiceDevices()
        {
            return db.tSourceServiceDevices;
        }

        // GET: api/SourceServiceDevices/5
        [Route("api/UserData/GetSourceServiceDevices/{id}")]
        [ResponseType(typeof(tSourceServiceDevice))]
        public async Task<IHttpActionResult> GettSourceServiceDevice(int id)
        {
            tSourceServiceDevice tSourceServiceDevice = await db.tSourceServiceDevices.FindAsync(id);
            if (tSourceServiceDevice == null)
            {
                return NotFound();
            }

            return Ok(tSourceServiceDevice);
        }

        // PUT: api/SourceServiceDevices/5
        [Route("api/UserData/UpdateSourceServiceDevices/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttSourceServiceDevice(int id, tSourceServiceDevice SourceServiceDevice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != SourceServiceDevice.ID)
            {
                return BadRequest();
            }

            db.Entry(SourceServiceDevice).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tSourceServiceDeviceExists(id))
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

        // POST: api/SourceServiceDevices
        [Route("api/UserData/PostSourceServiceDevices")]
        [ResponseType(typeof(tSourceServiceDevice))]
        public async Task<IHttpActionResult> PosttSourceServiceDevice(tSourceServiceDevice SourceServiceDevice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tSourceServiceDevices.Add(SourceServiceDevice);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = SourceServiceDevice.ID }, SourceServiceDevice);
        }

        // DELETE: api/SourceServiceDevices/5
        [Route("api/UserData/DeleteSourceServiceDevices/{id}")]
        [ResponseType(typeof(tSourceServiceDevice))]
        public async Task<IHttpActionResult> DeletetSourceServiceDevice(int id)
        {
            tSourceServiceDevice tSourceServiceDevice = await db.tSourceServiceDevices.FindAsync(id);
            if (tSourceServiceDevice == null)
            {
                return NotFound();
            }

            db.tSourceServiceDevices.Remove(tSourceServiceDevice);
            await db.SaveChangesAsync();

            return Ok(tSourceServiceDevice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tSourceServiceDeviceExists(int id)
        {
            return db.tSourceServiceDevices.Count(e => e.ID == id) > 0;
        }
    }
}