using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UsersAndRolesDemo.Models
{
    public class ReservationVM
    {
        public int? Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string RenterFirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string RenterLastName { get; set; }

        [Required]
        [Display(Name = "Reservation Start")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Reservation End")]
        public DateTime EndDate { get; set; }

        //[Required]
        //[Display(Name = "Base Rate Per Day")]
        //public decimal BaseRate { get; set; }

        //[Display(Name = "Property Name")]
        public string PropertyName { get; set; }

        //public string Email { get; set; }

        public decimal? TotalMoney { get; set; }
    }
}