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
using RESTfulBAL.Models;
using AutoMapper;
using System.Web.Routing;

namespace RESTfulBAL.Controllers.WebApp
{
    //[Route("api/WebApp/")]
    public class SurveyQuestionsController : ApiController
    {
        private WebApplicationEntities db = new WebApplicationEntities();

        // GET: api/SurveyQuestions
        [Route("api/WebApp/GetSurveyQuestions2")]
        public IQueryable<tSurveyQuestion> GettSurveyQuestions2()
        {
            return db.tSurveyQuestions;
        }

        // GET: api/SurveyQuestions/5
        [Route("api/WebApp/GetSurveyQuestions1/{id}")]
        [ResponseType(typeof(tSurveyQuestion))]
        public async Task<IHttpActionResult> GettSurveyQuestion1(int id)
        {
            tSurveyQuestion tSurveyQuestion = await db.tSurveyQuestions.FindAsync(id);
            if (tSurveyQuestion == null)
            {
                return NotFound();
            }

            return Ok(tSurveyQuestion);
        }

        //// PUT: api/SurveyQuestions/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PuttSurveyQuestion(int id, tSurveyQuestion tSurveyQuestion)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tSurveyQuestion.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tSurveyQuestion).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tSurveyQuestionExists(id))
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

        //// POST: api/SurveyQuestions
        //[ResponseType(typeof(tSurveyQuestion))]
        //public async Task<IHttpActionResult> PosttSurveyQuestion(tSurveyQuestion tSurveyQuestion)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tSurveyQuestions.Add(tSurveyQuestion);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = tSurveyQuestion.ID }, tSurveyQuestion);
        //}

        //// DELETE: api/SurveyQuestions/5
        //[ResponseType(typeof(tSurveyQuestion))]
        //public async Task<IHttpActionResult> DeletetSurveyQuestion(int id)
        //{
        //    tSurveyQuestion tSurveyQuestion = await db.tSurveyQuestions.FindAsync(id);
        //    if (tSurveyQuestion == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tSurveyQuestions.Remove(tSurveyQuestion);
        //    await db.SaveChangesAsync();

        //    return Ok(tSurveyQuestion);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tSurveyQuestionExists(int id)
        {
            return db.tSurveyQuestions.Count(e => e.ID == id) > 0;
        }


        [Route("api/WebApp/GetSurveyQuestions/{surveyId}")]
        public SurveyQuestionViewModel GettSurveyQuestions(int surveyId)
        {
            SurveyQuestionViewModel vm = null;
            var mcQuestionTypes = new[] { 1, 4 };
            var surQuestion = db.tSurveyQuestions.Where(a => a.SurveyID == surveyId).OrderBy(q => q.ID).FirstOrDefault();
            var totalCount = db.tSurveyQuestions.Where(a => a.SurveyID == surveyId).Count();

            if (surQuestion != null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<tSurveyQuestion, SurveyQuestionViewModel>());
                vm = Mapper.Map<SurveyQuestionViewModel>(surQuestion);

                if (mcQuestionTypes.Contains(surQuestion.QuestionTypeID))
                {
                    var ansList = db.tSurveyQuestionMCAnswers.Where(a => a.QuestionID == surQuestion.ID).OrderBy(a1 => a1.SequenceOrder).ToList();
                    if (ansList.Any())
                    {
                        Mapper.Initialize(cfg => cfg.CreateMap<tSurveyQuestionMCAnswer, SurveyQuestionMCAnswersViewModel>());
                        vm.tMCAnswers = ansList.Select(a => Mapper.Map<SurveyQuestionMCAnswersViewModel>(a)).ToList();
                    }
                }

                vm.IsLast = totalCount == 1;
            }
            return vm;
        }
    }
}