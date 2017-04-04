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
    public class PharmaciesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/Pharmacies
        [Route("api/UserData/GetPharmacies")]
        public IQueryable<tPharmacy> GettPharmacies()
        {
            return db.tPharmacies;
        }

        // GET: api/Pharmacies/
        [Route("api/UserData/GetPharmacies/{id}")]
        [ResponseType(typeof(tPharmacy))]
        public async Task<IHttpActionResult> GettPharmacy(int id)
        {
            tPharmacy tPharmacy = await db.tPharmacies.FindAsync(id);
            if (tPharmacy == null)
            {
                return NotFound();
            }

            return Ok(tPharmacy);
        }

        // PUT: api/Pharmacies/5
        [Route("api/UserData/UpdatePharmacies/{id}/Pharmacy")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttPharmacy(int id, tPharmacy Pharmacy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Pharmacy.ID)
            {
                return BadRequest();
            }

            db.Entry(Pharmacy).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tPharmacyExists(id))
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

        // POST: api/Pharmacies
        [Route("api/UserData/PostPharmacies/Pharmacy")]
        [ResponseType(typeof(tPharmacy))]
        public async Task<IHttpActionResult> PosttPharmacy(tPharmacy Pharmacy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tPharmacies.Add(Pharmacy);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = Pharmacy.ID }, Pharmacy);
        }

        // DELETE: api/Pharmacies/5
        [Route("api/UserData/DeletePharmacies/{id}")]
        [ResponseType(typeof(tPharmacy))]
        public async Task<IHttpActionResult> DeletetPharmacy(int id)
        {
            tPharmacy tPharmacy = await db.tPharmacies.FindAsync(id);
            if (tPharmacy == null)
            {
                return NotFound();
            }

            db.tPharmacies.Remove(tPharmacy);
            await db.SaveChangesAsync();

            return Ok(tPharmacy);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tPharmacyExists(int id)
        {
            return db.tPharmacies.Count(e => e.ID == id) > 0;
        }
    }
}