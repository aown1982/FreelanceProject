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
    public class XrefUserPrescriptionsCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserPrescriptionsCodes
        [Route("api/UserData/GetXrefUserPrescriptionsCodes")]
        public IQueryable<tXrefUserPrescriptionsCode> GettXrefUserPrescriptionsCodes()
        {
            return db.tXrefUserPrescriptionsCodes;
        }

        // GET: api/XrefUserPrescriptionsCodes/5
        [Route("api/UserData/GetXrefUserPrescriptionsCodes/{id}")]
        [ResponseType(typeof(tXrefUserPrescriptionsCode))]
        public async Task<IHttpActionResult> GettXrefUserPrescriptionsCode(int id)
        {
            tXrefUserPrescriptionsCode tXrefUserPrescriptionsCode = await db.tXrefUserPrescriptionsCodes.FindAsync(id);
            if (tXrefUserPrescriptionsCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserPrescriptionsCode);
        }

        // PUT: api/XrefUserPrescriptionsCodes/5
        [Route("api/UserData/UpdateXrefUserPrescriptionsCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserPrescriptionsCode(int id, tXrefUserPrescriptionsCode tXrefUserPrescriptionsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserPrescriptionsCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserPrescriptionsCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserPrescriptionsCodeExists(id))
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

        // POST: api/XrefUserPrescriptionsCodes
        [Route("api/UserData/PostXrefUserPrescriptionsCodes")]
        [ResponseType(typeof(tXrefUserPrescriptionsCode))]
        public async Task<IHttpActionResult> PosttXrefUserPrescriptionsCode(tXrefUserPrescriptionsCode tXrefUserPrescriptionsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserPrescriptionsCodes.Add(tXrefUserPrescriptionsCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserPrescriptionsCode.ID }, tXrefUserPrescriptionsCode);
        }

        // DELETE: api/XrefUserPrescriptionsCodes/5
        [Route("api/UserData/DeleteXrefUserPrescriptionsCodes/{id}")]
        [ResponseType(typeof(tXrefUserPrescriptionsCode))]
        public async Task<IHttpActionResult> DeletetXrefUserPrescriptionsCode(int id)
        {
            tXrefUserPrescriptionsCode tXrefUserPrescriptionsCode = await db.tXrefUserPrescriptionsCodes.FindAsync(id);
            if (tXrefUserPrescriptionsCode == null)
            {
                return NotFound();
            }

            db.tXrefUserPrescriptionsCodes.Remove(tXrefUserPrescriptionsCode);
            await db.SaveChangesAsync();

            return Ok(tXrefUserPrescriptionsCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserPrescriptionsCodeExists(int id)
        {
            return db.tXrefUserPrescriptionsCodes.Count(e => e.ID == id) > 0;
        }
    }
}