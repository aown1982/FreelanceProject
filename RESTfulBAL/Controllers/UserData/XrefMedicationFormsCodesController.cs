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
    public class XrefMedicationFormsCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefMedicationFormsCodes
        [Route("api/UserData/GetXrefMedicationFormsCodes")]
        public IQueryable<tXrefMedicationFormsCode> GettXrefMedicationFormsCodes()
        {
            return db.tXrefMedicationFormsCodes;
        }

        // GET: api/XrefMedicationFormsCodes/5
        [Route("api/UserData/GetXrefMedicationFormsCodes/{id}")]
        [ResponseType(typeof(tXrefMedicationFormsCode))]
        public async Task<IHttpActionResult> GettXrefMedicationFormsCode(int id)
        {
            tXrefMedicationFormsCode tXrefMedicationFormsCode = await db.tXrefMedicationFormsCodes.FindAsync(id);
            if (tXrefMedicationFormsCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefMedicationFormsCode);
        }

        // PUT: api/XrefMedicationFormsCodes/5
        [Route("api/UserData/UpdateXrefMedicationFormsCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefMedicationFormsCode(int id, tXrefMedicationFormsCode tXrefMedicationFormsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefMedicationFormsCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefMedicationFormsCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefMedicationFormsCodeExists(id))
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

        // POST: api/XrefMedicationFormsCodes
        [Route("api/UserData/PostXrefMedicationFormsCodes")]
        [ResponseType(typeof(tXrefMedicationFormsCode))]
        public async Task<IHttpActionResult> PosttXrefMedicationFormsCode(tXrefMedicationFormsCode tXrefMedicationFormsCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefMedicationFormsCodes.Add(tXrefMedicationFormsCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefMedicationFormsCode.ID }, tXrefMedicationFormsCode);
        }

        // DELETE: api/XrefMedicationFormsCodes/5
        [Route("api/UserData/DeleteXrefMedicationFormsCodes/{id}")]
        [ResponseType(typeof(tXrefMedicationFormsCode))]
        public async Task<IHttpActionResult> DeletetXrefMedicationFormsCode(int id)
        {
            tXrefMedicationFormsCode tXrefMedicationFormsCode = await db.tXrefMedicationFormsCodes.FindAsync(id);
            if (tXrefMedicationFormsCode == null)
            {
                return NotFound();
            }

            db.tXrefMedicationFormsCodes.Remove(tXrefMedicationFormsCode);
            await db.SaveChangesAsync();

            return Ok(tXrefMedicationFormsCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefMedicationFormsCodeExists(int id)
        {
            return db.tXrefMedicationFormsCodes.Count(e => e.ID == id) > 0;
        }
    }
}