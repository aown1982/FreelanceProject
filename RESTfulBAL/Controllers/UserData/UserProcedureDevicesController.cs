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
    public class UserProcedureDevicesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserProcedureDevices
        [Route("api/UserData/GetUserProcedureDevices")]
        public IQueryable<tUserProcedureDevice> GettUserProcedureDevices()
        {
            return db.tUserProcedureDevices;
        }

        // GET: api/UserProcedureDevices/5
        [Route("api/UserData/GetUserProcedureDevices/{id}")]
        [ResponseType(typeof(tUserProcedureDevice))]
        public async Task<IHttpActionResult> GettUserProcedureDevice(int id)
        {
            tUserProcedureDevice tUserProcedureDevice = await db.tUserProcedureDevices.FindAsync(id);
            if (tUserProcedureDevice == null)
            {
                return NotFound();
            }

            return Ok(tUserProcedureDevice);
        }

        // PUT: api/UserProcedureDevices/5
        [Route("api/UserData/UpdateUserProcedureDevices/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserProcedureDevice(int id, tUserProcedureDevice tUserProcedureDevice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserProcedureDevice.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserProcedureDevice).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserProcedureDeviceExists(id))
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

        // POST: api/UserProcedureDevices
        [Route("api/UserData/PostUserProcedureDevices")]
        [ResponseType(typeof(tUserProcedureDevice))]
        public async Task<IHttpActionResult> PosttUserProcedureDevice(tUserProcedureDevice tUserProcedureDevice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserProcedureDevices.Add(tUserProcedureDevice);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserProcedureDevice.ID }, tUserProcedureDevice);
        }

        // DELETE: api/UserProcedureDevices/5
        [Route("api/UserData/DeleteUserProcedureDevices/{id}")]
        [ResponseType(typeof(tUserProcedureDevice))]
        public async Task<IHttpActionResult> DeletetUserProcedureDevice(int id)
        {
            tUserProcedureDevice tUserProcedureDevice = await db.tUserProcedureDevices.FindAsync(id);
            if (tUserProcedureDevice == null)
            {
                return NotFound();
            }

            db.tUserProcedureDevices.Remove(tUserProcedureDevice);
            await db.SaveChangesAsync();

            return Ok(tUserProcedureDevice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserProcedureDeviceExists(int id)
        {
            return db.tUserProcedureDevices.Count(e => e.ID == id) > 0;
        }
    }
}