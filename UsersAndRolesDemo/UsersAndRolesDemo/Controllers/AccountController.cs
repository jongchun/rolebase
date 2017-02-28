using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using UsersAndRolesDemo.Models;
using UsersAndRolesDemo.Repositories;
using UsersAndRolesDemo.Services;

namespace UsersAndRolesDemo.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        const string EMAIL_CONFIRMATION = "EmailConfirmation";
        const string PASSWORD_RESET = "ResetPassword";

        private MyDbEntities db = new MyDbEntities();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginVM login)
        {
            // UserStore and UserManager manages data retreival.
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.Find(login.UserName, login.Password);

            if (ModelState.IsValid)
            {
                //RECAPTCHA CODE HERE...
                CaptchaHelper captchaHelper = new CaptchaHelper();
                string captchaResponse = captchaHelper.CheckRecaptcha();
                ViewBag.CaptchaResponse = captchaResponse;

                if (ValidLogin(login) && captchaResponse == "Valid")
                {
                    IAuthenticationManager authenticationManager
                                           = HttpContext.GetOwinContext()
                                            .Authentication;
                    authenticationManager
                   .SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    var identity = new ClaimsIdentity(new[] {
                                            new Claim(ClaimTypes.Name, login.UserName),
                                        },
                                        DefaultAuthenticationTypes.ApplicationCookie,
                                        ClaimTypes.Name, ClaimTypes.Role);
                    // SignIn() accepts ClaimsIdentity and issues logged in cookie. 
                    authenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, identity);
                    var role = identityUser.Roles.ElementAt(0).RoleId;
                    if (role == "Owner")
                    {
                        //System.Threading.Thread.Sleep(2000);
                        return RedirectToAction("Index", "Owner");
                    }
                    else if (role == "Admin")
                    {
                        //System.Threading.Thread.Sleep(2000);
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        //return RedirectToAction("SecureArea", "Home");
                    }
                }
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(RegisteredUserVM newUser)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //RECAPTCHA CODE HERE...
            CaptchaHelper captchaHelper = new CaptchaHelper();
            string captchaResponse = captchaHelper.CheckRecaptcha();
            ViewBag.CaptchaResponse = captchaResponse;

            if (captchaResponse == "Valid")
            {
                var userStore = new UserStore<IdentityUser>();
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore)
                {
                    UserLockoutEnabledByDefault = true,
                    DefaultAccountLockoutTimeSpan = new TimeSpan(0, 10, 0),
                    MaxFailedAccessAttemptsBeforeLockout = 5
                };

                var identityUser = new IdentityUser()
                {
                    UserName = newUser.UserName,
                    Email = newUser.Email
                };
                IdentityResult result = manager.Create(identityUser, newUser.Password);

                if (result.Succeeded)
                {
                    CreateTokenProvider(manager, EMAIL_CONFIRMATION);

                    var code = manager.GenerateEmailConfirmationToken(identityUser.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account",
                                                    new { userId = identityUser.Id, code = code },
                                                        protocol: Request.Url.Scheme);

                    string email = "Please confirm your account by clicking this link: <a href=\""
                                    + callbackUrl + "\">Confirm Registration</a>";

                    EmailService es = new EmailService();
                    es.SendEmail(newUser.Email, "Confirm Registration", email);
                    ViewBag.Confirmation = "We sent the confirm registration email. Please check the email first.";

                }
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        bool ValidLogin(LoginVM login)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(userStore)
            {
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = new TimeSpan(0, 10, 0),
                MaxFailedAccessAttemptsBeforeLockout = 3
            };
            var user = userManager.FindByName(login.UserName);

            if (user == null)
                return false;

            // User is locked out.
            if (userManager.SupportsUserLockout && userManager.IsLockedOut(user.Id))
                return false;

            // Validated user was locked out but now can be reset.
            if (userManager.CheckPassword(user, login.Password) && userManager.IsEmailConfirmed(user.Id))

            {
                if (userManager.SupportsUserLockout
                 && userManager.GetAccessFailedCount(user.Id) > 0)
                {
                    userManager.ResetAccessFailedCount(user.Id);
                }
            }
            // Login is invalid so increment failed attempts.
            else
            {
                bool lockoutEnabled = userManager.GetLockoutEnabled(user.Id);
                if (userManager.SupportsUserLockout && userManager.GetLockoutEnabled(user.Id))
                {
                    userManager.AccessFailed(user.Id);
                    return false;
                }
            }
            return true;
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string userID, string code)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindById(userID);
            CreateTokenProvider(manager, EMAIL_CONFIRMATION);
            try
            {
                IdentityResult result = manager.ConfirmEmail(userID, code);
                if (result.Succeeded)
                    ViewBag.Message = "You are now registered!";
            }
            catch
            {
                ViewBag.Message = "Validation attempt failed!";
            }
            return View();
        }

        [AllowAnonymous]
        void CreateTokenProvider(UserManager<IdentityUser> manager, string tokenType)
        {
            manager.UserTokenProvider = new EmailTokenProvider<IdentityUser>();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //RECAPTCHA CODE HERE...
            CaptchaHelper captchaHelper = new CaptchaHelper();
            string captchaResponse = captchaHelper.CheckRecaptcha();
            ViewBag.CaptchaResponse = captchaResponse;

            if (captchaResponse == "Valid")
            {
                var userStore = new UserStore<IdentityUser>();
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
                var user = manager.FindByEmail(model.Email);
                CreateTokenProvider(manager, PASSWORD_RESET);

                var code = manager.GeneratePasswordResetToken(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account",
                                             new { userId = user.Id, code = code },
                                             protocol: Request.Url.Scheme);
                string mail = "Please reset your password by clicking <a href=\""
                                         + callbackUrl + "\">here</a>";
                EmailService es = new EmailService();
                es.SendEmail(model.Email, "Password Reset", mail);

                ViewBag.EmailMessage = "We sent the password reset email. Please check the email.";
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string userID, string code)
        {
            ViewBag.PasswordToken = code;
            ViewBag.UserID = userID;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordVM model)
        {

            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindById(model.UserID);
            CreateTokenProvider(manager, PASSWORD_RESET);
            
            IdentityResult result = manager.ResetPassword(model.UserID, model.Code, model.Password);
            if (result.Succeeded)
                ViewBag.Result = "The password has been reset.";
            else
                ViewBag.Result = "The password has not been reset.";
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminProfile()
        {
            var n = User.Identity.Name;
            AccountRepo rp = new AccountRepo();
            AdminProfileVM user = rp.GetAdmin(n);

            return View(user);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminProfile([Bind(Exclude = "ProfilePicture")]AdminProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            byte[] profileImage = null;
            if(Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["ProfilePicture"];

                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    profileImage = binary.ReadBytes(poImgFile.ContentLength);
                }
                model.ProfilePicture = profileImage;
            }
            AccountRepo rp = new AccountRepo();
            if (rp.UpdateAdmin(model))
            {
                ViewBag.Message = "Updated.";
            }else
            {
                ViewBag.Message = "Updated failed.";
            }

            //return RedirectToAction("Index");
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public ActionResult OwnerProfile()
        {
            var n = User.Identity.Name;
            AccountRepo rp = new AccountRepo();
            OwnerProfileVM user = rp.GetOwner(n);

            return View(user);
        }
        [HttpPost]
        [Authorize(Roles = "Owner")]
        public ActionResult OwnerProfile([Bind(Exclude = "ProfilePicture")]OwnerProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            byte[] profileImage = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["ProfilePicture"];

                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    profileImage = binary.ReadBytes(poImgFile.ContentLength);
                }
                model.ProfilePicture = profileImage;
            }

            AccountRepo rp = new AccountRepo();
            if (rp.UpdateOwner(model))
            {
                ViewBag.Message = "Updated.";
            }
            else
            {
                ViewBag.Message = "Updated failed.";
            }

            //return RedirectToAction("Index");
            return View();
        }

        public FileContentResult UserPhotos()
        {
            string userName = User.Identity.GetUserName();
            AspNetUser user = db.AspNetUsers.Where(a => a.UserName == userName).FirstOrDefault();

            if (user.profilePicture != null)
            {
                return new FileContentResult(user.profilePicture, "image/jpeg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Content/Images/no_profile_image.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");
            }
        }
    }
}