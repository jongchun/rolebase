using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UsersAndRolesDemo.Models
{
    public class OwnerProfileVM
    {
        [Display(Name = "Profile Picture")]
        public byte[] ProfilePicture { get; set; }

        [Required]
        [Editable(false)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirm")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Cellphone Number")]
        public string CellPhone { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Province")]
        public string Region { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        [RegularExpression(@"^([ABCEGHJKLMNPRSTVXYabceghjklmnprstvxy]\d[ABCEGHJKLMNPRSTVW‌​XYZabceghjklmnprstvx‌​y])\ {0,1}(\d[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvxy]\d)$",
        ErrorMessage = "This is not a valid canadian postal code.")]
        public string PostalCode { get; set; }

        [Display(Name = "Routing Number")]
        public string DirectDepositRouting { get; set; }

        [Display(Name = "Bank Number")]
        public string DirectDepositBank { get; set; }

        [Display(Name = "Account Number")]
        public string DirectDepositAccount { get; set; }
    }
}