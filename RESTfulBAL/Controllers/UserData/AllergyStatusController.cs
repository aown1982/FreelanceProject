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
    public class AllergyStatusController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/AllergyStatus
        [Route("api/UserData/GetAllergyStatus")]
        public IQueryable<tAllergyStatus> GettAllergyStatuses()
        {
            return db.tAllergyStatuses
                        .Where(allergyStatus => allergyStatus.ParentID == null)
                        .OrderBy(allergyStatus => allergyStatus.Status);
        }

        // GET: api/AllergyStatus/5
        [Route("api/UserData/GetAllergyStatus/{id}")]
        [ResponseType(typeof(tAllergyStatus))]
        public async Task<IHttpActionResult> GettAllergyStatus(int id)
        {
            tAllergyStatus tAllergyStatus = await db.tAllergyStatuses.FindAsync(id);
            if (tAllergyStatus == null)
            {
                return NotFound();
            }

            return Ok(tAllergyStatus);
        }

        // PUT: api/AllergyStatus/5
        [Route("api/UserData/UpdateAllergyStatus/{id}/AllergyStatus")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttAllergyStatus(int id, tAllergyStatus AllergyStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != AllergyStatus.ID)
            {
                return BadRequest();
            }

            db.Entry(AllergyStatus).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tAllergyStatusExists(id))
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

        // POST: api/AllergyStatus
        [Route("api/UserData/PostAllergyStatus/AllergyStatus")]
        [ResponseType(typeof(tAllergyStatus))]
        public async Task<IHttpActionResult> PosttAllergyStatus(tAllergyStatus AllergyStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tAllergyStatuses.Add(AllergyStatus);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = AllergyStatus.ID }, AllergyStatus);
        }

        // DELETE: api/AllergyStatus/5
        [Route("api/UserData/DeleteAllergyStatus/{id}")]
        [ResponseType(typeof(tAllergyStatus))]
        public async Task<IHttpActionResult> DeletetAllergyStatus(int id)
        {
            tAllergyStatus tAllergyStatus = await db.tAllergyStatuses.FindAsync(id);
            if (tAllergyStatus == null)
            {
                return NotFound();
            }

            db.tAllergyStatuses.Remove(tAllergyStatus);
            await db.SaveChangesAsync();

            return Ok(tAllergyStatus);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tAllergyStatusExists(int id)
        {
            return db.tAllergyStatuses.Count(e => e.ID == id) > 0;
        }
    }
}