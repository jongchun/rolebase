using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UsersAndRolesDemo.Models
{
    public class PostPropertyVM
    {
  
        [Required]
        [Display(Name = "Summary")]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [Required]
        [Display(Name = "Property Type")]
        public string PropertyType { get; set; }

        [Required]
        [Display(Name = "Number of Bedrooms")]
        public int NumBedrooms { get; set; }    

        [Required]
        [Display(Name = "Number of Washrooms")]
        public int NumWashrooms { get; set; }

        [Required]
        [Display(Name = "Number of Kitchens")]
        public int Kitchen { get; set; }

        [Required]
        [Display(Name = "Base Rate Per Day")]
        public decimal BaseRate { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Built Year")]
        public string BuiltYear { get; set; }

        [Required]
        [Display(Name = "Smoking Allowed")]
        public string SmokingAllowed { get; set; }

        [Required]
        [Display(Name = "max Number of Guests")]
        public string MaxNumberGuests { get; set; }

        [Required]
        [Display(Name = "Available Dates")]
        public DateTime AvailableDates { get; set; }

        [Required]
        [Display(Name = "Dimensions")]
        public string Dimensions { get; set; }
    }
}