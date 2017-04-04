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
    public class UserProblemsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserProblems
        [Route("api/UserData/GetUserProblems")]
        public IQueryable<tUserProblem> GettUserProblems()
        {
            return db.tUserProblems;
        }

        // GET: api/UserProblems/5
        [Route("api/UserData/GetUserProblems/{id}")]
        [ResponseType(typeof(tUserProblem))]
        public async Task<IHttpActionResult> GettUserProblem(int id)
        {
            tUserProblem tUserProblem = await db.tUserProblems.FindAsync(id);
            if (tUserProblem == null)
            {
                return NotFound();
            }

            return Ok(tUserProblem);
        }

        // PUT: api/UserProblems/5
        [Route("api/UserData/UpdateUserProblems/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserProblem(int id, tUserProblem tUserProblem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserProblem.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserProblem).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserProblemExists(id))
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

        // POST: api/UserProblems
        [Route("api/UserData/PostUserProblems")]
        [ResponseType(typeof(tUserProblem))]
        public async Task<IHttpActionResult> PosttUserProblem(tUserProblem tUserProblem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserProblems.Add(tUserProblem);
            await db.SaveChangesAsync();

            return Ok(tUserProblem);
        }

        // DELETE: api/UserProblems/5
        [Route("api/UserData/DeleteUserProblems/{id}")]
        [ResponseType(typeof(tUserProblem))]
        public async Task<IHttpActionResult> DeletetUserProblem(int id)
        {
            tUserProblem tUserProblem = await db.tUserProblems.FindAsync(id);
            if (tUserProblem == null)
            {
                return NotFound();
            }

            db.tUserProblems.Remove(tUserProblem);
            await db.SaveChangesAsync();

            return Ok(tUserProblem);
        }

        // DELETE: api/SoftDeleteUserProblem/5/2
        [Route("api/UserData/SoftDeleteUserProblem/{id}/{status}")]
        [ResponseType(typeof(tUserProblem))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserProblem(int id, int status)
        {
            tUserProblem tUserProblem = await db.tUserProblems.FindAsync(id);
            if (tUserProblem == null)
            {
                return NotFound();
            }
            tUserProblem.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tUserProblem);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserProblemExists(int id)
        {
            return db.tUserProblems.Count(e => e.ID == id) > 0;
        }
    }
}