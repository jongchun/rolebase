using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using UsersAndRolesDemo;
using UsersAndRolesDemo.Models;

namespace UsersAndRolesDemo.Repositories
{
    public class OwnerRepo
    {
        private MyDbEntities db = new MyDbEntities();

        public Boolean PostProperty(PostPropertyVM property, string username, List<string> imageList)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager
            = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.FindByName(username);

            var images = new PropertyImage()
            {
                main = imageList[0],
                bedroom = imageList[1],
                livingroom = imageList[2],
                bathroom = imageList[3],
                kitchen = imageList[4]
            };

            var unit = new Property()
            {
                UserId = identityUser.Id,
                title = property.Title,
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
            unit.PropertyImages.Add(images); 
            db.Properties.Add(unit);

            
            
            //db.PropertyImages.Add(images);
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
            propertyVM.Id = id;
            propertyVM.Title = property.title;
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
            propertyVM.AvailableDates = property.availableDates;
            propertyVM.Dimensions = property.dimensions;

            return propertyVM;
        }
        /*
        public List<string> GetImages(int id)
        {
            List<string> images = new List<string>();


            return images;
        }
        */
        public Boolean EditProperty(PostPropertyVM property, string username, List<string> imageList)
        {/*
           // Property property = db.Properties.Find(id);
            AspNetUser user = db.AspNetUsers
                       .Where(a => a.Id == property.i).FirstOrDefault();

            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);*/

            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.FindByName(username);

            PropertyImage propertyimage = new PropertyImage();
            var temp = db.PropertyImages.Where(i => i.PropertyId == property.Id).FirstOrDefault();
            if (imageList[0] != null)
            {
                temp.main = imageList[0];
            }
            if (imageList[1] != null)
            {
                temp.bedroom = imageList[1];
            }
            if (imageList[2] != null)
            {
                temp.livingroom = imageList[2];
            }
            if (imageList[3] != null)
            {
                temp.bathroom = imageList[3];
            }
            if (imageList[4] != null)
            {
                temp.kitchen = imageList[4];
            }

            Property test = new Property
            {
                Id = (int)property.Id,
                UserId = identityUser.Id,
                title = property.Title,
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

            db.Entry(test).State = EntityState.Modified;
            db.Entry(temp).State = EntityState.Modified;

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

        public Boolean DeleteProperty(int id)
        {
            PropertyImage image = db.PropertyImages.Where(i => i.PropertyId == id).FirstOrDefault();
            Property property = db.Properties.Find(id);
            db.PropertyImages.Remove(image);
            db.Properties.Remove(property);
            try
            {
                db.SaveChanges();
            }catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
    