using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class SourceServiceTypeController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/Sources
        [Route("api/UserData/GetSourcesServiceTypes")]
        public IQueryable<tSourceServiceType> GetSourcesServiceTypes()
        {
            return db.tSourceServiceTypes;
        }

        [Route("api/UserData/GetSourcesServiceTypeSocial")]
        [ResponseType(typeof(tSourceServiceType))]
        public async Task<IHttpActionResult> GetSourcesServiceTypeSocial()
        {
            var tSourceServiceTypes = await db.tSourceServiceTypes.FirstAsync(x => x.Type == "Social");
            if (tSourceServiceTypes == null)
            {
                return NotFound();
            }
            return Ok(tSourceServiceTypes);
        }


        [Route("api/UserData/GetSourcesServiceTypeSocial/{userId}")]
        [ResponseType(typeof(IEnumerable<SourceServiceTypesViewModel>))]
        public async Task<IHttpActionResult> GetSources(int userId)
        {
            var list = new List<SourceServiceTypesViewModel>();
            var allResults = db.tSourceServiceTypes;
            foreach (var item in allResults)
            {
                var current = new SourceServiceTypesViewModel
                {
                    ID = item.ID,
                    Type = item.Type
                };

                if (current.ID == 1)
                {
                    var medicalSources =
                    current.SourcesList = await db.tOrganizations.Select(a => new SourceServiceViewModel
                    {
                        ID = a.ID,
                        ServiceName = a.Name,
                        SourceID = a.ID,
                        TypeID = current.ID,
                        CreateDateTime = a.CreateDateTime
                    }).OrderByDescending(t => t.CreateDateTime).ToListAsync();
                }
                else
                {
                    current.SourcesList = await db.tSourceServices.Where(s => s.SourceID != 1 && s.TypeID == current.ID).Select(a => new SourceServiceViewModel
                    {
                        ID = a.ID,
                        ServiceName = a.ServiceName,
                        SourceID = a.ID,
                        TypeID = current.ID,
                        CreateDateTime = a.CreateDateTime
                    }).OrderByDescending(f => f.CreateDateTime).ToListAsync();
                }

                list.Add(current);
            }

            return Ok(list);
        }
    }
}
