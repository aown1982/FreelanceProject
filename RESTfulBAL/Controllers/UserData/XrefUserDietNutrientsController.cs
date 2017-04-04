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
    public class XrefUserDietNutrientsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserDietNutrients
        [Route("api/UserData/GetXrefUserDietNutrients")]
        public IQueryable<tXrefUserDietNutrient> GettXrefUserDietNutrients()
        {
            return db.tXrefUserDietNutrients;
        }

        // GET: api/XrefUserDietNutrients/5
        [Route("api/UserData/GetXrefUserDietNutrients/{id}")]
        [ResponseType(typeof(tXrefUserDietNutrient))]
        public async Task<IHttpActionResult> GettXrefUserDietNutrient(int id)
        {
            tXrefUserDietNutrient tXrefUserDietNutrient = await db.tXrefUserDietNutrients.FindAsync(id);
            if (tXrefUserDietNutrient == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserDietNutrient);
        }

        // PUT: api/XrefUserDietNutrients/5
        [Route("api/UserData/UpdateXrefUserDietNutrients/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserDietNutrient(int id, tXrefUserDietNutrient tXrefUserDietNutrient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserDietNutrient.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserDietNutrient).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserDietNutrientExists(id))
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

        // POST: api/XrefUserDietNutrients
        [Route("api/UserData/PostXrefUserDietNutrients")]
        [ResponseType(typeof(tXrefUserDietNutrient))]
        public async Task<IHttpActionResult> PosttXrefUserDietNutrient(tXrefUserDietNutrient tXrefUserDietNutrient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserDietNutrients.Add(tXrefUserDietNutrient);
            await db.SaveChangesAsync();

           // return CreatedAtRoute("DefaultApi", new { id = tXrefUserDietNutrient.ID }, tXrefUserDietNutrient);
         return  Ok(tXrefUserDietNutrient);
        }

        // DELETE: api/XrefUserDietNutrients/5
        [Route("api/UserData/DeleteXrefUserDietNutrients/{id}")]
        [ResponseType(typeof(tXrefUserDietNutrient))]
        public async Task<IHttpActionResult> DeletetXrefUserDietNutrient(int id)
        {
            tXrefUserDietNutrient tXrefUserDietNutrient = await db.tXrefUserDietNutrients.FindAsync(id);
            if (tXrefUserDietNutrient == null)
            {
                return NotFound();
            }

            db.tXrefUserDietNutrients.Remove(tXrefUserDietNutrient);
            await db.SaveChangesAsync();

            return Ok(tXrefUserDietNutrient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserDietNutrientExists(int id)
        {
            return db.tXrefUserDietNutrients.Count(e => e.ID == id) > 0;
        }

        // GET: api/Nutrients/5
        [Route("api/UserData/GetUserDietNutrient/{id}")]
        public async Task<IHttpActionResult> GetUserDietNutrient(int id)
        {
            var tNutrient = await db.tXrefUserDietNutrients
                .Include(u => u.tUnitsOfMeasure)
                .Include(u => u.tNutrient)
                .Where(u => u.UserDietID == id && u.SystemStatusID == 1)
                .Select(u => new
                {
                    u.tNutrient.Name,
                    u.NutrientID,
                    u.UOMID,
                    u.tUnitsOfMeasure.UnitOfMeasure,
                    u.ID,
                    u.Value,
                    u.UserDietID
                }).OrderBy(u=>u.Name).ToListAsync();

            if (tNutrient == null)
            {
                return NotFound();
            }

            return Ok(tNutrient);
        }
        // DELETE: api/UserTest/5
        [Route("api/UserData/SoftDeleteUserDietNutrient/{id}/{status}")]
        [ResponseType(typeof(tUserDiet))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserDietNutrient(int id, int status)
        {
            tXrefUserDietNutrient tXrefUserDietNutrient = await db.tXrefUserDietNutrients.FindAsync(id);
            if (tXrefUserDietNutrient == null)
            {
                return NotFound();
            }
           tXrefUserDietNutrient.SystemStatusID = status;
           tXrefUserDietNutrient.LastUpdatedDateTime = DateTime.Now;
            await db.SaveChangesAsync();
            return Ok(tXrefUserDietNutrient);
        }
    }
}