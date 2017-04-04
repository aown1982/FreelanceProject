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
    public class ProviderPhoneNumbersController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/ProviderPhoneNumbers
        [Route("api/UserData/GetProviderPhoneNumbers")]
        public IQueryable<tProviderPhoneNumber> GettProviderPhoneNumbers()
        {
            return db.tProviderPhoneNumbers;
        }

        // GET: api/ProviderPhoneNumbers/5
        [Route("api/UserData/GetProviderPhoneNumbers/{id}")]
        [ResponseType(typeof(tProviderPhoneNumber))]
        public async Task<IHttpActionResult> GettProviderPhoneNumber(int id)
        {
            tProviderPhoneNumber tProviderPhoneNumber = await db.tProviderPhoneNumbers.FindAsync(id);
            if (tProviderPhoneNumber == null)
            {
                return NotFound();
            }

            return Ok(tProviderPhoneNumber);
        }

        // PUT: api/ProviderPhoneNumbers/5
        [Route("api/UserData/UpdateProviderPhoneNumbers/{id}/ProviderPhoneNumber")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttProviderPhoneNumber(int id, tProviderPhoneNumber ProviderPhoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ProviderPhoneNumber.ID)
            {
                return BadRequest();
            }

            db.Entry(ProviderPhoneNumber).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tProviderPhoneNumberExists(id))
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

        // POST: api/ProviderPhoneNumbers
        [Route("api/UserData/PostProviderPhoneNumbers/ProviderPhoneNumber")]
        [ResponseType(typeof(tProviderPhoneNumber))]
        public async Task<IHttpActionResult> PosttProviderPhoneNumber(tProviderPhoneNumber ProviderPhoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tProviderPhoneNumbers.Add(ProviderPhoneNumber);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ProviderPhoneNumber.ID }, ProviderPhoneNumber);
        }

        // DELETE: api/ProviderPhoneNumbers/5
        [Route("api/UserData/DeleteProviderPhoneNumbers/{id}")]
        [ResponseType(typeof(tProviderPhoneNumber))]
        public async Task<IHttpActionResult> DeletetProviderPhoneNumber(int id)
        {
            tProviderPhoneNumber tProviderPhoneNumber = await db.tProviderPhoneNumbers.FindAsync(id);
            if (tProviderPhoneNumber == null)
            {
                return NotFound();
            }

            db.tProviderPhoneNumbers.Remove(tProviderPhoneNumber);
            await db.SaveChangesAsync();

            return Ok(tProviderPhoneNumber);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tProviderPhoneNumberExists(int id)
        {
            return db.tProviderPhoneNumbers.Count(e => e.ID == id) > 0;
        }
    }
}