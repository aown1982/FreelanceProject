using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShareForCures.Models.Users;

namespace ShareForCures.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //if (System.Web.HttpContext.Current.Session["Login"] != null &&
            //    System.Web.HttpContext.Current.Session["Login"].ToString() == "yes")
            //{
            //    return View("User");
            //}
            //else
            //{
            //    return View();
            //}
            UserAuthLogin user = (UserAuthLogin)System.Web.HttpContext.Current.Session["User"];
            if (user != null)
            {
                ViewBag.Login = "yes";
                ViewBag.Message = "Welcome " + user.tUser.FirstName.ToString() + " " + user.tUser.LastName.ToString();
                System.Web.HttpContext.Current.Session["User"] = user;
                return RedirectToAction("Index","User");
            }
            else
            {
                return View();
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public void Logout()
        {
            Session.RemoveAll();
            Session.Abandon();

            System.Web.HttpContext.Current.Session["User"] = null;
            Response.Redirect("/");
        }
    }
}
