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
    public class UserGeneticTraitsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserGeneticTraits
        [Route("api/UserData/GetUserGeneticTraits")]
        public IQueryable<tUserGeneticTrait> GettUserGeneticTraits()
        {
            return db.tUserGeneticTraits;
        }

        // GET: api/UserGeneticTraits/5
        [Route("api/UserData/GetUserGeneticTraits/{id}")]
        [ResponseType(typeof(tUserGeneticTrait))]
        public async Task<IHttpActionResult> GettUserGeneticTrait(int id)
        {
            tUserGeneticTrait tUserGeneticTrait = await db.tUserGeneticTraits.FindAsync(id);
            if (tUserGeneticTrait == null)
            {
                return NotFound();
            }

            return Ok(tUserGeneticTrait);
        }

        // PUT: api/UserGeneticTraits/5
        [Route("api/UserData/UpdateUserGeneticTraits/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserGeneticTrait(int id, tUserGeneticTrait tUserGeneticTrait)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserGeneticTrait.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserGeneticTrait).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserGeneticTraitExists(id))
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

        // POST: api/UserGeneticTraits
        [Route("api/UserData/PostUserGeneticTraits")]
        [ResponseType(typeof(tUserGeneticTrait))]
        public async Task<IHttpActionResult> PosttUserGeneticTrait(tUserGeneticTrait tUserGeneticTrait)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserGeneticTraits.Add(tUserGeneticTrait);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tUserGeneticTrait.ID }, tUserGeneticTrait);
        }

        // DELETE: api/UserGeneticTraits/5
        [Route("api/UserData/DeleteUserGeneticTraits/{id}")]
        [ResponseType(typeof(tUserGeneticTrait))]
        public async Task<IHttpActionResult> DeletetUserGeneticTrait(int id)
        {
            tUserGeneticTrait tUserGeneticTrait = await db.tUserGeneticTraits.FindAsync(id);
            if (tUserGeneticTrait == null)
            {
                return NotFound();
            }

            db.tUserGeneticTraits.Remove(tUserGeneticTrait);
            await db.SaveChangesAsync();

            return Ok(tUserGeneticTrait);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserGeneticTraitExists(int id)
        {
            return db.tUserGeneticTraits.Count(e => e.ID == id) > 0;
        }
    }
}