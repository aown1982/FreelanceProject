using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShareForCures.Models.Users;

namespace ShareForCures.Controllers
{
    public class ConsentController : Controller
    {
        public ActionResult ConditionsOfUse()
        {
            ViewBag.Message = "Conditions of Use";

            UserAuthLogin user = (UserAuthLogin)System.Web.HttpContext.Current.Session["User"];
            if (user != null)
            {
                ViewBag.Login = "yes";
            }

            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            ViewBag.Message = "Privacy Policy";

            UserAuthLogin user = (UserAuthLogin)System.Web.HttpContext.Current.Session["User"];
            if (user != null)
            {
                ViewBag.Login = "yes";
            }

            return View();
        }
    }
}
