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
    public class UserCarePlansController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserCarePlans
        [Route("api/UserData/GetUserCarePlans")]
        public IQueryable<tUserCarePlan> GettUserCarePlans()
        {
            return db.tUserCarePlans;
        }

        // GET: api/UserCarePlans/5
        [Route("api/UserData/GetUserCarePlans/{id}")]
        [ResponseType(typeof(tUserCarePlan))]
        public async Task<IHttpActionResult> GettUserCarePlan(int id)
        {
            tUserCarePlan tUserCarePlan = await db.tUserCarePlans.FindAsync(id);
            if (tUserCarePlan == null)
            {
                return NotFound();
            }

            return Ok(tUserCarePlan);
        }

        // PUT: api/UserCarePlans/5
        [Route("api/UserData/UpdateUserCarePlans/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserCarePlan(int id, tUserCarePlan tUserCarePlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserCarePlan.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserCarePlan).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserCarePlanExists(id))
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

        // POST: api/UserCarePlans
        [Route("api/UserData/PostUserCarePlans")]
        [ResponseType(typeof(tUserCarePlan))]
        public async Task<IHttpActionResult> PosttUserCarePlan(tUserCarePlan tUserCarePlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserCarePlans.Add(tUserCarePlan);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tUserCarePlanExists(tUserCarePlan.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(tUserCarePlan);
        }

        // DELETE: api/UserCarePlans/5
        [Route("api/UserData/DeleteUserCarePlans/{id}")]
        [ResponseType(typeof(tUserCarePlan))]
        public async Task<IHttpActionResult> DeletetUserCarePlan(int id)
        {
            tUserCarePlan tUserCarePlan = await db.tUserCarePlans.FindAsync(id);
            if (tUserCarePlan == null)
            {
                return NotFound();
            }

            db.tUserCarePlans.Remove(tUserCarePlan);
            await db.SaveChangesAsync();

            return Ok(tUserCarePlan);
        }

        // DELETE: api/UserCarePlans/5/2
        [Route("api/UserData/SoftDeleteUserCarePlan/{id}/{status}")]
        [ResponseType(typeof(tUserCarePlan))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserCarePlan(int id, int status)
        {
            tUserCarePlan tUsercarePlan = await db.tUserCarePlans.FindAsync(id);
            if (tUsercarePlan == null)
            {
                return NotFound();
            }
            tUsercarePlan.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tUsercarePlan);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserCarePlanExists(int id)
        {
            return db.tUserCarePlans.Count(e => e.ID == id) > 0;
        }
    }
}