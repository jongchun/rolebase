using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UsersAndRolesDemo;
using UsersAndRolesDemo.Models;

namespace UsersAndRolesDemo.Controllers
{
    public class OwnerController : Controller
    {
        private MyDbEntities db = new MyDbEntities();

        [Authorize]
        public ActionResult Index()
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager
            = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.FindByName(User.Identity.GetUserName());

            ViewBag.Username = User.Identity.Name;
            var properties = db.Properties.Include(p => p.AspNetUser);
            //var properties = db.Properties.Where;
            return View(properties.ToList());
        }

        [Authorize]
        // GET: Owner/Details/5
        public ActionResult Details(int id)
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

        [Authorize]
        // GET: Owner/Create
        public ActionResult Create()
        {
            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Owner/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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

                var test = new Property {
                    UserId = identityUser.Id,
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

        [Authorize]
        // GET: Owner/Edit/5
        public ActionResult Edit(int id)
        {
            /*
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            */
            Property property = db.Properties.Find(id);

            if (property == null)
            {
                return HttpNotFound();
            }

            PostPropertyVM propertyVM = new PostPropertyVM();
            propertyVM.PropertyType = property.propertyType;
            propertyVM.NumBedrooms = (int)property.numBedrooms;
            propertyVM.NumWashrooms = (int)property.numWashrooms;
            propertyVM.Kitchen = (int)property.kitchen;
            propertyVM.BaseRate = (int)property.baseRate;
            propertyVM.Address = property.address;
            propertyVM.BuiltYear = property.builtYear;
            propertyVM.SmokingAllowed = property.smokingAllowed;
            propertyVM.MaxNumberGuests = property.maxNumberGuests;
            propertyVM.AvailableDates = (DateTime)property.availableDates;
            propertyVM.Dimensions = property.dimensions;

            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", property.UserId);
            return View(propertyVM);
        }

        // POST: Owner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(PostPropertyVM property)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.FindByName(User.Identity.GetUserName());

            if (ModelState.IsValid)
            {
                var test = new Property
                {
                    UserId = identityUser.Id,
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
                db.Entry(test).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", property.UserId);
            return View(property);
        }

        [Authorize]
        // GET: Owner/Delete/5
        public ActionResult Delete(int id)
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

        // POST: Owner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Property property = db.Properties.Find(id);
            db.Properties.Remove(property);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
