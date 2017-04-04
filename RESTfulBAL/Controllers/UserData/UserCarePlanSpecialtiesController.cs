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
    public class UserCarePlanSpecialtiesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserCarePlanSpecialties
        [Route("api/UserData/GetUserCarePlanSpecialties")]
        public IQueryable<tUserCarePlanSpecialty> GettUserCarePlanSpecialties()
        {
            return db.tUserCarePlanSpecialties;
        }

        // GET: api/UserCarePlanSpecialties/5
        [Route("api/UserData/GetUserCarePlanSpecialties/{id}")]
        [ResponseType(typeof(tUserCarePlanSpecialty))]
        public async Task<IHttpActionResult> GettUserCarePlanSpecialty(int id)
        {
            tUserCarePlanSpecialty tUserCarePlanSpecialty = await db.tUserCarePlanSpecialties.FindAsync(id);
            if (tUserCarePlanSpecialty == null)
            {
                return NotFound();
            }

            return Ok(tUserCarePlanSpecialty);
        }

        // PUT: api/UserCarePlanSpecialties/5
        [Route("api/UserData/UpdateUserCarePlanSpecialties/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserCarePlanSpecialty(int id, tUserCarePlanSpecialty tUserCarePlanSpecialty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserCarePlanSpecialty.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserCarePlanSpecialty).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserCarePlanSpecialtyExists(id))
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

        // POST: api/UserCarePlanSpecialties
        [Route("api/UserData/PostUserCarePlanSpecialties")]
        [ResponseType(typeof(tUserCarePlanSpecialty))]
        public async Task<IHttpActionResult> PosttUserCarePlanSpecialty(tUserCarePlanSpecialty tUserCarePlanSpecialty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserCarePlanSpecialties.Add(tUserCarePlanSpecialty);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserCarePlanSpecialty.ID }, tUserCarePlanSpecialty);
        }

        // DELETE: api/UserCarePlanSpecialties/5
        [Route("api/UserData/DeleteUserCarePlanSpecialties/{id}")]
        [ResponseType(typeof(tUserCarePlanSpecialty))]
        public async Task<IHttpActionResult> DeletetUserCarePlanSpecialty(int id)
        {
            tUserCarePlanSpecialty tUserCarePlanSpecialty = await db.tUserCarePlanSpecialties.FindAsync(id);
            if (tUserCarePlanSpecialty == null)
            {
                return NotFound();
            }

            db.tUserCarePlanSpecialties.Remove(tUserCarePlanSpecialty);
            await db.SaveChangesAsync();

            return Ok(tUserCarePlanSpecialty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserCarePlanSpecialtyExists(int id)
        {
            return db.tUserCarePlanSpecialties.Count(e => e.ID == id) > 0;
        }
    }
}