using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UsersAndRolesDemo.Models {
    public class LoginVM {
        [Required]
        [Display(Name = "User Name")]
        public string  UserName       { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password        { get; set; }
    }

}