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
    public class NutrientTypesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/NutrientTypes
        [Route("api/UserData/GetNutrientTypes")]
        public IQueryable<tNutrientType> GettNutrientTypes()
        {
            return db.tNutrientTypes;
        }

        // GET: api/NutrientTypes/5
        [Route("api/UserData/GetNutrientTypes/{id}")]
        [ResponseType(typeof(tNutrientType))]
        public async Task<IHttpActionResult> GettNutrientType(int id)
        {
            tNutrientType tNutrientType = await db.tNutrientTypes.FindAsync(id);
            if (tNutrientType == null)
            {
                return NotFound();
            }

            return Ok(tNutrientType);
        }

        // PUT: api/NutrientTypes/5
        [Route("api/UserData/UpdateNutrientTypes/{id}/NutrientType")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttNutrientType(int id, tNutrientType NutrientType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != NutrientType.ID)
            {
                return BadRequest();
            }

            db.Entry(NutrientType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tNutrientTypeExists(id))
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

        // POST: api/NutrientTypes
        [Route("api/UserData/PostNutrientTypes/NutrientType")]
        [ResponseType(typeof(tNutrientType))]
        public async Task<IHttpActionResult> PosttNutrientType(tNutrientType NutrientType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tNutrientTypes.Add(NutrientType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = NutrientType.ID }, NutrientType);
        }

        // DELETE: api/NutrientTypes/5
        [Route("api/UserData/DeleteNutrientTypes/{id}")]
        [ResponseType(typeof(tNutrientType))]
        public async Task<IHttpActionResult> DeletetNutrientType(int id)
        {
            tNutrientType tNutrientType = await db.tNutrientTypes.FindAsync(id);
            if (tNutrientType == null)
            {
                return NotFound();
            }

            db.tNutrientTypes.Remove(tNutrientType);
            await db.SaveChangesAsync();

            return Ok(tNutrientType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tNutrientTypeExists(int id)
        {
            return db.tNutrientTypes.Count(e => e.ID == id) > 0;
        }
    }
}