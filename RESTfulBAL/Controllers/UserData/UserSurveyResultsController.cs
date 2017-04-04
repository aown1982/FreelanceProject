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
using RESTfulBAL.Models;
using DAL.WebApplication;
using AutoMapper;
using RESTfulBAL.Models.UserData;

namespace RESTfulBAL.Controllers.UserData
{
    //[Route("api/UserData/")]
    public class UserSurveyResultsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private WebApplicationEntities webDb = new WebApplicationEntities();

        // GET: api/UserSurveyResults
        [Route("api/UserData/GetUserSurveyResults")]
        public IQueryable<tUserSurveyResult> GettUserSurveyResults()
        {
            return db.tUserSurveyResults;
        }

        // GET: api/UserSurveyResults/5
        [Route("api/UserData/GetUserSurveyResults/{id}")]
        [ResponseType(typeof(tUserSurveyResult))]
        public async Task<IHttpActionResult> GettUserSurveyResult(int id)
        {
            tUserSurveyResult tUserSurveyResult = await db.tUserSurveyResults.FindAsync(id);
            if (tUserSurveyResult == null)
            {
                return NotFound();
            }

            return Ok(tUserSurveyResult);
        }

        // PUT: api/UserSurveyResults/5
        [Route("api/UserData/UpdateUserSurveyResults/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserSurveyResult(int id, tUserSurveyResult tUserSurveyResult)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserSurveyResult.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserSurveyResult).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserSurveyResultExists(id))
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

        [Route("api/UserData/PostUserSurveyResults/{questionsPassed}")]
        [ResponseType(typeof(SurveyQuestionViewModel))]
        public async Task<IHttpActionResult> PosttUserSurveyResult(string questionsPassed, tUserSurveyResult tUserSurveyResult)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            /* Saving the Survey Result one by one
             */
            db.tUserSurveyResults.Add(tUserSurveyResult);
            var result = await db.SaveChangesAsync();

            /*Check if Result saved successfully*/
            if (result > 0)
            {
                var surveyId = tUserSurveyResult.SurveyID;
                SurveyQuestionViewModel vm = null;
                var mcQuestionTypes = new[] { 1, 4 };

                /* Get the total count of the questions */
                var totalCount = webDb.tSurveyQuestions.Where(a => a.SurveyID == surveyId).Count();
                var slist = !string.IsNullOrEmpty(questionsPassed) ? questionsPassed.Split(',').Select(a => int.Parse(a)).ToList() : new List<int>();

                /* Check if Saved Question is not the last question.
                 * If it is, then don't need to request for next question
                 */
                var isLast = (totalCount == slist.Count) && totalCount > 0;
                if (!isLast)
                {
                    var surQuestion = webDb.tSurveyQuestions.Where(a => a.SurveyID == surveyId && !slist.Contains(a.ID)).OrderBy(q => q.ID).FirstOrDefault();

                    if (surQuestion != null)
                    {
                        //if (mcQuestionTypes.Contains(surQuestion.QuestionTypeID))
                        //    surQuestion.tSurveyQuestionMCAnswers = webDb.tSurveyQuestionMCAnswers.Where(a => a.QuestionID == surQuestion.ID).OrderBy(a1 => a1.SequenceOrder).ToList();

                        //Mapper.Initialize(cfg => cfg.CreateMap<tSurveyQuestion, SurveyQuestionViewModel>());
                        //vm = Mapper.Map<SurveyQuestionViewModel>(surQuestion);

                        //var passedQuesArray = !string.IsNullOrEmpty(questionsPassed) ? questionsPassed.Split(',').Select(a => int.Parse(a)).ToList() : new List<int>();
                        Mapper.Initialize(cfg => cfg.CreateMap<tSurveyQuestion, SurveyQuestionViewModel>());
                        vm = Mapper.Map<SurveyQuestionViewModel>(surQuestion);

                        if (mcQuestionTypes.Contains(surQuestion.QuestionTypeID))
                        {
                            var ansList = webDb.tSurveyQuestionMCAnswers.Where(a => a.QuestionID == surQuestion.ID).OrderBy(a1 => a1.SequenceOrder).ToList();
                            if (ansList.Any())
                            {
                                Mapper.Initialize(cfg => cfg.CreateMap<tSurveyQuestionMCAnswer, SurveyQuestionMCAnswersViewModel>());
                                vm.tMCAnswers = ansList.Select(a => Mapper.Map<SurveyQuestionMCAnswersViewModel>(a)).ToList();
                            }
                        }

                        vm.IsLast = totalCount > 0 && ((totalCount - slist.Count) == 1);
                    }
                }
                else
                {
                    vm = new SurveyQuestionViewModel
                    {
                        IsLast = true
                    };
                }
                return Ok(vm);
            }
            else
            {
                return BadRequest(ModelState);
            }
            //return CreatedAtRoute("DefaultApi", new { id = tUserSurveyResult.ID }, tUserSurveyResult);
        }

        // DELETE: api/UserSurveyResults/5
        [Route("api/UserData/DeleteUserSurveyResults/{id}")]
        [ResponseType(typeof(tUserSurveyResult))]
        public async Task<IHttpActionResult> DeletetUserSurveyResult(int id)
        {
            tUserSurveyResult tUserSurveyResult = await db.tUserSurveyResults.FindAsync(id);
            if (tUserSurveyResult == null)
            {
                return NotFound();
            }

            db.tUserSurveyResults.Remove(tUserSurveyResult);
            await db.SaveChangesAsync();

            return Ok(tUserSurveyResult);
        }


        [Route("api/UserData/GetUserSurveyResultsData/{surveyId}")]
        public IEnumerable<UserSurveyResultsViewModel> GetUserSurveyResultsData(int surveyId)
        {
            var list = new List<UserSurveyResultsViewModel>();

            /* Get the list of Survey Questions from User Survey Results Database */
            var results = db.tUserSurveyResults.Where(a => a.SurveyID == surveyId);
            if (results.Any())
            {
                /* Map Model to ViewModel of UserSurveyResults Entity */
                Mapper.Initialize(cfg => cfg.CreateMap<tUserSurveyResult, UserSurveyResultsViewModel>());

                /* Iterate through each record to fill more properties to View Model */
                foreach (var item in results)
                {
                    var vm = Mapper.Map<UserSurveyResultsViewModel>(item);
                    var answers = new List<string>();

                    /* Get the Current Question from tSurveyQuestions based on Current Question ID */
                    var currentQuestion = webDb.tSurveyQuestions.Where(f => f.ID == vm.QuestionID).FirstOrDefault();

                    /* If Question Type is either 1 or 4, then it will fetch the answers from another table i.e. tSurveyQuestionMCAnswers 
                     * since it may have multiple answers in case of those question types.
                       Otherwise, it will just add single answer to the answers collection */
                    if (currentQuestion.QuestionTypeID == 1 || currentQuestion.QuestionTypeID == 4)
                    {
                        var ansArray = vm.Answer.Split(',').Select(a => int.Parse(a));
                        answers = webDb.tSurveyQuestionMCAnswers.Where(a => ansArray.Contains(a.ID)).Select(f => f.AnswerText).ToList();
                    }
                    else
                        answers.Add(vm.Answer);

                    vm.QuestionDescription = currentQuestion.Question;
                    vm.MCAnswers = answers;
                    list.Add(vm);
                }
            }
            return list;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserSurveyResultExists(int id)
        {
            return db.tUserSurveyResults.Count(e => e.ID == id) > 0;
        }
    }
}