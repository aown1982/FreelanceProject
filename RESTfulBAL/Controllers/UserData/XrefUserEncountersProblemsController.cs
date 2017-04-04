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
    public class XrefUserEncountersProblemsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserEncountersProblems
        [Route("api/UserData/GetXrefUserEncountersProblems")]
        public IQueryable<tXrefUserEncountersProblem> GettXrefUserEncountersProblems()
        {
            return db.tXrefUserEncountersProblems;
        }

        // GET: api/XrefUserEncountersProblems/5
        [Route("api/UserData/GetXrefUserEncountersProblems/{id}")]
        [ResponseType(typeof(tXrefUserEncountersProblem))]
        public async Task<IHttpActionResult> GettXrefUserEncountersProblem(int id)
        {
            tXrefUserEncountersProblem tXrefUserEncountersProblem = await db.tXrefUserEncountersProblems.FindAsync(id);
            if (tXrefUserEncountersProblem == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserEncountersProblem);
        }

        // PUT: api/XrefUserEncountersProblems/5
        [Route("api/UserData/UpdateXrefUserEncountersProblems/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserEncountersProblem(int id, tXrefUserEncountersProblem tXrefUserEncountersProblem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserEncountersProblem.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserEncountersProblem).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserEncountersProblemExists(id))
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

        // POST: api/XrefUserEncountersProblems
        [Route("api/UserData/PostXrefUserEncountersProblems")]
        [ResponseType(typeof(tXrefUserEncountersProblem))]
        public async Task<IHttpActionResult> PosttXrefUserEncountersProblem(tXrefUserEncountersProblem tXrefUserEncountersProblem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserEncountersProblems.Add(tXrefUserEncountersProblem);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserEncountersProblem.ID }, tXrefUserEncountersProblem);
        }

        // DELETE: api/XrefUserEncountersProblems/5
        [Route("api/UserData/DeleteXrefUserEncountersProblems/{id}")]
        [ResponseType(typeof(tXrefUserEncountersProblem))]
        public async Task<IHttpActionResult> DeletetXrefUserEncountersProblem(int id)
        {
            tXrefUserEncountersProblem tXrefUserEncountersProblem = await db.tXrefUserEncountersProblems.FindAsync(id);
            if (tXrefUserEncountersProblem == null)
            {
                return NotFound();
            }

            db.tXrefUserEncountersProblems.Remove(tXrefUserEncountersProblem);
            await db.SaveChangesAsync();

            return Ok(tXrefUserEncountersProblem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserEncountersProblemExists(int id)
        {
            return db.tXrefUserEncountersProblems.Count(e => e.ID == id) > 0;
        }
    }
}