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
    public class XrefUserSourceServiceDevicesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserSourceServiceDevices
        [Route("api/UserData/GetXrefUserSourceServiceDevices")]
        public IQueryable<tXrefUserSourceServiceDevice> GettXrefUserSourceServiceDevices()
        {
            return db.tXrefUserSourceServiceDevices;
        }

        // GET: api/XrefUserSourceServiceDevices/5
        [Route("api/UserData/GetXrefUserSourceServiceDevices/{id}")]
        [ResponseType(typeof(tXrefUserSourceServiceDevice))]
        public async Task<IHttpActionResult> GettXrefUserSourceServiceDevice(int id)
        {
            tXrefUserSourceServiceDevice tXrefUserSourceServiceDevice = await db.tXrefUserSourceServiceDevices.FindAsync(id);
            if (tXrefUserSourceServiceDevice == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserSourceServiceDevice);
        }

        // PUT: api/XrefUserSourceServiceDevices/5
        [Route("api/UserData/UpdateXrefUserSourceServiceDevices/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserSourceServiceDevice(int id, tXrefUserSourceServiceDevice tXrefUserSourceServiceDevice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserSourceServiceDevice.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserSourceServiceDevice).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserSourceServiceDeviceExists(id))
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

        // POST: api/XrefUserSourceServiceDevices
        [Route("api/UserData/PostXrefUserSourceServiceDevices")]
        [ResponseType(typeof(tXrefUserSourceServiceDevice))]
        public async Task<IHttpActionResult> PosttXrefUserSourceServiceDevice(tXrefUserSourceServiceDevice tXrefUserSourceServiceDevice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserSourceServiceDevices.Add(tXrefUserSourceServiceDevice);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserSourceServiceDevice.ID }, tXrefUserSourceServiceDevice);
        }

        // DELETE: api/XrefUserSourceServiceDevices/5
        [Route("api/UserData/DeleteXrefUserSourceServiceDevices/{id}")]
        [ResponseType(typeof(tXrefUserSourceServiceDevice))]
        public async Task<IHttpActionResult> DeletetXrefUserSourceServiceDevice(int id)
        {
            tXrefUserSourceServiceDevice tXrefUserSourceServiceDevice = await db.tXrefUserSourceServiceDevices.FindAsync(id);
            if (tXrefUserSourceServiceDevice == null)
            {
                return NotFound();
            }

            db.tXrefUserSourceServiceDevices.Remove(tXrefUserSourceServiceDevice);
            await db.SaveChangesAsync();

            return Ok(tXrefUserSourceServiceDevice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserSourceServiceDeviceExists(int id)
        {
            return db.tXrefUserSourceServiceDevices.Count(e => e.ID == id) > 0;
        }
    }
}