using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UsersAndRolesDemo.Models
{
    public class PostPropertyVM
    {
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Summary")]
        public string Summary { get; set; }

        [Required]
        [Display(Name = "Property Type")]
        public string PropertyType { get; set; }

        [Required]
        [Range(1, 10,ErrorMessage ="Invalid. Between 1~10 please")]
        [Display(Name = "Number of Bedrooms")]
        public int NumBedrooms { get; set; }    

        [Required]
        [Range(1, 10, ErrorMessage = "Invalid. Between 1~10 please")]
        [Display(Name = "Number of Washrooms")]
        public int NumWashrooms { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Invalid. Between 1~10 please")]
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
        [Display(Name = "Max Guests")]
        public string MaxNumberGuests { get; set; }

        
        [Display(Name = "Available Dates")]
        public DateTime? AvailableDates { get; set; }

        [Required]
        [Display(Name = "Dimensions")]
        public string Dimensions { get; set; }
    }
}
