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
    public class UserAllergiesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserAllergies
        [Route("api/UserData/GetUserAllergies")]
        public IQueryable<tUserAllergy> GettUserAllergies()
        {
            return db.tUserAllergies;
        }

        // GET: api/UserAllergies/5
        [Route("api/UserData/GetUserAllergies/{id}")]
        [ResponseType(typeof(tUserAllergy))]
        public async Task<IHttpActionResult> GettUserAllergy(int id)
        {
            tUserAllergy tUserAllergy = await db.tUserAllergies.FindAsync(id);
            if (tUserAllergy == null)
            {
                return NotFound();
            }

            return Ok(tUserAllergy);
        }

        // PUT: api/UserAllergies/5
        [Route("api/UserData/UpdateUserAllergies/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserAllergy(int id, tUserAllergy tUserAllergy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserAllergy.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserAllergy).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserAllergyExists(id))
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

        // POST: api/UserAllergies
        [Route("api/UserData/PostUserAllergies")]
        [ResponseType(typeof(tUserAllergy))]
        public async Task<IHttpActionResult> PosttUserAllergy(tUserAllergy tUserAllergy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserAllergies.Add(tUserAllergy);
            await db.SaveChangesAsync();

            //return CreatedAtRoute("DefaultApi", new { id = tUserAllergy.ID }, tUserAllergy);
            return Ok(tUserAllergy);
        }

        // DELETE: api/UserAllergies/5
        [Route("api/UserData/DeleteUserAllergies/{id}")]
        [ResponseType(typeof(tUserAllergy))]
        public async Task<IHttpActionResult> DeletetUserAllergy(int id)
        {
            tUserAllergy tUserAllergy = await db.tUserAllergies.FindAsync(id);
            if (tUserAllergy == null)
            {
                return NotFound();
            }

            db.tUserAllergies.Remove(tUserAllergy);
            await db.SaveChangesAsync();

            return Ok(tUserAllergy);
        }

        // DELETE: api/UserAllergies/5
        [Route("api/UserData/SoftDeleteUserAllergies/{id}/{status}")]
        [ResponseType(typeof(tUserAllergy))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeletetUserAllergy(int id,int status)
        {
            tUserAllergy tUserAllergy = await db.tUserAllergies.FindAsync(id);
            if (tUserAllergy == null)
            {
                return NotFound();
            }
            tUserAllergy.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tUserAllergy);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserAllergyExists(int id)
        {
            return db.tUserAllergies.Count(e => e.ID == id) > 0;
        }
    }
}