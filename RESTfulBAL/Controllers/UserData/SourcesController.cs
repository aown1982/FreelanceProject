using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;
using DAL.UserData;
using RESTfulBAL.Models.UserData;
using AutoMapper;

namespace RESTfulBAL.Controllers.UserData
{
    public class SourcesController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/Sources
        [Route("api/UserData/GetSources")]
        public IQueryable<tSource> GettSources()
        {
            return db.tSources;
        }
        [HttpGet]
        [Route("api/UserData/GetSocialSourceByName/{name}")]
        [ResponseType(typeof(tSource))]
        public async Task<IHttpActionResult> GettSocialSources(string name)
        {
            try
            {
                var source = await db.tSources.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
                if (source == null)
                {
                    return NotFound();
                }
                return Ok(source);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        // GET: api/Sources/5
        [Route("api/UserData/GetSources/{id}")]
        [ResponseType(typeof(tSource))]
        public async Task<IHttpActionResult> GettSource(int id)
        {
            tSource tSource = await db.tSources.FindAsync(id);
            if (tSource == null)
            {
                return NotFound();
            }

            return Ok(tSource);
        }

        // PUT: api/Sources/5
        [Route("api/UserData/UpdateSources/{id}/Source")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttSource(int id, tSource Source)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Source.ID)
            {
                return BadRequest();
            }

            db.Entry(Source).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tSourceExists(id))
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

        // POST: api/Sources
        [Route("api/UserData/PostSources/Source")]
        [ResponseType(typeof(tSource))]
        public async Task<IHttpActionResult> PosttSource(tSource Source)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tSources.Add(Source);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = Source.ID }, Source);
        }

        // DELETE: api/Sources/5
        [Route("api/UserData/DeleteSources/{id}")]
        [ResponseType(typeof(tSource))]
        public async Task<IHttpActionResult> DeletetSource(int id)
        {
            tSource tSource = await db.tSources.FindAsync(id);
            if (tSource == null)
            {
                return NotFound();
            }

            db.tSources.Remove(tSource);
            await db.SaveChangesAsync();

            return Ok(tSource);
        }

        [Route("api/UserData/GetSourceConnectedChartData/{userId}")]
        public IEnumerable<ConnectedSourceViewModel> GetSourceConnectedChartData(int userId)
        {
            List<ConnectedSourceViewModel> list = new List<ConnectedSourceViewModel>();
            var list1 = db.tSourceServiceTypes;

            var list2 = (from t in db.tSourceServiceTypes
                         join s in db.tSourceServices on t.ID equals s.TypeID
                         join u in db.tUserSourceServices on s.ID equals u.SourceServiceID
                         where u.UserID == userId
                         group t by t.ID into newGroup
                         select new ConnectedSourceViewModel
                         {
                             ID = newGroup.Key,
                             Value = newGroup.Count()
                         });


            Mapper.Initialize(c => c.CreateMap<tSourceServiceType, ConnectedSourceViewModel>());
            foreach (var item in list1)
            {
                var vm = Mapper.Map<ConnectedSourceViewModel>(item);
                vm.Value = list2.Where(v => v.ID == vm.ID).Select(a => a.Value).FirstOrDefault(); ;
                list.Add(vm);
            }            

            //list = (from uss in db.tUserSourceServices
            //        join ss in db.tSourceServices on uss.SourceServiceID equals ss.ID
            //        join s in db.tSources on ss.TypeID equals s.ID
            //        join sst in db.tSourceServiceTypes on ss.TypeID equals sst.ID
            //        where uss.SystemStatusID == 1 && uss.UserID == userId
            //        select new ConnectedSourceViewModel
            //        {
            //            Category = sst.Type,
            //            UserID = uss.UserID
            //        });

            //if (list.Any())
            //{

            //    list = list.GroupBy(c => c.Category).Select(s => new ConnectedSourceViewModel
            //    {
            //        Category = s.FirstOrDefault().Category,
            //        Value = s.Count(),
            //        UserID = s.FirstOrDefault().UserID
            //    }).ToList();
            //}
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

        private bool tSourceExists(int id)
        {
            return db.tSources.Count(e => e.ID == id) > 0;
        }
    }
}