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
    public class MedicationFormsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/MedicationForms
        [Route("api/UserData/GetMedicationForms")]
        public IQueryable<tMedicationForm> GettMedicationForms()
        {
            return db.tMedicationForms;
        }

        // GET: api/MedicationForms/5
        [Route("api/UserData/GetMedicationForms/{id}")]
        [ResponseType(typeof(tMedicationForm))]
        public async Task<IHttpActionResult> GettMedicationForm(int id)
        {
            tMedicationForm tMedicationForm = await db.tMedicationForms.FindAsync(id);
            if (tMedicationForm == null)
            {
                return NotFound();
            }

            return Ok(tMedicationForm);
        }

        // PUT: api/MedicationForms/5
        [Route("api/UserData/UpdateMedicationForms/{id}/MedicationForm")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttMedicationForm(int id, tMedicationForm MedicationForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != MedicationForm.ID)
            {
                return BadRequest();
            }

            db.Entry(MedicationForm).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tMedicationFormExists(id))
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

        // POST: api/MedicationForms
        [Route("api/UserData/PostMedicationForms/MedicationForm")]
        [ResponseType(typeof(tMedicationForm))]
        public async Task<IHttpActionResult> PosttMedicationForm(tMedicationForm MedicationForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tMedicationForms.Add(MedicationForm);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = MedicationForm.ID }, MedicationForm);
        }

        // DELETE: api/MedicationForms/5
        [Route("api/UserData/DeleteMedicationForms/{id}")]
        [ResponseType(typeof(tMedicationForm))]
        public async Task<IHttpActionResult> DeletetMedicationForm(int id)
        {
            tMedicationForm tMedicationForm = await db.tMedicationForms.FindAsync(id);
            if (tMedicationForm == null)
            {
                return NotFound();
            }

            db.tMedicationForms.Remove(tMedicationForm);
            await db.SaveChangesAsync();

            return Ok(tMedicationForm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tMedicationFormExists(int id)
        {
            return db.tMedicationForms.Count(e => e.ID == id) > 0;
        }
    }
}