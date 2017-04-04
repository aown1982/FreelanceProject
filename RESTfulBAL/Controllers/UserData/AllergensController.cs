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
    public class AllergensController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/Allergens
        [Route("api/UserData/GetAllergen")]
        public IQueryable<tAllergen> GettAllergens()
        {
            return db.tAllergens
                        .Where(allergen => allergen.ParentID == null)
                        .OrderBy(allergen => allergen.Name);
        }

        // GET: api/Allergens/5        
        [Route("api/UserData/GetAllergen/{id}")]
        [ResponseType(typeof(tAllergen))]
        public async Task<IHttpActionResult> GettAllergen(int id)
        {
            tAllergen tAllergen = await db.tAllergens.FindAsync(id);
            if (tAllergen == null)
            {
                return NotFound();
            }

            return Ok(tAllergen);
        }

        // PUT: api/Allergens/5        
        [Route("api/UserData/UpdateAllergen/{id}/Allergen")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttAllergen(int id, tAllergen Allergen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Allergen.ID)
            {
                return BadRequest();
            }

            db.Entry(Allergen).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tAllergenExists(id))
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

        // POST: api/Allergens
        [Route("api/UserData/PostAllergen/Allergen")]
        [ResponseType(typeof(tAllergen))]
        public async Task<IHttpActionResult> PosttAllergen(tAllergen tAllergen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tAllergens.Add(tAllergen);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tAllergen.ID }, tAllergen);
        }

        // DELETE: api/Allergens/5
        [Route("api/UserData/DeleteAllergen/{id}")]
        [ResponseType(typeof(tAllergen))]
        public async Task<IHttpActionResult> DeletetAllergen(int id)
        {
            tAllergen tAllergen = await db.tAllergens.FindAsync(id);
            if (tAllergen == null)
            {
                return NotFound();
            }

            db.tAllergens.Remove(tAllergen);
            await db.SaveChangesAsync();

            return Ok(tAllergen);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tAllergenExists(int id)
        {
            return db.tAllergens.Count(e => e.ID == id) > 0;
        }
    }
}