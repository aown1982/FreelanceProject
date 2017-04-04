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
    public class UserInstructionsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserInstructions
        [Route("api/UserData/GetUserInstructions")]
        public IQueryable<tUserInstruction> GettUserInstructions()
        {
            return db.tUserInstructions;
        }

        // GET: api/UserInstructions/5
        [Route("api/UserData/GetUserInstructions/{id}")]
        [ResponseType(typeof(tUserInstruction))]
        public async Task<IHttpActionResult> GettUserInstruction(int id)
        {
            tUserInstruction tUserInstruction = await db.tUserInstructions.FindAsync(id);
            if (tUserInstruction == null)
            {
                return NotFound();
            }

            return Ok(tUserInstruction);
        }

        // PUT: api/UserInstructions/5
        [Route("api/UserData/UpdateUserInstructions/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserInstruction(int id, tUserInstruction tUserInstruction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserInstruction.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserInstruction).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserInstructionExists(id))
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

        // POST: api/UserInstructions
        [Route("api/UserData/PostUserInstructions")]
        [ResponseType(typeof(tUserInstruction))]
        public async Task<IHttpActionResult> PosttUserInstruction(tUserInstruction tUserInstruction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserInstructions.Add(tUserInstruction);
            await db.SaveChangesAsync();

            return Ok(tUserInstruction);
        }

        // DELETE: api/UserInstructions/5
        [Route("api/UserData/DeleteUserInstructions/{id}")]
        [ResponseType(typeof(tUserInstruction))]
        public async Task<IHttpActionResult> DeletetUserInstruction(int id)
        {
            tUserInstruction tUserInstruction = await db.tUserInstructions.FindAsync(id);
            if (tUserInstruction == null)
            {
                return NotFound();
            }

            db.tUserInstructions.Remove(tUserInstruction);
            await db.SaveChangesAsync();

            return Ok(tUserInstruction);
        }

        // DELETE: api/SoftDeleteUserInstruction/5/2
        [Route("api/UserData/SoftDeleteUserInstruction/{id}/{status}")]
        [ResponseType(typeof(tUserInstruction))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserInstruction(int id, int status)
        {
            tUserInstruction tUserInstruction = await db.tUserInstructions.FindAsync(id);
            if (tUserInstruction == null)
            {
                return NotFound();
            }
            tUserInstruction.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tUserInstruction);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserInstructionExists(int id)
        {
            return db.tUserInstructions.Count(e => e.ID == id) > 0;
        }
    }
}