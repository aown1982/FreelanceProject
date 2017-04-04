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
using DAL.UserData;
using RESTfulBAL.Common;
using AutoMapper;

namespace RESTfulBAL.Controllers.WebApp
{
    public class SurveysController : ApiController
    {
        private WebApplicationEntities db = new WebApplicationEntities();
        private UserDataEntities dbUserData = new UserDataEntities();

        // GET: api/Surveys
        [Route("api/WebApp/GetSurveys")]
        public IQueryable<tSurvey> GettSurveys()
        {
            return db.tSurveys;
        }

        // GET: api/Surveys/5
        [Route("api/WebApp/GetSurveys/{id}")]
        [ResponseType(typeof(tSurvey))]
        public async Task<IHttpActionResult> GettSurvey(int id)
        {
            tSurvey tSurvey = await db.tSurveys.FindAsync(id);
            if (tSurvey == null)
            {
                return NotFound();
            }

            return Ok(tSurvey);
        }

        //// PUT: api/Surveys/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PuttSurvey(int id, tSurvey tSurvey)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tSurvey.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tSurvey).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tSurveyExists(id))
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

        //// POST: api/Surveys
        //[ResponseType(typeof(tSurvey))]
        //public async Task<IHttpActionResult> PosttSurvey(tSurvey tSurvey)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tSurveys.Add(tSurvey);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = tSurvey.ID }, tSurvey);
        //}

        //// DELETE: api/Surveys/5
        //[ResponseType(typeof(tSurvey))]
        //public async Task<IHttpActionResult> DeletetSurvey(int id)
        //{
        //    tSurvey tSurvey = await db.tSurveys.FindAsync(id);
        //    if (tSurvey == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tSurveys.Remove(tSurvey);
        //    await db.SaveChangesAsync();

        //    return Ok(tSurvey);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [Route("api/WebApp/GetSurveyChartData/{userId}")]
        public IEnumerable<SurveyViewModel> GetSurveyChartData(int userId)
        {
            var list = GetEmptyList(DateTime.Now);
            //var results = dbUserData.tUserSurveyResults.Where(s => s.UserID == userId).Select(d => d.CreateDateTime.Month);

            //if (results.Any())
            //{
            //    foreach (var item in list)
            //        item.SurveysCount = results.Where(f => f == item.MonthValue).Count();
            //}
                var results = from b in dbUserData.tUserSurveyResults
                                    where b.UserID==userId
                                    group b by b.SurveyID into g
                                    select g.FirstOrDefault();

            if (results.Any())
            {
                foreach (var item in list)
                    item.SurveysCount = results.Where(f => f.CreateDateTime.Month== item.MonthValue).Count();
            }
            return list;
        }

        private List<SurveyViewModel> GetEmptyList(DateTime dt)
        {
            var list = new List<SurveyViewModel>();
            var month = dt.Month;
            DateTime dtStartOfYear;

            for (int i = 1; i <= month; i++)
            {
                dtStartOfYear = new DateTime(dt.Year, i, 1);
                list.Add(new SurveyViewModel
                {
                    Category = dtStartOfYear.ToMonthName(),
                    MonthValue = dtStartOfYear.Month
                });
                dtStartOfYear.AddMonths(i);
            }
            return list;
        }

        private bool tSurveyExists(int id)
        {
            return db.tSurveys.Count(e => e.ID == id) > 0;
        }


        [Route("api/WebApp/GetSurveysByUser/{userId}")]
        public IEnumerable<SurveyViewModel> GettSurveys(int userId)
        {
            var surveysList = new List<SurveyViewModel>();

            var qSurveys = db.tSurveys.OrderBy(a => a.UserFilterID);
            if (qSurveys.Count() > 0)
            {
                var userFilterId = 0;
                Mapper.Initialize(cfg => cfg.CreateMap<tSurvey, SurveyViewModel>());

                foreach (var item in qSurveys)
                {
                    var vm = Mapper.Map<SurveyViewModel>(item);

                    var totalAttemptedCount = dbUserData.tUserSurveyResults.Count(a => a.SurveyID == item.ID);
                    var totalCount = db.tSurveyQuestions.Count(a => a.SurveyID == item.ID);

                    vm.IsAllCompleted = (totalCount == totalAttemptedCount) && totalCount > 0;
                    vm.IsStarted = dbUserData.tUserSurveyResults.Any(a => a.SurveyID == item.ID);

                    if (item.UserFilterID != userFilterId)
                    {
                        userFilterId = item.UserFilterID;
                        var isExists = CommonMethods.IsUserExistsInUserFilters(item.UserFilterID, userId);
                        if (isExists)
                            surveysList.Add(vm);

                        //var sqlQuery = db.tUserFilters.Where(u => u.ID == userFilterId).Select(a => a.FilterQuery).FirstOrDefault();
                        //var userFiltersList = db.Database.SqlQuery<tUser>(sqlQuery).ToList();

                        //if (userFiltersList.Count() > 0 && userFiltersList.Any(a => a.ID == userId))
                        //    surveysList.Add(item);
                    }
                    else
                    {
                        surveysList.Add(vm);
                    }
                }

                if (surveysList.Count > 0)
                    surveysList = surveysList.OrderBy(a => a.ID).ToList();
            }
            return surveysList;
        }
    }
}