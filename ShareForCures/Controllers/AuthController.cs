using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;
using Google.Apis.Services;
using Newtonsoft.Json;
using ShareForCures.Helpers;
using ShareForCures.Models.Google;
using ShareForCures.Models.UserData;
using ShareForCures.Models.Users;

namespace ShareForCures.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public async Task<ActionResult> GoogleLogin(CancellationToken cancelToken)
        {
            var result = await new AuthorizationCodeMvcApp(this,
                   new GoogleAuth()).AuthorizeAsync(cancelToken);

            if (result.Credential == null)
                return new RedirectResult(result.RedirectUri);
            if (result.Credential.Token.IsExpired(result.Credential.Flow.Clock))
            {
                if (result.Credential.RefreshTokenAsync(CancellationToken.None).Result)
                {

                }
            }
            var plusService = new PlusService(new BaseClientService.Initializer
            {
                HttpClientInitializer = result.Credential,
                ApplicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"]
            });

            //get the user basic information
            Person me = plusService.People.Get("me").Execute();
            var client = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };

            var credentialRequest = await client.GetAsync(Service.GET_CREDENTIALS_BY_SOURCE_USER_ID + me.Id);
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
                    //return View(user);
                }

            }
            else if (credentialRequest.StatusCode == HttpStatusCode.NotFound)
            {
                //this is a new user -> register view
                var user = new UsersModel
                {
                    SocialUserId = me.Id,
                    AccessToken = result.Credential.Token.AccessToken,
                    SourceUserIDToken = result.Credential.Token.IdToken,
                    RefreshToken = result.Credential.Token.RefreshToken,
                    FirstName = me.Name.GivenName,
                    LastName = me.Name.FamilyName,
                    Email = me.Emails[0].Value,
                    ExpiresIn =result.Credential.Token.ExpiresInSeconds.Value.ToString(),
                    SocialType = Service.SOCIAL_TYPE_GOOGLE
                };
                return View(user);
            }
            
            //In other case return back to home page.
            return RedirectToAction("index", "home");
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
    }
}