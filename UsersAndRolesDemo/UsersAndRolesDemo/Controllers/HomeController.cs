using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using UsersAndRolesDemo.Models;
using UsersAndRolesDemo.Services;

namespace UsersAndRolesDemo.Controllers
{
    public class HomeController : Controller
    {
        private MyDbEntities db = new MyDbEntities();

        public ActionResult Index()
        {
            
            return View(db.Properties.ToList());
        }

        public ActionResult Properties()
        {
            //var properties = db.Properties.Include(p => p.AspNetUser);
            return View(db.Properties.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", property.UserId);
            return View(property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,UserId,propertyType,numBedrooms,numWashrooms,kitchen,baseRate,address,builtYear,smokingAllowed,maxNumberGuests,availableDates,dimensions")] Property property)
        {
            if (ModelState.IsValid)
            {
                db.Entry(property).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", property.UserId);
            return View(property);
        }

        // GET: Properties/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Property property = db.Properties.Find(id);
            db.Properties.Remove(property);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewData["Message"] = "Luxury rental cabins located throughout the Sea to Sky corridor, " + 
                "Our Service has been praised by our loyal costumers and new guests. Our properties are increasing in Wistler area. Book your stay with us today!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(Contact contact)
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
                EmailService es = new EmailService();
                es.ContactUS(contact);
                ViewBag.Messages = "Email sent successfully.";
            }

            return View();
        }

        public ActionResult Info()
        {
            return View();
        }

        // GET: Home/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        //public ActionResult Create([Bind(Include = "Id,UserId,propertyType,numBedrooms,numWashrooms,kitchen,baseRate,address,builtYear,smokingAllowed,maxNumberGuests,availableDates,dimensions")] Property property)
        public ActionResult Create(PostPropertyVM property)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager
            = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.FindByName(User.Identity.GetUserName());

            //IdentityUser identityUser = manager.Find(login.UserName,
            //                                                 login.Password);
            //string user = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                //UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
                //UserManager<IdentityUser> manager
                //= new UserManager<IdentityUser>(userStore);
                //IdentityUser identityUser = manager.Find(login.UserName,
                //                                                 login.Password);

                var test = new Property
                {
                    UserId = identityUser.Id,
                    summary = property.Summary,
                    propertyType = property.PropertyType,
                    numBedrooms = property.NumBedrooms,
                    numWashrooms = property.NumWashrooms,
                    kitchen = property.Kitchen,
                    baseRate = property.BaseRate,
                    address = property.Address,
                    builtYear = property.BuiltYear,
                    smokingAllowed = property.SmokingAllowed,
                    maxNumberGuests = property.MaxNumberGuests,
                    availableDates = property.AvailableDates,
                    dimensions = property.Dimensions
                };
                db.Properties.Add(test);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", property.UserId);
            return View(property);
        }
    }
}