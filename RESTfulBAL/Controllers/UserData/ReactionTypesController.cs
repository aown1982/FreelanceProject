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
    public class ReactionTypesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/ReactionTypes
        [Route("api/UserData/GetReactionTypes")]
        public IQueryable<tReactionType> GettReactionTypes()
        {
            return db.tReactionTypes;
        }

        // GET: api/ReactionTypes/5
        [Route("api/UserData/GetReactionTypes/{id}")]
        [ResponseType(typeof(tReactionType))]
        public async Task<IHttpActionResult> GettReactionType(int id)
        {
            tReactionType tReactionType = await db.tReactionTypes.FindAsync(id);
            if (tReactionType == null)
            {
                return NotFound();
            }

            return Ok(tReactionType);
        }

        // PUT: api/ReactionTypes/5
        [Route("api/UserData/UpdateReactionTypes/{id}/ReactionType")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttReactionType(int id, tReactionType ReactionType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ReactionType.ID)
            {
                return BadRequest();
            }

            db.Entry(ReactionType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tReactionTypeExists(id))
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

        // POST: api/ReactionTypes
        [Route("api/UserData/PostReactionTypes/ReactionType")]
        [ResponseType(typeof(tReactionType))]
        public async Task<IHttpActionResult> PosttReactionType(tReactionType ReactionType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tReactionTypes.Add(ReactionType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ReactionType.ID }, ReactionType);
        }

        // DELETE: api/ReactionTypes/5
        [Route("api/UserData/DeleteReactionTypes/{id}")]
        [ResponseType(typeof(tReactionType))]
        public async Task<IHttpActionResult> DeletetReactionType(int id)
        {
            tReactionType tReactionType = await db.tReactionTypes.FindAsync(id);
            if (tReactionType == null)
            {
                return NotFound();
            }

            db.tReactionTypes.Remove(tReactionType);
            await db.SaveChangesAsync();

            return Ok(tReactionType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tReactionTypeExists(int id)
        {
            return db.tReactionTypes.Count(e => e.ID == id) > 0;
        }
    }
}