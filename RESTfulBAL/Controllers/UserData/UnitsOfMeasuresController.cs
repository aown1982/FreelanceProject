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
    public class UnitsOfMeasuresController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UnitsOfMeasures
        [Route("api/UserData/GetUnitsOfMeasures")]
        public IQueryable<tUnitsOfMeasure> GettUnitsOfMeasures()
        {
            return db.tUnitsOfMeasures;
        }

        // GET: api/UnitsOfMeasures/5
        [Route("api/UserData/GetUnitsOfMeasures/{id}")]
        [ResponseType(typeof(tUnitsOfMeasure))]
        public async Task<IHttpActionResult> GettUnitsOfMeasure(int id)
        {
            tUnitsOfMeasure tUnitsOfMeasure = await db.tUnitsOfMeasures.FindAsync(id);
            if (tUnitsOfMeasure == null)
            {
                return NotFound();
            }

            return Ok(tUnitsOfMeasure);
        }

        // PUT: api/UnitsOfMeasures/5
        [Route("api/UserData/UpdateUnitsOfMeasures/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUnitsOfMeasure(int id, tUnitsOfMeasure tUnitsOfMeasure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUnitsOfMeasure.ID)
            {
                return BadRequest();
            }

            db.Entry(tUnitsOfMeasure).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUnitsOfMeasureExists(id))
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

        // POST: api/UnitsOfMeasures
        [Route("api/UserData/PostUnitsOfMeasures")]
        [ResponseType(typeof(tUnitsOfMeasure))]
        public async Task<IHttpActionResult> PosttUnitsOfMeasure(tUnitsOfMeasure tUnitsOfMeasure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUnitsOfMeasures.Add(tUnitsOfMeasure);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUnitsOfMeasure.ID }, tUnitsOfMeasure);
        }

        // DELETE: api/UnitsOfMeasures/5
        [Route("api/UserData/DeleteUnitsOfMeasures/{id}")]
        [ResponseType(typeof(tUnitsOfMeasure))]
        public async Task<IHttpActionResult> DeletetUnitsOfMeasure(int id)
        {
            tUnitsOfMeasure tUnitsOfMeasure = await db.tUnitsOfMeasures.FindAsync(id);
            if (tUnitsOfMeasure == null)
            {
                return NotFound();
            }

            db.tUnitsOfMeasures.Remove(tUnitsOfMeasure);
            await db.SaveChangesAsync();

            return Ok(tUnitsOfMeasure);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUnitsOfMeasureExists(int id)
        {
            return db.tUnitsOfMeasures.Count(e => e.ID == id) > 0;
        }
    }
}