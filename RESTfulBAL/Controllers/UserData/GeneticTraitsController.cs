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
    public class GeneticTraitsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/GeneticTraits
        [Route("api/UserData/GetGeneticTraits")]
        public IQueryable<tGeneticTrait> GettGeneticTraits()
        {
            return db.tGeneticTraits;
        }

        // GET: api/GeneticTraits/5
        [Route("api/UserData/GetGeneticTraits/{id}")]
        [ResponseType(typeof(tGeneticTrait))]
        public async Task<IHttpActionResult> GettGeneticTrait(int id)
        {
            tGeneticTrait tGeneticTrait = await db.tGeneticTraits.FindAsync(id);
            if (tGeneticTrait == null)
            {
                return NotFound();
            }

            return Ok(tGeneticTrait);
        }

        // PUT: api/GeneticTraits/5
        [Route("api/UserData/UpdateGeneticTraits/{id}/GeneticTrait")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttGeneticTrait(int id, tGeneticTrait GeneticTrait)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != GeneticTrait.ID)
            {
                return BadRequest();
            }

            db.Entry(GeneticTrait).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tGeneticTraitExists(id))
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

        // POST: api/GeneticTraits
        [Route("api/UserData/PostGeneticTraits/GeneticTrait")]
        [ResponseType(typeof(tGeneticTrait))]
        public async Task<IHttpActionResult> PosttGeneticTrait(tGeneticTrait GeneticTrait)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tGeneticTraits.Add(GeneticTrait);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = GeneticTrait.ID }, GeneticTrait);
        }

        // DELETE: api/GeneticTraits/5
        [Route("api/UserData/DeleteGeneticTraits/{id}")]
        [ResponseType(typeof(tGeneticTrait))]
        public async Task<IHttpActionResult> DeletetGeneticTrait(int id)
        {
            tGeneticTrait tGeneticTrait = await db.tGeneticTraits.FindAsync(id);
            if (tGeneticTrait == null)
            {
                return NotFound();
            }

            db.tGeneticTraits.Remove(tGeneticTrait);
            await db.SaveChangesAsync();

            return Ok(tGeneticTrait);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tGeneticTraitExists(int id)
        {
            return db.tGeneticTraits.Count(e => e.ID == id) > 0;
        }
    }
}