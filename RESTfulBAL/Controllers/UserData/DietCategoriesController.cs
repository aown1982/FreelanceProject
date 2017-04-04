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
    public class DietCategoriesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/DietCategories
        [Route("api/UserData/GetDietCategories")]
        public IQueryable<tDietCategory> GettDietCategories()
        {
            return db.tDietCategories;
        }

        // GET: api/DietCategories/5
        [Route("api/UserData/GetDietCategories/{id}")]
        [ResponseType(typeof(tDietCategory))]
        public async Task<IHttpActionResult> GettDietCategory(int id)
        {
            tDietCategory tDietCategory = await db.tDietCategories.FindAsync(id);
            if (tDietCategory == null)
            {
                return NotFound();
            }

            return Ok(tDietCategory);
        }

        // PUT: api/DietCategories/5
        [Route("api/UserData/UpdateDietCategories/{id}/DietCategory")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttDietCategory(int id, tDietCategory DietCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != DietCategory.ID)
            {
                return BadRequest();
            }

            db.Entry(DietCategory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tDietCategoryExists(id))
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

        // POST: api/DietCategories
        [Route("api/UserData/PostDietCategories/DietCategory")]
        [ResponseType(typeof(tDietCategory))]
        public async Task<IHttpActionResult> PosttDietCategory(tDietCategory DietCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tDietCategories.Add(DietCategory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = DietCategory.ID }, DietCategory);
        }

        // DELETE: api/DietCategories/5
        [Route("api/UserData/DeleteDietCategories/{id}")]
        [ResponseType(typeof(tDietCategory))]
        public async Task<IHttpActionResult> DeletetDietCategory(int id)
        {
            tDietCategory tDietCategory = await db.tDietCategories.FindAsync(id);
            if (tDietCategory == null)
            {
                return NotFound();
            }

            db.tDietCategories.Remove(tDietCategory);
            await db.SaveChangesAsync();

            return Ok(tDietCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tDietCategoryExists(int id)
        {
            return db.tDietCategories.Count(e => e.ID == id) > 0;
        }
    }
}