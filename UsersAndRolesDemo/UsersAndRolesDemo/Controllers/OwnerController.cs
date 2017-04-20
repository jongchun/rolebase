using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UsersAndRolesDemo;
using UsersAndRolesDemo.Models;
using UsersAndRolesDemo.Repositories;

namespace UsersAndRolesDemo.Controllers
{
    [Authorize]
    public class OwnerController : Controller
    {
        private MyDbEntities db = new MyDbEntities();

        public ActionResult Index()
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.FindByName(User.Identity.GetUserName());
            
            ViewBag.Username = User.Identity.Name;
            //var properties = db.Properties.Include(p => p.AspNetUser);
            var id = identityUser.Id;
            var properties = from b in db.Properties
                        where b.UserId == id
                        select b;
            
            return View(properties.ToList());
        }

        // GET: Owner/Details/5
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

        // GET: Owner/Create
        [Authorize(Roles = "Owner")]
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
        [Authorize(Roles = "Owner")]
        //public ActionResult Create([Bind(Include = "Id,UserId,propertyType,numBedrooms,numWashrooms,kitchen,baseRate,address,builtYear,smokingAllowed,maxNumberGuests,availableDates,dimensions")] Property property)
        public ActionResult Create([Bind(Exclude ="Id")]PostPropertyVM property)
        {
            if (ModelState.IsValid)
            {
                UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
                IdentityUser identityUser = manager.FindByName(User.Identity.GetUserName());

                OwnerRepo or = new OwnerRepo();
                if(!or.PostProperty(property, identityUser.Id, SaveImages(identityUser.Id)))
                {
                    return View(property);
                }

                return RedirectToAction("Index");
            }

            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", property.UserId);
            return View(property);
        }

        // GET: Owner/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", property.UserId);

            OwnerRepo or = new OwnerRepo();
            
            return View(or.GetProperty(id));
        }

        // POST: Owner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PostPropertyVM property)
        {
            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", property.UserId);
            OwnerRepo or = new OwnerRepo();
            if(or.EditProperty(property, property.UserId, SaveImages(property.UserId)))
            {
                return RedirectToAction("Index");
            }
            return View(property);
        }

        // GET: Owner/Delete/5
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

        // POST: Owner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OwnerRepo or = new OwnerRepo();
            if (!or.DeleteProperty(id))
            {
                Property property = db.Properties.Find(id);
                return View(property);
            }
            return RedirectToAction("Index");
        }

        // Save images to Content\images\{username}\ and return imagelist
        public List<string> SaveImages(string userid)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.FindById(userid);
           
            List<string> imageList = new List<string>();
            var directoryToSave = Server.MapPath(Url.Content("~/Content/Images/") + identityUser.UserName);
            
            if (!Directory.Exists(directoryToSave))
            {
                Directory.CreateDirectory(directoryToSave);
            }
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                if (file.ContentLength > 0)
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    file.SaveAs(Path.Combine(directoryToSave, filename));
                    imageList.Add("~/Content/Images/" + identityUser.UserName + "/"+ filename);
                }
                else
                {
                    imageList.Add(null);
                }
            }

            return imageList;
        }

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
