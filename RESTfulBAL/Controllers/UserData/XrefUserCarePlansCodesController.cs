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
    public class XrefUserCarePlansCodesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/XrefUserCarePlansCodes
        [Route("api/UserData/GetXrefUserCarePlansCodes")]
        public IQueryable<tXrefUserCarePlansCode> GettXrefUserCarePlansCodes()
        {
            return db.tXrefUserCarePlansCodes;
        }

        // GET: api/XrefUserCarePlansCodes/5
        [Route("api/UserData/GetXrefUserCarePlansCodes/{id}")]
        [ResponseType(typeof(tXrefUserCarePlansCode))]
        public async Task<IHttpActionResult> GettXrefUserCarePlansCode(int id)
        {
            tXrefUserCarePlansCode tXrefUserCarePlansCode = await db.tXrefUserCarePlansCodes.FindAsync(id);
            if (tXrefUserCarePlansCode == null)
            {
                return NotFound();
            }

            return Ok(tXrefUserCarePlansCode);
        }

        // PUT: api/XrefUserCarePlansCodes/5
        [Route("api/UserData/UpdateXrefUserCarePlansCodes/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttXrefUserCarePlansCode(int id, tXrefUserCarePlansCode tXrefUserCarePlansCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tXrefUserCarePlansCode.ID)
            {
                return BadRequest();
            }

            db.Entry(tXrefUserCarePlansCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tXrefUserCarePlansCodeExists(id))
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

        // POST: api/XrefUserCarePlansCodes
        [Route("api/UserData/PostXrefUserCarePlansCodes")]
        [ResponseType(typeof(tXrefUserCarePlansCode))]
        public async Task<IHttpActionResult> PosttXrefUserCarePlansCode(tXrefUserCarePlansCode tXrefUserCarePlansCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tXrefUserCarePlansCodes.Add(tXrefUserCarePlansCode);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tXrefUserCarePlansCode.ID }, tXrefUserCarePlansCode);
        }

        // DELETE: api/XrefUserCarePlansCodes/5
        [Route("api/UserData/DeleteXrefUserCarePlansCodes/{id}")]
        [ResponseType(typeof(tXrefUserCarePlansCode))]
        public async Task<IHttpActionResult> DeletetXrefUserCarePlansCode(int id)
        {
            tXrefUserCarePlansCode tXrefUserCarePlansCode = await db.tXrefUserCarePlansCodes.FindAsync(id);
            if (tXrefUserCarePlansCode == null)
            {
                return NotFound();
            }

            db.tXrefUserCarePlansCodes.Remove(tXrefUserCarePlansCode);
            await db.SaveChangesAsync();

            return Ok(tXrefUserCarePlansCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tXrefUserCarePlansCodeExists(int id)
        {
            return db.tXrefUserCarePlansCodes.Count(e => e.ID == id) > 0;
        }
    }
}