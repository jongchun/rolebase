using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace UsersAndRolesDemo.Controllers
{
  
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OwnerPropertyController : ApiController
    {
        // GET: api/OwnerProperty
        [HttpGet]
        //public IHttpActionResult Get()
        //{
        //    MyDbEntities context = new MyDbEntities();
        //    context.Configuration.LazyLoadingEnabled = false;
        //    // Property property = new Property();
        //    //Properties
        //    Reservation reservation = new Reservation();
        //    return Ok(context.Reservations);

        //}
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //public string GetManufacturers()
        //{
        //    // Highly recommended.
        //    // Localizing the context prevents occasional query conflicts.
        //    MyDbEntities context = new MyDbEntities();

        //    // Disable lazy loading otherwise the REST service returns
        //    // all data in the database.
        //    context.Configuration.LazyLoadingEnabled = false;

        //    return "Hi";
        //}

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IQueryable<Property> GetReservations()
        {
            // Highly recommended.
            // Localizing the context prevents occasional query conflicts.
            MyDbEntities context = new MyDbEntities();

            // Disable lazy loading otherwise the REST service returns
            // all data in the database.
            context.Configuration.LazyLoadingEnabled = false;
            List<Property> somedata = context.Properties.ToList();
            return context.Properties;
        }

        // GET: api/OwnerProperty/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OwnerProperty
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/OwnerProperty/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/OwnerProperty/5
        public void Delete(int id)
        {
        }
    }
}
