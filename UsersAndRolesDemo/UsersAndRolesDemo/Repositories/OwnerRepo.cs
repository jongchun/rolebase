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

        public Boolean UpdateOwner(OwnerProfileVM model)
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
                if (model.ProfilePicture != null)
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
                user.directDepositRouting = model.DirectDepositRouting;
                user.directDepositBank = model.DirectDepositBank;
                user.directDepositAccount = model.DirectDepositAccount;

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
    }
}
    