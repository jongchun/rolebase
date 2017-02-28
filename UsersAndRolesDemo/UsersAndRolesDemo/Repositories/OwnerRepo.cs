using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UsersAndRolesDemo.Models;

namespace UsersAndRolesDemo.Repositories
{
    public class OwnerRepo
    {
        private MyDbEntities db = new MyDbEntities();

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