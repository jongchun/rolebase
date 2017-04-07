using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UsersAndRolesDemo.Models
{
    public class IPN
    {
        [Key]
        [Display(Name = "Transaction ID")]
        public string transactionID { get; set; }
        [Display(Name = "Transaction Time (UTC)")]
        public DateTime txTime { get; set; }
        [Display(Name = "Session ID")]
        public string custom { get; set; }
        [Display(Name = "First Name")]
        public string fname { get; set; }
        [Display(Name = "Last Name")]
        public string lname { get; set; }
        [Display(Name = "Buyer Email")]
        public string buyerEmail { get; set; }
        [Display(Name = "Total Reservations")]
        public string quantity { get; set; }
        [Display(Name = "Transaction Amount")]
        [DataType(DataType.Currency)]
        public decimal amount { get; set; }
        [Display(Name = "Payment Status")]
        public string paymentStatus { get; set; }

    }
}