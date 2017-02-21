using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAndRolesDemo.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string summary { get; set; }
        public string propertyType { get; set; }
        public Nullable<int> numBedrooms { get; set; }
        public Nullable<int> numWashrooms { get; set; }
        public Nullable<int> kitchen { get; set; }
        public Nullable<decimal> baseRate { get; set; }
        public string address { get; set; }
        public string builtYear { get; set; }
        public string smokingAllowed { get; set; }
        public string maxNumberGuests { get; set; }
        public Nullable<System.DateTime> availableDates { get; set; }
        public string dimensions { get; set; }
    }
}