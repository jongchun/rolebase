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
        [Authorize]
        public ActionResult AddRole() {    
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddRole(RoleVM roleVM) {
            if(ModelState.IsValid) { 
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
        [Authorize]
        public ActionResult AddUserToRole() {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddUserToRole(UserRoleVM userRoleVM) {
            if(ModelState.IsValid) { 
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

        [Authorize(Roles = "Admin")]
        // To allow more than one role access use syntax like the following:
        // [Authorize(Roles="Admin, Staff")]
        public ActionResult AdminOnly() {
            return View();
        }

        [HttpGet]
        public ActionResult Login() {
            return View();
        }   
        [HttpPost]
        public ActionResult Login(LoginVM login) {   
            // UserStore and UserManager manages data retreival.
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager 
            = new UserManager<IdentityUser>(userStore);
            IdentityUser         identityUser = manager.Find(login.UserName, 
                                                             login.Password);
            
            if (ModelState.IsValid) {
                if (identityUser != null) { 
                    IAuthenticationManager authenticationManager 
                                           = HttpContext.GetOwinContext()
                                            .Authentication;
                    authenticationManager
                   .SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    var identity = new ClaimsIdentity(new [] {
                                            new Claim(ClaimTypes.Name, login.UserName),
                                        },
                                        DefaultAuthenticationTypes.ApplicationCookie,
                                        ClaimTypes.Name, ClaimTypes.Role);
                    // SignIn() accepts ClaimsIdentity and issues logged in cookie. 
                    authenticationManager.SignIn(new AuthenticationProperties {
                                                 IsPersistent = false }, identity);
                    var test = identityUser.Roles.ElementAt(0).RoleId;
                    if (test == "Owner")
                    {
                        return RedirectToAction("Index", "Owner");
                    }
                    else if (test == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("SecureArea", "Home");
                    }
                }
            }
            return View(); 
        }
        [HttpGet]
        public ActionResult Register() {
            return View();
        }   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisteredUserVM newUser) {
            var userStore         = new UserStore<IdentityUser>();
            var manager           = new UserManager<IdentityUser>(userStore);
            var identityUser      = new IdentityUser() { UserName = newUser.UserName, 
                                                         Email    = newUser.Email };
            IdentityResult result = manager.Create(identityUser, newUser.Password);

            if (result.Succeeded) {
                var authenticationManager 
                                  = HttpContext.Request.GetOwinContext()
                                   .Authentication;
                var userIdentity  = manager.CreateIdentity(identityUser,  
                                  DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { }, 
                                             userIdentity);
            }
            return View();
        }
        [Authorize]
        public ActionResult SecureArea() {
            return View();
        }
        
        public ActionResult Logout() { 
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}