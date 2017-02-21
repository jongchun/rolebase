using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using UsersAndRolesDemo.Models;

namespace UsersAndRolesDemo.Controllers
{
    public class HomeController : Controller
    {
        private MyDbEntities db = new MyDbEntities();
        public ActionResult Index()
        {
            
            return View(db.Properties.ToList());
        }

        public ActionResult About()
        {
            //ViewData["Message"] = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            //ViewData["Message"] = "Your contact page.";

            return View();
        }
    }
}