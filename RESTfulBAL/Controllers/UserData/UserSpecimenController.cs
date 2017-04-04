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
    public class UserSpecimenController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserSpecimen
        [Route("api/UserData/GetUserSpecimens")]
        public IQueryable<tUserSpecimen> GettUserSpecimens()
        {
            return db.tUserSpecimens;
        }

        // GET: api/UserSpecimen/5
        [Route("api/UserData/GetUserSpecimens/{id}")]
        [ResponseType(typeof(tUserSpecimen))]
        public async Task<IHttpActionResult> GettUserSpecimen(int id)
        {
            tUserSpecimen tUserSpecimen = await db.tUserSpecimens.FindAsync(id);
            if (tUserSpecimen == null)
            {
                return NotFound();
            }

            return Ok(tUserSpecimen);
        }

        // PUT: api/UserSpecimen/5
        [Route("api/UserData/UpdateUserSpecimens/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserSpecimen(int id, tUserSpecimen tUserSpecimen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserSpecimen.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserSpecimen).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserSpecimenExists(id))
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

        // POST: api/UserSpecimen
        [Route("api/UserData/PostUserSpecimens")]
        [ResponseType(typeof(tUserSpecimen))]
        public async Task<IHttpActionResult> PosttUserSpecimen(tUserSpecimen tUserSpecimen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserSpecimens.Add(tUserSpecimen);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserSpecimen.ID }, tUserSpecimen);
        }

        // DELETE: api/UserSpecimen/5
        [Route("api/UserData/DeleteUserSpecimens/{id}")]
        [ResponseType(typeof(tUserSpecimen))]
        public async Task<IHttpActionResult> DeletetUserSpecimen(int id)
        {
            tUserSpecimen tUserSpecimen = await db.tUserSpecimens.FindAsync(id);
            if (tUserSpecimen == null)
            {
                return NotFound();
            }

            db.tUserSpecimens.Remove(tUserSpecimen);
            await db.SaveChangesAsync();

            return Ok(tUserSpecimen);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserSpecimenExists(int id)
        {
            return db.tUserSpecimens.Count(e => e.ID == id) > 0;
        }
    }
}