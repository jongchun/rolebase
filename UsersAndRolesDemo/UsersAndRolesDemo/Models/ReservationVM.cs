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
        public string StartDate { get; set; }

        [Required]
        [Display(Name = "Reservation End")]
        public int EndDate { get; set; }

        //[Required]
        //[Display(Name = "Base Rate Per Day")]
        //public decimal BaseRate { get; set; }
    }
}