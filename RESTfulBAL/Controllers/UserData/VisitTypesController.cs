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
    public class VisitTypesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/VisitTypes
        [Route("api/UserData/GetVisitTypes")]
        public IQueryable<tVisitType> GettVisitTypes()
        {
            return db.tVisitTypes;
        }

        // GET: api/VisitTypes/5
        [Route("api/UserData/GetVisitTypes/{id}")]
        [ResponseType(typeof(tVisitType))]
        public async Task<IHttpActionResult> GettVisitType(int id)
        {
            tVisitType tVisitType = await db.tVisitTypes.FindAsync(id);
            if (tVisitType == null)
            {
                return NotFound();
            }

            return Ok(tVisitType);
        }

        // PUT: api/VisitTypes/5
        [Route("api/UserData/UpdateVisitTypes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttVisitType(int id, tVisitType tVisitType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tVisitType.ID)
            {
                return BadRequest();
            }

            db.Entry(tVisitType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tVisitTypeExists(id))
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

        // POST: api/VisitTypes
        [Route("api/UserData/PostVisitTypes")]
        [ResponseType(typeof(tVisitType))]
        public async Task<IHttpActionResult> PosttVisitType(tVisitType tVisitType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tVisitTypes.Add(tVisitType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tVisitType.ID }, tVisitType);
        }

        // DELETE: api/VisitTypes/5
        [Route("api/UserData/DeleteVisitTypes/{id}")]
        [ResponseType(typeof(tVisitType))]
        public async Task<IHttpActionResult> DeletetVisitType(int id)
        {
            tVisitType tVisitType = await db.tVisitTypes.FindAsync(id);
            if (tVisitType == null)
            {
                return NotFound();
            }

            db.tVisitTypes.Remove(tVisitType);
            await db.SaveChangesAsync();

            return Ok(tVisitType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tVisitTypeExists(int id)
        {
            return db.tVisitTypes.Count(e => e.ID == id) > 0;
        }
    }
}