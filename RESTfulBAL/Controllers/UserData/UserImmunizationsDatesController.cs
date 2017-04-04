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
    public class UserImmunizationsDatesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserImmunizationsDates
        [Route("api/UserData/GetUserImmunizationsDates")]
        public IQueryable<tUserImmunizationsDate> GettUserImmunizationsDates()
        {
            return db.tUserImmunizationsDates;
        }

        // GET: api/UserImmunizationsDates/5
        [Route("api/UserData/GetUserImmunizationsDates/{id}")]
        [ResponseType(typeof(tUserImmunizationsDate))]
        public async Task<IHttpActionResult> GettUserImmunizationsDate(int id)
        {
            tUserImmunizationsDate tUserImmunizationsDate = await db.tUserImmunizationsDates.FindAsync(id);
            if (tUserImmunizationsDate == null)
            {
                return NotFound();
            }

            return Ok(tUserImmunizationsDate);
        }

        // PUT: api/UserImmunizationsDates/5
        [Route("api/UserData/UpdateUserImmunizationsDates/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserImmunizationsDate(int id, tUserImmunizationsDate tUserImmunizationsDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserImmunizationsDate.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserImmunizationsDate).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserImmunizationsDateExists(id))
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

        // POST: api/UserImmunizationsDates
        [Route("api/UserData/PostUserImmunizationsDates")]
        [ResponseType(typeof(tUserImmunizationsDate))]
        public async Task<IHttpActionResult> PosttUserImmunizationsDate(tUserImmunizationsDate tUserImmunizationsDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserImmunizationsDates.Add(tUserImmunizationsDate);
            await db.SaveChangesAsync();

            return Ok(tUserImmunizationsDate);
        }

        // DELETE: api/UserImmunizationsDates/5
        [Route("api/UserData/DeleteUserImmunizationsDates/{id}")]
        [ResponseType(typeof(tUserImmunizationsDate))]
        public async Task<IHttpActionResult> DeletetUserImmunizationsDate(int id)
        {
            tUserImmunizationsDate tUserImmunizationsDate = await db.tUserImmunizationsDates.FindAsync(id);
            if (tUserImmunizationsDate == null)
            {
                return NotFound();
            }

            db.tUserImmunizationsDates.Remove(tUserImmunizationsDate);
            await db.SaveChangesAsync();

            return Ok(tUserImmunizationsDate);
        }

        // DELETE: api/SoftDeleteUserImmunizationsDate/5/2
        [Route("api/UserData/SoftDeleteUserImmunizationsDate/{id}/{status}")]
        [ResponseType(typeof(tUserImmunizationsDate))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserImmunizationsDate(int id, int status)
        {
            tUserImmunizationsDate tUserImmunizationsDate = await db.tUserImmunizationsDates.FindAsync(id);
            if (tUserImmunizationsDate == null)
            {
                return NotFound();
            }

            tUserImmunizationsDate.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tUserImmunizationsDate);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserImmunizationsDateExists(int id)
        {
            return db.tUserImmunizationsDates.Count(e => e.ID == id) > 0;
        }
    }
}