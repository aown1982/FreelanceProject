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
    public class UserPrescriptionsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserPrescriptions
        [Route("api/UserData/GetUserPrescriptions")]
        public IQueryable<tUserPrescription> GettUserPrescriptions()
        {
            return db.tUserPrescriptions;
        }

        // GET: api/UserPrescriptions/5
        [Route("api/UserData/GetUserPrescriptions/{id}")]
        [ResponseType(typeof(tUserPrescription))]
        public async Task<IHttpActionResult> GettUserPrescription(int id)
        {
            tUserPrescription tUserPrescription = await db.tUserPrescriptions.FindAsync(id);
            if (tUserPrescription == null)
            {
                return NotFound();
            }

            return Ok(tUserPrescription);
        }

        // PUT: api/UserPrescriptions/5
        [Route("api/UserData/UpdateUserPrescriptions/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserPrescription(int id, tUserPrescription tUserPrescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserPrescription.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserPrescription).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserPrescriptionExists(id))
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

        // POST: api/UserPrescriptions
        [Route("api/UserData/PostUserPrescriptions")]
        [ResponseType(typeof(tUserPrescription))]
        public async Task<IHttpActionResult> PosttUserPrescription(tUserPrescription tUserPrescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserPrescriptions.Add(tUserPrescription);
            await db.SaveChangesAsync();

            return Ok(tUserPrescription);
        }

        // DELETE: api/UserPrescriptions/5
        [Route("api/UserData/DeleteUserPrescriptions/{id}")]
        [ResponseType(typeof(tUserPrescription))]
        public async Task<IHttpActionResult> DeletetUserPrescription(int id)
        {
            tUserPrescription tUserPrescription = await db.tUserPrescriptions.FindAsync(id);
            if (tUserPrescription == null)
            {
                return NotFound();
            }

            db.tUserPrescriptions.Remove(tUserPrescription);
            await db.SaveChangesAsync();

            return Ok(tUserPrescription);
        }
        // DELETE: api/SoftDeleteUserPrescription/5/2
        [Route("api/UserData/SoftDeleteUserPrescription/{id}/{status}")]
        [ResponseType(typeof(tUserPrescription))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserPrescription(int id, int status)
        {
            tUserPrescription tUserPrescription = await db.tUserPrescriptions.FindAsync(id);
            if (tUserPrescription == null)
            {
                return NotFound();
            }
            tUserPrescription.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tUserPrescription);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserPrescriptionExists(int id)
        {
            return db.tUserPrescriptions.Count(e => e.ID == id) > 0;
        }
    }
}