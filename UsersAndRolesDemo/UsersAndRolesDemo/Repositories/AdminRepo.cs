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

namespace UsersAndRolesDemo.Repositories
{
    public class AdminRepo
    {
        private MyDbEntities db = new MyDbEntities();

        public AdminProfileVM GetAdmin(string name)
        {
            AspNetUser user = db.AspNetUsers
                        .Where(a => a.UserName == name).FirstOrDefault();

            AdminProfileVM model = new AdminProfileVM();
            model.Id = user.Id;
            model.ProfilePicture = user.profilePicture;
            model.UserName = user.UserName;
            model.FirstName = user.firstName;
            model.LastName = user.lastName;
            model.Email = user.Email;
            model.PhoneNumber = user.PhoneNumber;
            model.CellPhone = user.cellPhone;
            model.Address = user.address;
            model.City = user.city;
            model.Region = user.region;
            model.PostalCode = user.postalCode;
            model.HireDate = user.hireDate;

            return model;
        }

        public Boolean UpdateAdmin(AdminProfileVM model)
        {
            AspNetUser user = db.AspNetUsers
                        .Where(a => a.Id == model.Id).FirstOrDefault();

            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);

            IdentityResult result = null;
            if (model.CurrentPassword != null && model.Password != null && model.ConfirmPassword != null)
            {
                result = manager.ChangePassword(model.Id, model.CurrentPassword, model.Password);
            }
            if (result == null || result.Succeeded)
            {
                if (user.profilePicture != null)
                {
                    user.profilePicture = model.ProfilePicture;
                }
                user.UserName = model.UserName;
                user.firstName = model.FirstName;
                user.lastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.cellPhone = model.CellPhone;
                user.address = model.Address;
                user.city = model.City;
                user.region = model.Region;
                user.postalCode = model.PostalCode;
                //user.hireDate = model.HireDate;

                db.Entry(user).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public OwnerProfileVM GetOwner(string name)
        {
            AspNetUser user = db.AspNetUsers
                        .Where(a => a.UserName == name).FirstOrDefault();

            OwnerProfileVM model = new OwnerProfileVM();
            model.Id = user.Id;
            model.ProfilePicture = user.profilePicture;
            model.UserName = user.UserName;
            model.FirstName = user.firstName;
            model.LastName = user.lastName;
            model.Email = user.Email;
            model.PhoneNumber = user.PhoneNumber;
            model.CellPhone = user.cellPhone;
            model.Address = user.address;
            model.City = user.city;
            model.Region = user.region;
            model.PostalCode = user.postalCode;
            model.DirectDepositRouting = user.directDepositRouting;
            model.DirectDepositBank = user.directDepositBank;
            model.DirectDepositAccount = user.directDepositAccount;

            return model;
        }
    }
}