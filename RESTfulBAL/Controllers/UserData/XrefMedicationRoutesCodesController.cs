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
    public class XrefMedicationRoutesCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefMedicationRoutesCodes
        [Route("api/UserData/GetXrefMedicationRoutesCodes")]
        public IQueryable<tXrefMedicationRoutesCode> GettXrefMedicationRoutesCodes()
        {
            return db.tXrefMedicationRoutesCodes;
        }

        // GET: api/XrefMedicationRoutesCodes/5
        [Route("api/UserData/GetXrefMedicationRoutesCodes/{id}")]
        [ResponseType(typeof(tXrefMedicationRoutesCode))]
        public async Task<IHttpActionResult> GettXrefMedicationRoutesCode(int id)
        {
            tXrefMedicationRoutesCode tXrefMedicationRoutesCode = await db.tXrefMedicationRoutesCodes.FindAsync(id);
            if (tXrefMedicationRoutesCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefMedicationRoutesCode);
        }

        // PUT: api/XrefMedicationRoutesCodes/5
        [Route("api/UserData/UpdateXrefMedicationRoutesCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefMedicationRoutesCode(int id, tXrefMedicationRoutesCode tXrefMedicationRoutesCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefMedicationRoutesCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefMedicationRoutesCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefMedicationRoutesCodeExists(id))
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

        // POST: api/XrefMedicationRoutesCodes
        [Route("api/UserData/PostXrefMedicationRoutesCodes")]
        [ResponseType(typeof(tXrefMedicationRoutesCode))]
        public async Task<IHttpActionResult> PosttXrefMedicationRoutesCode(tXrefMedicationRoutesCode tXrefMedicationRoutesCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefMedicationRoutesCodes.Add(tXrefMedicationRoutesCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefMedicationRoutesCode.ID }, tXrefMedicationRoutesCode);
        }

        // DELETE: api/XrefMedicationRoutesCodes/5
        [Route("api/UserData/DeleteXrefMedicationRoutesCodes/{id}")]
        [ResponseType(typeof(tXrefMedicationRoutesCode))]
        public async Task<IHttpActionResult> DeletetXrefMedicationRoutesCode(int id)
        {
            tXrefMedicationRoutesCode tXrefMedicationRoutesCode = await db.tXrefMedicationRoutesCodes.FindAsync(id);
            if (tXrefMedicationRoutesCode == null)
            {
                return NotFound();
            }

            db.tXrefMedicationRoutesCodes.Remove(tXrefMedicationRoutesCode);
            await db.SaveChangesAsync();

            return Ok(tXrefMedicationRoutesCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefMedicationRoutesCodeExists(int id)
        {
            return db.tXrefMedicationRoutesCodes.Count(e => e.ID == id) > 0;
        }
    }
}