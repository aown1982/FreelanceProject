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
    public class UserTestResultComponentsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/UserTestResultComponents
        [Route("api/UserData/GetUserTestResultComponents")]
        public IQueryable<tUserTestResultComponent> GettUserTestResultComponents()
        {
            return db.tUserTestResultComponents;
        }

        // GET: api/UserTestResultComponents/5
        [Route("api/UserData/GetUserTestResultComponents/{id}")]
        [ResponseType(typeof(tUserTestResultComponent))]
        public async Task<IHttpActionResult> GettUserTestResultComponent(int id)
        {
            tUserTestResultComponent tUserTestResultComponent = await db.tUserTestResultComponents.FindAsync(id);
            if (tUserTestResultComponent == null)
            {
                return NotFound();
            }

            return Ok(tUserTestResultComponent);
        }

        // PUT: api/UserTestResultComponents/5
        [Route("api/UserData/UpdateUserTestResultComponents/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserTestResultComponent(int id, tUserTestResultComponent tUserTestResultComponent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserTestResultComponent.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserTestResultComponent).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserTestResultComponentExists(id))
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

        // POST: api/UserTestResultComponents
        [Route("api/UserData/PostUserTestResultComponents")]
        [ResponseType(typeof(tUserTestResultComponent))]
        public async Task<IHttpActionResult> PosttUserTestResultComponent(tUserTestResultComponent tUserTestResultComponent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserTestResultComponents.Add(tUserTestResultComponent);
            await db.SaveChangesAsync();

            // return CreatedAtRoute("DefaultApi", new { id = tUserTestResultComponent.ID }, tUserTestResultComponent);
           return Ok(tUserTestResultComponent);
        }

        // DELETE: api/UserTestResultComponents/5
        [Route("api/UserData/DeleteUserTestResultComponents/{id}")]
        [ResponseType(typeof(tUserTestResultComponent))]
        public async Task<IHttpActionResult> DeletetUserTestResultComponent(int id)
        {
            tUserTestResultComponent tUserTestResultComponent = await db.tUserTestResultComponents.FindAsync(id);
            if (tUserTestResultComponent == null)
            {
                return NotFound();
            }

            db.tUserTestResultComponents.Remove(tUserTestResultComponent);
            await db.SaveChangesAsync();

            return Ok(tUserTestResultComponent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserTestResultComponentExists(int id)
        {
            return db.tUserTestResultComponents.Count(e => e.ID == id) > 0;
        }

        // DELETE: api/UserTest/5
        [Route("api/UserData/SoftDeleteUserTestResultComponent/{id}/{status}")]
        [ResponseType(typeof(tUserTestResultComponent))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeletetUserTestResult(int id, int status)
        {
            tUserTestResultComponent tUserTestResultComponent = await db.tUserTestResultComponents.FindAsync(id);
            if (tUserTestResultComponent == null)
            {
                return NotFound();
            }
            tUserTestResultComponent.SystemStatusID = status;
            await db.SaveChangesAsync();
            return Ok(tUserTestResultComponent);
        }

    }
}