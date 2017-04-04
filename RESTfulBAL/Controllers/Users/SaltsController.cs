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
using DAL.Users;

namespace RESTfulBAL.Controllers.Users
{
    public class SaltsController : ApiController
    {
        private UsersEntities db = new UsersEntities();

        // GET: api/Salts
        [Route("api/WebApp/GetSalts")]
        public IQueryable<tSalt> GettSalts()
        {
            return db.tSalts;
        }

        // GET: api/Salts/5
        [Route("api/WebApp/GetSalts/{id}")]
        [ResponseType(typeof(tSalt))]
        public async Task<IHttpActionResult> GettSalt(int id)
        {
            tSalt tSalt = await db.tSalts.FindAsync(id);
            if (tSalt == null)
            {
                return NotFound();
            }

            return Ok(tSalt);
        }

        // PUT: api/Salts/5
        [Route("api/WebApp/EditSalts/{id}")]
        [ResponseType(typeof(tSalt))]
        public async Task<IHttpActionResult> PuttSalt(int id, tSalt tSalt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tSalt.Id)
            {
                return BadRequest();
            }

            db.Entry(tSalt).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tSaltExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(tSalt);
        }
        [Route("api/WebApp/EditSalt/{id}")]
        [ResponseType(typeof(tSalt))]
        public async Task<tSalt> EditSalt(int id, tSalt tSalt)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }

            if (id != tSalt.Id)
            {
                return null;
            }

            db.Entry(tSalt).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tSaltExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return tSalt;
        }

        // POST: api/Salts
        [Route("api/WebApp/Salts/Add")]
        [ResponseType(typeof(tSalt))]
        public async Task<tSalt> PosttSalt(tSalt tSalt)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
            }

            db.tSalts.Add(tSalt);
            await db.SaveChangesAsync();
            return tSalt;
            //return CreatedAtRoute("DefaultApi", new { id = tSalt.Id }, tSalt);
        }

        // DELETE: api/Salts/5
        [Route("api/WebApp/DeleteSalts/{id}")]
        [ResponseType(typeof(tSalt))]
        public async Task<IHttpActionResult> DeletetSalt(int id)
        {
            tSalt tSalt = await db.tSalts.FindAsync(id);
            if (tSalt == null)
            {
                return NotFound();
            }

            db.tSalts.Remove(tSalt);
            await db.SaveChangesAsync();

            return Ok(tSalt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tSaltExists(int id)
        {
            return db.tSalts.Count(e => e.Id == id) > 0;
        }
    }
}