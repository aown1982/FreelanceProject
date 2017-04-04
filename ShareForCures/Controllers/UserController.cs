using ShareForCures.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ShareForCures.Helpers;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Diagnostics;
using System.Threading;
using Facebook;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using LinqToTwitter;
using ShareForCures.Models.UserData;
using ShareForCures.Models.WebApp;
using System.IO;

namespace ShareForCures.Controllers
{
    public class UserController : Controller
    {

        public async Task<ActionResult> User()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //List<SourceServiceTypeViewModel> sourceType
            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            //var sourceType = await GetSourceTypeUnderAccount(userId);

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var resultSource = await client.GetAsync(Service.Get_SourceSeviceType + userId);
            if (resultSource.StatusCode != HttpStatusCode.OK) return null;
            var content = await resultSource.Content.ReadAsStringAsync();
            var sourceServiceTypes = JsonConvert.DeserializeObject<List<SourceServiceTypeViewModel>>(content);


            var result = await client.GetAsync(Service.Get_AdsByUserId + userId);
            if (result.StatusCode != HttpStatusCode.OK) return null;
            var list = JsonConvert.DeserializeObject<IEnumerable<tAdsSponsoredViewModel>>(await result.Content.ReadAsStringAsync()).ToList();


            /* Get Recent Activity & Relevant Information */
            var recentActivitiesLog = await GetRecentActivitiesByUserId(1);

            var m = new UserDataViewModel
            {
                AdsList = list,
                SourceServiceTypeData = sourceServiceTypes,
                RecentActivities = recentActivitiesLog
            };

            return View(m);
        }

        public ActionResult Settings()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            UserAuthLogin user = (UserAuthLogin)System.Web.HttpContext.Current.Session["User"];
            if (user != null)
            {
                ViewBag.Login = "yes";
                ViewBag.Message = "Welcome " + user.tUser.FirstName.ToString() + " " + user.tUser.LastName.ToString();
                System.Web.HttpContext.Current.Session["User"] = user;
            }

            //SetLogin();
            return View("User");
        }

        public ActionResult MyData()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult HealthGoals()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Trends()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult BloodGlucose()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("tdBloodGlucose");
        }

        public ActionResult TrendDetails()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Bloodpresure()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ActiveSubNav = "list";
            return View("tdBloodPressure");
        }

        public ActionResult BodyComposition()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ActiveSubNav = "list";
            return View("tdBodyComposition");
        }

        public ActionResult Weight()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ActiveSubNav = "list";
            return View("tdWeight");
        }

        public ActionResult Cholesterol()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ActiveSubNav = "list";
            return View("tdCholesterol");
        }

        public ActionResult Activity()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ActiveSubNav = "list";
            return View("tdActivity");
        }
        public ActionResult Diet()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ActiveSubNav = "list";
            return View("tdDiet");
        }

        public ActionResult BloodpresureAccounts()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ActiveSubNav = "accounts";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            model.IpAddress = GetUserIP();
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.LOGIN_USER, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var user = JsonConvert.DeserializeObject<UserAuthLogin>(await result.Content.ReadAsStringAsync());
                System.Web.HttpContext.Current.Session["User"] = user;
                ViewBag.Login = "yes";
                ViewBag.Message = "Welcome " + user.tUser.FirstName.ToString() + " " + user.tUser.LastName.ToString();

                if (model.RememberMe)
                {
                    HttpCookie cookie = new HttpCookie("Login");
                    cookie.Values.Add("Username", model.Username);
                    cookie.Expires = DateTime.Now.AddDays(15);
                    Response.Cookies.Add(cookie);

                }

                return Json(Url.Action("User", "User"));
            }
            if (result.StatusCode == HttpStatusCode.NotFound)//bad email
            {
                return Json("Invalid Email or Password. Please try again.", JsonRequestBehavior.AllowGet);
            }
            if (result.StatusCode == HttpStatusCode.Unauthorized)//bad password
            {
                return Json("Invalid Email or Password. Please try again.", JsonRequestBehavior.AllowGet);
            }
            if (result.StatusCode == HttpStatusCode.InternalServerError)
            {
                return Json("Unfortunately, something went wrong. We have logged this error. Please try again later.", JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        private string GetUserIP()
        {
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }

        public ActionResult ResetForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> VerifyForgotPassword(string resetCode, string externalId )
        {
             var forgotPasswordViewModel = new ForgotPasswordViewModal();
            if(!string.IsNullOrEmpty(resetCode) && !string.IsNullOrEmpty(externalId))
            {
                var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
                var passwordReset = new tUserPasswordResets
                {
                     ResetCodeID = new Guid(resetCode),
                     ExternalUserID = new Guid(externalId)
                };
                var validateJson = JsonConvert.SerializeObject(passwordReset);

                var result = await client.PostAsync(Service.GET_USER_PASSWORD_RESET, new StringContent(validateJson, Encoding.UTF8, "application/json"));
               
                if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    return Json("Reset Code has expired!", JsonRequestBehavior.AllowGet);
                }
                else if(result.StatusCode == HttpStatusCode.OK)
                {
                  var tUserPasswordResets = JsonConvert.DeserializeObject<tUserPasswordResets>(await result.Content.ReadAsStringAsync());
                    
                    return Json(tUserPasswordResets, JsonRequestBehavior.AllowGet);
                }
                else if(result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Json("Code has already been used!", JsonRequestBehavior.AllowGet);
                }
                else if(result.StatusCode == HttpStatusCode.NotFound)
                {
                    return Json("Invalid Reset Code entered!", JsonRequestBehavior.AllowGet);
                }
                
            }
            else
            {
                return Json("Invalid password details entered, please contact administrator!", JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        [HttpPost]
        public async Task<JsonResult> ForgotPasswordReset(tUserPasswordResets modal)
        {
            if (!ModelState.IsValid)
            {
                return Json("Please make sure all information you entered is correct.", JsonRequestBehavior.AllowGet);
            }

            if (modal.NewPassword != modal.ConfirmPassword)
            {
                return Json("Passwords do not match. Please try again.", JsonRequestBehavior.AllowGet);
            }

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var forgotPasswordViewModel = new ForgotPasswordViewModal();

            if (modal.ResetCodeID != Guid.Empty && modal.ExternalUserID != Guid.Empty )
            {
                var validateJson = JsonConvert.SerializeObject(modal);
                var _result = await client.PostAsync(Service.GET_USER_PASSWORD_RESET, new StringContent(validateJson, Encoding.UTF8, "application/json"));

                if (_result.StatusCode == HttpStatusCode.BadRequest)
                {
                    return Json("Reset Code has expired!", JsonRequestBehavior.AllowGet);
                }
                else if (_result.StatusCode == HttpStatusCode.OK)
                {
                    var tUser = new UsersModel();
                    tUser.PasswordHash = modal.NewPassword;
                    tUser.ComparePasswordHash = modal.ConfirmPassword;
                    tUser.ID = modal.UserID;

                    var json = JsonConvert.SerializeObject(tUser);
                    var result = await client.PutAsync(Service.UPDATE_USER + tUser.ID, new StringContent(json, Encoding.UTF8, "application/json"));
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var userPwdReset = JsonConvert.SerializeObject(modal);
                        result = await client.PutAsync(Service.UPDATE_USER_PASSWORD_RESET + tUser.ID, new StringContent(userPwdReset, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            var user = JsonConvert.DeserializeObject<UsersModel>(await result.Content.ReadAsStringAsync());
                            if(user !=null)
                            {
                                string body = "";
                                //Read template file from the App_Data folder
                                using (var sr = new StreamReader(Server.MapPath("\\App_Data\\") + "EmailTemplateSuccess.txt"))
                                {
                                    body = sr.ReadToEnd();
                                }
                                body = string.Format(body, user.LastName.Trim(), user.Email,user.Email);
                                var sent = new Email().SendEmail(user.Email, "Password reset for SharesForCures", body);

                                if (sent)
                                {
                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                            }
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                else if (_result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Json("Code has already been used!", JsonRequestBehavior.AllowGet);
                }
                else if (_result.StatusCode == HttpStatusCode.NotFound)
                {
                    return Json("Invalid Reset Code entered!", JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json("Invalid password details entered, please contact administrator!", JsonRequestBehavior.AllowGet);
            }

            return Json("FAILED", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendEmail(string name, string email, string message,string emailTo)
        {
            if(!ModelState.IsValid)
            {
                return Json("Please make sure all information you entered is correct.", JsonRequestBehavior.AllowGet);
            }

            string body = "";
            //Read template file from the App_Data folder
            using (var sr = new StreamReader(Server.MapPath("\\App_Data\\") + "LocalEmailTemplate.txt"))
            {
                body = sr.ReadToEnd();
            }
            body = string.Format(body, name, email, message);
            var Email = System.Configuration.ConfigurationManager.AppSettings[emailTo];
            var sent = new Email().SendEmail(Email, "Enquiry", body);

            if (sent)
            {
                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("FAILED", JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> ForgotPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                return Json("Please make sure all information you entered is correct.", JsonRequestBehavior.AllowGet);
            }
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var user = new UsersModel();
            user.Email = email;
            var validateJson = JsonConvert.SerializeObject(user);

            var result = await client.PostAsync(Service.GET_USER_PASSWORD_RESET_CODE, new StringContent(validateJson, Encoding.UTF8, "application/json"));

            if (result.StatusCode == HttpStatusCode.NotFound ||
                result.StatusCode == HttpStatusCode.BadRequest ||
                result.StatusCode == HttpStatusCode.InternalServerError)
            {
                return Json("Email you entered is invalid.", JsonRequestBehavior.AllowGet);
            }

            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                var users = JsonConvert.DeserializeObject<UsersModel>(await result.Content.ReadAsStringAsync());

                string body = "";
                //Read template file from the App_Data folder
                using (var sr = new StreamReader(Server.MapPath("\\App_Data\\") + "EmailTemplate.txt"))
                {
                    body = sr.ReadToEnd();
                }
               body = string.Format(body, users.LastName, users.Email, users.Email, users.tUserPasswordResets.First().ResetCodeID, users.tUserPasswordResets.First().ExternalUserID);

                var sent = new Email().SendEmail(user.Email, "Reset your ShareForCares password", body);
               
                if(sent)
                {
                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("FAIL", JsonRequestBehavior.AllowGet);
                }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            return null;
        }
        public async Task<JsonResult> RestPassword(ResetPasswordModal modal)
        {
            if (!ModelState.IsValid)
            {
                return Json("Please make sure all information you entered is correct.", JsonRequestBehavior.AllowGet);
            }

            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return Json("Please login to proceed.", JsonRequestBehavior.AllowGet);
            }

            var user = (UserAuthLogin)System.Web.HttpContext.Current.Session["User"];

            if (modal.NewPassword != modal.ConfirmPassword)
            {
                return Json("Passwords do not match. Please try again.", JsonRequestBehavior.AllowGet);
            }

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            //var validateUser = user.tUser;
            //validateUser.ComparePasswordHash = modal.CurrentPassword;

            UsersModel validateUser = new UsersModel { ID = user.tUser.ID, PasswordHash = modal.CurrentPassword };

            var validateJson = JsonConvert.SerializeObject(validateUser);
            var result = await client.PostAsync(Service.VALIDATE_PASSWORD, new StringContent(validateJson, Encoding.UTF8, "application/json"));
            if (result.StatusCode == HttpStatusCode.Unauthorized || result.StatusCode == HttpStatusCode.NotFound)
            {
                return Json("Current password you entered is wrong. Please try again.", JsonRequestBehavior.AllowGet);
            }
            else if (result.StatusCode == HttpStatusCode.InternalServerError)
            {
                return Json("Something went wrong. Please try again.", JsonRequestBehavior.AllowGet);
            }

            user.tUser.PasswordHash = modal.NewPassword;
            user.tUser.ComparePasswordHash = modal.ConfirmPassword;

            var json = JsonConvert.SerializeObject(user.tUser);
            result = await client.PutAsync(Service.UPDATE_USER + user.tUser.ID, new StringContent(json, Encoding.UTF8, "application/json"));
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
            }
            return Json("FAILED", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> UpdateUserInfo(UsersModel modal)
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return Json("Please login to proceed.", JsonRequestBehavior.AllowGet);
            }

            var user = (UserAuthLogin)System.Web.HttpContext.Current.Session["User"];
            modal.ID = user.tUser.ID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(modal);
            var result = await client.PostAsync(Service.GET_USER_BY_EMAIL, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return Json("Email address you entered is not available. Please try another.", JsonRequestBehavior.AllowGet);
            }
            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                user.tUser.FirstName = modal.FirstName;
                user.tUser.LastName = modal.LastName;
                user.tUser.Email = modal.Email;

                json = JsonConvert.SerializeObject(user.tUser);
                result = await client.PutAsync(Service.UPDATE_USER_BASIC_INFO + user.tUser.ID, new StringContent(json, Encoding.UTF8, "application/json"));
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    System.Web.HttpContext.Current.Session["User"] = user;

                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }

            }

            return Json("FAILED", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> UpdateUserDobAndGender(UsersModel modal)
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return Json("Please login to proceed.", JsonRequestBehavior.AllowGet);
            }

            UserAuthLogin userAuth = (UserAuthLogin)System.Web.HttpContext.Current.Session["User"];

            var user = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser;
            user.GenderID = modal.GenderID;
            user.DOB = modal.DOB;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(user);
            var result = await client.PostAsync(Service.UPDATE_USER_DOB_GENDER, new StringContent(json, Encoding.UTF8, "application/json"));
            if (result.StatusCode == HttpStatusCode.OK)
            {
                userAuth.tUser = user;
                System.Web.HttpContext.Current.Session["User"] = userAuth;
                return Json("SUCCESS", JsonRequestBehavior.AllowGet);

            }


            return Json("FAILED", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> UpdateUserLanguageAndLocation(UsersModel modal)
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return Json("Please login to proceed.", JsonRequestBehavior.AllowGet);
            }
            UserAuthLogin userAuth = (UserAuthLogin)System.Web.HttpContext.Current.Session["User"];

            var user = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser;
            user.LanguageID = modal.LanguageID;

            if (user.tUsersAddressHistories.Count > 0)
            {
                user.tUsersAddressHistories.FirstOrDefault().StateID = modal.LocationID;
            }
            else
            {
                user.tUsersAddressHistories.Add(new tUsersAddressHistory { UserID = user.ID, StateID = modal.LocationID });
            }

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(user);
            var result = await client.PostAsync(Service.UPDATE_USER_LANGUAGAE_LOCATION, new StringContent(json, Encoding.UTF8, "application/json"));
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return Json("SUCCESS", JsonRequestBehavior.AllowGet);

            }


            return Json("FAILED", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> DeactivateAccount()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return Json("Please login to proceed.", JsonRequestBehavior.AllowGet);
            }

            var user = (UserAuthLogin)System.Web.HttpContext.Current.Session["User"];

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            //Disable Account
            user.tUser.AccountStatusID = 2;

            var json = JsonConvert.SerializeObject(user.tUser);

            json = JsonConvert.SerializeObject(user.tUser);
            var result = await client.PutAsync(Service.UPDATE_USER_BASIC_INFO + user.tUser.ID, new StringContent(json, Encoding.UTF8, "application/json"));
            if (result.StatusCode == HttpStatusCode.OK)
            {


                result = await client.GetAsync(Service.GET_CREDENTIALS_BY_USER_ID + user.tUser.ID);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var credential = JsonConvert.DeserializeObject<Credential>(await result.Content.ReadAsStringAsync());
                    credential.SystemStatusID = 2;
                    json = JsonConvert.SerializeObject(credential);
                    result = await client.PutAsync(Service.UPDATE_CREDENTIALS, new StringContent(json, Encoding.UTF8, "application/json"));

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        System.Web.HttpContext.Current.Session["User"] = null;
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }

                }
                //If anything went wrong with setitng SystemStatusID for Credential Table, but user is also deactivated in user table so make it deactivated.
                System.Web.HttpContext.Current.Session["User"] = null;
                return Json("SUCCESS", JsonRequestBehavior.AllowGet);

            }

            return Json("FAILED", JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> RegisterUser(UsersModel model)
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
                var activationCode = await client.GetAsync(Service.GET_INVITATION_BY_CODE + model.InvitationCode);

                if (activationCode.StatusCode == HttpStatusCode.NotFound ||
                    activationCode.StatusCode == HttpStatusCode.BadRequest ||
                    activationCode.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return Json("Invitation Code you entered is invalid.", JsonRequestBehavior.AllowGet);
                }

                tInvitationCode code = JsonConvert.DeserializeObject<tInvitationCode>(await activationCode.Content.ReadAsStringAsync());
                if (code.UsedDateTime != null)
                {
                    return Json("Invitation Code you entered is already used.", JsonRequestBehavior.AllowGet);
                }

                var json = JsonConvert.SerializeObject(model);

                var result = await client.PostAsync(Service.GET_USER_BY_EMAIL, new StringContent(json, Encoding.UTF8, "application/json"));

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return Json("Email address you entered is not available. Please try another.", JsonRequestBehavior.AllowGet);
                }

                if (model.SocialUserId != null && model.SocialType != null)
                {
                    result = await client.GetAsync(Service.GET_CREDENTIALS_BY_SOURCE_USER_ID + model.SocialUserId.ToString());

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("This user is already registered. Please try another.", JsonRequestBehavior.AllowGet);
                    }
                }

                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    result = await client.PostAsync(Service.DBUSERS_REGISTER_USER, new StringContent(json, Encoding.UTF8, "application/json"));

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var user = JsonConvert.DeserializeObject<UsersModel>(await result.Content.ReadAsStringAsync());

                        if (model.SocialType != "USER ENTERED")
                        {
                            result = await client.GetAsync(Service.GET_SOCIAL_SOURCE_BY_NAME + model.SocialType.ToLower());

                            if (result.StatusCode == HttpStatusCode.OK)
                            {
                                var source = JsonConvert.DeserializeObject<Source>(await result.Content.ReadAsStringAsync());

                                Credential credential = new Credential
                                {
                                    SourceID = source.ID,
                                    AccessToken = model.AccessToken,
                                    PublicToken = model.PublicToken,
                                    SourceUserID = model.SocialUserId,
                                    UserID = user.ID,
                                    CreateDateTime = DateTime.Now,
                                    SystemStatusID = 1,
                                    AccessTokenExpiration = string.IsNullOrEmpty(model.ExpiresIn) ? null : (DateTime?)DateTime.Now.AddSeconds(Convert.ToInt32(model.ExpiresIn)),
                                    RefreshToken = model.RefreshToken,
                                    SourceUserIDToken = model.SourceUserIDToken,
                                    LastUpdatedDateTime = DateTime.Now

                                };
                                string jsonCredential = JsonConvert.SerializeObject(credential);

                                result = await client.PostAsync(Service.ADD_NEW_CREDENTIALS, new StringContent(jsonCredential, Encoding.UTF8, "application/json"));
                                if (result.StatusCode != HttpStatusCode.OK)
                                {
                                    throw new Exception("Unable to store credentials for social media source: " + jsonCredential);
                                }
                            }
                        }

                        code.UsedDateTime = DateTime.Now;
                        string jsonCode = JsonConvert.SerializeObject(code);
                        result = await client.PutAsync(Service.UPDATE_INVITATION_CODE + code.ID, new StringContent(jsonCode, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            LoginModel loginModel = new LoginModel
                            {
                                IpAddress = GetUserIP(),
                                Username = user.Email,
                                Password = model.PasswordHash
                            };
                            var jsonLogin = JsonConvert.SerializeObject(loginModel);

                            result = await client.PostAsync(Service.LOGIN_USER, new StringContent(jsonLogin, Encoding.UTF8, "application/json"));

                            if (result.StatusCode == HttpStatusCode.OK)
                            {
                                var login = JsonConvert.DeserializeObject<UserAuthLogin>(await result.Content.ReadAsStringAsync());
                                System.Web.HttpContext.Current.Session["User"] = login;
                                ViewBag.Login = "yes";
                                ViewBag.Message = "Welcome " + login.tUser.FirstName.ToString() + " " + login.tUser.LastName.ToString();

                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Insert Error Log
                var result = await InsertWebAppError(ex.HResult.ToString(), ex.Message, ex.StackTrace);

                return Json("System Error", JsonRequestBehavior.AllowGet);

            }

            return null;
        }

        public async Task<JsonResult> InsertWebAppError(string code, string description, string trace)
        {
            if (!ModelState.IsValid) return null;

            var waErrorLog = new WAErrorLog
            {
                ErrTypeID = 2,
                ErrSourceID = 4,
                Code = code,
                Description = description,
                Trace = trace,
                CreateDateTime = DateTime.Now
            };
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(waErrorLog);

            var result = await client.PostAsync(Service.ADD_WEBAPP_ERROR, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            return Json("Error Logged", JsonRequestBehavior.AllowGet);
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public async Task<ActionResult> FacebookSignup(UsersModel model)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            if (model.InvitationCode != null)
            {
                var json = JsonConvert.SerializeObject(model);
                var activationCode = await client.GetAsync(Service.GET_INVITATION_BY_CODE + model.InvitationCode);



                if (activationCode.StatusCode == HttpStatusCode.NotFound ||
                    activationCode.StatusCode == HttpStatusCode.BadRequest ||
                    activationCode.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return Json("Invitation Code you entered is invalid.", JsonRequestBehavior.AllowGet);
                }

                tInvitationCode code = JsonConvert.DeserializeObject<tInvitationCode>(await activationCode.Content.ReadAsStringAsync());
                if (code.UsedDateTime != null)
                {
                    return Json("Invitation Code you entered is already used.", JsonRequestBehavior.AllowGet);
                }

                System.Web.HttpContext.Current.Session["InvitationCode"] = model.InvitationCode;
            }

            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = System.Configuration.ConfigurationManager.AppSettings["FacebookClientId"].ToString(),
                client_secret = System.Configuration.ConfigurationManager.AppSettings["FacebookClientSecret"].ToString(),
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email" // Add other permissions as needed
            });

            return JavaScript("window.location = '" + loginUrl.AbsoluteUri + "'");
        }

        public async Task<ActionResult> FacebookCallback(string code)
        {

            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = System.Configuration.ConfigurationManager.AppSettings["FacebookClientId"].ToString(),
                client_secret = System.Configuration.ConfigurationManager.AppSettings["FacebookClientSecret"].ToString(),
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });


            // update the facebook client with the access token so
            // we can make requests on behalf of the user
            fb.AccessToken = result.access_token;


            dynamic facebookUser =
                fb.Get(string.Format("me?fields=email,birthday,gender,first_name,last_name,id,name,link"));
            string invitationCode = null;
            if (System.Web.HttpContext.Current.Session["InvitationCode"] != null)
            {
                invitationCode = System.Web.HttpContext.Current.Session["InvitationCode"].ToString();
            }


            UsersModel user = new UsersModel
            {
                FirstName = facebookUser.first_name,
                LastName = facebookUser.last_name,
                Email = facebookUser.email,
                SocialUserId = facebookUser.id,
                SocialType = Service.SOCIAL_TYPE_FACEBOOK,
                AccessToken = fb.AccessToken,
                InvitationCode = invitationCode
            };

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var credentialRequest = await client.GetAsync(Service.GET_CREDENTIALS_BY_SOURCE_USER_ID + user.SocialUserId);
            //If found in database
            if (credentialRequest.StatusCode == HttpStatusCode.OK)
            {
                var credential = JsonConvert.DeserializeObject<Credential>(await credentialRequest.Content.ReadAsStringAsync());

                var userRequest = await client.GetAsync(Service.GET_USER_BY_ID + credential.UserID);
                if (userRequest.StatusCode == HttpStatusCode.OK)
                {
                    var modal = JsonConvert.DeserializeObject<UsersModel>(await userRequest.Content.ReadAsStringAsync());
                    if (modal.AccountStatusID == 1)
                    {
                        LoginModel loginModel = new LoginModel
                        {
                            IpAddress = GetUserIP(),
                            Username = modal.Email,
                            Password = null
                        };

                        var jsonLogin = JsonConvert.SerializeObject(loginModel);

                        var loginUser = await client.PostAsync(Service.LOGIN_USER, new StringContent(jsonLogin, Encoding.UTF8, "application/json"));
                        if (loginUser.StatusCode == HttpStatusCode.OK)
                        {
                            var _user = JsonConvert.DeserializeObject<UserAuthLogin>(await loginUser.Content.ReadAsStringAsync());
                            System.Web.HttpContext.Current.Session["User"] = _user;
                            ViewBag.Login = "yes";
                            ViewBag.Message = "Welcome " + _user.tUser.FirstName.ToString() + " " + _user.tUser.LastName.ToString();
                            return RedirectToAction("Index", "User");
                        }
                        else if (loginUser.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            return PartialView("_UnAuthorize");
                        }
                    }
                }

                if (userRequest.StatusCode == HttpStatusCode.NotFound)
                {
                    return View(user);
                }

            }
            if (credentialRequest.StatusCode == HttpStatusCode.NotFound)
            {
                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> TwitterSignupBegin(UsersModel model)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            if (model.InvitationCode != null)
            {
                var json = JsonConvert.SerializeObject(model);
                var activationCode = await client.GetAsync(Service.GET_INVITATION_BY_CODE + model.InvitationCode);



                if (activationCode.StatusCode == HttpStatusCode.NotFound ||
                    activationCode.StatusCode == HttpStatusCode.BadRequest ||
                    activationCode.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return Json("Invitation Code you entered is invalid.", JsonRequestBehavior.AllowGet);
                }

                tInvitationCode code = JsonConvert.DeserializeObject<tInvitationCode>(await activationCode.Content.ReadAsStringAsync());
                if (code.UsedDateTime != null)
                {
                    return Json("Invitation Code you entered is already used.", JsonRequestBehavior.AllowGet);
                }

                System.Web.HttpContext.Current.Session["InvitationCode"] = model.InvitationCode;
            }

            var auth = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore
                {
                    ConsumerKey = System.Configuration.ConfigurationManager.AppSettings["TwitterConsumerKey"].ToString(),
                    ConsumerSecret = System.Configuration.ConfigurationManager.AppSettings["TwitterConsumerSecret"].ToString()
                }

            };
            if (Request.Url != null)
            {

                string twitterCallbackUrl = Request.Url.ToString().Replace("Begin", "Complete");
                var callback = await auth.BeginAuthorizationAsync(new Uri(twitterCallbackUrl));

                return JavaScript("window.location = '" + ((System.Web.Mvc.RedirectResult)callback).Url + "&oauth_callback=" + Url.Encode(twitterCallbackUrl) + "'");
            }

            return null;
        }

        public async Task<ActionResult> TwitterSignupComplete()
        {
            var auth = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore()
            };

            await auth.CompleteAuthorizeAsync(Request.Url);

            // This is how you access credentials after authorization.
            // The oauthToken and oauthTokenSecret do not expire.
            // You can use the userID to associate the credentials with the user.
            // You can save credentials any way you want - database, 
            //   isolated storage, etc. - it's up to you.
            // You can retrieve and load all 4 credentials on subsequent 
            //   queries to avoid the need to re-authorize.
            // When you've loaded all 4 credentials, LINQ to Twitter will let 
            //   you make queries without re-authorizing.

            var auth1 = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore()
            };

            var ctx = new TwitterContext(auth1);

            var verifyResponse =
              await
                  (from acct in ctx.Account
                   where (acct.Type == AccountType.VerifyCredentials) && (acct.IncludeEmail == true)
                   select acct)
                  .SingleOrDefaultAsync();

            UsersModel model = new UsersModel();

            if (verifyResponse != null && verifyResponse.User != null)
            {
                string invitationCode = null;
                User twitterUser = verifyResponse.User;
                if (System.Web.HttpContext.Current.Session["InvitationCode"] != null)
                {
                    invitationCode = System.Web.HttpContext.Current.Session["InvitationCode"].ToString();
                }

                model.FirstName = string.Empty;
                model.LastName = string.Empty;
                model.Email = twitterUser.Email ?? string.Empty;
                model.SocialUserId = twitterUser.UserIDResponse;
                model.SocialType = Service.SOCIAL_TYPE_TWITTER;
                model.AccessToken = auth.CredentialStore.OAuthTokenSecret;
                model.PublicToken = auth.CredentialStore.OAuthToken;
                model.InvitationCode = invitationCode;

            }
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var credentialRequest = await client.GetAsync(Service.GET_CREDENTIALS_BY_SOURCE_USER_ID + model.SocialUserId);
            //If found in database
            if (credentialRequest.StatusCode == HttpStatusCode.OK)
            {
                var credential = JsonConvert.DeserializeObject<Credential>(await credentialRequest.Content.ReadAsStringAsync());

                var userRequest = await client.GetAsync(Service.GET_USER_BY_ID + credential.UserID);
                if (userRequest.StatusCode == HttpStatusCode.OK)
                {
                    var modal = JsonConvert.DeserializeObject<UsersModel>(await userRequest.Content.ReadAsStringAsync());
                    if (modal.AccountStatusID == 1)
                    {
                        LoginModel loginModel = new LoginModel
                        {
                            IpAddress = GetUserIP(),
                            Username = modal.Email,
                            Password = null
                        };

                        var jsonLogin = JsonConvert.SerializeObject(loginModel);

                        var loginUser = await client.PostAsync(Service.LOGIN_USER, new StringContent(jsonLogin, Encoding.UTF8, "application/json"));
                        if (loginUser.StatusCode == HttpStatusCode.OK)
                        {
                            var user = JsonConvert.DeserializeObject<UserAuthLogin>(await loginUser.Content.ReadAsStringAsync());
                            System.Web.HttpContext.Current.Session["User"] = user;
                            ViewBag.Login = "yes";
                            ViewBag.Message = "Welcome " + user.tUser.FirstName.ToString() + " " + user.tUser.LastName.ToString();
                            return RedirectToAction("Index", "User");
                        }
                        else if (loginUser.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            return PartialView("_UnAuthorize");
                        }
                    }
                }

                if (userRequest.StatusCode == HttpStatusCode.NotFound)
                {
                    //return View(user);
                }

            }
            if (credentialRequest.StatusCode == HttpStatusCode.NotFound)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        #region HealthGoal

        public async Task<JsonResult> Last5GoalsWeight(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_LAST5WEIGHT_VITALS, new StringContent(json, Encoding.UTF8, "application/json"));
            var goalResult = await client.GetAsync(Service.GET_USER_WEIGHT_GOAL_VITAL + model.UserId);

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userVital = JsonConvert.DeserializeObject<IEnumerable<tUserVital>>(await result.Content.ReadAsStringAsync());
            var userGoalDetails = JsonConvert.DeserializeObject<tUserVital>(await goalResult.Content.ReadAsStringAsync());

            return Json(new
            {
                userVital,
                userGoalDetails
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> WeightChart(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_WEIGHT_VITALS, new StringContent(json, Encoding.UTF8, "application/json"));
            var goalResult = await client.GetAsync(Service.GET_USER_WEIGHT_GOAL_VITAL + model.UserId);

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userVital = JsonConvert.DeserializeObject<IEnumerable<tUserVital>>(await result.Content.ReadAsStringAsync());
            var userGoalDetails = JsonConvert.DeserializeObject<tUserVital>(await goalResult.Content.ReadAsStringAsync());

            return Json(new
            {
                userVital,
                userGoalDetails
            }, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> Last5GoalsExercise(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_LAST5EXERCISE_VITALS, new StringContent(json, Encoding.UTF8, "application/json"));
            var goalResult = await client.GetAsync(Service.GET_USER_EXERCISE_GOAL_VITAL + model.UserId);

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userActivity = JsonConvert.DeserializeObject<IEnumerable<tUserActivity>>(await result.Content.ReadAsStringAsync());
            var userGoalDetails = JsonConvert.DeserializeObject<tUserActivity>(await goalResult.Content.ReadAsStringAsync());

            return Json(new
            {
                userActivity,
                userGoalDetails
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> ExerciseChart(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_EXERCISE_VITALS, new StringContent(json, Encoding.UTF8, "application/json"));
            var goalResult = await client.GetAsync(Service.GET_USER_EXERCISE_GOAL_VITAL + model.UserId);

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userActivity = JsonConvert.DeserializeObject<IEnumerable<tUserActivity>>(await result.Content.ReadAsStringAsync());
            var userGoalDetails = JsonConvert.DeserializeObject<tUserActivity>(await goalResult.Content.ReadAsStringAsync());

            return Json(new
            {
                userActivity,
                userGoalDetails
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Last5GoalsDiet(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_LAST5DIET_VITALS, new StringContent(json, Encoding.UTF8, "application/json"));
            var goalResult = await client.GetAsync(Service.GET_USER_DIET_GOAL_VITAL + model.UserId);

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userDiet = JsonConvert.DeserializeObject<IEnumerable<tUserDiet>>(await result.Content.ReadAsStringAsync());
            var userGoalDetails = JsonConvert.DeserializeObject<tUserDiet>(await goalResult.Content.ReadAsStringAsync());

            return Json(new
            {
                userDiet,
                userGoalDetails
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> DietChart(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_DIET_VITALS, new StringContent(json, Encoding.UTF8, "application/json"));
            var goalResult = await client.GetAsync(Service.GET_USER_DIET_GOAL_VITAL + model.UserId);

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userDiet = JsonConvert.DeserializeObject<IEnumerable<tUserDiet>>(await result.Content.ReadAsStringAsync());
            var userGoalDetails = JsonConvert.DeserializeObject<tUserDiet>(await goalResult.Content.ReadAsStringAsync());

            return Json(new
            {
                userDiet,
                userGoalDetails
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Last5GoalsSleep(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;


            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_LAST5SLEEP_VITALS, new StringContent(json, Encoding.UTF8, "application/json"));
            var goalResult = await client.GetAsync(Service.GET_USER_SLEEP_GOAL_VITAL + model.UserId);

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userSleep = JsonConvert.DeserializeObject<IEnumerable<tUserSleep>>(await result.Content.ReadAsStringAsync());
            var userGoalDetails = JsonConvert.DeserializeObject<tUserSleep>(await goalResult.Content.ReadAsStringAsync());

            return Json(new
            {
                userSleep,
                userGoalDetails
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SleepChart(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;


            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_SLEEP_VITALS, new StringContent(json, Encoding.UTF8, "application/json"));
            var goalResult = await client.GetAsync(Service.GET_USER_SLEEP_GOAL_VITAL + model.UserId);

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userSleep = JsonConvert.DeserializeObject<IEnumerable<tUserSleep>>(await result.Content.ReadAsStringAsync());
            var userGoalDetails = JsonConvert.DeserializeObject<tUserSleep>(await goalResult.Content.ReadAsStringAsync());

            return Json(new
            {
                userSleep,
                userGoalDetails
            }, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> SetWeightGoal(string goal)
        {
            if (!ModelState.IsValid) return null;

            var tUserHealthGoal = new tUserHealthGoal
            {
                UserID = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID,
                Value = Convert.ToDecimal(goal)
            };

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(tUserHealthGoal);

            var result = await client.PostAsync(Service.SET_USER_WEIGHT_GOAL, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            return Json("Goal set", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SetDietGoal(string goal)
        {
            if (!ModelState.IsValid) return null;

            var tUserHealthGoal = new tUserHealthGoal
            {
                UserID = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID,
                Value = Convert.ToDecimal(goal)
            };

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(tUserHealthGoal);

            var result = await client.PostAsync(Service.SET_USER_DIET_GOAL, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            return Json("Goal set", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SetExerciseGoal(string goal)
        {
            if (!ModelState.IsValid) return null;

            var tUserHealthGoal = new tUserHealthGoal
            {
                UserID = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID,
                Value = Convert.ToDecimal(goal)
            };

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(tUserHealthGoal);

            var result = await client.PostAsync(Service.SET_USER_EXERCISE_GOAL, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            return Json("Goal set", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SetSleepGoal(string goal)
        {
            if (!ModelState.IsValid) return null;

            var tUserHealthGoal = new tUserHealthGoal
            {
                UserID = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID,
                Value = Convert.ToDecimal(goal)
            };

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(tUserHealthGoal);

            var result = await client.PostAsync(Service.SET_USER_SLEEP_GOAL, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            return Json("Goal set", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Trends

        public async Task<JsonResult> BloodGlucoseChart(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_BLOOD_GLUCOSE, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userBloodGlucose = JsonConvert.DeserializeObject<IEnumerable<tUserTestResult>>(await result.Content.ReadAsStringAsync()).ToList();
            var bloodGlucose = userBloodGlucose.LastOrDefault();

            double value = 0;
            if (bloodGlucose != null)
            {
                double.TryParse(bloodGlucose.Value, out value);
            }

            return Json(new
            {
                userBloodGlucose,
                ResultDateTime = bloodGlucose?.ResultDateTime,
                Value = value
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> BloodPressureChart(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_BLOOD_PRESSURE, new StringContent(json, Encoding.UTF8, "application/json"));
            var current = await client.PostAsync(Service.GET_USER_CURRENT_BLOOD_PRESSURE, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userBloodPressure = JsonConvert.DeserializeObject<IEnumerable<tUserVital>>(await result.Content.ReadAsStringAsync()).ToList();
            var details = JsonConvert.DeserializeObject<tUserVital>(await current.Content.ReadAsStringAsync());
            return Json(new
            {
                userBloodPressure,
                details.Diastolic,
                details.Systolic,
                details.HeartRate,
                details.ResultDateTime
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> BodyCompositionChart(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_BODY_COMPOSITION, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userBodyComposition = JsonConvert.DeserializeObject<IEnumerable<UserBodyComposition>>(await result.Content.ReadAsStringAsync()).ToList();

            var composition = userBodyComposition.LastOrDefault();
            DateTime? resultDateTime = null;
            if (composition != null) resultDateTime = composition.ResultDateTime;

            return Json(new
            {
                userBodyComposition,
                ResultDateTime = resultDateTime
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> CholestrolChart(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_CHOLESTEROL, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userTest = JsonConvert.DeserializeObject<IEnumerable<tUserTestResult>>(await result.Content.ReadAsStringAsync());
            return Json(new
            {
                userTest,
                userResult = userTest.LastOrDefault()

            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> StepsChart(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_EXERCISE_VITALS, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            var userActivity = JsonConvert.DeserializeObject<IEnumerable<tUserActivity>>(await result.Content.ReadAsStringAsync()).ToList();
            var steps = userActivity.Where(a => a.Steps > 0).OrderByDescending(s => s.StartDateTime).FirstOrDefault();
            DateTimeOffset? startDateTime = null;

            if (steps != null)
            {
                startDateTime = steps.StartDateTime;
            }
            return Json(new
            {
                userActivity,
                Steps = steps == null ? 0 : steps.Steps,
                StartDateTime = startDateTime
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> WeightTrendChart(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_WEIGHT_VITALS, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userVital = JsonConvert.DeserializeObject<IEnumerable<tUserVital>>(await result.Content.ReadAsStringAsync()).ToList();
            var weight = userVital.Where(a => a.Value > 0).OrderByDescending(s => s.ResultDateTime).FirstOrDefault();


            return Json(new
            {
                userVital,
                WeightLbs = weight?.WeightLbs ?? 0,
                ResultDateTime = weight?.ResultDateTime ?? null
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> DietTrendChart(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_DIET_VITALS, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;


            var userDiet = JsonConvert.DeserializeObject<IEnumerable<tUserDiet>>(await result.Content.ReadAsStringAsync()).ToList();
            var diet = userDiet.OrderByDescending(u => u.EnteredDateTime).FirstOrDefault();
            return Json(new
            {
                userDiet,
                Value = diet?.Value ?? 0,
                EnteredDateTime = diet?.EnteredDateTime ?? null
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region TrendDetails


        [HttpGet]
        public async Task<ActionResult> GetAllTestResultStatus()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_TEST_RESULT_STATUS);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var testResultStatus = JsonConvert.DeserializeObject<List<StatusViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(testResultStatus, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllDietCategory()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_DIET_CATEGORY);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var dietCategories = JsonConvert.DeserializeObject<List<DietCategory>>(await result.Content.ReadAsStringAsync());
                return Json(dietCategories, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllNutrient()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_NUTRIENT);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var nutrients = JsonConvert.DeserializeObject<List<Nutrient>>(await result.Content.ReadAsStringAsync());
                return Json(nutrients, JsonRequestBehavior.AllowGet);
            }

            return null;
        }


        [HttpGet]
        public async Task<ActionResult> GetAllUOM()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_UOM);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var testResultStatus = JsonConvert.DeserializeObject<List<UnitOfMeasures>>(await result.Content.ReadAsStringAsync());
                return Json(testResultStatus, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetUserSettingDetails()
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
                var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
                var result = await client.GetAsync(Service.GET_USER_SETTING_DETAILS + userId);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var settings = JsonConvert.DeserializeObject<SettingViewModel>(await result.Content.ReadAsStringAsync());
                    return Json(settings, JsonRequestBehavior.AllowGet);
                }

                return null;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllGenders()
        {
            var user = (UserAuthLogin)System.Web.HttpContext.Current.Session["User"];

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_GENDERS);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var genders = JsonConvert.DeserializeObject<List<Genders>>(await result.Content.ReadAsStringAsync());
                return Json(genders, JsonRequestBehavior.AllowGet);
            }

            return null;
        }


        [HttpGet]
        public async Task<ActionResult> GetAllLanguages()
        {
            var user = (UserAuthLogin)System.Web.HttpContext.Current.Session["User"];

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_LANGUAGES);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var languages = JsonConvert.DeserializeObject<List<Models.UserData.Languages>>(await result.Content.ReadAsStringAsync());
                return Json(languages, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllStates()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_STATES);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var states = JsonConvert.DeserializeObject<List<State>>(await result.Content.ReadAsStringAsync());
                return Json(states, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetUserTrendBloodGlucoseAccounts()
        {
            if (!ModelState.IsValid) return null;

            var id = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_USER_TREND_BLOOD_GLUCOSE_ACCOUNTS + id);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var organizations = JsonConvert.DeserializeObject<List<Organization>>(await result.Content.ReadAsStringAsync());
                return Json(organizations, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetUserTrendBloodPressureAccounts()
        {
            if (!ModelState.IsValid) return null;

            var id = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_USER_TREND_BLOOD_PRESSURE_ACCOUNTS + id);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var organizations = JsonConvert.DeserializeObject<List<Organization>>(await result.Content.ReadAsStringAsync());
                return Json(organizations, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetUserTrendBodyCompositionAccounts()
        {
            if (!ModelState.IsValid) return null;

            var id = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_USER_TREND_BODY_COMPOSITION_ACCOUNTS + id);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var organizations = JsonConvert.DeserializeObject<List<Organization>>(await result.Content.ReadAsStringAsync());
                return Json(organizations, JsonRequestBehavior.AllowGet);
            }

            return null;
        }


        [HttpGet]
        public async Task<ActionResult> GetUserTrendCholesterolAccounts()
        {
            if (!ModelState.IsValid) return null;

            var id = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_USER_TREND_CHOLESTEROL_ACCOUNTS + id);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var organizations = JsonConvert.DeserializeObject<List<Organization>>(await result.Content.ReadAsStringAsync());
                return Json(organizations, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetUserTrendDietAccounts()
        {
            if (!ModelState.IsValid) return null;

            var id = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_USER_TREND_DIET_ACCOUNTS + id);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var organizations = JsonConvert.DeserializeObject<List<Organization>>(await result.Content.ReadAsStringAsync());
                return Json(organizations, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetUserTrendWeightAccounts()
        {
            if (!ModelState.IsValid) return null;

            var id = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_USER_TREND_WEIGHT_ACCOUNTS + id);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var organizations = JsonConvert.DeserializeObject<List<Organization>>(await result.Content.ReadAsStringAsync());
                return Json(organizations, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetUserTrendActivityAccounts()
        {
            if (!ModelState.IsValid) return null;

            var id = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_USER_TREND_ACTIVITY_ACCOUNTS + id);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var organizations = JsonConvert.DeserializeObject<List<Organization>>(await result.Content.ReadAsStringAsync());
                return Json(organizations, JsonRequestBehavior.AllowGet);
            }

            return null;
        }


        [HttpPost]
        public async Task<JsonResult> GetUserDiet(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_TREND_DIET, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            var userDietViewModel = JsonConvert.DeserializeObject<List<UserDietViewModel>>(await result.Content.ReadAsStringAsync());
            return Json(userDietViewModel, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaveDiet(UserDietViewModel model)
        {
            int userId = (System.Web.HttpContext.Current.Session["User"] as UserAuthLogin).UserID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            TUserSourceServiceViewModel _TUserSourceService = null;
            UserDietViewModel _UserDietViewModel = null;
            HttpResponseMessage _result = null;
            if (model.ID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                _result = await client.GetAsync(Service.SOFTDELETE_USERDIET + model.ID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (model.NoteID > 0)
                    {
                        var _res = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + model.NoteID + "/" + InvalidatedSystemStatusID);
                    }
                }
            }
            // Get TUSer Source Service for the selected category
            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "BloodGlucose");
            if (_result.StatusCode == HttpStatusCode.OK)
            {
                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
            }


            if (_result.StatusCode == HttpStatusCode.OK)
            {
                var tUserDietViewModel = new UserDietViewModel();
                tUserDietViewModel.CreateDateTime = DateTime.Now;
                tUserDietViewModel.LastUpdatedDateTime = DateTime.Now;
                tUserDietViewModel.ObjectID = Guid.NewGuid();
                tUserDietViewModel.SystemStatusID = 1; // Valid Entry
                tUserDietViewModel.Name = model.Name;
                tUserDietViewModel.ServingUOMID = model.ServingUOMID;
                tUserDietViewModel.DietCategoryID = model.DietCategoryID;
                tUserDietViewModel.Servings = model.Servings;
                tUserDietViewModel.EnteredDateTime = model.EnteredDateTime;
                tUserDietViewModel.SourceObjectID = null;
                tUserDietViewModel.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                tUserDietViewModel.UserID = userId;
                tUserDietViewModel.ID = model.ID;

                var jsontUserDietViewModel = JsonConvert.SerializeObject(tUserDietViewModel);
                _result = await client.PostAsync(Service.ADD_NEW_USER_DIET, new StringContent(jsontUserDietViewModel, Encoding.UTF8, "application/json"));
                _UserDietViewModel = JsonConvert.DeserializeObject<UserDietViewModel>(await _result.Content.ReadAsStringAsync());
            }

            if (_result.StatusCode == HttpStatusCode.OK)
            {
                if (!string.IsNullOrEmpty(model.Note))
                {
                    model.fkObjectID = _UserDietViewModel.ObjectID;
                    model.UserID = userId;
                    model.SystemStatusID = 1;
                    model.CreateDateTime = DateTime.Now;
                    model.LastUpdateDateTime = DateTime.Now;
                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);

                    _result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        public async Task<ActionResult> DeleteDiet(int ID, int noteID)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };


            if (ID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                var _result = await client.GetAsync(Service.SOFTDELETE_USERDIET + ID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (noteID != 0)
                    {
                        _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + noteID + "/" + InvalidatedSystemStatusID);

                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetUserDietNutrient(int id)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var result = await client.GetAsync(Service.GET_USER_DIET_NUTRIENT + id);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var userdietNutrients = JsonConvert.DeserializeObject<List<UserDietNutrientViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(userdietNutrients, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        public async Task<ActionResult> SaveUserDietNutrient(UserDietNutrientViewModel model)
        {
            int userId = (System.Web.HttpContext.Current.Session["User"] as UserAuthLogin).UserID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            UserDietNutrientViewModel _UserDietNutrientViewModel = null;
            HttpResponseMessage _result = null;
            if (model.ID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                _result = await client.GetAsync(Service.SOFTDELETE_USERDIET_NUTRIENT + model.ID + "/" + InvalidatedSystemStatusID);

            }

            var tUserDietNutrientViewModel = new UserDietNutrientViewModel();
            tUserDietNutrientViewModel.NutrientID = model.NutrientID;
            tUserDietNutrientViewModel.UOMID = model.UOMID;
            tUserDietNutrientViewModel.UserDietID = model.UserDietID;
            tUserDietNutrientViewModel.SystemStatusID = 1; // Valid Entry
            tUserDietNutrientViewModel.Value = model.Value;

            var jsontUserDietViewModel = JsonConvert.SerializeObject(tUserDietNutrientViewModel);
            _result = await client.PostAsync(Service.ADD_NEW_USER_DIET_NUTRIENT, new StringContent(jsontUserDietViewModel, Encoding.UTF8, "application/json"));
            _UserDietNutrientViewModel = JsonConvert.DeserializeObject<UserDietNutrientViewModel>(await _result.Content.ReadAsStringAsync());

            if (_result.StatusCode == HttpStatusCode.OK)
            {
                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        public async Task<ActionResult> DeleteDietNutrient(int ID)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            if (ID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                var _result = await client.GetAsync(Service.SOFTDELETE_USERDIET_NUTRIENT + ID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }





        [HttpPost]
        public async Task<ActionResult> GetUserBloodGlucose(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_TREND_BLOOD_GLUCOSE, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            var userBloodGlucoseViewModel = JsonConvert.DeserializeObject<List<UserBloodGlucoseViewModel>>(await result.Content.ReadAsStringAsync());
            return Json(userBloodGlucoseViewModel, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaveBloodGlucose(UserBloodGlucoseViewModel model)
        {
            int userId = (System.Web.HttpContext.Current.Session["User"] as UserAuthLogin).UserID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            TUserSourceServiceViewModel _TUserSourceService = null;
            tUserTestResultViewModel _TUserTestResult = null;
            tUserTestResultComponentViewModel _TUserTestResultComponent = null;
            HttpResponseMessage _result = null;
            if (model.TestResultComponentID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                _result = await client.GetAsync(Service.SOFTDELETE_TUSERTESTRESULTCOMPONENT + model.TestResultComponentID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (model.Count <= 1) _result = await client.GetAsync(Service.SOFTDELETE_TUSERTESTRESULT + model.TestResultID + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (model.NoteID > 0)
                        {
                            var _res = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + model.NoteID + "/" + InvalidatedSystemStatusID);
                        }
                    }
                }
            }
            // Get TUSer Source Service for the selected category
            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "BloodGlucose");
            if (_result.StatusCode == HttpStatusCode.OK)
            {
                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
            }

            if (model.Count <= 1)
            {
                var tUserTestResult = new tUserTestResultViewModel();
                tUserTestResult.Comments = model.Comments;
                tUserTestResult.CreateDateTime = DateTime.Now;
                tUserTestResult.LastUpdatedDateTime = DateTime.Now;
                tUserTestResult.ObjectID = Guid.NewGuid();
                tUserTestResult.StatusID = model.StatusID == 0 ? 3 : model.StatusID;
                tUserTestResult.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                tUserTestResult.SystemStatusID = 1; // Valid Entry
                tUserTestResult.UserID = userId;
                tUserTestResult.Name = "Blood Glucose";
                tUserTestResult.ResultDateTime = model.ResultDateTime;
                tUserTestResult.SourceObjectID = null;
                tUserTestResult.SourceOrganizationID = model.SourceOrganizationID == 0 ? null : model.SourceOrganizationID;

                var jsontUserTestResult = JsonConvert.SerializeObject(tUserTestResult);
                _result = await client.PostAsync(Service.ADD_NEW_tUSERTESTRESULT, new StringContent(jsontUserTestResult, Encoding.UTF8, "application/json"));
                _TUserTestResult = JsonConvert.DeserializeObject<tUserTestResultViewModel>(await _result.Content.ReadAsStringAsync());
            }

            if (_result.StatusCode == HttpStatusCode.OK)
            {
                var tUserTestResultComponent = new tUserTestResultComponentViewModel();
                tUserTestResultComponent.Comments = model.Comments;
                tUserTestResultComponent.CreateDateTime = DateTime.Now;
                tUserTestResultComponent.LastUpdatedDateTime = DateTime.Now;
                tUserTestResultComponent.ObjectID = Guid.NewGuid();
                tUserTestResultComponent.SystemStatusID = 1; // Valid Entry
                tUserTestResultComponent.Name = "Blood Glucose";
                tUserTestResultComponent.TestResultID = _TUserTestResult.ID;
                tUserTestResultComponent.UOMID = model.UOMID;
                tUserTestResultComponent.Value = model.Value;

                var jsontUserTestResultComponent = JsonConvert.SerializeObject(tUserTestResultComponent);
                _result = await client.PostAsync(Service.ADD_NEW_tUSERTESTRESULTCOMPONENT, new StringContent(jsontUserTestResultComponent, Encoding.UTF8, "application/json"));
                _TUserTestResultComponent = JsonConvert.DeserializeObject<tUserTestResultComponentViewModel>(await _result.Content.ReadAsStringAsync());
            }

            if (_result.StatusCode == HttpStatusCode.OK)
            {
                var tUserTestResultComponentCode = new tUserTestResultComponentCodeViewModel();
                tUserTestResultComponentCode.CodeID = 604;
                tUserTestResultComponentCode.UserTestResultComponentID = _TUserTestResultComponent.ID;

                var jsontUserTestResultComponentCode = JsonConvert.SerializeObject(tUserTestResultComponentCode);
                _result = await client.PostAsync(Service.ADD_NEW_tUSERTESTRESULTCOMPONENTCODE, new StringContent(jsontUserTestResultComponentCode, Encoding.UTF8, "application/json"));
            }

            if (_result.StatusCode == HttpStatusCode.OK)
            {

                if (!string.IsNullOrEmpty(model.Note))
                {

                    model.fkObjectID = _TUserTestResultComponent.ObjectID;
                    model.UserID = userId;
                    model.SystemStatusID = 1;
                    model.CreateDateTime = DateTime.Now;
                    model.LastUpdateDateTime = DateTime.Now;
                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);

                    _result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        public async Task<ActionResult> DeleteBloodGlucose(int testResultComponentID, int testResultID, int count, int noteID)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };


            if (testResultComponentID > 0)
            {

                int InvalidatedSystemStatusID = 2;// User Invalidated
                var _result = await client.GetAsync(Service.SOFTDELETE_TUSERTESTRESULTCOMPONENT + testResultComponentID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (count <= 1) _result = await client.GetAsync(Service.SOFTDELETE_TUSERTESTRESULT + testResultID + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (noteID != 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + noteID + "/" + InvalidatedSystemStatusID);

                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return null;
        }

        [HttpPost]
        public async Task<JsonResult> GetUserBloodPressure(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_TREND_BLOOD_PRESSURE, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            var userVitalViewModel = JsonConvert.DeserializeObject<List<UserVitalViewModel>>(await result.Content.ReadAsStringAsync());
            return Json(userVitalViewModel, JsonRequestBehavior.AllowGet);
        }



        public async Task<ActionResult> SaveBloodPressure(UserVitalViewModel model)
        {
            int userId = (System.Web.HttpContext.Current.Session["User"] as UserAuthLogin).UserID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            TUserSourceServiceViewModel _TUserSourceService = null;
            UserVitalViewModel _UserVitalViewModel = null;
            HttpResponseMessage _result = null;
            if (model.ID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                _result = await client.GetAsync(Service.SOFTDELETE_TUSERVITALS + model.ID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (model.NoteID > 0)
                        {
                            var _res = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + model.NoteID + "/" + InvalidatedSystemStatusID);
                        }
                    }
                }
            }
            // Get TUSer Source Service for the selected category
            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "BloodGlucose");
            if (_result.StatusCode == HttpStatusCode.OK)
            {
                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
            }


            if (_result.StatusCode == HttpStatusCode.OK)
            {
                var tUserVitalViewModel = new UserVitalViewModel();
                tUserVitalViewModel.CreateDateTime = DateTime.Now;
                tUserVitalViewModel.LastUpdatedDateTime = DateTime.Now;
                tUserVitalViewModel.ObjectID = Guid.NewGuid();
                tUserVitalViewModel.SystemStatusID = 1; // Valid Entry
                tUserVitalViewModel.Name = model.Name;
                tUserVitalViewModel.UOMID = model.UOMID;
                tUserVitalViewModel.Value = model.Value;
                tUserVitalViewModel.ResultDateTime = model.ResultDateTime;
                tUserVitalViewModel.SourceObjectID = null;
                tUserVitalViewModel.SourceOrganizationID = model.SourceOrganizationID == 0 ? null : model.SourceOrganizationID;
                tUserVitalViewModel.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                tUserVitalViewModel.UserID = userId;

                var jsontUserVitalViewModel = JsonConvert.SerializeObject(tUserVitalViewModel);
                _result = await client.PostAsync(Service.ADD_NEW_BLOOD_PRESSURE_USERVITAL, new StringContent(jsontUserVitalViewModel, Encoding.UTF8, "application/json"));
                _UserVitalViewModel = JsonConvert.DeserializeObject<UserVitalViewModel>(await _result.Content.ReadAsStringAsync());
            }

            if (_result.StatusCode == HttpStatusCode.OK)
            {

                if (!string.IsNullOrEmpty(model.Note))
                {


                    model.fkObjectID = _UserVitalViewModel.ObjectID;
                    model.UserID = userId;
                    model.SystemStatusID = 1;
                    model.CreateDateTime = DateTime.Now;
                    model.LastUpdateDateTime = DateTime.Now;
                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);

                    _result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        public async Task<ActionResult> DeleteBloodPressure(int ID, int noteID)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };


            if (ID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                var _result = await client.GetAsync(Service.SOFTDELETE_TUSERVITALS + ID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (noteID != 0)
                    {
                        _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + noteID + "/" + InvalidatedSystemStatusID);

                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }



        [HttpPost]
        public async Task<JsonResult> GetUserBodyComposition(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_TREND_BODY_COMPOSITION, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            var userVitalViewModel = JsonConvert.DeserializeObject<List<UserVitalViewModel>>(await result.Content.ReadAsStringAsync());
            return Json(userVitalViewModel, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> SaveBodyComposition(UserVitalViewModel model)
        {
            int userId = (System.Web.HttpContext.Current.Session["User"] as UserAuthLogin).UserID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            TUserSourceServiceViewModel _TUserSourceService = null;
            UserVitalViewModel _UserVitalViewModel = null;
            HttpResponseMessage _result = null;
            if (model.ID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                _result = await client.GetAsync(Service.SOFTDELETE_TUSERVITALS + model.ID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (model.NoteID > 0)
                        {
                            var _res = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + model.NoteID + "/" + InvalidatedSystemStatusID);
                        }
                    }
                }
            }
            // Get TUSer Source Service for the selected category
            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "BloodGlucose");
            if (_result.StatusCode == HttpStatusCode.OK)
            {
                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
            }


            if (_result.StatusCode == HttpStatusCode.OK)
            {
                var tUserVitalViewModel = new UserVitalViewModel();
                tUserVitalViewModel.CreateDateTime = DateTime.Now;
                tUserVitalViewModel.LastUpdatedDateTime = DateTime.Now;
                tUserVitalViewModel.ObjectID = Guid.NewGuid();
                tUserVitalViewModel.SystemStatusID = 1; // Valid Entry
                tUserVitalViewModel.Name = model.Name;
                tUserVitalViewModel.UOMID = model.UOMID;
                tUserVitalViewModel.Value = model.Value;
                tUserVitalViewModel.ResultDateTime = model.ResultDateTime;
                tUserVitalViewModel.SourceObjectID = null;
                tUserVitalViewModel.SourceOrganizationID = model.SourceOrganizationID == 0 ? null : model.SourceOrganizationID;
                tUserVitalViewModel.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                tUserVitalViewModel.UserID = userId;

                var jsontUserVitalViewModel = JsonConvert.SerializeObject(tUserVitalViewModel);
                _result = await client.PostAsync(Service.ADD_NEW_BODY_COMPOSITION_USERVITAL, new StringContent(jsontUserVitalViewModel, Encoding.UTF8, "application/json"));
                _UserVitalViewModel = JsonConvert.DeserializeObject<UserVitalViewModel>(await _result.Content.ReadAsStringAsync());
            }

            if (_result.StatusCode == HttpStatusCode.OK)
            {

                if (!string.IsNullOrEmpty(model.Note))
                {


                    model.fkObjectID = _UserVitalViewModel.ObjectID;
                    model.UserID = userId;
                    model.SystemStatusID = 1;
                    model.CreateDateTime = DateTime.Now;
                    model.LastUpdateDateTime = DateTime.Now;
                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);

                    _result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        public async Task<ActionResult> DeleteBodyComposition(int ID, int noteID)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };


            if (ID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                var _result = await client.GetAsync(Service.SOFTDELETE_TUSERVITALS + ID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (noteID != 0)
                    {
                        _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + noteID + "/" + InvalidatedSystemStatusID);

                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }




        [HttpPost]
        public async Task<JsonResult> GetUserWeight(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_TREND_WEIGHT, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            var userVitalViewModel = JsonConvert.DeserializeObject<List<UserVitalViewModel>>(await result.Content.ReadAsStringAsync());
            return Json(userVitalViewModel, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> SaveWeight(UserVitalViewModel model)
        {
            int userId = (System.Web.HttpContext.Current.Session["User"] as UserAuthLogin).UserID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            TUserSourceServiceViewModel _TUserSourceService = null;
            UserVitalViewModel _UserVitalViewModel = null;
            HttpResponseMessage _result = null;
            if (model.ID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                _result = await client.GetAsync(Service.SOFTDELETE_TUSERVITALS + model.ID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (model.NoteID > 0)
                        {
                            var _res = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + model.NoteID + "/" + InvalidatedSystemStatusID);
                        }
                    }
                }
            }
            // Get TUSer Source Service for the selected category
            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "BloodGlucose");
            if (_result.StatusCode == HttpStatusCode.OK)
            {
                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
            }


            if (_result.StatusCode == HttpStatusCode.OK)
            {
                var tUserVitalViewModel = new UserVitalViewModel();
                tUserVitalViewModel.CreateDateTime = DateTime.Now;
                tUserVitalViewModel.LastUpdatedDateTime = DateTime.Now;
                tUserVitalViewModel.ObjectID = Guid.NewGuid();
                tUserVitalViewModel.SystemStatusID = 1; // Valid Entry
                tUserVitalViewModel.Name = model.Name;
                tUserVitalViewModel.UOMID = model.UOMID;
                tUserVitalViewModel.Value = model.Value;
                tUserVitalViewModel.ResultDateTime = model.ResultDateTime;
                tUserVitalViewModel.SourceObjectID = null;
                tUserVitalViewModel.SourceOrganizationID = model.SourceOrganizationID == 0 ? null : model.SourceOrganizationID;
                tUserVitalViewModel.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                tUserVitalViewModel.UserID = userId;

                var jsontUserVitalViewModel = JsonConvert.SerializeObject(tUserVitalViewModel);
                _result = await client.PostAsync(Service.ADD_NEW_WEIGHT_USERVITAL, new StringContent(jsontUserVitalViewModel, Encoding.UTF8, "application/json"));
                _UserVitalViewModel = JsonConvert.DeserializeObject<UserVitalViewModel>(await _result.Content.ReadAsStringAsync());
            }

            if (_result.StatusCode == HttpStatusCode.OK)
            {

                if (!string.IsNullOrEmpty(model.Note))
                {


                    model.fkObjectID = _UserVitalViewModel.ObjectID;
                    model.UserID = userId;
                    model.SystemStatusID = 1;
                    model.CreateDateTime = DateTime.Now;
                    model.LastUpdateDateTime = DateTime.Now;
                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);

                    _result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        public async Task<ActionResult> DeleteWeight(int ID, int noteID)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };


            if (ID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                var _result = await client.GetAsync(Service.SOFTDELETE_TUSERVITALS + ID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (noteID != 0)
                    {
                        _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + noteID + "/" + InvalidatedSystemStatusID);

                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<ActionResult> GetUserCholesterol(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_TREND_CHOLESTEROL, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            var userCholesterolViewModel = JsonConvert.DeserializeObject<List<UserCholesterolViewModel>>(await result.Content.ReadAsStringAsync());
            return Json(userCholesterolViewModel, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaveCholesterol(UserCholesterolViewModel model)
        {
            int userId = (System.Web.HttpContext.Current.Session["User"] as UserAuthLogin).UserID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            TUserSourceServiceViewModel _TUserSourceService = null;
            tUserTestResultViewModel _TUserTestResult = null;
            tUserTestResultComponentViewModel _TUserTestResultComponent = null;
            HttpResponseMessage _result = null;
            if (model.TestResultComponentID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                _result = await client.GetAsync(Service.SOFTDELETE_TUSERTESTRESULTCOMPONENT + model.TestResultComponentID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (model.Count <= 1) _result = await client.GetAsync(Service.SOFTDELETE_TUSERTESTRESULT + model.TestResultID + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (model.NoteID > 0)
                        {
                            var _res = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + model.NoteID + "/" + InvalidatedSystemStatusID);
                        }
                    }
                }
            }
            // Get TUSer Source Service for the selected category
            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "BloodGlucose");
            if (_result.StatusCode == HttpStatusCode.OK)
            {
                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
            }

            if (model.Count <= 1)
            {
                var tUserTestResult = new tUserTestResultViewModel();
                tUserTestResult.CreateDateTime = DateTime.Now;
                tUserTestResult.LastUpdatedDateTime = DateTime.Now;
                tUserTestResult.ObjectID = Guid.NewGuid();
                tUserTestResult.StatusID = model.StatusID == 0 ? 3 : model.StatusID;
                tUserTestResult.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                tUserTestResult.SystemStatusID = 1; // Valid Entry
                tUserTestResult.UserID = userId;
                tUserTestResult.Name = model.Name;
                tUserTestResult.ResultDateTime = model.ResultDateTime;
                tUserTestResult.SourceObjectID = null;
                tUserTestResult.SourceOrganizationID = model.SourceOrganizationID == 0 ? null : model.SourceOrganizationID;

                var jsontUserTestResult = JsonConvert.SerializeObject(tUserTestResult);
                _result = await client.PostAsync(Service.ADD_NEW_tUSERTESTRESULT, new StringContent(jsontUserTestResult, Encoding.UTF8, "application/json"));
                _TUserTestResult = JsonConvert.DeserializeObject<tUserTestResultViewModel>(await _result.Content.ReadAsStringAsync());
            }

            if (_result.StatusCode == HttpStatusCode.OK)
            {
                var tUserTestResultComponent = new tUserTestResultComponentViewModel();
                tUserTestResultComponent.CreateDateTime = DateTime.Now;
                tUserTestResultComponent.LastUpdatedDateTime = DateTime.Now;
                tUserTestResultComponent.ObjectID = Guid.NewGuid();
                tUserTestResultComponent.SystemStatusID = 1; // Valid Entry
                tUserTestResultComponent.Name = model.Name;
                tUserTestResultComponent.TestResultID = _TUserTestResult.ID;
                tUserTestResultComponent.UOMID = model.UOMID;
                tUserTestResultComponent.Value = model.Value;
                tUserTestResultComponent.HighValue = model.HighValue;
                tUserTestResultComponent.LowValue = model.LowValue;
                tUserTestResultComponent.RefRange = model.RefRange;

                var jsontUserTestResultComponent = JsonConvert.SerializeObject(tUserTestResultComponent);
                _result = await client.PostAsync(Service.ADD_NEW_tUSERTESTRESULTCOMPONENT, new StringContent(jsontUserTestResultComponent, Encoding.UTF8, "application/json"));
                _TUserTestResultComponent = JsonConvert.DeserializeObject<tUserTestResultComponentViewModel>(await _result.Content.ReadAsStringAsync());
            }

            if (_result.StatusCode == HttpStatusCode.OK)
            {
                var tUserTestResultComponentCode = new tUserTestResultComponentCodeViewModel();
                tUserTestResultComponentCode.CodeID = model.CodeID;
                tUserTestResultComponentCode.UserTestResultComponentID = _TUserTestResultComponent.ID;

                var jsontUserTestResultComponentCode = JsonConvert.SerializeObject(tUserTestResultComponentCode);
                _result = await client.PostAsync(Service.ADD_NEW_tUSERTESTRESULTCOMPONENTCODE, new StringContent(jsontUserTestResultComponentCode, Encoding.UTF8, "application/json"));
            }

            if (_result.StatusCode == HttpStatusCode.OK)
            {

                if (!string.IsNullOrEmpty(model.Note))
                {

                    model.fkObjectID = _TUserTestResultComponent.ObjectID;
                    model.UserID = userId;
                    model.SystemStatusID = 1;
                    model.CreateDateTime = DateTime.Now;
                    model.LastUpdateDateTime = DateTime.Now;
                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);

                    _result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        public async Task<ActionResult> DeleteCholesterol(int testResultComponentID, int testResultID, int count, int noteID)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            if (testResultComponentID > 0)
            {

                int InvalidatedSystemStatusID = 2;// User Invalidated
                var _result = await client.GetAsync(Service.SOFTDELETE_TUSERTESTRESULTCOMPONENT + testResultComponentID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (count <= 1) _result = await client.GetAsync(Service.SOFTDELETE_TUSERTESTRESULT + testResultID + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (noteID != 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + noteID + "/" + InvalidatedSystemStatusID);

                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return null;
        }



        [HttpGet]
        public async Task<ActionResult> GetAllActivities()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_ACTIVITIES);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var activities = JsonConvert.DeserializeObject<List<Activity>>(await result.Content.ReadAsStringAsync());
                return Json(activities, JsonRequestBehavior.AllowGet);
            }

            return null;
        }


        [HttpPost]
        public async Task<JsonResult> GetUserActivity(HealthGoalModel model)
        {
            if (!ModelState.IsValid) return null;

            model.UserId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.GET_USER_TREND_ACTIVITY, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            var userActivityViewModel = JsonConvert.DeserializeObject<List<UserAcitivityViewModel>>(await result.Content.ReadAsStringAsync());
            return Json(userActivityViewModel, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaveActivity(UserAcitivityViewModel model)
        {
            int userId = (System.Web.HttpContext.Current.Session["User"] as UserAuthLogin).UserID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            TUserSourceServiceViewModel _TUserSourceService = null;
            UserAcitivityViewModel _UserAcitivityViewModel = null;
            HttpResponseMessage _result = null;
            if (model.ID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                _result = await client.GetAsync(Service.SOFTDELETE_USERACTIVITY + model.ID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (model.NoteID > 0)
                        {
                            var _res = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + model.NoteID + "/" + InvalidatedSystemStatusID);
                        }
                    }
                }
            }
            // Get TUSer Source Service for the selected category
            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "BloodGlucose");
            if (_result.StatusCode == HttpStatusCode.OK)
            {
                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
            }


            if (_result.StatusCode == HttpStatusCode.OK)
            {
                var tUserActivityViewModel = new UserAcitivityViewModel();
                tUserActivityViewModel.CreateDateTime = DateTime.Now;
                tUserActivityViewModel.LastUpdatedDateTime = DateTime.Now;
                tUserActivityViewModel.ObjectID = Guid.NewGuid();
                tUserActivityViewModel.SystemStatusID = 1; // Valid Entry
                tUserActivityViewModel.ActivityID = model.ActivityID;
                tUserActivityViewModel.DistanceUOMID = model.DistanceUOMID;
                tUserActivityViewModel.Distance = model.Distance;
                tUserActivityViewModel.DurationUOMID = model.DurationUOMID;
                tUserActivityViewModel.Duration = model.Duration;
                tUserActivityViewModel.StartDateTime = model.StartDateTime;
                tUserActivityViewModel.EndDateTime = model.EndDateTime;
                tUserActivityViewModel.Steps = model.Steps;
                tUserActivityViewModel.Calories = model.Calories;
                tUserActivityViewModel.LightActivityMin = model.LightActivityMin;
                tUserActivityViewModel.ModerateActivityMin = model.ModerateActivityMin;
                tUserActivityViewModel.VigorousActivityMin = model.VigorousActivityMin;
                tUserActivityViewModel.SedentaryActivityMin = model.SedentaryActivityMin;
                tUserActivityViewModel.SourceObjectID = null;
                tUserActivityViewModel.SourceOrganizationID = model.SourceOrganizationID == 0 ? null : model.SourceOrganizationID;
                tUserActivityViewModel.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                tUserActivityViewModel.UserID = userId;

                var jsontUserActivityViewModel = JsonConvert.SerializeObject(tUserActivityViewModel);
                _result = await client.PostAsync(Service.ADD_NEW_USER_ACTIVITY, new StringContent(jsontUserActivityViewModel, Encoding.UTF8, "application/json"));
                _UserAcitivityViewModel = JsonConvert.DeserializeObject<UserAcitivityViewModel>(await _result.Content.ReadAsStringAsync());
            }

            if (_result.StatusCode == HttpStatusCode.OK)
            {

                if (!string.IsNullOrEmpty(model.Note))
                {


                    model.fkObjectID = _UserAcitivityViewModel.ObjectID;
                    model.UserID = userId;
                    model.SystemStatusID = 1;
                    model.CreateDateTime = DateTime.Now;
                    model.LastUpdateDateTime = DateTime.Now;
                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);

                    _result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        public async Task<ActionResult> DeleteActivity(int ID, int noteID)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };


            if (ID > 0)
            {
                int InvalidatedSystemStatusID = 2;// User Invalidated
                var _result = await client.GetAsync(Service.SOFTDELETE_USERACTIVITY + ID + "/" + InvalidatedSystemStatusID);
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    if (noteID != 0)
                    {
                        _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + noteID + "/" + InvalidatedSystemStatusID);

                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }



        #endregion


        #region User MyData View
        [HttpGet]
        public async Task<ActionResult> GetUserDataWithNotes(string categoryName)
        {
            int UserID = (System.Web.HttpContext.Current.Session["User"] as UserAuthLogin).UserID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_UserDataByCategoryWithNotes + UserID + "/" + categoryName);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var userdataWithNotes = JsonConvert.DeserializeObject<List<UserDataNotesViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(userdataWithNotes, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetSingleUserDataWithNote(int itemId, string categoryName)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var result = await client.GetAsync(Service.GET_UserDataWithNotes + itemId + "/" + categoryName);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var userdataNotes = JsonConvert.DeserializeObject<UserDataNotesViewModel>(await result.Content.ReadAsStringAsync());
                return Json(userdataNotes, JsonRequestBehavior.AllowGet);
            }

            return null;
        }


        [HttpGet]
        public async Task<ActionResult> GetAllSourceOrganizations()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_SourceOrganizations);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var retval = JsonConvert.DeserializeObject<List<tSourceOrganizationViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(retval, JsonRequestBehavior.AllowGet);
            }

            return null;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllProcedureDevices()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_ProcedureDevices);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var retval = JsonConvert.DeserializeObject<List<tUserProcedureDeviceViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(retval, JsonRequestBehavior.AllowGet);
            }

            return null;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllSpecimens()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_Specimens);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var retval = JsonConvert.DeserializeObject<List<tUserSpecimenViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(retval, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPharmacies()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_Pharmacies);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var retval = JsonConvert.DeserializeObject<List<tPharmacyViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(retval, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllMedicationRoutes()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_MedicationRoutes);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var retval = JsonConvert.DeserializeObject<List<tMedicationRouteViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(retval, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllMedicationForms()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_MedicationForms);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var retval = JsonConvert.DeserializeObject<List<tMedicationFormViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(retval, JsonRequestBehavior.AllowGet);
            }

            return null;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllUnitsOfMeasures()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_UnitsOfMeasures);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var retval = JsonConvert.DeserializeObject<List<tUnitsOfMeasureViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(retval, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllInstructionTypes()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_InstructionTypes);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var retval = JsonConvert.DeserializeObject<List<InstructionTypesViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(retval, JsonRequestBehavior.AllowGet);
            }

            return null;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllProviders()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_Providers);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var retval = JsonConvert.DeserializeObject<List<ProviderViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(retval, JsonRequestBehavior.AllowGet);
            }

            return null;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllVisitTypes()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_VisitTypes);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var retval = JsonConvert.DeserializeObject<List<VisitTypeViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(retval, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCarePlanTypes()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_CarePlanTypes);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var retval = JsonConvert.DeserializeObject<List<CarePlanTypeViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(retval, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCarePlanSpecialties()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_CarePlanSpecialties);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var retval = JsonConvert.DeserializeObject<List<CarePlanSpecialtyViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(retval, JsonRequestBehavior.AllowGet);
            }

            return null;
        }


        [HttpGet]
        public async Task<ActionResult> GetAllAllergens()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_Allergens);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var allergens = JsonConvert.DeserializeObject<List<AllergenViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(allergens, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllReactions()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_Reactions);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var reactions = JsonConvert.DeserializeObject<List<ReactionViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(reactions, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAllergyStatus()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_AllergyStatus);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var allergyStatus = JsonConvert.DeserializeObject<List<AllergyStatusViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(allergyStatus, JsonRequestBehavior.AllowGet);
            }

            return null;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAllergyServerties()
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_AllergySeverity);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var allergyServerties = JsonConvert.DeserializeObject<List<AllergySeverityViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(allergyServerties, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        public async Task<ActionResult> SaveUserDataWithNote(UserDataNotesViewModel model)
        {
            int userId = (System.Web.HttpContext.Current.Session["User"] as UserAuthLogin).UserID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            switch (model.Category)
            {
                case "Allergies":
                    #region User Allergy
                    #region Edit Mode just invalidate the old row and add new 
                    if (model.ParentId > 0)
                    {
                        int InvalidatedSystemStatusID = 2;// User Invalidated
                        var _result = await client.GetAsync(Service.SoftDelete_tUserAllergies + model.ParentId + "/" + InvalidatedSystemStatusID);
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                            _result = await client.DeleteAsync(Service.DELETE_tUserDerivedNote + model.ID);

                            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Allergies");
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                            }

                            tUserAllergyViewModel tUserAllergy = new tUserAllergyViewModel();
                            tUserAllergy.AllergenID = model.AllergenID;
                            tUserAllergy.CreateDateTime = DateTime.Now;
                            tUserAllergy.EndDateTime = model.EndDateTime;
                            tUserAllergy.StartDateTime = model.StartDateTime;
                            tUserAllergy.LastUpdateDateTime = DateTime.Now;
                            tUserAllergy.ReactionID = model.ReactionID.Value;
                            tUserAllergy.SeverityID = model.SeverityID.Value;
                            tUserAllergy.StatusID = model.StatusID.Value;
                            tUserAllergy.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                            tUserAllergy.SystemStatusID = 1; // Valid Entry
                            tUserAllergy.UserID = userId;

                            var jsontUserAllergy = JsonConvert.SerializeObject(tUserAllergy);
                            var result = await client.PostAsync(Service.ADD_NEW_tUserAllergies, new StringContent(jsontUserAllergy, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {

                                if (string.IsNullOrEmpty(model.Note))
                                {

                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    var r_tUserAllergy = JsonConvert.DeserializeObject<tUserAllergyViewModel>(await result.Content.ReadAsStringAsync());
                                    #region Derived Notes
                                    model.fkObjectID = r_tUserAllergy.ObjectID;
                                    model.UserID = userId;
                                    model.SystemStatusID = 1;
                                    model.CreateDateTime = DateTime.Now;
                                    model.LastUpdateDateTime = DateTime.Now;
                                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                    #endregion
                                    result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                    if (result.StatusCode == HttpStatusCode.OK)
                                    {
                                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    #region Add New  
                    else
                    {
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        var _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Allergies");
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                        }
                        tUserAllergyViewModel tUserAllergy = new tUserAllergyViewModel();
                        tUserAllergy.AllergenID = model.AllergenID;
                        tUserAllergy.CreateDateTime = DateTime.Now;
                        tUserAllergy.EndDateTime = model.EndDateTime;
                        tUserAllergy.StartDateTime = model.StartDateTime;
                        tUserAllergy.LastUpdateDateTime = DateTime.Now;
                        tUserAllergy.ReactionID = model.ReactionID.Value;
                        tUserAllergy.SeverityID = model.SeverityID.Value;
                        tUserAllergy.StatusID = model.StatusID.Value;
                        tUserAllergy.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                        tUserAllergy.SystemStatusID = 1; // Valid Entry
                        tUserAllergy.UserID = userId;


                        var jsontUserAllergy = JsonConvert.SerializeObject(tUserAllergy);
                        var result = await client.PostAsync(Service.ADD_NEW_tUserAllergies, new StringContent(jsontUserAllergy, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {

                            if (string.IsNullOrEmpty(model.Note))
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var r_tUserAllergy = JsonConvert.DeserializeObject<tUserAllergyViewModel>(await result.Content.ReadAsStringAsync());
                                #region Derived Notes
                                model.fkObjectID = r_tUserAllergy.ObjectID;
                                model.UserID = userId;
                                model.SystemStatusID = 1;
                                model.CreateDateTime = DateTime.Now;
                                model.LastUpdateDateTime = DateTime.Now;
                                var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                #endregion
                                result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                if (result.StatusCode == HttpStatusCode.OK)
                                {
                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    break;
                case "Care Plans":
                    #region User Care Plan
                    #region Edit Mode just invalidate the old row and add new 
                    if (model.ParentId > 0)
                    {
                        int InvalidatedSystemStatusID = 2;// User Invalidated
                        var _result = await client.GetAsync(Service.SoftDelete_tUserCarePlan + model.ParentId + "/" + InvalidatedSystemStatusID);
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                            _result = await client.DeleteAsync(Service.DELETE_tUserDerivedNote + model.ID);

                            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Care Plans");
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                            }

                            tUserCarePlanViewModel tUserCarePlan = new tUserCarePlanViewModel();
                            tUserCarePlan.TypeID = model.CarePlanTypeID;
                            tUserCarePlan.SpecialtyID = model.CarePlanSpecialtyID;
                            tUserCarePlan.CreateDateTime = DateTime.Now;
                            tUserCarePlan.EndDateTime = model.EndDateTime;
                            tUserCarePlan.StartDateTime = model.StartDateTime;
                            tUserCarePlan.LastUpdatedDateTime = DateTime.Now;
                            tUserCarePlan.Text = model.CarePlanText;
                            tUserCarePlan.Name = model.CarePlanName;
                            tUserCarePlan.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                            tUserCarePlan.SystemStatusID = 1; // Valid Entry
                            tUserCarePlan.UserID = userId;

                            var jsontUserCarePlan = JsonConvert.SerializeObject(tUserCarePlan);
                            var result = await client.PostAsync(Service.ADD_NEW_tUserCarePlans, new StringContent(jsontUserCarePlan, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {

                                if (string.IsNullOrEmpty(model.Note))
                                {

                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    var r_tUserCarePlan = JsonConvert.DeserializeObject<tUserCarePlanViewModel>(await result.Content.ReadAsStringAsync());
                                    #region Derived Notes
                                    model.fkObjectID = r_tUserCarePlan.ObjectID;
                                    model.UserID = userId;
                                    model.SystemStatusID = 1;
                                    model.CreateDateTime = DateTime.Now;
                                    model.LastUpdateDateTime = DateTime.Now;
                                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                    #endregion
                                    result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                    if (result.StatusCode == HttpStatusCode.OK)
                                    {
                                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    #region Add New  
                    else
                    {
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        var _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Care Plans");
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                        }
                        tUserCarePlanViewModel tUserCarePlan = new tUserCarePlanViewModel();
                        tUserCarePlan.TypeID = model.CarePlanTypeID;
                        tUserCarePlan.SpecialtyID = model.CarePlanSpecialtyID;
                        tUserCarePlan.CreateDateTime = DateTime.Now;
                        tUserCarePlan.EndDateTime = model.EndDateTime;
                        tUserCarePlan.StartDateTime = model.StartDateTime;
                        tUserCarePlan.LastUpdatedDateTime = DateTime.Now;
                        tUserCarePlan.Text = model.CarePlanText;
                        tUserCarePlan.Name = model.CarePlanName;
                        tUserCarePlan.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                        tUserCarePlan.SystemStatusID = 1; // Valid Entry
                        tUserCarePlan.UserID = userId;

                        var jsontUserCarePlan = JsonConvert.SerializeObject(tUserCarePlan);
                        var result = await client.PostAsync(Service.ADD_NEW_tUserCarePlans, new StringContent(jsontUserCarePlan, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {

                            if (string.IsNullOrEmpty(model.Note))
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var r_tUserCarePlan = JsonConvert.DeserializeObject<tUserCarePlanViewModel>(await result.Content.ReadAsStringAsync());
                                #region Derived Notes
                                model.fkObjectID = r_tUserCarePlan.ObjectID;
                                model.UserID = userId;
                                model.SystemStatusID = 1;
                                model.CreateDateTime = DateTime.Now;
                                model.LastUpdateDateTime = DateTime.Now;
                                var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                #endregion
                                result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                if (result.StatusCode == HttpStatusCode.OK)
                                {
                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    break;
                case "Encounters":
                    #region Encounters
                    #region Edit Mode just invalidate the old row and add new 
                    if (model.ParentId > 0)
                    {
                        int InvalidatedSystemStatusID = 2;// User Invalidated
                        int? _FollowUpID = null;
                        int? _PatientID = null;
                        var _result = await client.GetAsync(Service.SoftDelete_tUserEncounter + model.ParentId + "/" + InvalidatedSystemStatusID);
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                            _result = await client.DeleteAsync(Service.DELETE_tUserDerivedNote + model.ID);

                            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Encounters");
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                            }

                            #region User Instruction
                            model.FollowUpInstruction.UserID = userId;
                            model.FollowUpInstruction.SystemStatusID = 1;
                            model.FollowUpInstruction.CreateDateTime = DateTime.Now;
                            var jsontUserFollowUpInstruction = JsonConvert.SerializeObject(model.FollowUpInstruction);
                            var result = await client.PostAsync(Service.ADD_NEW_tUserInstruction, new StringContent(jsontUserFollowUpInstruction, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {
                                var r_tUserFollowUpIns = JsonConvert.DeserializeObject<tUserInstructionViewModel>(await result.Content.ReadAsStringAsync());
                                _FollowUpID = r_tUserFollowUpIns.ID;
                            }
                            model.PatientInstruction.UserID = userId;
                            model.PatientInstruction.SystemStatusID = 1;
                            model.PatientInstruction.CreateDateTime = DateTime.Now;
                            var jsontUserPatientInstruction = JsonConvert.SerializeObject(model.PatientInstruction);
                            result = await client.PostAsync(Service.ADD_NEW_tUserInstruction, new StringContent(jsontUserPatientInstruction, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {
                                var r_tUserPatientIns = JsonConvert.DeserializeObject<tUserInstructionViewModel>(await result.Content.ReadAsStringAsync());
                                _PatientID = r_tUserPatientIns.ID;
                            }
                            #endregion
                            tUserEncounterViewModel tUserEncounter = new tUserEncounterViewModel();
                            tUserEncounter.ProviderID = model.ProviderID;
                            tUserEncounter.VisitTypeID = model.VisitTypeID;
                            tUserEncounter.EncounterDateTime = model.EncounterDateTime;
                            tUserEncounter.LastUpdateDateTime = DateTime.Now;
                            tUserEncounter.CreateDateTime = DateTime.Now;
                            tUserEncounter.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                            tUserEncounter.SystemStatusID = 1; // Valid Entry
                            tUserEncounter.UserID = userId;
                            tUserEncounter.FollowUpInstructionID = _FollowUpID;
                            tUserEncounter.PatientInstructionID = _PatientID;

                            var jsontUserCarePlan = JsonConvert.SerializeObject(tUserEncounter);
                            result = await client.PostAsync(Service.ADD_NEW_tUserEncounters, new StringContent(jsontUserCarePlan, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {

                                if (string.IsNullOrEmpty(model.Note))
                                {

                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    var r_tUserEncounter = JsonConvert.DeserializeObject<tUserEncounterViewModel>(await result.Content.ReadAsStringAsync());
                                    #region Derived Notes
                                    model.fkObjectID = r_tUserEncounter.ObjectID;
                                    model.UserID = userId;
                                    model.SystemStatusID = 1;
                                    model.CreateDateTime = DateTime.Now;
                                    model.LastUpdateDateTime = DateTime.Now;
                                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                    #endregion
                                    result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                    if (result.StatusCode == HttpStatusCode.OK)
                                    {
                                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    #region Add New  
                    else
                    {
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        int? _FollowUpID = null;
                        int? _PatientID = null;
                        var _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Encounters");
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                        }

                        #region User Instruction
                        model.FollowUpInstruction.UserID = userId;
                        model.FollowUpInstruction.SystemStatusID = 1;
                        model.FollowUpInstruction.CreateDateTime = DateTime.Now;
                        var jsontUserFollowUpInstruction = JsonConvert.SerializeObject(model.FollowUpInstruction);
                        var result = await client.PostAsync(Service.ADD_NEW_tUserInstruction, new StringContent(jsontUserFollowUpInstruction, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            var r_tUserFollowUpIns = JsonConvert.DeserializeObject<tUserInstructionViewModel>(await result.Content.ReadAsStringAsync());
                            _FollowUpID = r_tUserFollowUpIns.ID;
                        }
                        model.PatientInstruction.UserID = userId;
                        model.PatientInstruction.SystemStatusID = 1;
                        model.PatientInstruction.CreateDateTime = DateTime.Now;
                        var jsontUserPatientInstruction = JsonConvert.SerializeObject(model.PatientInstruction);
                        result = await client.PostAsync(Service.ADD_NEW_tUserInstruction, new StringContent(jsontUserPatientInstruction, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            var r_tUserPatientIns = JsonConvert.DeserializeObject<tUserInstructionViewModel>(await result.Content.ReadAsStringAsync());
                            _PatientID = r_tUserPatientIns.ID;
                        }
                        #endregion
                        tUserEncounterViewModel tUserEncounter = new tUserEncounterViewModel();
                        tUserEncounter.ProviderID = model.ProviderID;
                        tUserEncounter.VisitTypeID = model.VisitTypeID;
                        tUserEncounter.EncounterDateTime = model.EncounterDateTime;
                        tUserEncounter.LastUpdateDateTime = DateTime.Now;
                        tUserEncounter.CreateDateTime = DateTime.Now;
                        tUserEncounter.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                        tUserEncounter.SystemStatusID = 1; // Valid Entry
                        tUserEncounter.UserID = userId;
                        tUserEncounter.FollowUpInstructionID = _FollowUpID;
                        tUserEncounter.PatientInstructionID = _PatientID;

                        var jsontUserEncounter = JsonConvert.SerializeObject(tUserEncounter);
                        result = await client.PostAsync(Service.ADD_NEW_tUserEncounters, new StringContent(jsontUserEncounter, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {

                            if (string.IsNullOrEmpty(model.Note))
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var r_tUserEncounter = JsonConvert.DeserializeObject<tUserEncounterViewModel>(await result.Content.ReadAsStringAsync());
                                #region Derived Notes
                                model.fkObjectID = r_tUserEncounter.ObjectID;
                                model.UserID = userId;
                                model.SystemStatusID = 1;
                                model.CreateDateTime = DateTime.Now;
                                model.LastUpdateDateTime = DateTime.Now;
                                var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                #endregion
                                result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                if (result.StatusCode == HttpStatusCode.OK)
                                {
                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    break;
                case "Functional Statuses":
                    #region User Functional Statuses
                    #region Edit Mode just invalidate the old row and add new 
                    if (model.ParentId > 0)
                    {
                        int InvalidatedSystemStatusID = 2;// User Invalidated
                        var _result = await client.GetAsync(Service.SoftDelete_tUserFunctionalStatuses + model.ParentId + "/" + InvalidatedSystemStatusID);
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                            _result = await client.DeleteAsync(Service.DELETE_tUserDerivedNote + model.ID);

                            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Functional Statuses");
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                            }

                            tUserFunctionalStatusViewModel tUserFunctionalStatus = new tUserFunctionalStatusViewModel();
                            tUserFunctionalStatus.CreateDateTime = DateTime.Now;
                            tUserFunctionalStatus.LastUpdatedDateTime = DateTime.Now;
                            tUserFunctionalStatus.EndDateTime = model.EndDateTime;
                            tUserFunctionalStatus.StartDateTime = model.StartDateTime;
                            tUserFunctionalStatus.Name = model.Name;
                            tUserFunctionalStatus.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                            tUserFunctionalStatus.SystemStatusID = 1; // Valid Entry
                            tUserFunctionalStatus.UserID = userId;

                            var jsontUserFunctionalStatus = JsonConvert.SerializeObject(tUserFunctionalStatus);
                            var result = await client.PostAsync(Service.ADD_NEW_tUserFunctionalStatuses, new StringContent(jsontUserFunctionalStatus, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {

                                if (string.IsNullOrEmpty(model.Note))
                                {

                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    var r_tFunctionalStatus = JsonConvert.DeserializeObject<tUserFunctionalStatusViewModel>(await result.Content.ReadAsStringAsync());
                                    #region Derived Notes
                                    model.fkObjectID = r_tFunctionalStatus.ObjectID;
                                    model.UserID = userId;
                                    model.SystemStatusID = 1;
                                    model.CreateDateTime = DateTime.Now;
                                    model.LastUpdateDateTime = DateTime.Now;
                                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                    #endregion
                                    result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                    if (result.StatusCode == HttpStatusCode.OK)
                                    {
                                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    #region Add New  
                    else
                    {
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        var _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Functional Statuses");
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                        }
                        tUserFunctionalStatusViewModel tUserFunctionalStatus = new tUserFunctionalStatusViewModel();
                        tUserFunctionalStatus.CreateDateTime = DateTime.Now;
                        tUserFunctionalStatus.LastUpdatedDateTime = DateTime.Now;
                        tUserFunctionalStatus.EndDateTime = model.EndDateTime;
                        tUserFunctionalStatus.StartDateTime = model.StartDateTime;
                        tUserFunctionalStatus.Name = model.Name;
                        tUserFunctionalStatus.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                        tUserFunctionalStatus.SystemStatusID = 1; // Valid Entry
                        tUserFunctionalStatus.UserID = userId;

                        var jsontUserFunctionalStatus = JsonConvert.SerializeObject(tUserFunctionalStatus);
                        var result = await client.PostAsync(Service.ADD_NEW_tUserFunctionalStatuses, new StringContent(jsontUserFunctionalStatus, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {

                            if (string.IsNullOrEmpty(model.Note))
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var r_tFunctionalStatus = JsonConvert.DeserializeObject<tUserFunctionalStatusViewModel>(await result.Content.ReadAsStringAsync());
                                #region Derived Notes
                                model.fkObjectID = r_tFunctionalStatus.ObjectID;
                                model.UserID = userId;
                                model.SystemStatusID = 1;
                                model.CreateDateTime = DateTime.Now;
                                model.LastUpdateDateTime = DateTime.Now;
                                var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                #endregion
                                result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                if (result.StatusCode == HttpStatusCode.OK)
                                {
                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    break;
                case "Immunizations":
                    #region User Immunizations
                    #region Edit Mode just invalidate the old row and add new 
                    if (model.ParentId > 0)
                    {
                        int InvalidatedSystemStatusID = 2;// User Invalidated
                        var _result = await client.GetAsync(Service.SoftDelete_tUserImmunization + model.ParentId + "/" + InvalidatedSystemStatusID);
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                            _result = await client.DeleteAsync(Service.DELETE_tUserDerivedNote + model.ID);

                            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Immunizations");
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                            }

                            tUserImmunizationViewModel tUserImmunization = new tUserImmunizationViewModel();
                            tUserImmunization.CreateDateTime = DateTime.Now;
                            tUserImmunization.Name = model.Name;
                            tUserImmunization.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                            tUserImmunization.SystemStatusID = 1; // Valid Entry
                            tUserImmunization.UserID = userId;

                            var jsontUserImmunization = JsonConvert.SerializeObject(tUserImmunization);
                            var result = await client.PostAsync(Service.ADD_NEW_tUserImmunization, new StringContent(jsontUserImmunization, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {

                                if (string.IsNullOrEmpty(model.Note))
                                {

                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    var r_tImmunization = JsonConvert.DeserializeObject<tUserImmunizationViewModel>(await result.Content.ReadAsStringAsync());
                                    #region Derived Notes
                                    model.fkObjectID = r_tImmunization.ObjectID;
                                    model.UserID = userId;
                                    model.SystemStatusID = 1;
                                    model.CreateDateTime = DateTime.Now;
                                    model.LastUpdateDateTime = DateTime.Now;
                                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                    #endregion
                                    result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                    if (result.StatusCode == HttpStatusCode.OK)
                                    {
                                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    #region Add New  
                    else
                    {
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        var _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Immunizations");
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                        }
                        tUserImmunizationViewModel tUserImmunization = new tUserImmunizationViewModel();
                        tUserImmunization.CreateDateTime = DateTime.Now;
                        tUserImmunization.Name = model.Name;
                        tUserImmunization.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                        tUserImmunization.SystemStatusID = 1; // Valid Entry
                        tUserImmunization.UserID = userId;

                        var jsontUserImmunization = JsonConvert.SerializeObject(tUserImmunization);
                        var result = await client.PostAsync(Service.ADD_NEW_tUserImmunization, new StringContent(jsontUserImmunization, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {

                            if (string.IsNullOrEmpty(model.Note))
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var r_tImmunization = JsonConvert.DeserializeObject<tUserImmunizationViewModel>(await result.Content.ReadAsStringAsync());
                                #region Derived Notes
                                model.fkObjectID = r_tImmunization.ObjectID;
                                model.UserID = userId;
                                model.SystemStatusID = 1;
                                model.CreateDateTime = DateTime.Now;
                                model.LastUpdateDateTime = DateTime.Now;
                                var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                #endregion
                                result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                if (result.StatusCode == HttpStatusCode.OK)
                                {
                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    break;
                case "Instructions":
                    #region User Instructions
                    #region Edit Mode just invalidate the old row and add new 
                    if (model.ParentId > 0)
                    {
                        int InvalidatedSystemStatusID = 2;// User Invalidated
                        var _result = await client.GetAsync(Service.SoftDelete_tUserInstruction + model.ParentId + "/" + InvalidatedSystemStatusID);
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                            _result = await client.DeleteAsync(Service.DELETE_tUserDerivedNote + model.ID);

                            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Instructions");
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                            }

                            tUserInstructionViewModel tUserInstruction = new tUserInstructionViewModel();
                            tUserInstruction.CreateDateTime = DateTime.Now;
                            tUserInstruction.Name = model.Name;
                            tUserInstruction.Text = model.Text;
                            tUserInstruction.StartDateTime = model.StartDateTime;
                            tUserInstruction.EndDateTime = model.EndDateTime;
                            tUserInstruction.InstructionTypeID = model.InstructionTypeID;
                            tUserInstruction.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                            tUserInstruction.SystemStatusID = 1; // Valid Entry
                            tUserInstruction.UserID = userId;

                            var jsontUserInstruction = JsonConvert.SerializeObject(tUserInstruction);
                            var result = await client.PostAsync(Service.ADD_NEW_tUserInstruction, new StringContent(jsontUserInstruction, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {

                                if (string.IsNullOrEmpty(model.Note))
                                {

                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    var r_tInstruction = JsonConvert.DeserializeObject<tUserInstructionViewModel>(await result.Content.ReadAsStringAsync());
                                    #region Derived Notes
                                    model.fkObjectID = r_tInstruction.ObjectID;
                                    model.UserID = userId;
                                    model.SystemStatusID = 1;
                                    model.CreateDateTime = DateTime.Now;
                                    model.LastUpdateDateTime = DateTime.Now;
                                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                    #endregion
                                    result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                    if (result.StatusCode == HttpStatusCode.OK)
                                    {
                                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    #region Add New  
                    else
                    {
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        var _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Instructions");
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                        }
                        tUserInstructionViewModel tUserInstruction = new tUserInstructionViewModel();
                        tUserInstruction.CreateDateTime = DateTime.Now;
                        tUserInstruction.Name = model.Name;
                        tUserInstruction.Text = model.Text;
                        tUserInstruction.StartDateTime = model.StartDateTime;
                        tUserInstruction.EndDateTime = model.EndDateTime;
                        tUserInstruction.InstructionTypeID = model.InstructionTypeID;
                        tUserInstruction.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                        tUserInstruction.SystemStatusID = 1; // Valid Entry
                        tUserInstruction.UserID = userId;

                        var jsontUserInstruction = JsonConvert.SerializeObject(tUserInstruction);
                        var result = await client.PostAsync(Service.ADD_NEW_tUserInstruction, new StringContent(jsontUserInstruction, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {

                            if (string.IsNullOrEmpty(model.Note))
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var r_tInstruction = JsonConvert.DeserializeObject<tUserInstructionViewModel>(await result.Content.ReadAsStringAsync());
                                #region Derived Notes
                                model.fkObjectID = r_tInstruction.ObjectID;
                                model.UserID = userId;
                                model.SystemStatusID = 1;
                                model.CreateDateTime = DateTime.Now;
                                model.LastUpdateDateTime = DateTime.Now;
                                var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                #endregion
                                result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                if (result.StatusCode == HttpStatusCode.OK)
                                {
                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    break;
                case "Narratives":
                    #region User Narratives
                    #region Edit Mode just invalidate the old row and add new 
                    if (model.ParentId > 0)
                    {
                        int InvalidatedSystemStatusID = 2;// User Invalidated
                        var _result = await client.GetAsync(Service.SoftDelete_tUserNarrative + model.ParentId + "/" + InvalidatedSystemStatusID);
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                            _result = await client.DeleteAsync(Service.DELETE_tUserDerivedNote + model.ID);

                            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Narratives");
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                            }

                            tUserNarrativeViewModel tUserNarrative = new tUserNarrativeViewModel();
                            tUserNarrative.CreateDateTime = DateTime.Now;
                            tUserNarrative.ProviderID = model.ProviderID;
                            tUserNarrative.StartDateTime = model.StartDateTime;
                            tUserNarrative.EndDateTime = model.EndDateTime;
                            tUserNarrative.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                            tUserNarrative.SystemStatusID = 1; // Valid Entry
                            tUserNarrative.UserID = userId;

                            var jsontUserNarrative = JsonConvert.SerializeObject(tUserNarrative);
                            var result = await client.PostAsync(Service.ADD_NEW_tUserNarrative, new StringContent(jsontUserNarrative, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {

                                if (string.IsNullOrEmpty(model.Note))
                                {

                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    var r_tNarrative = JsonConvert.DeserializeObject<tUserNarrativeViewModel>(await result.Content.ReadAsStringAsync());
                                    #region Derived Notes
                                    model.fkObjectID = r_tNarrative.ObjectID;
                                    model.UserID = userId;
                                    model.SystemStatusID = 1;
                                    model.CreateDateTime = DateTime.Now;
                                    model.LastUpdateDateTime = DateTime.Now;
                                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                    #endregion
                                    result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                    if (result.StatusCode == HttpStatusCode.OK)
                                    {
                                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    #region Add New  
                    else
                    {
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        var _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Narratives");
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                        }
                        tUserNarrativeViewModel tUserNarrative = new tUserNarrativeViewModel();
                        tUserNarrative.CreateDateTime = DateTime.Now;
                        tUserNarrative.ProviderID = model.ProviderID;
                        tUserNarrative.StartDateTime = model.StartDateTime;
                        tUserNarrative.EndDateTime = model.EndDateTime;
                        tUserNarrative.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                        tUserNarrative.SystemStatusID = 1; // Valid Entry
                        tUserNarrative.UserID = userId;

                        var jsontUserNarrative = JsonConvert.SerializeObject(tUserNarrative);
                        var result = await client.PostAsync(Service.ADD_NEW_tUserNarrative, new StringContent(jsontUserNarrative, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {

                            if (string.IsNullOrEmpty(model.Note))
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var r_tNarrative = JsonConvert.DeserializeObject<tUserNarrativeViewModel>(await result.Content.ReadAsStringAsync());
                                #region Derived Notes
                                model.fkObjectID = r_tNarrative.ObjectID;
                                model.UserID = userId;
                                model.SystemStatusID = 1;
                                model.CreateDateTime = DateTime.Now;
                                model.LastUpdateDateTime = DateTime.Now;
                                var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                #endregion
                                result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                if (result.StatusCode == HttpStatusCode.OK)
                                {
                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    break;
                case "Prescriptions":
                    #region User Prescriptions
                    #region Edit Mode just invalidate the old row and add new 
                    if (model.ParentId > 0)
                    {
                        int InvalidatedSystemStatusID = 2;// User Invalidated
                        var _result = await client.GetAsync(Service.SoftDelete_tUserPrescription + model.ParentId + "/" + InvalidatedSystemStatusID);
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                            _result = await client.DeleteAsync(Service.DELETE_tUserDerivedNote + model.ID);

                            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Prescriptions");
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                            }

                            tUserPrescriptionViewModel tUserPrescription = new tUserPrescriptionViewModel();
                            tUserPrescription.CreateDateTime = DateTime.Now;
                            tUserPrescription.ProviderID = model.ProviderID;
                            tUserPrescription.MedFormID = model.MedFormID;
                            tUserPrescription.FrequencyUOMID = model.FrequencyUOMID;
                            tUserPrescription.StrengthUOMID = model.StrengthUOMID;
                            tUserPrescription.RouteID = model.RouteID;
                            tUserPrescription.PharmacyID = model.PharmacyID;
                            tUserPrescription.Name = model.Name;
                            tUserPrescription.Instructions = model.Instructions;
                            tUserPrescription.ProductName = model.ProductName;
                            tUserPrescription.BrandName = model.BrandName;
                            tUserPrescription.DosageText = model.DosageText;
                            tUserPrescription.DosageValue = model.DosageValue;
                            tUserPrescription.FrequencyValue = model.FrequencyValue;
                            tUserPrescription.StrengthValue = model.StrengthValue;
                            tUserPrescription.RefillsRemaining = model.RefillsRemaining;
                            tUserPrescription.RefillsTotal = model.RefillsTotal;
                            tUserPrescription.ExpirationDateTime = model.ExpirationDateTime;
                            tUserPrescription.StartDateTime = model.StartDateTime;
                            tUserPrescription.EndDateTime = model.EndDateTime;
                            tUserPrescription.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                            tUserPrescription.SystemStatusID = 1; // Valid Entry
                            tUserPrescription.UserID = userId;

                            var jsontUserPrescription = JsonConvert.SerializeObject(tUserPrescription);
                            var result = await client.PostAsync(Service.ADD_NEW_tUserPrescriptions, new StringContent(jsontUserPrescription, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {

                                if (string.IsNullOrEmpty(model.Note))
                                {

                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    var r_tPrescription = JsonConvert.DeserializeObject<tUserPrescriptionViewModel>(await result.Content.ReadAsStringAsync());
                                    #region Derived Notes
                                    model.fkObjectID = r_tPrescription.ObjectID;
                                    model.UserID = userId;
                                    model.SystemStatusID = 1;
                                    model.CreateDateTime = DateTime.Now;
                                    model.LastUpdateDateTime = DateTime.Now;
                                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                    #endregion
                                    result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                    if (result.StatusCode == HttpStatusCode.OK)
                                    {
                                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    #region Add New  
                    else
                    {
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        var _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Prescriptions");
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                        }
                        tUserPrescriptionViewModel tUserPrescription = new tUserPrescriptionViewModel();
                        tUserPrescription.CreateDateTime = DateTime.Now;
                        tUserPrescription.ProviderID = model.ProviderID;
                        tUserPrescription.MedFormID = model.MedFormID;
                        tUserPrescription.FrequencyUOMID = model.FrequencyUOMID;
                        tUserPrescription.StrengthUOMID = model.StrengthUOMID;
                        tUserPrescription.RouteID = model.RouteID;
                        tUserPrescription.PharmacyID = model.PharmacyID;
                        tUserPrescription.Name = model.Name;
                        tUserPrescription.Instructions = model.Instructions;
                        tUserPrescription.ProductName = model.ProductName;
                        tUserPrescription.BrandName = model.BrandName;
                        tUserPrescription.DosageText = model.DosageText;
                        tUserPrescription.DosageValue = model.DosageValue;
                        tUserPrescription.FrequencyValue = model.FrequencyValue;
                        tUserPrescription.StrengthValue = model.StrengthValue;
                        tUserPrescription.RefillsRemaining = model.RefillsRemaining;
                        tUserPrescription.RefillsTotal = model.RefillsTotal;
                        tUserPrescription.ExpirationDateTime = model.ExpirationDateTime;
                        tUserPrescription.StartDateTime = model.StartDateTime;
                        tUserPrescription.EndDateTime = model.EndDateTime;
                        tUserPrescription.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                        tUserPrescription.SystemStatusID = 1; // Valid Entry
                        tUserPrescription.UserID = userId;

                        var jsontUserPrescription = JsonConvert.SerializeObject(tUserPrescription);
                        var result = await client.PostAsync(Service.ADD_NEW_tUserPrescriptions, new StringContent(jsontUserPrescription, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {

                            if (string.IsNullOrEmpty(model.Note))
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var r_tPrescription = JsonConvert.DeserializeObject<tUserPrescriptionViewModel>(await result.Content.ReadAsStringAsync());
                                #region Derived Notes
                                model.fkObjectID = r_tPrescription.ObjectID;
                                model.UserID = userId;
                                model.SystemStatusID = 1;
                                model.CreateDateTime = DateTime.Now;
                                model.LastUpdateDateTime = DateTime.Now;
                                var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                #endregion
                                result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                if (result.StatusCode == HttpStatusCode.OK)
                                {
                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    break;
                case "Problems":
                    #region User Problems
                    #region Edit Mode just invalidate the old row and add new 
                    if (model.ParentId > 0)
                    {
                        int InvalidatedSystemStatusID = 2;// User Invalidated
                        var _result = await client.GetAsync(Service.SoftDelete_tUserProblem + model.ParentId + "/" + InvalidatedSystemStatusID);
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                            _result = await client.DeleteAsync(Service.DELETE_tUserDerivedNote + model.ID);

                            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Problems");
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                            }

                            tUserProblemViewModel tUserProblem = new tUserProblemViewModel();
                            tUserProblem.CreateDateTime = DateTime.Now;
                            tUserProblem.Name = model.Name;
                            tUserProblem.StartDateTime = model.StartDateTime;
                            tUserProblem.EndDateTime = model.EndDateTime;
                            tUserProblem.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                            tUserProblem.SystemStatusID = 1; // Valid Entry
                            tUserProblem.UserID = userId;

                            var jsontUserProblem = JsonConvert.SerializeObject(tUserProblem);
                            var result = await client.PostAsync(Service.ADD_NEW_tUserProblems, new StringContent(jsontUserProblem, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {

                                if (string.IsNullOrEmpty(model.Note))
                                {

                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    var r_tProblem = JsonConvert.DeserializeObject<tUserProblemViewModel>(await result.Content.ReadAsStringAsync());
                                    #region Derived Notes
                                    model.fkObjectID = r_tProblem.ObjectID;
                                    model.UserID = userId;
                                    model.SystemStatusID = 1;
                                    model.CreateDateTime = DateTime.Now;
                                    model.LastUpdateDateTime = DateTime.Now;
                                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                    #endregion
                                    result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                    if (result.StatusCode == HttpStatusCode.OK)
                                    {
                                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    #region Add New  
                    else
                    {
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        var _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Problems");
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                        }
                        tUserProblemViewModel tUserProblem = new tUserProblemViewModel();
                        tUserProblem.CreateDateTime = DateTime.Now;
                        tUserProblem.Name = model.Name;
                        tUserProblem.StartDateTime = model.StartDateTime;
                        tUserProblem.EndDateTime = model.EndDateTime;
                        tUserProblem.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                        tUserProblem.SystemStatusID = 1; // Valid Entry
                        tUserProblem.UserID = userId;

                        var jsontUserProblem = JsonConvert.SerializeObject(tUserProblem);
                        var result = await client.PostAsync(Service.ADD_NEW_tUserProblems, new StringContent(jsontUserProblem, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {

                            if (string.IsNullOrEmpty(model.Note))
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var r_tProblem = JsonConvert.DeserializeObject<tUserProblemViewModel>(await result.Content.ReadAsStringAsync());
                                #region Derived Notes
                                model.fkObjectID = r_tProblem.ObjectID;
                                model.UserID = userId;
                                model.SystemStatusID = 1;
                                model.CreateDateTime = DateTime.Now;
                                model.LastUpdateDateTime = DateTime.Now;
                                var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                #endregion
                                result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                if (result.StatusCode == HttpStatusCode.OK)
                                {
                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    break;
                case "Procedures":
                    #region User Procedures
                    #region Edit Mode just invalidate the old row and add new 
                    if (model.ParentId > 0)
                    {
                        int InvalidatedSystemStatusID = 2;// User Invalidated
                        var _result = await client.GetAsync(Service.SoftDelete_UserProcedure + model.ParentId + "/" + InvalidatedSystemStatusID);
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                            _result = await client.DeleteAsync(Service.DELETE_tUserDerivedNote + model.ID);

                            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Procedures");
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                            }

                            tUserProcedureViewModel tUserProcedure = new tUserProcedureViewModel();
                            tUserProcedure.CreateDateTime = DateTime.Now;
                            tUserProcedure.PerformingOrganizationID = model.PerformingOrganizationID;
                            tUserProcedure.SpecimenID = model.SpecimenID;
                            tUserProcedure.DeviceID = model.DeviceID;
                            tUserProcedure.Name = model.Name;
                            tUserProcedure.StartDateTime = model.StartDateTime;
                            tUserProcedure.EndDateTime = model.EndDateTime;
                            tUserProcedure.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                            tUserProcedure.SystemStatusID = 1; // Valid Entry
                            tUserProcedure.UserID = userId;

                            var jsontUserProcedure = JsonConvert.SerializeObject(tUserProcedure);
                            var result = await client.PostAsync(Service.ADD_NEW_UserProcedures, new StringContent(jsontUserProcedure, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {

                                if (string.IsNullOrEmpty(model.Note))
                                {

                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    var r_tProcedure = JsonConvert.DeserializeObject<tUserProcedureViewModel>(await result.Content.ReadAsStringAsync());
                                    #region Derived Notes
                                    model.fkObjectID = r_tProcedure.ObjectID;
                                    model.UserID = userId;
                                    model.SystemStatusID = 1;
                                    model.CreateDateTime = DateTime.Now;
                                    model.LastUpdateDateTime = DateTime.Now;
                                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                    #endregion
                                    result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                    if (result.StatusCode == HttpStatusCode.OK)
                                    {
                                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    #region Add New  
                    else
                    {
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        var _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Procedures");
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                        }
                        tUserProcedureViewModel tUserProcedure = new tUserProcedureViewModel();
                        tUserProcedure.CreateDateTime = DateTime.Now;
                        tUserProcedure.PerformingOrganizationID = model.PerformingOrganizationID;
                        tUserProcedure.SpecimenID = model.SpecimenID;
                        tUserProcedure.DeviceID = model.DeviceID;
                        tUserProcedure.Name = model.Name;
                        tUserProcedure.StartDateTime = model.StartDateTime;
                        tUserProcedure.EndDateTime = model.EndDateTime;
                        tUserProcedure.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                        tUserProcedure.SystemStatusID = 1; // Valid Entry
                        tUserProcedure.UserID = userId;

                        var jsontUserProcedure = JsonConvert.SerializeObject(tUserProcedure);
                        var result = await client.PostAsync(Service.ADD_NEW_UserProcedures, new StringContent(jsontUserProcedure, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {

                            if (string.IsNullOrEmpty(model.Note))
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var r_tProcedure = JsonConvert.DeserializeObject<tUserProcedureViewModel>(await result.Content.ReadAsStringAsync());
                                #region Derived Notes
                                model.fkObjectID = r_tProcedure.ObjectID;
                                model.UserID = userId;
                                model.SystemStatusID = 1;
                                model.CreateDateTime = DateTime.Now;
                                model.LastUpdateDateTime = DateTime.Now;
                                var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                #endregion
                                result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                if (result.StatusCode == HttpStatusCode.OK)
                                {
                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    break;
                case "Test Results":
                    #region User Test Results
                    #region Edit Mode just invalidate the old row and add new 
                    if (model.ParentId > 0)
                    {
                        int InvalidatedSystemStatusID = 2;// User Invalidated
                        var _result = await client.GetAsync(Service.SOFTDELETE_TUSERTESTRESULT + model.ParentId + "/" + InvalidatedSystemStatusID);
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                            _result = await client.DeleteAsync(Service.DELETE_tUserDerivedNote + model.ID);

                            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Test Results");
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                            }

                            tUserTestResultViewModel tUserTestResult = new tUserTestResultViewModel();
                            tUserTestResult.CreateDateTime = DateTime.Now;
                            tUserTestResult.OrderingProviderID = model.ProviderID;
                            tUserTestResult.StatusID = model.StatusID;
                            tUserTestResult.Name = model.Name;
                            tUserTestResult.Comments = model.Comments;
                            tUserTestResult.Narrative = model.Narrative;
                            tUserTestResult.Impression = model.Impression;
                            tUserTestResult.Transcriptions = model.Transcriptions;
                            tUserTestResult.ResultDateTime = model.ResultDateTime;
                            tUserTestResult.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                            tUserTestResult.SystemStatusID = 1; // Valid Entry
                            tUserTestResult.UserID = userId;

                            var jsontUserTestResult = JsonConvert.SerializeObject(tUserTestResult);
                            var result = await client.PostAsync(Service.ADD_NEW_tUSERTESTRESULT, new StringContent(jsontUserTestResult, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {

                                if (string.IsNullOrEmpty(model.Note))
                                {

                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    var r_tTestResult = JsonConvert.DeserializeObject<tUserTestResultViewModel>(await result.Content.ReadAsStringAsync());
                                    #region Derived Notes
                                    model.fkObjectID = r_tTestResult.ObjectID;
                                    model.UserID = userId;
                                    model.SystemStatusID = 1;
                                    model.CreateDateTime = DateTime.Now;
                                    model.LastUpdateDateTime = DateTime.Now;
                                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                    #endregion
                                    result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                    if (result.StatusCode == HttpStatusCode.OK)
                                    {
                                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    #region Add New  
                    else
                    {
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        var _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Test Results");
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                        }
                        tUserTestResultViewModel tUserTestResult = new tUserTestResultViewModel();
                        tUserTestResult.CreateDateTime = DateTime.Now;
                        tUserTestResult.OrderingProviderID = model.ProviderID;
                        tUserTestResult.StatusID = model.StatusID;
                        tUserTestResult.Name = model.Name;
                        tUserTestResult.Comments = model.Comments;
                        tUserTestResult.Narrative = model.Narrative;
                        tUserTestResult.Impression = model.Impression;
                        tUserTestResult.Transcriptions = model.Transcriptions;
                        tUserTestResult.ResultDateTime = model.ResultDateTime;
                        tUserTestResult.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                        tUserTestResult.SystemStatusID = 1; // Valid Entry
                        tUserTestResult.UserID = userId;

                        var jsontUserTestResult = JsonConvert.SerializeObject(tUserTestResult);
                        var result = await client.PostAsync(Service.ADD_NEW_tUSERTESTRESULT, new StringContent(jsontUserTestResult, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {

                            if (string.IsNullOrEmpty(model.Note))
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var r_tTestResult = JsonConvert.DeserializeObject<tUserTestResultViewModel>(await result.Content.ReadAsStringAsync());
                                #region Derived Notes
                                model.fkObjectID = r_tTestResult.ObjectID;
                                model.UserID = userId;
                                model.SystemStatusID = 1;
                                model.CreateDateTime = DateTime.Now;
                                model.LastUpdateDateTime = DateTime.Now;
                                var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                #endregion
                                result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                if (result.StatusCode == HttpStatusCode.OK)
                                {
                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    break;
                case "Vitals":
                    #region User Vitals
                    #region Edit Mode just invalidate the old row and add new 
                    if (model.ParentId > 0)
                    {
                        int InvalidatedSystemStatusID = 2;// User Invalidated
                        var _result = await client.GetAsync(Service.SOFTDELETE_TUSERVITALS + model.ParentId + "/" + InvalidatedSystemStatusID);
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                            _result = await client.DeleteAsync(Service.DELETE_tUserDerivedNote + model.ID);

                            _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Vitals");
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                            }

                            UserVitalViewModel tUserVital = new UserVitalViewModel();
                            tUserVital.CreateDateTime = DateTime.Now;
                            tUserVital.ProviderID = model.ProviderID;
                            tUserVital.UOMID = model.UOMID;
                            tUserVital.Name = model.Name;
                            tUserVital.Value = model.Value;
                            tUserVital.ResultDateTime = model.ResultDateTime.DateTime;
                            tUserVital.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                            tUserVital.SystemStatusID = 1; // Valid Entry
                            tUserVital.UserID = userId;

                            var jsontUserVital = JsonConvert.SerializeObject(tUserVital);
                            var result = await client.PostAsync(Service.ADD_NEW_BODY_COMPOSITION_USERVITAL, new StringContent(jsontUserVital, Encoding.UTF8, "application/json"));
                            if (result.StatusCode == HttpStatusCode.OK)
                            {

                                if (string.IsNullOrEmpty(model.Note))
                                {

                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    var r_tTvital = JsonConvert.DeserializeObject<UserVitalViewModel>(await result.Content.ReadAsStringAsync());
                                    #region Derived Notes
                                    model.fkObjectID = r_tTvital.ObjectID;
                                    model.UserID = userId;
                                    model.SystemStatusID = 1;
                                    model.CreateDateTime = DateTime.Now;
                                    model.LastUpdateDateTime = DateTime.Now;
                                    var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                    #endregion
                                    result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                    if (result.StatusCode == HttpStatusCode.OK)
                                    {
                                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    #region Add New  
                    else
                    {
                        TUserSourceServiceViewModel _TUserSourceService = null;
                        var _result = await client.GetAsync(Service.Get_UserSourceServiceByUserIdAndCategory + userId + "/" + "Vitals");
                        if (_result.StatusCode == HttpStatusCode.OK)
                        {
                            _TUserSourceService = JsonConvert.DeserializeObject<TUserSourceServiceViewModel>(await _result.Content.ReadAsStringAsync());
                        }
                        UserVitalViewModel tUserVital = new UserVitalViewModel();
                        tUserVital.CreateDateTime = DateTime.Now;
                        tUserVital.ProviderID = model.ProviderID;
                        tUserVital.UOMID = model.UOMID;
                        tUserVital.Name = model.Name;
                        tUserVital.Value = model.Value;
                        tUserVital.ResultDateTime = model.ResultDateTime.DateTime;
                        tUserVital.UserSourceServiceID = _TUserSourceService != null ? _TUserSourceService.ID : (int?)null;
                        tUserVital.SystemStatusID = 1; // Valid Entry
                        tUserVital.UserID = userId;

                        var jsontUserVital = JsonConvert.SerializeObject(tUserVital);
                        var result = await client.PostAsync(Service.ADD_NEW_BODY_COMPOSITION_USERVITAL, new StringContent(jsontUserVital, Encoding.UTF8, "application/json"));
                        if (result.StatusCode == HttpStatusCode.OK)
                        {

                            if (string.IsNullOrEmpty(model.Note))
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var r_tVital = JsonConvert.DeserializeObject<UserVitalViewModel>(await result.Content.ReadAsStringAsync());
                                #region Derived Notes
                                model.fkObjectID = r_tVital.ObjectID;
                                model.UserID = userId;
                                model.SystemStatusID = 1;
                                model.CreateDateTime = DateTime.Now;
                                model.LastUpdateDateTime = DateTime.Now;
                                var jsontUserDerivedNote = JsonConvert.SerializeObject(model);
                                #endregion
                                result = await client.PostAsync(Service.ADD_NEW_tUserDerivedNote, new StringContent(jsontUserDerivedNote, Encoding.UTF8, "application/json"));
                                if (result.StatusCode == HttpStatusCode.OK)
                                {
                                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    break;
                default:
                    break;
            }



            return null;
        }
        [HttpGet]
        public async Task<ActionResult> DeleteUserDataWithNote(int id, int parentId, string categoryName)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            int InvalidatedSystemStatusID = 2;// User Invalidated

            switch (categoryName)
            {
                case "Allergies":
                    #region User Allergy
                    var _result = await client.GetAsync(Service.SoftDelete_tUserAllergies + parentId + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (id > 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + id + "/" + InvalidatedSystemStatusID);
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion
                    break;
                case "Care Plans":
                    #region User Care Plan
                    _result = await client.GetAsync(Service.SoftDelete_tUserCarePlan + parentId + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (id > 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + id + "/" + InvalidatedSystemStatusID);
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion
                    break;
                case "Encounters":
                    #region User Encounters
                    _result = await client.GetAsync(Service.SoftDelete_tUserEncounter + parentId + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (id > 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + id + "/" + InvalidatedSystemStatusID);
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion
                    break;
                case "Functional Statuses":
                    #region Functional Statuses
                    _result = await client.GetAsync(Service.SoftDelete_tUserFunctionalStatuses + parentId + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (id > 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + id + "/" + InvalidatedSystemStatusID);
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion
                    break;
                case "Immunizations":
                    #region Immunizations
                    _result = await client.GetAsync(Service.SoftDelete_tUserImmunization + parentId + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (id > 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + id + "/" + InvalidatedSystemStatusID);
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion
                    break;
                case "Instructions":
                    #region Instructions
                    _result = await client.GetAsync(Service.SoftDelete_tUserInstruction + parentId + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (id > 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + id + "/" + InvalidatedSystemStatusID);
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion
                    break;
                case "Narratives":
                    #region Narratives
                    _result = await client.GetAsync(Service.SoftDelete_tUserNarrative + parentId + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (id > 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + id + "/" + InvalidatedSystemStatusID);
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion
                    break;
                case "Prescriptions":
                    #region Prescriptions
                    _result = await client.GetAsync(Service.SoftDelete_tUserPrescription + parentId + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (id > 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + id + "/" + InvalidatedSystemStatusID);
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion
                    break;
                case "Problems":
                    #region Problems
                    _result = await client.GetAsync(Service.SoftDelete_tUserProblem + parentId + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (id > 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + id + "/" + InvalidatedSystemStatusID);
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion
                    break;
                case "Procedures":
                    #region Procedures
                    _result = await client.GetAsync(Service.SoftDelete_UserProcedure + parentId + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (id > 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + id + "/" + InvalidatedSystemStatusID);
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion
                    break;
                case "Test Results":
                    #region Test Results
                    _result = await client.GetAsync(Service.SOFTDELETE_TUSERTESTRESULT + parentId + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (id > 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + id + "/" + InvalidatedSystemStatusID);
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion
                    break;
                case "Vitals":
                    #region Vitals
                    _result = await client.GetAsync(Service.SOFTDELETE_TUSERVITALS + parentId + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        if (id > 0)
                        {
                            _result = await client.GetAsync(Service.SoftDelete_tUserDerivedNote + id + "/" + InvalidatedSystemStatusID);
                            if (_result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion
                    break;
                default:
                    break;
            }
            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetSubDataByParentIdAndCategory(int parentId, string categoryName)
        {
            int UserID = (System.Web.HttpContext.Current.Session["User"] as UserAuthLogin).UserID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var result = await client.GetAsync(Service.GET_ALL_SubDataByParentIdAndCategory + parentId + "/" + categoryName);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var subData = JsonConvert.DeserializeObject<List<UserDataNotesViewModel>>(await result.Content.ReadAsStringAsync());
                return Json(subData, JsonRequestBehavior.AllowGet);
            }

            return null;
        }
        [HttpGet]
        public async Task<ActionResult> GetSingleSubData(int itemId, string categoryName)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var result = await client.GetAsync(Service.GET_SubDataByCategory + itemId + "/" + categoryName);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var userdataNotes = JsonConvert.DeserializeObject<UserDataNotesViewModel>(await result.Content.ReadAsStringAsync());
                return Json(userdataNotes, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        public async Task<ActionResult> SaveSubUserData(UserDataNotesViewModel model)
        {
            int userId = (System.Web.HttpContext.Current.Session["User"] as UserAuthLogin).UserID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            int InvalidatedSystemStatusID = 2;// User Invalidated
            switch (model.Category)
            {
                #region Immunizations Date
                case "Immunizations":
                    if (model.ID > 0)
                    {
                        var _result = await client.GetAsync(Service.SoftDelete_UserImmunizationsDate + model.ID + "/" + InvalidatedSystemStatusID);

                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                        }
                    }
                    // Add new Immunizations Dates

                    tUserImmunizationsDateViewModel tUserImmunizationDate = new tUserImmunizationsDateViewModel();
                    tUserImmunizationDate.DateTime = model.Date;
                    tUserImmunizationDate.UserImmunizationID = model.ParentId;
                    tUserImmunizationDate.SystemStatusID = 1; // Valid Entry

                    var jsontUserImmunizationDate = JsonConvert.SerializeObject(tUserImmunizationDate);
                    var result = await client.PostAsync(Service.ADD_NEW_UserImmunizationsDates, new StringContent(jsontUserImmunizationDate, Encoding.UTF8, "application/json"));
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }

                    break;
                #endregion

                #region Narratives Entry
                case "Narratives":
                    if (model.ID > 0)
                    {
                        var _result = await client.GetAsync(Service.SoftDelete_UserNarrativeEntry + model.ID + "/" + InvalidatedSystemStatusID);

                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                        }
                    }
                    // Add new Immunizations Dates

                    tUserNarrativeEntryViewModel tUserNarrativeEntry = new tUserNarrativeEntryViewModel();
                    tUserNarrativeEntry.SectionSeqNum = model.SeqNum;
                    tUserNarrativeEntry.SectionTitle = model.Title;
                    tUserNarrativeEntry.SectionText = model.Text;
                    tUserNarrativeEntry.NarrativeID = model.ParentId;
                    tUserNarrativeEntry.SystemStatusID = 1; // Valid Entry

                    var jsontUserNarrativeEntry = JsonConvert.SerializeObject(tUserNarrativeEntry);
                    result = await client.PostAsync(Service.ADD_NEW_UserNarrativeEntry, new StringContent(jsontUserNarrativeEntry, Encoding.UTF8, "application/json"));
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }

                    break;
                #endregion

                #region Test Results Component
                case "Test Results":
                    if (model.ID > 0)
                    {
                        var _result = await client.GetAsync(Service.SOFTDELETE_TUSERTESTRESULTCOMPONENT + model.ID + "/" + InvalidatedSystemStatusID);

                        if (_result.StatusCode == HttpStatusCode.OK)
                        {

                        }
                    }
                    // Add new Immunizations Dates

                    tUserTestResultComponentVM tUserTestResultComponent = new tUserTestResultComponentVM();
                    tUserTestResultComponent.UOMID = model.UOMID;
                    tUserTestResultComponent.Name = model.Name;
                    tUserTestResultComponent.Value = model.ComponentsValue;
                    tUserTestResultComponent.LowValue = model.LowValue;
                    tUserTestResultComponent.HighValue = model.HighValue;
                    tUserTestResultComponent.RefRange = model.RefRange;
                    tUserTestResultComponent.Comments = model.Comments;
                    tUserTestResultComponent.TestResultID = model.ParentId;
                    tUserTestResultComponent.SystemStatusID = 1; // Valid Entry

                    var jsontUserTestResultComponent = JsonConvert.SerializeObject(tUserTestResultComponent);
                    result = await client.PostAsync(Service.ADD_NEW_tUSERTESTRESULTCOMPONENT, new StringContent(jsontUserTestResultComponent, Encoding.UTF8, "application/json"));
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }

                    break;
                #endregion
                default:
                    break;
            }
            return null;
        }

        [HttpGet]
        public async Task<ActionResult> DeleteSubData(int id, string categoryName)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            int InvalidatedSystemStatusID = 2;// User Invalidated
            switch (categoryName)
            {
                #region Immunizations Dates

                case "Immunizations":
                    var _result = await client.GetAsync(Service.SoftDelete_UserImmunizationsDate + id + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }

                    break;
                #endregion

                #region Narratives Entry
                case "Narratives":

                    _result = await client.GetAsync(Service.SoftDelete_UserNarrativeEntry + id + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }

                    break;
                #endregion

                #region Test Results Component
                case "Test Results":

                    _result = await client.GetAsync(Service.SOFTDELETE_TUSERTESTRESULTCOMPONENT + id + "/" + InvalidatedSystemStatusID);

                    if (_result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                    }

                    break;
                #endregion
                default:
                    break;
            }
            return null;
        }
        #endregion

        #region HumanAPI
        [HttpPost]
        public async Task<JsonResult> PostHumanApi(HumanApiModel model)
        {
            if (!ModelState.IsValid) return null;

            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            var userauth = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]);

            model.clientSecret = System.Configuration.ConfigurationManager.AppSettings["HumanApiClientSecret"].ToString();

            var client = new HttpClient { BaseAddress = new Uri(Service.HUMAN_API_SERVER) };
            var json = JsonConvert.SerializeObject(model);

            var result = await client.PostAsync(Service.ADD_HUMAN_API, new StringContent(json, Encoding.UTF8, "application/json"));

            if (result.StatusCode != HttpStatusCode.OK) return null;

            var humanApiAuthorizationModel = JsonConvert.DeserializeObject<HumanApiAuthorizationModel>(await result.Content.ReadAsStringAsync());


            if (humanApiAuthorizationModel != null)
            {
                var clientapi = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
                var credentialModel = new CredentialModel
                {
                    UserID = userId,
                    SourceUserID = model.humanId,
                    AccessToken = humanApiAuthorizationModel.accessToken,
                    PublicToken = humanApiAuthorizationModel.publicToken,
                    SourceID = 5,
                    SystemStatusID = 1,
                    CreateDateTime = DateTime.Now,
                    LastUpdatedDateTime = DateTime.Now
                };
                var jsontCredential = JsonConvert.SerializeObject(credentialModel);

                var _result = await clientapi.PostAsync(Service.ADD_NEW_CREDENTIALS, new StringContent(jsontCredential, Encoding.UTF8, "application/json"));
                if (_result.StatusCode == HttpStatusCode.OK)
                {
                    return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        public async Task<JsonResult> GetUserPublicToken()
        {
            if (!ModelState.IsValid) return null;

            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var result = await client.GetAsync(Service.GET_PUBLICTOKEN_BY_USER_ID + userId);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var credential = JsonConvert.DeserializeObject<Credential>(await result.Content.ReadAsStringAsync());
                return Json(credential.PublicToken, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        #endregion



        #region Audit-Log Screen
        public ActionResult AuditLogReport()
        {
            return View();
        }

        public async Task<ActionResult> AuditLogReportData()
        {

            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var result = await client.GetAsync(Service.Get_AuditLogRepot + userId);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var auditLogs = JsonConvert.DeserializeObject<List<AuditLogCustomModel>>(await result.Content.ReadAsStringAsync()).ToList();
                return Json(auditLogs, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Surveys Main
        public async Task<ActionResult> SurveysMain()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
                return RedirectToAction("Index", "Home");

            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            var list = await GetSurveys(userId);
            var viewData = new SurveysData
            {
                Surveys = list,
                TotalSurveysCount = list == null ? 0 : list.Count(),
                SurveysCompletedCount = list.Where(a => a.IsAllCompleted).Count()
            };
            return View(viewData);
        }

        private async Task<List<SurveysViewModel>> GetSurveys(int userId)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var result = await client.GetAsync(Service.Get_Surveys + userId);

            if (result.StatusCode != HttpStatusCode.OK) return null;
            var list = JsonConvert.DeserializeObject<IEnumerable<SurveysViewModel>>(await result.Content.ReadAsStringAsync()).ToList();

            return list;
        }





        #endregion

        #region Surveys Questions And Answers
        public async Task<JsonResult> GetSurveyQuestion(int surveyId)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var result = await client.GetAsync(Service.Get_SurveyQuestion + surveyId);

            if (result.StatusCode != HttpStatusCode.OK) return null;
            var content = await result.Content.ReadAsStringAsync();
            var currentQues = JsonConvert.DeserializeObject<SurveyQuestionsViewModel>(content);

            return Json(currentQues, JsonRequestBehavior.AllowGet);
        }



        public async Task<JsonResult> SaveAndBindQuestion(SurveyResultViewModel model)
        {
            SurveyQuestionsViewModel nextQuestion = null;
            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };


            if (model != null)
            {
                var vm = new SurveyResultViewModel();
                vm.UserID = userId;
                vm.CreateDateTime = DateTime.Now;
                vm.LastUpdatedDateTime = DateTime.Now;
                vm.ObjectID = Guid.NewGuid();
                vm.Answer = string.IsNullOrEmpty(model.Answer) ? string.Empty : model.Answer;
                vm.QuestionID = model.QuestionID;
                vm.SurveyID = model.SurveyID;

                //ToDo:  Need to make the below things dynamic
                //vm.SourceOrganizationID = 1;
                //vm.UserSourceServiceID = 1;
                //vm.SourceObjectID = string.Empty;
                vm.SystemStatusID = 1;

                var jsontUserTestResult = JsonConvert.SerializeObject(vm);
                var _result = await client.PostAsync(Service.Save_UserSurveyResults + model.QuestionIDsPassed, new StringContent(jsontUserTestResult, Encoding.UTF8, "application/json"));
                if (_result.StatusCode == HttpStatusCode.OK)
                    nextQuestion = JsonConvert.DeserializeObject<SurveyQuestionsViewModel>(await _result.Content.ReadAsStringAsync());
            }

            return Json(nextQuestion, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region View Surveys Submissions
        public async Task<JsonResult> ViewSubmissions(int surveyId)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var result = await client.GetAsync(Service.Get_UserSurveyResults + surveyId);

            if (result.StatusCode != HttpStatusCode.OK) return null;
            var list = JsonConvert.DeserializeObject<IEnumerable<SurveyResultViewModel>>(await result.Content.ReadAsStringAsync()).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region SHARE Settings

        public async Task<ActionResult> Share()
        {
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            var allows = await GetUserSHAREAllows(userId);
            var denies = await GetUserSHAREDenies(userId);

            allUserSHARESettings viewData = new allUserSHARESettings();
            viewData.allows = allows;
            viewData.denies = denies;

            return View(viewData);
        }

        private async Task<List<SHARESettings>> GetUserSHAREAllows(int userId)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var result = await client.GetAsync(Service.GET_USER_SHAREALLOWS + "/" + userId);

            if (result.StatusCode != HttpStatusCode.OK) return null;
            var list = JsonConvert.DeserializeObject<IEnumerable<SHARESettings>>(await result.Content.ReadAsStringAsync()).ToList();

            return list;
        }

        private async Task<List<SHARESettings>> GetUserSHAREDenies(int userId)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var result = await client.GetAsync(Service.GET_USER_SHAREDENIES + "/" + userId);

            if (result.StatusCode != HttpStatusCode.OK) return null;
            var list = JsonConvert.DeserializeObject<IEnumerable<SHARESettings>>(await result.Content.ReadAsStringAsync()).ToList();

            return list;
        }

        #endregion


        #region Save Ads Audit Log
        public async Task<ActionResult> SaveAdsAuditLog(AdsLogViewModel model)
        {
            SurveyQuestionsViewModel nextQuestion = null;
            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };


            if (model != null)
            {
                var vm = new AdsLogViewModel();
                vm.SponsoredAdID = model.SponsoredAdID;
                vm.CreateDateTime = DateTime.Now;
                vm.UserID = userId;
                vm.ViewClick = 1;

                var jsontUserTestResult = JsonConvert.SerializeObject(vm);
                var _result = await client.PostAsync(Service.Save_AdsLogData, new StringContent(jsontUserTestResult, Encoding.UTF8, "application/json"));
                if (_result.StatusCode == HttpStatusCode.OK)
                    return Json("Ok", JsonRequestBehavior.AllowGet);

            }


            return Json(" ", JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Source Types under Account on overview page(Home Page)
        private async Task<JsonResult> GetSourceTypeUnderAccount(int userId)
        {
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var result = await client.GetAsync(Service.Get_SourceSeviceType + userId);

            if (result.StatusCode != HttpStatusCode.OK) return null;
            var content = await result.Content.ReadAsStringAsync();
            var sourceServiceTypes = JsonConvert.DeserializeObject<List<SourceServiceTypeViewModel>>(content);

            return Json(sourceServiceTypes, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Charts Bindings in Overview Screen
        public async Task<JsonResult> GetSurveyChart()
        {
            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var result = await client.GetAsync(Service.GetSurveyChartData + userId);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var surveyChartData = JsonConvert.DeserializeObject<List<SurveysData>>(await result.Content.ReadAsStringAsync()).ToList();
                return Json(surveyChartData, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }



        public async Task<JsonResult> GetSourceConnectedChart()
        {
            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var result = await client.GetAsync(Service.GetSourceChartData + userId);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var surveyChartData = JsonConvert.DeserializeObject<List<ConnectedSourceViewModel>>(await result.Content.ReadAsStringAsync()).ToList();
                return Json(surveyChartData, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Get Source Service Type
        public async Task<ActionResult> GetSourceServiceType()
        {
            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            //var sourceType = await GetSourceTypeUnderAccount(userId);

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var resultSource = await client.GetAsync(Service.Get_SourceSeviceType + userId);
            if (resultSource.StatusCode != HttpStatusCode.OK) return null;
            var content = await resultSource.Content.ReadAsStringAsync();
            var sourceServiceTypes = JsonConvert.DeserializeObject<List<SourceServiceTypeViewModel>>(content);
            return Json(sourceServiceTypes, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Share Purpose Data
        public async Task<ActionResult> GetSharePurposeData()
        {
            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            //var sourceType = await GetSourceTypeUnderAccount(userId);

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var resultSource = await client.GetAsync(Service.Get_SharePurposeData + userId);
            if (resultSource.StatusCode != HttpStatusCode.OK) return null;
            var content = await resultSource.Content.ReadAsStringAsync();
            var sourceServiceTypes = JsonConvert.DeserializeObject<List<SharePurposeCategoriesViewModel>>(content);
            return Json(sourceServiceTypes, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> SaveSharePurposeData(string shareIds, string purposeIds)
        {
            //UserShareSettingViewModel item;
            List<UserShareSettingViewModel> list = new List<UserShareSettingViewModel>();
            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
            var slist = shareIds.Split(',');
            var pList = purposeIds.Split(',');


            if (slist.Length > 0)
            {
                foreach (var item in slist)
                {

                    var vm = new UserShareSettingViewModel();
                    vm.UserID = userId;
                    vm.AllData = true;
                    vm.SystemStatusID = 1;
                    vm.SHARESettingID = 1;
                    vm.UserSourceServiceID = Convert.ToInt32(item);
                    vm.CreateDateTime = DateTime.Now;
                    list.Add(vm);
                }
            }
            if (pList.Length > 0)
            {
                foreach (var value in pList)
                {

                    var vm = new UserShareSettingViewModel();
                    vm.UserID = userId;
                    vm.AllData = true;
                    vm.SystemStatusID = 1;
                    vm.SHARESettingID = 2;
                    vm.UserSourceServiceID = Convert.ToInt32(value);

                    vm.CreateDateTime = DateTime.Now;

                    list.Add(vm);

                }
            }

            var jsontUserTestResult = JsonConvert.SerializeObject(list);
            var _result = await client.PostAsync(Service.Save_UserShareData, new StringContent(jsontUserTestResult, Encoding.UTF8, "application/json"));
            if (_result.StatusCode == HttpStatusCode.OK)
                JsonConvert.DeserializeObject<IEnumerable<UserShareSettingViewModel>>(await _result.Content.ReadAsStringAsync());

            return Json(1);
        }
        #endregion


        #region Overview - Recent Activity & Relevant Information
        private async Task<IEnumerable<UserAuditLogsViewModel>> GetRecentActivitiesByUserId(int currentPage)
        {
            var userId = ((UserAuthLogin)System.Web.HttpContext.Current.Session["User"]).tUser.ID;
            //var sourceType = await GetSourceTypeUnderAccount(userId);

            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var apiUrl = string.Format("{0}{1}/5/{2}", Service.Get_ListOfAuditTypesByUser, userId, currentPage);
            var resultSource = await client.GetAsync(apiUrl);
            if (resultSource.StatusCode != HttpStatusCode.OK) return null;
            var content = await resultSource.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<IEnumerable<UserAuditLogsViewModel>>(content);
            return list;
        }

        public async Task<JsonResult> GetRecentActivities(int currentPage)
        {
            var recordsCount = 5;
            var list = await GetRecentActivitiesByUserId(currentPage);
            int skipped = (currentPage * recordsCount);
            var totalCount = list.FirstOrDefault().TotalCount;
            var jsData = new
            {
                list,
                MoreRecords = (skipped <= totalCount)
            };
            return Json(jsData, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}