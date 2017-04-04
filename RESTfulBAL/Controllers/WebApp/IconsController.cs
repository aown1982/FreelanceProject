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
    public class IconsController : ApiController
    {
        private WebApplicationEntities db = new WebApplicationEntities();

        // GET: api/Icons
        [Route("api/WebApp/GetIcons")]
        public IQueryable<tIcon> GettIcons()
        {
            return db.tIcons;
        }

        // GET: api/Icons/5
        [Route("api/WebApp/GetIcons/{id}")]
        [ResponseType(typeof(tIcon))]
        public async Task<IHttpActionResult> GettIcon(int id)
        {
            tIcon tIcon = await db.tIcons.FindAsync(id);
            if (tIcon == null)
            {
                return NotFound();
            }

            return Ok(tIcon);
        }

        //// PUT: api/Icons/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PuttIcon(int id, tIcon tIcon)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tIcon.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tIcon).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tIconExists(id))
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

        //// POST: api/Icons
        //[ResponseType(typeof(tIcon))]
        //public async Task<IHttpActionResult> PosttIcon(tIcon tIcon)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tIcons.Add(tIcon);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = tIcon.ID }, tIcon);
        //}

        //// DELETE: api/Icons/5
        //[ResponseType(typeof(tIcon))]
        //public async Task<IHttpActionResult> DeletetIcon(int id)
        //{
        //    tIcon tIcon = await db.tIcons.FindAsync(id);
        //    if (tIcon == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tIcons.Remove(tIcon);
        //    await db.SaveChangesAsync();

        //    return Ok(tIcon);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tIconExists(int id)
        {
            return db.tIcons.Count(e => e.ID == id) > 0;
        }
    }
}