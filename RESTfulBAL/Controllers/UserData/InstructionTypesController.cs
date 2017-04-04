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
    public class InstructionTypesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/InstructionTypes
        [Route("api/UserData/GetInstructionTypes")]
        public IQueryable<tInstructionType> GettInstructionTypes()
        {
            return db.tInstructionTypes;
        }

        // GET: api/InstructionTypes/5
        [Route("api/UserData/GetInstructionTypes/{id}")]
        [ResponseType(typeof(tInstructionType))]
        public async Task<IHttpActionResult> GettInstructionType(int id)
        {
            tInstructionType tInstructionType = await db.tInstructionTypes.FindAsync(id);
            if (tInstructionType == null)
            {
                return NotFound();
            }

            return Ok(tInstructionType);
        }

        // PUT: api/InstructionTypes/5
        [Route("api/UserData/UpdateInstructionTypes/{id}/InstructionType")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttInstructionType(int id, tInstructionType InstructionType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != InstructionType.ID)
            {
                return BadRequest();
            }

            db.Entry(InstructionType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tInstructionTypeExists(id))
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

        // POST: api/InstructionTypes
        [Route("api/UserData/PostInstructionTypes/InstructionType")]
        [ResponseType(typeof(tInstructionType))]
        public async Task<IHttpActionResult> PosttInstructionType(tInstructionType InstructionType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tInstructionTypes.Add(InstructionType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = InstructionType.ID }, InstructionType);
        }

        // DELETE: api/InstructionTypes/5
        [ResponseType(typeof(tInstructionType))]
        [Route("api/UserData/DeleteInstructionTypes/{id}")]
        public async Task<IHttpActionResult> DeletetInstructionType(int id)
        {
            tInstructionType tInstructionType = await db.tInstructionTypes.FindAsync(id);
            if (tInstructionType == null)
            {
                return NotFound();
            }

            db.tInstructionTypes.Remove(tInstructionType);
            await db.SaveChangesAsync();

            return Ok(tInstructionType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tInstructionTypeExists(int id)
        {
            return db.tInstructionTypes.Count(e => e.ID == id) > 0;
        }
    }
}