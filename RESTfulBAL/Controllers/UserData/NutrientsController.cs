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
    public class NutrientsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/Nutrients
        [Route("api/UserData/GetNutrients")]
        public IQueryable<tNutrient> GettNutrients()
        {
            return db.tNutrients;
        }

        // GET: api/Nutrients/5
        [Route("api/UserData/GetNutrients/{id}")]
        [ResponseType(typeof(tNutrient))]
        public async Task<IHttpActionResult> GettNutrient(int id)
        {
            tNutrient tNutrient = await db.tNutrients.FindAsync(id);
            if (tNutrient == null)
            {
                return NotFound();
            }

            return Ok(tNutrient);
        }

        // PUT: api/Nutrients/5
        [Route("api/UserData/UpdateNutrients/{id}/Nutrient")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttNutrient(int id, tNutrient Nutrient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Nutrient.ID)
            {
                return BadRequest();
            }

            db.Entry(Nutrient).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tNutrientExists(id))
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

        // POST: api/Nutrients
        [Route("api/UserData/PostNutrients/Nutrient")]
        [ResponseType(typeof(tNutrient))]
        public async Task<IHttpActionResult> PosttNutrient(tNutrient Nutrient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tNutrients.Add(Nutrient);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = Nutrient.ID }, Nutrient);
        }

        // DELETE: api/Nutrients/5
        [Route("api/UserData/DeleteNutrients/{id}")]
        [ResponseType(typeof(tNutrient))]
        public async Task<IHttpActionResult> DeletetNutrient(int id)
        {
            tNutrient tNutrient = await db.tNutrients.FindAsync(id);
            if (tNutrient == null)
            {
                return NotFound();
            }

            db.tNutrients.Remove(tNutrient);
            await db.SaveChangesAsync();

            return Ok(tNutrient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tNutrientExists(int id)
        {
            return db.tNutrients.Count(e => e.ID == id) > 0;
        }
    }
}