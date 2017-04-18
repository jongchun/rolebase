using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using UsersAndRolesDemo;

namespace UsersAndRolesDemo.Controllers
{

    // *** Only copy things from here to OwnerPropertyController.cs but in baby steps. Test after each step *.
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class _OwnerPropertyContoller : ApiController
    {
        private MyDbEntities db = new MyDbEntities();

        // GET: api/_OwnerPropertyContoller
        [HttpGet]
        public IQueryable<Property> GetProperties()
        {
            MyDbEntities context = new MyDbEntities();
            context.Configuration.LazyLoadingEnabled = false;
            return context.Properties;
        }

        // GET: api/_OwnerPropertyContoller/5
        [ResponseType(typeof(Property))]
        public IHttpActionResult GetProperty(int id)
        {
            MyDbEntities context = new MyDbEntities();
            context.Configuration.LazyLoadingEnabled = false;
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);

        }

        //no adjustment made bellow 
        // PUT: api/_OwnerPropertyContoller/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProperty(int id, Property property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != property.Id)
            {
                return BadRequest();
            }

            db.Entry(property).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/_OwnerPropertyContoller
        [ResponseType(typeof(Property))]
        public IHttpActionResult PostProperty(Property property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Properties.Add(property);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = property.Id }, property);
        }

        // DELETE: api/_OwnerPropertyContoller/5
        [ResponseType(typeof(Property))]
        public IHttpActionResult DeleteProperty(int id)
        {
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return NotFound();
            }

            db.Properties.Remove(property);
            db.SaveChanges();

            return Ok(property);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PropertyExists(int id)
        {
            return db.Properties.Count(e => e.Id == id) > 0;
        }
    }
}