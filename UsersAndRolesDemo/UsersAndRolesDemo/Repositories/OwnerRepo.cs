using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using UsersAndRolesDemo.Models;

namespace UsersAndRolesDemo.Repositories
{
    public class OwnerRepo
    {
        private MyDbEntities db = new MyDbEntities();

        public Boolean Property(PostPropertyVM property, string username)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager
            = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.FindByName(username);

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
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public PostPropertyVM GetProperty(int id)
        {
            Property property = db.Properties.Find(id);
           

            PostPropertyVM propertyVM = new PostPropertyVM();
            propertyVM.PropertyType = property.propertyType;
            propertyVM.Summary = property.summary;
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

            return propertyVM;
        }


        public Boolean EditProperty(PostPropertyVM property, int id)
        {
           // Property property = db.Properties.Find(id);
            AspNetUser user = db.AspNetUsers
                       .Where(a => a.Id == property.i).FirstOrDefault();

            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            return false;
        }
    }
}
    