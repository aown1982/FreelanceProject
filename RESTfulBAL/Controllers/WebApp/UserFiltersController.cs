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
using DAL.WebApplication;

namespace RESTfulBAL.Controllers.WebApp
{
    public class UserFiltersController : ApiController
    {
        private WebApplicationEntities db = new WebApplicationEntities();

        // GET: api/UserFilters
        [Route("api/WebApp/GetUserFilters")]
        public IQueryable<tUserFilter> GettUserFilters()
        {
            return db.tUserFilters;
        }

        // GET: api/UserFilters/5
        [Route("api/WebApp/GetUserFilters/{id}")]
        [ResponseType(typeof(tUserFilter))]
        public async Task<IHttpActionResult> GettUserFilter(int id)
        {
            tUserFilter tUserFilter = await db.tUserFilters.FindAsync(id);
            if (tUserFilter == null)
            {
                return NotFound();
            }

            return Ok(tUserFilter);
        }

        //// PUT: api/UserFilters/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PuttUserFilter(int id, tUserFilter tUserFilter)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tUserFilter.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tUserFilter).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tUserFilterExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/UserFilters
        //[ResponseType(typeof(tUserFilter))]
        //public async Task<IHttpActionResult> PosttUserFilter(tUserFilter tUserFilter)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tUserFilters.Add(tUserFilter);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = tUserFilter.ID }, tUserFilter);
        //}

        //// DELETE: api/UserFilters/5
        //[ResponseType(typeof(tUserFilter))]
        //public async Task<IHttpActionResult> DeletetUserFilter(int id)
        //{
        //    tUserFilter tUserFilter = await db.tUserFilters.FindAsync(id);
        //    if (tUserFilter == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tUserFilters.Remove(tUserFilter);
        //    await db.SaveChangesAsync();

        //    return Ok(tUserFilter);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserFilterExists(int id)
        {
            return db.tUserFilters.Count(e => e.ID == id) > 0;
        }
    }
}