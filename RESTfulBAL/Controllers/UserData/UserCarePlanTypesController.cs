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
    public class UserCarePlanTypesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserCarePlanTypes
        [Route("api/UserData/GetUserCarePlanTypes")]
        public IQueryable<tUserCarePlanType> GettUserCarePlanTypes()
        {
            return db.tUserCarePlanTypes;
        }

        // GET: api/UserCarePlanTypes/5
        [Route("api/UserData/GetUserCarePlanTypes/{id}")]
        [ResponseType(typeof(tUserCarePlanType))]
        public async Task<IHttpActionResult> GettUserCarePlanType(int id)
        {
            tUserCarePlanType tUserCarePlanType = await db.tUserCarePlanTypes.FindAsync(id);
            if (tUserCarePlanType == null)
            {
                return NotFound();
            }

            return Ok(tUserCarePlanType);
        }

        // PUT: api/UserCarePlanTypes/5
        [Route("api/UserData/UpdateUserCarePlanTypes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserCarePlanType(int id, tUserCarePlanType tUserCarePlanType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserCarePlanType.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserCarePlanType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserCarePlanTypeExists(id))
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

        // POST: api/UserCarePlanTypes
        [Route("api/UserData/PostUserCarePlanTypes")]
        [ResponseType(typeof(tUserCarePlanType))]
        public async Task<IHttpActionResult> PosttUserCarePlanType(tUserCarePlanType tUserCarePlanType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserCarePlanTypes.Add(tUserCarePlanType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserCarePlanType.ID }, tUserCarePlanType);
        }

        // DELETE: api/UserCarePlanTypes/5
        [Route("api/UserData/DeleteUserCarePlanTypes/{id}")]
        [ResponseType(typeof(tUserCarePlanType))]
        public async Task<IHttpActionResult> DeletetUserCarePlanType(int id)
        {
            tUserCarePlanType tUserCarePlanType = await db.tUserCarePlanTypes.FindAsync(id);
            if (tUserCarePlanType == null)
            {
                return NotFound();
            }

            db.tUserCarePlanTypes.Remove(tUserCarePlanType);
            await db.SaveChangesAsync();

            return Ok(tUserCarePlanType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserCarePlanTypeExists(int id)
        {
            return db.tUserCarePlanTypes.Count(e => e.ID == id) > 0;
        }
    }
}