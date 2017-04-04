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
using RESTfulBAL.Models.UserData;

namespace RESTfulBAL.Controllers.UserData
{
    public class SHAREPurposeCategoriesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/SHAREPurposeCategories
        public IQueryable<tSHAREPurposeCategory> GettSHAREPurposeCategories()
        {
            return db.tSHAREPurposeCategories;
        }

        // GET: api/SHAREPurposeCategories/5
        [ResponseType(typeof(tSHAREPurposeCategory))]
        public async Task<IHttpActionResult> GettSHAREPurposeCategory(int id)
        {
            tSHAREPurposeCategory tSHAREPurposeCategory = await db.tSHAREPurposeCategories.FindAsync(id);
            if (tSHAREPurposeCategory == null)
            {
                return NotFound();
            }

            return Ok(tSHAREPurposeCategory);
        }

        // PUT: api/SHAREPurposeCategories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttSHAREPurposeCategory(int id, tSHAREPurposeCategory tSHAREPurposeCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tSHAREPurposeCategory.ID)
            {
                return BadRequest();
            }

            db.Entry(tSHAREPurposeCategory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tSHAREPurposeCategoryExists(id))
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

        // POST: api/SHAREPurposeCategories
        [ResponseType(typeof(tSHAREPurposeCategory))]
        public async Task<IHttpActionResult> PosttSHAREPurposeCategory(tSHAREPurposeCategory tSHAREPurposeCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tSHAREPurposeCategories.Add(tSHAREPurposeCategory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tSHAREPurposeCategory.ID }, tSHAREPurposeCategory);
        }

        // DELETE: api/SHAREPurposeCategories/5
        [ResponseType(typeof(tSHAREPurposeCategory))]
        public async Task<IHttpActionResult> DeletetSHAREPurposeCategory(int id)
        {
            tSHAREPurposeCategory tSHAREPurposeCategory = await db.tSHAREPurposeCategories.FindAsync(id);
            if (tSHAREPurposeCategory == null)
            {
                return NotFound();
            }

            db.tSHAREPurposeCategories.Remove(tSHAREPurposeCategory);
            await db.SaveChangesAsync();

            return Ok(tSHAREPurposeCategory);
        }



        [Route("api/UserData/GetSharePurposeData/{userId}")]
        [ResponseType(typeof(IEnumerable<SharePurposeCategoriesViewModel>))]
        public async Task<IHttpActionResult> GetSharePurpose(int userId)
        {
            var list = new List<SharePurposeCategoriesViewModel>();
            var allResults = db.tSHAREPurposeCategories;
            foreach (var item in allResults)
            {
                var current = new SharePurposeCategoriesViewModel
                {
                    ID = item.ID,
                    Name = item.Name,
                    Description = item.Description
                };

                if (current.ID > 0)
                {

                    var sharePurpose =
                                current.SahrePurposeList = await db.tSHAREPurposes.Where(s => s.CategoryID == current.ID).Select(a => new SharePurposeViewModel
                                {
                                    ID = a.ID,
                                    Name = a.Name,
                                    PurposeDescription = a.PurposeDescription,
                                    CreateDateTime = a.CreateDateTime
                                }).OrderByDescending(t => t.CreateDateTime).ToListAsync();
                }
                //else
                //{
                //    current.SahrePurposeList = await db.tSHAREPurposes.Where(s=> s.CategoryID == current.ID).Select(a => new SharePurposeViewModel
                //    {
                //        ID = a.ID,
                //        Name = a.Name,
                //        PurposeDescription = a.PurposeDescription,
                //        CreateDateTime = a.CreateDateTime
                //    }).OrderByDescending(f => f.CreateDateTime).ToListAsync();
                //}

                list.Add(current);
            }

            return Ok(list);
        }


        [Route("api/UserData/PostUserShareData")]
        [ResponseType(typeof(IEnumerable<tUserSHARESetting>))]
        public async Task<IHttpActionResult> PostUserShareData(List<tUserSHARESetting> userShareSetting)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            /* Saving the Share Setting one by one */
            db.tUserSHARESettings.AddRange(userShareSetting);

            var result = await db.SaveChangesAsync();

            if (result <= 0)
                return BadRequest(ModelState);

            var list = new List<tUserSHARESetting>();
            return Ok(list);
        }






        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tSHAREPurposeCategoryExists(int id)
        {
            return db.tSHAREPurposeCategories.Count(e => e.ID == id) > 0;
        }
    }
}