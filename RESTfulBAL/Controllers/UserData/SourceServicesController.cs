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
    public class SourceServicesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/SourceServices
        [Route("api/UserData/GetSourceServices")]
        public IQueryable<tSourceService> GettSourceServices()
        {
            return db.tSourceServices;
        }

        // GET: api/SourceServices/5
        [Route("api/UserData/GetSourceServices/{id}")]
        [ResponseType(typeof(tSourceService))]
        public async Task<IHttpActionResult> GettSourceService(int id)
        {
            tSourceService tSourceService = await db.tSourceServices.FindAsync(id);
            if (tSourceService == null)
            {
                return NotFound();
            }

            return Ok(tSourceService);
        }
        // POST: api/SourceServices/5
        [HttpPost]
        [Route("api/UserData/GetSourceServicesBySourceIDAndType/")]
        [ResponseType(typeof(tSourceService))]
        public async Task<IHttpActionResult> GettSourceService(tSourceService service)
        {
            tSourceService tSourceService = await db.tSourceServices.FirstAsync(x=>x.SourceID == service.SourceID && x.TypeID ==service.TypeID);
            if (tSourceService == null)
            {
                return NotFound();
            }

            return Ok(tSourceService);
        }

        // PUT: api/SourceServices/5
        [Route("api/UserData/UpdateSourceServices/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttSourceService(int id, tSourceService tSourceService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tSourceService.ID)
            {
                return BadRequest();
            }

            db.Entry(tSourceService).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tSourceServiceExists(id))
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

        // POST: api/SourceServices
        [Route("api/UserData/PostSourceServices")]
        [ResponseType(typeof(tSourceService))]
        public async Task<IHttpActionResult> PosttSourceService(tSourceService tSourceService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tSourceServices.Add(tSourceService);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tSourceService.ID }, tSourceService);
        }

        // DELETE: api/SourceServices/5
        [Route("api/UserData/DeleteSourceServices/{id}")]
        [ResponseType(typeof(tSourceService))]
        public async Task<IHttpActionResult> DeletetSourceService(int id)
        {
            tSourceService tSourceService = await db.tSourceServices.FindAsync(id);
            if (tSourceService == null)
            {
                return NotFound();
            }

            db.tSourceServices.Remove(tSourceService);
            await db.SaveChangesAsync();

            return Ok(tSourceService);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tSourceServiceExists(int id)
        {
            return db.tSourceServices.Count(e => e.ID == id) > 0;
        }
    }
}