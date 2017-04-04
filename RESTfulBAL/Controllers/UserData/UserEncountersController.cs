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
    public class UserEncountersController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserEncounters
        [Route("api/UserData/GetUserEncounters")]
        public IQueryable<tUserEncounter> GettUserEncounters()
        {
            return db.tUserEncounters;
        }

        // GET: api/UserEncounters/5
        [Route("api/UserData/GetUserEncounters/{id}")]
        [ResponseType(typeof(tUserEncounter))]
        public async Task<IHttpActionResult> GettUserEncounter(int id)
        {
            tUserEncounter tUserEncounter = await db.tUserEncounters.FindAsync(id);
            if (tUserEncounter == null)
            {
                return NotFound();
            }

            return Ok(tUserEncounter);
        }

        // PUT: api/UserEncounters/5
        [Route("api/UserData/UpdateUserEncounters/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserEncounter(int id, tUserEncounter tUserEncounter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserEncounter.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserEncounter).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserEncounterExists(id))
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

        // POST: api/UserEncounters
        [Route("api/UserData/PostUserEncounters")]
        [ResponseType(typeof(tUserEncounter))]
        public async Task<IHttpActionResult> PosttUserEncounter(tUserEncounter tUserEncounter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserEncounters.Add(tUserEncounter);
            await db.SaveChangesAsync();

            return Ok(tUserEncounter);
        }

        // DELETE: api/UserEncounters/5
        [Route("api/UserData/DeleteUserEncounters/{id}")]
        [ResponseType(typeof(tUserEncounter))]
        public async Task<IHttpActionResult> DeletetUserEncounter(int id)
        {
            tUserEncounter tUserEncounter = await db.tUserEncounters.FindAsync(id);
            if (tUserEncounter == null)
            {
                return NotFound();
            }

            db.tUserEncounters.Remove(tUserEncounter);
            await db.SaveChangesAsync();

            return Ok(tUserEncounter);
        }

        // DELETE: api/SoftDeleteUserEncounter/5/2
        [Route("api/UserData/SoftDeleteUserEncounter/{id}/{status}")]
        [ResponseType(typeof(tUserEncounter))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserEncounter(int id, int status)
        {
            tUserEncounter tUserEncounter = await db.tUserEncounters.FindAsync(id);
            if (tUserEncounter == null)
            {
                return NotFound();
            }
            if (tUserEncounter.FollowUpInstructionID != null)
            {
                tUserInstruction _followIns = await db.tUserInstructions.FindAsync(tUserEncounter.FollowUpInstructionID);
                _followIns.SystemStatusID = status;
            }
            if (tUserEncounter.PatientInstructionID != null)
            {
                tUserInstruction _patientIns = await db.tUserInstructions.FindAsync(tUserEncounter.PatientInstructionID);
                _patientIns.SystemStatusID = status;
            }
            tUserEncounter.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tUserEncounter);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserEncounterExists(int id)
        {
            return db.tUserEncounters.Count(e => e.ID == id) > 0;
        }
    }
}