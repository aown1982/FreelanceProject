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
    public class ReactionsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/Reactions
        [Route("api/UserData/GetReactions")]
        public IQueryable<tReaction> GettReactions()
        {
            return db.tReactions
                        .Where(reaction => reaction.ParentID == null)
                        .OrderBy(reaction => reaction.Name);
        }

        // GET: api/Reactions/5
        [Route("api/UserData/GetReactions/{id}")]
        [ResponseType(typeof(tReaction))]
        public async Task<IHttpActionResult> GettReaction(int id)
        {
            tReaction tReaction = await db.tReactions.FindAsync(id);
            if (tReaction == null)
            {
                return NotFound();
            }

            return Ok(tReaction);
        }

        // PUT: api/Reactions/5
        [Route("api/UserData/UpdateReactions/{id}/Reaction")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttReaction(int id, tReaction Reaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Reaction.ID)
            {
                return BadRequest();
            }

            db.Entry(Reaction).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tReactionExists(id))
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

        // POST: api/Reactions
        [Route("api/UserData/UpdateReactions/Reaction")]
        [ResponseType(typeof(tReaction))]
        public async Task<IHttpActionResult> PosttReaction(tReaction Reaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tReactions.Add(Reaction);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = Reaction.ID }, Reaction);
        }

        // DELETE: api/Reactions/5
        [Route("api/UserData/DeleteReactions/{id}")]
        [ResponseType(typeof(tReaction))]
        public async Task<IHttpActionResult> DeletetReaction(int id)
        {
            tReaction tReaction = await db.tReactions.FindAsync(id);
            if (tReaction == null)
            {
                return NotFound();
            }

            db.tReactions.Remove(tReaction);
            await db.SaveChangesAsync();

            return Ok(tReaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tReactionExists(int id)
        {
            return db.tReactions.Count(e => e.ID == id) > 0;
        }
    }
}