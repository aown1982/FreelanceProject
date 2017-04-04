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
    public class SurveyQuestionTypesController : ApiController
    {
        private WebApplicationEntities db = new WebApplicationEntities();

        // GET: api/SurveyQuestionTypes
        [Route("api/WebApp/GetSurveyQuestionTypes")]
        public IQueryable<tSurveyQuestionType> GettSurveyQuestionTypes()
        {
            return db.tSurveyQuestionTypes;
        }

        // GET: api/SurveyQuestionTypes/5
        [Route("api/WebApp/GetSurveyQuestionTypes/{id}")]
        [ResponseType(typeof(tSurveyQuestionType))]
        public async Task<IHttpActionResult> GettSurveyQuestionType(int id)
        {
            tSurveyQuestionType tSurveyQuestionType = await db.tSurveyQuestionTypes.FindAsync(id);
            if (tSurveyQuestionType == null)
            {
                return NotFound();
            }

            return Ok(tSurveyQuestionType);
        }

        //// PUT: api/SurveyQuestionTypes/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PuttSurveyQuestionType(int id, tSurveyQuestionType tSurveyQuestionType)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tSurveyQuestionType.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tSurveyQuestionType).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tSurveyQuestionTypeExists(id))
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

        //// POST: api/SurveyQuestionTypes
        //[ResponseType(typeof(tSurveyQuestionType))]
        //public async Task<IHttpActionResult> PosttSurveyQuestionType(tSurveyQuestionType tSurveyQuestionType)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tSurveyQuestionTypes.Add(tSurveyQuestionType);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = tSurveyQuestionType.ID }, tSurveyQuestionType);
        //}

        //// DELETE: api/SurveyQuestionTypes/5
        //[ResponseType(typeof(tSurveyQuestionType))]
        //public async Task<IHttpActionResult> DeletetSurveyQuestionType(int id)
        //{
        //    tSurveyQuestionType tSurveyQuestionType = await db.tSurveyQuestionTypes.FindAsync(id);
        //    if (tSurveyQuestionType == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tSurveyQuestionTypes.Remove(tSurveyQuestionType);
        //    await db.SaveChangesAsync();

        //    return Ok(tSurveyQuestionType);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tSurveyQuestionTypeExists(int id)
        {
            return db.tSurveyQuestionTypes.Count(e => e.ID == id) > 0;
        }
    }
}