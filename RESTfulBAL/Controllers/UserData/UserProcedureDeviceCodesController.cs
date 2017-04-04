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
    public class UserProcedureDeviceCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserProcedureDeviceCodes
        [Route("api/UserData/GetUserProcedureDeviceCodes")]
        public IQueryable<tUserProcedureDeviceCode> GettUserProcedureDeviceCodes()
        {
            return db.tUserProcedureDeviceCodes;
        }

        // GET: api/UserProcedureDeviceCodes/5
        [Route("api/UserData/GetUserProcedureDeviceCodes/{id}")]
        [ResponseType(typeof(tUserProcedureDeviceCode))]
        public async Task<IHttpActionResult> GettUserProcedureDeviceCode(int id)
        {
            tUserProcedureDeviceCode tUserProcedureDeviceCode = await db.tUserProcedureDeviceCodes.FindAsync(id);
            if (tUserProcedureDeviceCode == null)
            {
                return NotFound();
            }

            return Ok(tUserProcedureDeviceCode);
        }

        // PUT: api/UserProcedureDeviceCodes/5
        [Route("api/UserData/UpdateUserProcedureDeviceCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserProcedureDeviceCode(int id, tUserProcedureDeviceCode tUserProcedureDeviceCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserProcedureDeviceCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserProcedureDeviceCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserProcedureDeviceCodeExists(id))
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

        // POST: api/UserProcedureDeviceCodes
        [Route("api/UserData/PostUserProcedureDeviceCodes")]
        [ResponseType(typeof(tUserProcedureDeviceCode))]
        public async Task<IHttpActionResult> PosttUserProcedureDeviceCode(tUserProcedureDeviceCode tUserProcedureDeviceCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserProcedureDeviceCodes.Add(tUserProcedureDeviceCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserProcedureDeviceCode.ID }, tUserProcedureDeviceCode);
        }

        // DELETE: api/UserProcedureDeviceCodes/5
        [Route("api/UserData/DeleteUserProcedureDeviceCodes/{id}")]
        [ResponseType(typeof(tUserProcedureDeviceCode))]
        public async Task<IHttpActionResult> DeletetUserProcedureDeviceCode(int id)
        {
            tUserProcedureDeviceCode tUserProcedureDeviceCode = await db.tUserProcedureDeviceCodes.FindAsync(id);
            if (tUserProcedureDeviceCode == null)
            {
                return NotFound();
            }

            db.tUserProcedureDeviceCodes.Remove(tUserProcedureDeviceCode);
            await db.SaveChangesAsync();

            return Ok(tUserProcedureDeviceCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserProcedureDeviceCodeExists(int id)
        {
            return db.tUserProcedureDeviceCodes.Count(e => e.ID == id) > 0;
        }
    }
}