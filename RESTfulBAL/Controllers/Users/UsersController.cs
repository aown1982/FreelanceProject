using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;
using DAL.Users;
using RESTfulBAL.Utilities;
using RESTfulBAL.Utilities.PasswordSecurity;
using RESTfulBAL.Utilities.AuditHandling;
using RESTfulBAL.Utilities.ErrorHandling;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using RESTfulBAL.Models;
using DAL.UserData;

namespace RESTfulBAL.Controllers.Users
{
    public class UsersController : ApiController
    {
        private UsersEntities db = new UsersEntities();
        private UserDataEntities userDatadb = new UserDataEntities();

        // GET: api/Users
        //[Route("api/Users")]
        //public IQueryable<tUser> GettUsers()
        //{
        //    return db.tUsers;
        //}

        // GET: api/Users/5

        [Route("api/Users/UserByEID/{ExternalId}")]
        [ResponseType(typeof(tUser))]
        public IHttpActionResult GetUserByEID(System.Guid ExternalId)
        {
            tUser tUser = db.tUsers.SingleOrDefault(x => x.ExternalID == ExternalId); ;
            if (tUser == null)
            {
                return NotFound();
            }

            return Ok(tUser);
        }

        // GET: api/Users/UserByAuth/5
        [Route("api/Users/UserByAuth/{AuthId}")]
        [ResponseType(typeof(tUserLoginAuth))]
        public IHttpActionResult GetUserByAuth(System.Guid AuthId)
        {
            tUserLoginAuth tUserLoginAuth = db.tUserLoginAuths
                                                        .Include("tUser")
                                                        .SingleOrDefault(x => x.ID == AuthId);
            if (tUserLoginAuth == null)
            {
                return NotFound();
            }

            return Ok(tUserLoginAuth);
        }

        // GET: api/Users/Login
        [HttpPost]
        [Route("api/Users/Login")]
        [ResponseType(typeof(tUserLoginAuth))]
        public IHttpActionResult GetLogin(LoginModel model)
        {
            try
            {
                tUser tUser = db.tUsers
                                .Include("tSalt")
                                //.Include("tUserLoginAuths")
                                .SingleOrDefault(x => x.Email == model.Username &&
                                                      x.AccountStatusID == 1);

                if (tUser == null)
                {
                    throw new UserInvalidLoginException(AuditLogging.ErrMsg_Invalid_Username);
                    //return NotFound();
                }

                if (model.Password != null)
                {
                    PasswordStorage oPassUtil = new PasswordStorage();
                    oPassUtil.Hashstring = tUser.PasswordHash;
                    oPassUtil.Saltstring = tUser.tSalt.Salt;
                    if (!oPassUtil.VerifyPassword(model.Password))
                    {
                        throw new UserInvalidLoginException(AuditLogging.ErrMsg_Invalid_Password, tUser.ID);
                        //return Unauthorized();
                    }
                }


                tUserLoginAuth userLoginAuth = db.tUserLoginAuths.FirstOrDefault(x => x.UserID == tUser.ID && x.ExpirationDate > DateTime.Now);
                if (userLoginAuth != null)
                {
                    //return existing auth
                    return Ok(userLoginAuth);
                }
                else
                {
                    //Insert new auth into LoginAuth
                    userLoginAuth = new tUserLoginAuth();
                    userLoginAuth.UserID = tUser.ID;
                    userLoginAuth.tUser = tUser;

                    db.tUserLoginAuths.Add(userLoginAuth);
                }

                //Insert Audit Log
                tUsersAudit userAuditLog = new tUsersAudit();
                userAuditLog.ApplicationID = (int)AuditLogging.enumApplication.SFCWebSite;
                userAuditLog.EventID = (int)AuditLogging.enumEvent.Security_Login_Success;
                userAuditLog.UserID = tUser.ID;
                userAuditLog.Description = AuditLogging.const_Successful_Login + " from IP Address: " + model.IpAddress;
                userAuditLog.TypeID = 7;//Login

                db.tUsersAudits.Add(userAuditLog);

                //Commit All
                db.SaveChanges();

                return Ok(userLoginAuth);

            }
            catch (UserInvalidLoginException exLogin)
            {
                if (exLogin.Message == AuditLogging.ErrMsg_Invalid_Username)
                {
                    //Insert Error Log for bad username
                    string sTrace = "UserName: " + model.Username +
                                    "| IP Address: " + model.IpAddress;

                    tUsersErrLog userErrorLog = new tUsersErrLog();

                    userErrorLog.ErrTypeID = (int)ErrorLogging.enumErrorType.Security;
                    userErrorLog.ErrSourceID = (int)AuditLogging.enumApplication.SFCWebSite;
                    userErrorLog.Description = exLogin.Message;
                    userErrorLog.Trace = sTrace;


                    db.tUsersErrLogs.Add(userErrorLog);
                    db.SaveChanges();

                    return NotFound();
                }
                else if (exLogin.Message == AuditLogging.ErrMsg_Invalid_Password)
                {
                    //Insert Audit Log for bad password
                    tUsersAudit userAuditLog = new tUsersAudit();
                    userAuditLog.ApplicationID = (int)AuditLogging.enumApplication.SFCWebSite;
                    userAuditLog.EventID = (int)AuditLogging.enumEvent.Security_Login_Failed;
                    userAuditLog.UserID = exLogin.UserID;
                    userAuditLog.Description = exLogin.Message + " from IP Address: " + model.IpAddress;
                    userAuditLog.TypeID = 12;//LoginErr

                    db.tUsersAudits.Add(userAuditLog);
                    db.SaveChanges();

                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                //Insert Error Log
                tUsersErrLog userErrorLog = new tUsersErrLog();

                userErrorLog.ErrTypeID = (int)ErrorLogging.enumErrorType.Application;
                userErrorLog.ErrSourceID = (int)AuditLogging.enumApplication.SFCBAL;
                userErrorLog.Code = ex.HResult.ToString();
                userErrorLog.Description = ex.Message;
                userErrorLog.Trace = ex.StackTrace;

                db.tUsersErrLogs.Add(userErrorLog);
                db.SaveChanges();

                string ErrMsg = "An error occured and we have logged the error. Please try again later.";

                Exception Err = new Exception(ErrMsg, ex);

                return InternalServerError(Err);
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/Users/ValidatePassword")]
        [ResponseType(typeof(tUser))]
        public IHttpActionResult ValidatePassword(tUser modal)
        {
            tUser tUser = db.tUsers
                        .Include("tSalt")
                        .SingleOrDefault(x => x.ID == modal.ID);

            if (modal.PasswordHash != null)
            {
                PasswordStorage oPassUtil = new PasswordStorage();
                if (tUser != null)
                {
                    oPassUtil.Hashstring = tUser.PasswordHash;
                    oPassUtil.Saltstring = tUser.tSalt.Salt;
                    if (!oPassUtil.VerifyPassword(modal.PasswordHash))
                    {
                        return Unauthorized();
                        //return Unauthorized();
                    }
                }
            }
            return Ok();
        }

        //****** Account Management *********//
        // POST: api/Users/RegisterUser
        [HttpPost]
        [Route("api/Users/GetUserByEmail")]
        [ResponseType(typeof(tUser))]
        public IHttpActionResult UserAlreadyExist(tUser tUser)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = db.tUsers.FirstOrDefault(x => x.Email == tUser.Email && x.ID != tUser.ID);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }



        [HttpPost]
        [Route("api/Users/UpdateUserLanguageLocation")]
        [ResponseType(typeof(tUser))]
        public IHttpActionResult UpdateUserLanguageLocation(tUser tUser)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = db.tUsers.Include(u => u.tUsersAddressHistories).FirstOrDefault(x => x.ID == tUser.ID);
            var state = tUser.tUsersAddressHistories.FirstOrDefault().StateID;
            if (user == null)
            {
                return NotFound();
            }
            user.LanguageID = tUser.LanguageID;

            if (user.tUsersAddressHistories.Count > 0)
            {
                user.tUsersAddressHistories.FirstOrDefault().StateID = tUser.tUsersAddressHistories.FirstOrDefault().StateID;
            }
            else
            {
                user.tUsersAddressHistories.Add(new tUsersAddressHistory { UserID = user.ID, StateID = state });
            }

            db.SaveChanges();
            return Ok(user);
        }




        [HttpPost]
        [Route("api/Users/UpdateUserDobGender")]
        [ResponseType(typeof(tUser))]
        public IHttpActionResult UpdateUserDobGender(tUser tUser)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = db.tUsers.Include(u => u.tUsersAddressHistories).FirstOrDefault(x => x.ID == tUser.ID);
            if (user == null)
            {
                return NotFound();
            }
            user.GenderID = tUser.GenderID;
            user.DOB = tUser.DOB;

            db.SaveChanges();
            return Ok(user);
        }




        [Route("api/Users/GetUserById/{id}")]
        [ResponseType(typeof(tUser))]
        public IHttpActionResult GetUserById(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = db.tUsers.FirstOrDefault(x => x.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/Users/RegisterUser
        [Route("api/Users/RegisterUser")]
        [ResponseType(typeof(tUser))]
        public async Task<IHttpActionResult> RegisterUser(tUser tUser)
        {
            //tUser user = await db.tUsers.FirstAsync(x => x.Email == tUser.Email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var user = db.tUsers.FirstOrDefault(x => x.Email == tUser.Email);
            //if (user != null)
            //{
            //    return Conflict();
            //}

            try
            {
                tUser.ExternalID = Guid.NewGuid();
                tUser.CreateDateTime = DateTime.Now;
                tUser.AccountStatusID = 1;
                //tUser.PHSaltID = 1;

                if (tUser.PasswordHash != null)
                {
                    PasswordStorage oPassUtil = new PasswordStorage();
                    //oPassUtil.Hashstring = ;
                    oPassUtil.CreateHash(tUser.PasswordHash);
                    //oPassUtil.VerifyPassword
                    tUser.PasswordHash = oPassUtil.Hashstring;

                    tSalt salt = new tSalt();
                    salt.Salt = oPassUtil.Saltstring;

                    SaltsController sl = new SaltsController();
                    tUser.tSalt = await sl.PosttSalt(salt);
                    tUser.PHSaltID = tUser.tSalt.Id;
                }

                db.tUsers.Add(tUser);
                db.SaveChanges();

                //post new userid to other dbs
                UserData.UserIDsController userIds = new UserData.UserIDsController();
                tUserID userID = new tUserID();
                userID.UserID = tUser.ID;

                await userIds.PosttUserID(userID);

                WebApp.UserIDsController webUsers = new WebApp.UserIDsController();
                await webUsers.PosttUserID(userID);

                //Add default consent to new user's share settings
                tUserSHARESetting userSHARESetting = new tUserSHARESetting();
                tXrefUserSHARESettingsPurpos userXrefSHARE = new tXrefUserSHARESettingsPurpos();
                userSHARESetting.AllData = true;
                userSHARESetting.UserID = tUser.ID;
                userSHARESetting.SHARESettingID = 1; //allow
                userSHARESetting.SystemStatusID = 1; //valid
                userXrefSHARE.SHARESettingID = userSHARESetting.ID;
                userXrefSHARE.SHAREPurposeID = 2; //Research only, any available
                userSHARESetting.tXrefUserSHARESettingsPurposes.Add(userXrefSHARE);

                UserData.UserSHARESettingsController shareController = new UserData.UserSHARESettingsController();
                await shareController.PosttUserSHARESetting(userSHARESetting);

                //return CreatedAtRoute("UsersAPI", new { id = tUser.ID }, tUser);
                return Ok(tUser);
            }
            catch (Exception ex)
            {
                //Insert Error Log
                tUsersErrLog userErrorLog = new tUsersErrLog();

                userErrorLog.ErrTypeID = (int)ErrorLogging.enumErrorType.Application;
                userErrorLog.ErrSourceID = (int)AuditLogging.enumApplication.SFCBAL;
                userErrorLog.Code = ex.HResult.ToString();
                userErrorLog.Description = ex.Message;
                userErrorLog.Trace = ex.StackTrace;

                db.tUsersErrLogs.Add(userErrorLog);
                db.SaveChanges();

                string ErrMsg = "An error occured and we have logged the error. Please try again later.";

                Exception Err = new Exception(ErrMsg, ex);

                return InternalServerError(Err);
            }
        }

        // PUT: api/Users/UpdateUser/5
        [HttpPut]
        [Route("api/Users/UpdateUser/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateUser(int id, tUser tUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUser.ID)
            {
                return BadRequest();
            }
            var user = await db.tUsers.FindAsync(id);
            user.PasswordHash = tUser.PasswordHash;

            if (user.PasswordHash != null)
            {
                PasswordStorage oPassUtil = new PasswordStorage();
                //oPassUtil.Hashstring = ;
                oPassUtil.CreateHash(user.PasswordHash);
                //oPassUtil.VerifyPassword
                user.PasswordHash = oPassUtil.Hashstring;

                tSalt salt = new tSalt { Salt = oPassUtil.Saltstring };

                SaltsController sl = new SaltsController();
                user.tSalt = await sl.EditSalt(salt.Id, salt);
                if (user.tSalt != null)
                {
                    user.PHSaltID = tUser.tSalt.Id;
                }

            }
            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!tUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.OK);
        }

        // PUT: api/Users/UpdateUserBasicInfo/5
        [HttpPut]
        [Route("api/Users/UpdateUserBasicInfo/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateUserBasicInfo(int id, tUser tUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tUser.ID)
            {
                return BadRequest();
            }

            db.Entry(tUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("api/Users/GetAuditLogReport/{userId}")]
        public List<tUsersAudit> GetAuditLogReport(int userId)
        {
            var mainList = new List<tUsersAudit>();
            List<tUsersAudit> list1 = db.tUsersAudits.Where(a => a.UserID == userId).ToList();
            if (list1.Any())
            {
                mainList.AddRange(list1);
            }
            var list2 = userDatadb.tUserDataAudits.Where(x => x.UserID == userId).ToList();
            if (list2.Any())
            {
                foreach (var item in list2)
                {
                    mainList.Add(new tUsersAudit
                    {
                        ApplicationID = item.ApplicationID,
                        DateTimeStamp = item.DateTimeStamp,
                        Description = item.Description,
                        EventID = item.EventID,
                        Id = item.Id,
                        OriginalValue = item.OriginalValue,
                        NewValue = item.NewValue,
                        UserID = item.UserID
                    });
                }
            }
            return mainList;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tUserExists(int id)
        {
            return db.tUsers.Count(e => e.ID == id) > 0;
        }

        [HttpGet]
        [Route("api/Users/GetUserSettingDetails/{id}")]
        public async Task<IHttpActionResult> GetUserSettingDetails(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await db.tUsers
                    .Include(a => a.tLanguage)
                    .Where(u => u.ID == id)
                    .SelectMany(u => u.tUsersAddressHistories).Select(x => new
                    {
                        TimeZone = x.tTimeZone,
                        Location = x.tState,
                        Language = x.tUser.tLanguage,
                    }).FirstOrDefaultAsync();

                var settings = new SettingViewModel();
                if (user != null)
                {
                    if (user.Language != null)
                    {
                        settings.Language = user.Language.Language;
                        settings.LanguageId = user.Language.Id;
                    }
                    if (user.Location != null)
                    {
                        settings.Location = user.Location.StateName;
                        settings.LocationId = user.Location.Id;
                    }
                    if (user.TimeZone != null)
                    {
                        settings.TimeZone = user.TimeZone.TimeZone;
                        settings.TimeZoneId = user.TimeZone.Id;
                    }
                }

                return Ok(settings);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [HttpPost]
        [Route("api/Users/GetUserPasswordResetCode")]
        [ResponseType(typeof(tUser))]
        public async Task<IHttpActionResult> GetUserPasswordResetCode(tUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var users = await db.tUsers.Where(u => u.Email == user.Email && u.AccountStatusID == 1).FirstOrDefaultAsync();

                if (users != null)
                {
                    var userPasswordReset = new tUserPasswordReset
                    {
                        UserID = users.ID,
                        ExternalUserID = users.ExternalID,
                        ResetCodeID = Guid.NewGuid(),
                        CreateDateTime = DateTime.Now,
                    };
                    db.tUserPasswordResets.Add(userPasswordReset);
                    await db.SaveChangesAsync();

                    users.tUserPasswordResets.Add(userPasswordReset);

                    return Ok(users);
                }
                else
                {
                    
                    return StatusCode(HttpStatusCode.NotFound);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }

        }


        [HttpPost]
        [Route("api/Users/GetUserPasswordReset")]
        public async Task<IHttpActionResult> GetUserPasswordReset(tUserPasswordReset passwordReset)
        {
          try
            {
                var pwdReset = await db.tUserPasswordResets.Where(u => u.ExternalUserID== passwordReset.ExternalUserID 
                && u.ResetCodeID == passwordReset.ResetCodeID).OrderByDescending(u=>u.CreateDateTime).FirstOrDefaultAsync();

                if (pwdReset != null)
                {
                   if(pwdReset.UsedDateTime != null)
                    {
                        return Unauthorized();// BadRequest("Code has already been used!");
                    }
                   else
                    {
                        if((DateTime.Now - pwdReset.CreateDateTime).TotalDays > 2)
                        {
                            return BadRequest("Reset Code has expired");
                        }
                        else
                        {
                            return Ok(pwdReset);
                        }
                    }
                }
                else
                {

                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }

        }


        [HttpPut]
        [Route("api/Users/UpdateUserPasswordReset/{id}")]
        public async Task<IHttpActionResult> UpdateUserPasswordReset(int id, tUserPasswordReset passwordReset)
        {
            try
            {
                var tUserPwd = await db.tUserPasswordResets.Include(u=>u.tUser).Where(u => u.UserID == passwordReset.UserID && u.ResetCodeID == passwordReset.ResetCodeID
                && u.ExternalUserID == passwordReset.ExternalUserID).FirstOrDefaultAsync();

                if (tUserPwd != null)
                {
                    tUserPwd.UsedDateTime = DateTime.Now;
                   await db.SaveChangesAsync();
                    return Ok(tUserPwd.tUser);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }

        }




    }
}