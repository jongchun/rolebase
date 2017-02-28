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
    }
}
    