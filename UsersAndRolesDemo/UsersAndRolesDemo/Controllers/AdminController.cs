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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private MyDbEntities db = new MyDbEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRole(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                AspNetRole role = new AspNetRole();
                role.Id = roleVM.RoleName;
                role.Name = roleVM.RoleName;
                MyDbEntities context = new MyDbEntities();
                context.AspNetRoles.Add(role);
                context.SaveChanges();
            }
            return View();
        }

        [HttpGet]
        public ActionResult AddUserToRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUserToRole(UserRoleVM userRoleVM)
        {
            if (ModelState.IsValid)
            {
                MyDbEntities context = new MyDbEntities();
                AspNetUser user = context.AspNetUsers
                                    .Where(u => u.UserName == userRoleVM.UserName).FirstOrDefault();
                AspNetRole role = context.AspNetRoles
                                    .Where(r => r.Name == userRoleVM.RoleName).FirstOrDefault();

                user.AspNetRoles.Add(role);
                context.SaveChanges();
            }
            return View();
        }

    }
}