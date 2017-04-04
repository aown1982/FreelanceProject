using DAL;
using DAL.UserData;
using RESTfulBAL.Models.UserData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RESTfulBAL.Controllers.UserData
{
    public class UserDerivedNotesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserDerivedNote
        [Route("api/UserData/GetUserDerivedNotes")]
        public IQueryable<tUserDerivedNote> GetUserDerivedNotes()
        {
            return db.tUserDerivedNotes;
        }

        // PUT: api/UserDerivedNote/5
        [Route("api/UserData/UpdateUserDerivedNote/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserDerivedNote(int id, tUserAllergy tUserDerivedNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserDerivedNote.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserDerivedNote).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserDerivedNotesExists(id))
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

        // POST: api/UserDerivedNote
        [Route("api/UserData/PostUserDerivedNote")]
        [ResponseType(typeof(tUserDerivedNote))]
        public async Task<IHttpActionResult> PostUserDerivedNote(tUserDerivedNote tUserDerivedNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserDerivedNotes.Add(tUserDerivedNote);
            await db.SaveChangesAsync();

            return Ok(tUserDerivedNote);
        }

        // DELETE: api/UserDerivedNote/5
        [Route("api/UserData/DeleteUserDerivedNote/{id}")]
        [ResponseType(typeof(tUserDerivedNote))]
        public async Task<IHttpActionResult> DeletetUserDerivedNote(int id)
        {
            tUserDerivedNote tUserDerivedNote = await db.tUserDerivedNotes.FindAsync(id);
            if (tUserDerivedNote == null)
            {
                return NotFound();
            }

            db.tUserDerivedNotes.Remove(tUserDerivedNote);
            await db.SaveChangesAsync();

            return Ok(tUserDerivedNote);
        }

        [Route("api/UserData/SoftDeletetDerivedNote/{id}/{status}")]
        [ResponseType(typeof(tUserDerivedNote))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeletetDerivedNote(int id, int status)
        {
            tUserDerivedNote tDerivedNote = await db.tUserDerivedNotes.FindAsync(id);
            if (tDerivedNote == null)
            {
                return NotFound();
            }
            tDerivedNote.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tDerivedNote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserDerivedNotesExists(int id)
        {
            return db.tUserDerivedNotes.Count(e => e.ID == id) > 0;
        }
    }
}
