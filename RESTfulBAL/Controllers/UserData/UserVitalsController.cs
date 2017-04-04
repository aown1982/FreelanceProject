using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;
using DAL.UserData;
using DAL.Users;
using RESTfulBAL.Models;
using RESTfulBAL.Models.UserData;

namespace RESTfulBAL.Controllers.UserData
{
    public class UserVitalsController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();
        private UsersEntities dbUsers = new UsersEntities();
        // GET: api/UserVitals
        [Route("api/UserData/GetUserVitals")]
        public IQueryable<tUserVital> GettUserVitals()
        {
            return db.tUserVitals;
        }

        // GET: api/UserVitals/5
        [Route("api/UserData/GetUserVitals/{id}")]
        [ResponseType(typeof(tUserVital))]
        public async Task<IHttpActionResult> GettUserVital(int id)
        {
            tUserVital tUserVital = await db.tUserVitals.FindAsync(id);
            if (tUserVital == null)
            {
                return NotFound();
            }

            return Ok(tUserVital);
        }

        // PUT: api/UserVitals/5
        [Route("api/UserData/UpdateUserVitals/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttUserVital(int id, tUserVital tUserVital)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUserVital.ID)
            {
                return BadRequest();
            }

            db.Entry(tUserVital).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserVitalExists(id))
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

        // POST: api/UserVitals
        [Route("api/UserData/PostUserVitals")]
        [ResponseType(typeof(tUserVital))]
        public async Task<IHttpActionResult> PosttUserVital(tUserVital tUserVital)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tUserVitals.Add(tUserVital);
            await db.SaveChangesAsync();
            return Ok(tUserVital);
            //   return CreatedAtRoute("DefaultApi", new { id = tUserVital.ID }, tUserVital);
        }

        // DELETE: api/UserVitals/5
        [Route("api/UserData/DeleteUserVitals/{id}")]
        [ResponseType(typeof(tUserVital))]
        public async Task<IHttpActionResult> DeletetUserVital(int id)
        {
            tUserVital tUserVital = await db.tUserVitals.FindAsync(id);
            if (tUserVital == null)
            {
                return NotFound();
            }

            db.tUserVitals.Remove(tUserVital);
            await db.SaveChangesAsync();

            return Ok(tUserVital);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserVitalExists(int id)
        {
            return db.tUserVitals.Count(e => e.ID == id) > 0;
        }

        // GET: api/UserData/GetUserLast5WeightVitals
        [HttpPost]
        [Route("api/UserData/GetUserLast5WeightVitals")]
        public async Task<IHttpActionResult> GetUserLast5WeightVitals(HealthGoalModel healthGoalModel)
        {
            try
            {
                var userVitals = await db.tUserVitals
                                           .Where(u => u.UserID == healthGoalModel.UserId &&
                                                       u.SystemStatusID == 1 &&
                                                       u.Name == "Weight")
                                           .OrderByDescending(u => u.ResultDateTime)
                                           .Take(5)
                                           .ToListAsync();
                return Ok(userVitals);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // GET: api/UserData/GetUserWeightVitals
        [HttpPost]
        [Route("api/UserData/GetUserWeightVitals")]
        public async Task<IHttpActionResult> GetUserWeightVitals(HealthGoalModel healthGoalModel)
        {
            try
            {
                if (healthGoalModel.Last5 != null && healthGoalModel.Last5 == true)
                {
                    var userVitals = await db.tUserVitals
                                .Where(u => u.UserID == healthGoalModel.UserId && 
                                            u.SystemStatusID == 1 && 
                                            u.Name == "Weight")
                                .OrderByDescending(u => u.ResultDateTime)
                                .Take(5)
                                .ToListAsync();
                    return Ok(userVitals);
                }
                else
                {
                    var userVitals = await db.tUserVitals
                                .OrderBy(u => u.ResultDateTime)
                                .Where(u => u.UserID == healthGoalModel.UserId &&
                                            u.ResultDateTime >= healthGoalModel.StartDate &&
                                            u.ResultDateTime <= healthGoalModel.EndDate &&
                                            u.SystemStatusID == 1 &&
                                            u.Name == "Weight")
                                .ToListAsync();

                    return Ok(userVitals);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [Route("api/UserData/GetUserWeightGoalVital/{id}")]
        public async Task<IHttpActionResult> GetUserWeightGoalVital(int id)
        {
            var tUserVital =
               await db.tUserVitals.Where(u => u.UserID == id &&
                                          u.ResultDateTime <= DateTimeOffset.Now &&
                                          u.Name == "Weight" &&
                                          u.SystemStatusID == 1)
                    .OrderByDescending(u => u.ResultDateTime)
                    .FirstOrDefaultAsync() ?? new tUserVital();

            var goal = await db.tUserHealthGoals
                .Where(g => g.GoalTypeID == 1 && g.SystemStatusID == 1 && g.UserID == id)
                .OrderByDescending(g => g.CreateDateTime)
                .FirstOrDefaultAsync() ?? new tUserHealthGoal();

            return Ok(new
            {
                ResultDateTime = tUserVital.ResultDateTime,
                Weight = tUserVital.Value,
                UOMID = tUserVital.UOMID,
                Goal = goal.Value
            });
        }

        // GET: api/UserVitals
        [Route("api/UserData/SetUserWeightGoal")]
        [HttpPost]
        public async Task<IHttpActionResult> SetUserWeightGoal(tUserHealthGoal healthGoal)
        {
            var tHealthGoal = await db.tUserHealthGoals.Where(u => u.UserID == healthGoal.UserID && u.SystemStatusID == 1 && u.GoalTypeID == 1).SingleOrDefaultAsync();

            if (tHealthGoal != null)
            {
                tHealthGoal.SystemStatusID = 2;
                tHealthGoal.LastUpdatedDateTime = DateTime.Now;
            }

            healthGoal.GoalTypeID = 1;
            healthGoal.CreateDateTime = DateTime.Now;
            healthGoal.ObjectID = new Guid();
            healthGoal.SystemStatusID = 1;

            db.tUserHealthGoals.Add(healthGoal);
            await db.SaveChangesAsync();
            return Ok();
        }



        // GET: api/UserVitals
        [Route("api/UserData/GetUserBloodPressure")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserBloodPressure(HealthGoalModel healthGoalModel)
        {
            try
            {
                var idList = new string[] { "Systolic Blood Pressure", "Diastolic Blood Pressure", "Heart Rate" };

                if (healthGoalModel.Last5 != null && healthGoalModel.Last5 == true)
                {
                    var vitals = await db.tUserVitals
                                            .Where(u => u.UserID == healthGoalModel.UserId && 
                                                        u.SystemStatusID == 1 && 
                                                        idList.Contains(u.Name))
                                            .GroupBy(u =>
                                                new { ResulDateTime = u.ResultDateTime })
                                            .Select(details => new UserVitalModel
                                            {
                                                ResultDateTime = details.Key.ResulDateTime,
                                                Systolic = details.FirstOrDefault(u => (u.Name == "Systolic Blood Pressure")).Value,
                                                Diastolic = details.FirstOrDefault(u => (u.Name == "Diastolic Blood Pressure")).Value,
                                                HeartRate = details.FirstOrDefault(u => (u.Name == "Heart Rate")).Value
                                            })
                                            .OrderByDescending(u => u.ResultDateTime)
                                            .Take(5)
                                            .ToListAsync();

                    return Ok(vitals);
                }
                else
                {
                    var vitals = await db.tUserVitals
                                        .Where(u => u.UserID == healthGoalModel.UserId && 
                                                    u.ResultDateTime >= healthGoalModel.StartDate &&
                                                    u.ResultDateTime <= healthGoalModel.EndDate && 
                                                    u.SystemStatusID == 1
                                                    && idList.Contains(u.Name))
                                        .GroupBy(u =>
                                            new { ResulDateTime = u.ResultDateTime })
                                        .Select(details => new UserVitalModel
                                        {
                                            ResultDateTime = details.Key.ResulDateTime,
                                            Systolic = details.FirstOrDefault(u => (u.Name == "Systolic Blood Pressure")).Value,
                                            Diastolic = details.FirstOrDefault(u => (u.Name == "Diastolic Blood Pressure")).Value,
                                            HeartRate = details.FirstOrDefault(u => (u.Name == "Heart Rate")).Value
                                        })
                                        .OrderBy(u => u.ResultDateTime)
                                        .ToListAsync();

                    return Ok(vitals);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // GET: api/UserVitals
        [Route("api/UserData/GetUserCurrentBloodPressure")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserCurrentBloodPressure(HealthGoalModel healthGoalModel)
        {
            try
            {
                var idList = new string[] { "Systolic Blood Pressure", "Diastolic Blood Pressure", "Heart Rate" };
                List<DAL.tUserVital> vitals;

                if (healthGoalModel.Last5 != null && healthGoalModel.Last5 == true)
                {
                    vitals = await db.tUserVitals
                                        .Where(u => u.UserID == healthGoalModel.UserId && 
                                                    u.SystemStatusID == 1 &&
                                                    idList.Contains(u.Name))
                                        .ToListAsync();
                }
                else
                {
                    vitals = await db.tUserVitals
                                        .Where(u => u.UserID == healthGoalModel.UserId &&
                                                    u.ResultDateTime >= healthGoalModel.StartDate &&
                                                    u.ResultDateTime <= healthGoalModel.EndDate &&
                                                    u.SystemStatusID == 1 &&
                                                    idList.Contains(u.Name))
                                        .ToListAsync();
                }

                var systolic = vitals.Where(u => u.Name == "Systolic Blood Pressure" && u.Value > 0)
                    .OrderByDescending(u => u.ResultDateTime)
                    .FirstOrDefault();

                var diastolic = vitals.Where(u => u.Name == "Diastolic Blood Pressure" && u.Value > 0)
                       .OrderByDescending(u => u.ResultDateTime)
                       .FirstOrDefault();

                var heartRate = vitals.Where(u => u.Name == "Heart Rate" && u.Value > 0)
                     .OrderByDescending(u => u.ResultDateTime)
                     .FirstOrDefault();
                
                return Ok(new
                {
                    Systolic = systolic?.Value ?? 0,
                    Diastolic = diastolic?.Value ?? 0,
                    HeartRate = heartRate?.Value ?? 0,
                    ResultDateTime = diastolic?.ResultDateTime
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // GET: api/UserVitals
        [HttpPost]
        [Route("api/UserData/GetUserBodyComposition")]
        public async Task<IHttpActionResult> GetUserBodyComposition(HealthGoalModel healthGoalModel)
        {
            try
            {
                var idList = new string[] { "Weight", "Height" };
                var femaleGender = new string[] { "Female", "Female-to-Male (FTM)" };

                var userVitals = await db.tUserVitals
               .Where(u => u.UserID == healthGoalModel.UserId && u.SystemStatusID == 1 && idList.Contains(u.Name))
               .ToListAsync();

                var user = await dbUsers.tUsers.Include(u => u.tGender).Where(u => u.ID == healthGoalModel.UserId).FirstOrDefaultAsync();

                int genderMode;
                if (user.tGender != null)
                {
                    genderMode = femaleGender.Contains(user.tGender.Gender) ? 0 : 1;
                }
                else
                {
                    genderMode = 1;
                }

                var weightDetails = userVitals.Where(u => u.Name == "Weight" && u.Value > 0).OrderByDescending(u => u.ResultDateTime).FirstOrDefault();

                double weight = 0, height = 0, bmi = 0, adultBodyFatPercent = 0, bodyFat = 0, leanTissue = 0;
                if (weightDetails != null)
                {
                    switch (weightDetails.UOMID)
                    {
                        case 128://kg
                            weight = Convert.ToDouble(weightDetails.Value) * 2.20462;
                            break;
                        case 139://lbs
                            weight = Convert.ToDouble(weightDetails.Value);
                            break;
                    }
                }

                var heightDetails = userVitals.Where(u => u.Name == "Height" && u.Value > 0).OrderByDescending(u => u.ResultDateTime).FirstOrDefault();
                if (heightDetails != null)
                {
                    switch (heightDetails.UOMID)
                    {
                        case 251://mm
                            height = (Convert.ToDouble(heightDetails.Value) / 25.4);
                            break;
                        case 50://cm
                            height = (Convert.ToDouble(heightDetails.Value) / 2.54);
                            break;
                        case 109://in
                            height = Convert.ToDouble(heightDetails.Value);
                            break;
                        default:
                            switch (genderMode)
                            {
                                case 0:
                                    height = 62;
                                    break;
                                case 1:
                                    height = 66;
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    switch (genderMode)
                    {
                        case 0:
                            height = 62;
                            break;
                        case 1:
                            height = 66;
                            break;
                    }
                }

                int iAge = 45;
                if (user.DOB != null)
                {
                    iAge = (DateTime.Now.Year - user.DOB.Value.Year);
                }

                if (weight > 0 && height > 0)
                {
                    bmi = Math.Round((weight / (height * height)) * 703,2);
                    adultBodyFatPercent = Math.Round((1.2 * bmi) + (0.23 * iAge) - (10.8 * genderMode) - 5.4,2);
                    bodyFat = Math.Round(((adultBodyFatPercent / 100) * weight),2);
                    leanTissue = Math.Round(weight - ((adultBodyFatPercent / 100) * weight), 2);
                }

                var lstBodyComposition = new List<UserBodyComposition>
                {
                    new UserBodyComposition
                    {
                        Source = "BodyFat",
                        Value = bodyFat,
                        ResultDateTime = weightDetails?.ResultDateTime.Date
                    },
                    new UserBodyComposition
                    {
                        Source = "LeanTissue",
                        Value = leanTissue,
                        ResultDateTime = weightDetails?.ResultDateTime.Date
                    }
                };
                
                return Ok(lstBodyComposition);
            }
            catch (Exception ex)
            {

                throw;
            }


        }



        // DELETE: api/UserTest/5
        [Route("api/UserData/SoftDeleteUserVitals/{id}/{status}")]
        [ResponseType(typeof(tUserVital))]
        [HttpGet]
        public async Task<IHttpActionResult> SoftDeleteUserVitals(int id, int status)
        {
            tUserVital tUserVital = await db.tUserVitals.FindAsync(id);
            if (tUserVital == null)
            {
                return NotFound();
            }
            tUserVital.SystemStatusID = status;
            tUserVital.LastUpdatedDateTime = DateTime.Now;
            await db.SaveChangesAsync();
            return Ok(tUserVital);
        }


        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendBloodPressure")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserTrendBloodPressure(HealthGoalModel healthGoalModel)
        {
            try
            {

                var idList = new string[] { "Systolic Blood Pressure", "Diastolic Blood Pressure", "Heart Rate" };

                var result = await
                            db.tUserVitals
                            .Include(u => u.tUnitsOfMeasure)
                            .Include(u => u.tSourceOrganization.tOrganization)
                            .Include(u => u.tUserSourceService.tSourceService)
                            .Where(u =>  u.SystemStatusID == 1 && u.UserID == healthGoalModel.UserId && u.ResultDateTime >=
                            healthGoalModel.StartDate && u.ResultDateTime <= healthGoalModel.EndDate && idList.Contains(u.Name) )
                            .Select(t => new {
                                t.UserID,
                                t.ResultDateTime,
                                t.ID,
                                tSourceOrganization = t.tSourceOrganization.tOrganization,
                                tUserSourceService = t.tUserSourceService.tSourceService,
                                t.Name,
                                t.ObjectID,
                                t.SourceObjectID,
                                t.SourceOrganizationID,
                                t.UserSourceServiceID,
                                t.Value,
                                t.UOMID,
                                t.SystemStatusID,
                                t.tUnitsOfMeasure.UnitOfMeasure,
                                Note = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == t.ObjectID)
                            }).OrderBy(u=>u.ResultDateTime)
                            .ToListAsync();



                var retVal = new List<UserVital>();

                foreach (var ts in result)
                {


                    var bpitem = new UserVital
                    {
                        NoteID = ts.Note?.ID ?? 0,
                        ID = ts.ID,
                        Note = ts.Note?.Note ?? "",
                        ResultDateTime = ts.ResultDateTime,
                        Source = ts.SourceOrganizationID != null ? ts.tSourceOrganization.Name : ts.tUserSourceService.ServiceName,
                        UOMID = ts.UOMID,
                        Value = ts.Value,
                        UserID = ts.UserID,
                        SourceObjectID = ts.SourceObjectID,
                        SourceOrganizationID = ts.SourceOrganizationID,
                        UserSourceServiceID = ts.UserSourceServiceID,
                        SystemStatusID = ts.SystemStatusID,
                        UnitOfMeasure = ts.UnitOfMeasure,
                        Name = ts.Name
                    };
                    retVal.Add(bpitem);
                }

                return Ok(retVal);

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendBodyComposition")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserTrendBodyComposition(HealthGoalModel healthGoalModel)
        {
            try
            {


                var result = await
                            db.tUserVitals
                            .Include(u => u.tUnitsOfMeasure)
                            .Include(u => u.tSourceOrganization.tOrganization)
                            .Include(u => u.tUserSourceService.tSourceService)
                            .Where(u => u.SystemStatusID == 1 && u.UserID == healthGoalModel.UserId && u.ResultDateTime >=
                            healthGoalModel.StartDate && u.ResultDateTime <= healthGoalModel.EndDate && u.Name == "Weight")
                            .Select(t => new {
                                t.UserID,
                                t.ResultDateTime,
                                t.ID,
                                tSourceOrganization = t.tSourceOrganization.tOrganization,
                                tUserSourceService = t.tUserSourceService.tSourceService,
                                t.Name,
                                t.ObjectID,
                                t.SourceObjectID,
                                t.SourceOrganizationID,
                                t.UserSourceServiceID,
                                t.Value,
                                t.UOMID,
                                t.SystemStatusID,
                                t.tUnitsOfMeasure.UnitOfMeasure,
                                Note = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == t.ObjectID)
                            }).OrderBy(u => u.ResultDateTime)
                            .ToListAsync();



                var retVal = new List<UserVital>();

                foreach (var ts in result)
                {

                    var bpitem = new UserVital
                    {
                        NoteID = ts.Note?.ID ?? 0,
                        ID = ts.ID,
                        Note = ts.Note?.Note ?? "",
                        ResultDateTime = ts.ResultDateTime,
                        Source = ts.SourceOrganizationID != null ? ts.tSourceOrganization.Name : ts.tUserSourceService.ServiceName,
                        UOMID = ts.UOMID,
                        Value = ts.Value,
                        UserID = ts.UserID,
                        SourceObjectID = ts.SourceObjectID,
                        SourceOrganizationID = ts.SourceOrganizationID,
                        UserSourceServiceID = ts.UserSourceServiceID,
                        SystemStatusID = ts.SystemStatusID,
                        UnitOfMeasure = ts.UnitOfMeasure,
                        Name = ts.Name
                    };
                    retVal.Add(bpitem);
                }

                return Ok(retVal);

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendWeight")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserTrendWeight(HealthGoalModel healthGoalModel)
        {
            try
            {


                var result = await
                            db.tUserVitals
                            .Include(u => u.tUnitsOfMeasure)
                            .Include(u => u.tSourceOrganization.tOrganization)
                            .Include(u => u.tUserSourceService.tSourceService)
                            .Where(u => u.SystemStatusID == 1 && u.UserID == healthGoalModel.UserId && u.ResultDateTime >=
                            healthGoalModel.StartDate && u.ResultDateTime <= healthGoalModel.EndDate && u.Name == "Weight")
                            .Select(t => new {
                                t.UserID,
                                t.ResultDateTime,
                                t.ID,
                                tSourceOrganization = t.tSourceOrganization.tOrganization,
                                tUserSourceService = t.tUserSourceService.tSourceService,
                                t.Name,
                                t.ObjectID,
                                t.SourceObjectID,
                                t.SourceOrganizationID,
                                t.UserSourceServiceID,
                                t.Value,
                                t.UOMID,
                                t.SystemStatusID,
                                t.tUnitsOfMeasure.UnitOfMeasure,
                                Note = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == t.ObjectID)
                            }).OrderBy(u => u.ResultDateTime)
                            .ToListAsync();


                var retVal = new List<UserVital>();

                foreach (var ts in result)
                {

                    var bpitem = new UserVital
                    {
                        NoteID = ts.Note?.ID ?? 0,
                        ID = ts.ID,
                        Note = ts.Note?.Note ?? "",
                        ResultDateTime = ts.ResultDateTime,
                        Source = ts.SourceOrganizationID != null ? ts.tSourceOrganization.Name : ts.tUserSourceService.ServiceName,
                        UOMID = ts.UOMID,
                        Value = ts.Value,
                        UserID = ts.UserID,
                        SourceObjectID = ts.SourceObjectID,
                        SourceOrganizationID = ts.SourceOrganizationID,
                        UserSourceServiceID = ts.UserSourceServiceID,
                        SystemStatusID = ts.SystemStatusID,
                        UnitOfMeasure = ts.UnitOfMeasure,
                        Name = ts.Name
                    };
                    retVal.Add(bpitem);
                }

                return Ok(retVal);

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendBloodPressureAccounts/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserTrendBloodPressureAccounts(int id)
        {
            try
            {
                var idList = new string[] { "Systolic Blood Pressure", "Diastolic Blood Pressure", "Heart Rate" };

                var list = await (from tr in db.tUserVitals
                                  join uss in db.tUserSourceServices on new { p1 = tr.UserSourceServiceID, p2 = tr.UserID } equals new { p1 = (int?)uss.ID, p2 = uss.UserID }
                                  join so in db.tSourceOrganizations on uss.SourceServiceID equals so.SourceServiceID
                                  join o in db.tOrganizations on so.OrganizationID equals o.ID
                                  where uss.SystemStatusID == 1 && tr.UserID == id && idList.Contains(tr.Name) 
                                  select new
                                  {
                                      ID = o.ID,
                                      Name = o.Name
                                  }).Distinct().ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendBodyCompositionAccounts/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserTrendBodyCompositionAccounts(int id)
        {
            try
            {

                var list = await (from tr in db.tUserVitals
                                  join uss in db.tUserSourceServices on new { p1 = tr.UserSourceServiceID, p2 = tr.UserID } equals new { p1 = (int?)uss.ID, p2 = uss.UserID }
                                  join so in db.tSourceOrganizations on uss.SourceServiceID equals so.SourceServiceID
                                  join o in db.tOrganizations on so.OrganizationID equals o.ID
                                  where uss.SystemStatusID == 1 && tr.UserID == id && tr.Name == "Weight"
                                  select new
                                  {
                                      ID = o.ID,
                                      Name = o.Name
                                  }).Distinct().ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        // GET: api/UserVitals
        [Route("api/UserData/GetUserTrendWeightAccounts/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserTrendWeightAccounts(int id)
        {
            try
            {

                var list = await (from tr in db.tUserVitals
                                  join uss in db.tUserSourceServices on new { p1 = tr.UserSourceServiceID, p2 = tr.UserID } equals new { p1 = (int?)uss.ID, p2 = uss.UserID }
                                  join so in db.tSourceOrganizations on uss.SourceServiceID equals so.SourceServiceID
                                  join o in db.tOrganizations on so.OrganizationID equals o.ID
                                  where uss.SystemStatusID == 1 && tr.UserID == id && tr.Name == "Weight"
                                  select new
                                  {
                                      ID = o.ID,
                                      Name = o.Name
                                  }).Distinct().ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}