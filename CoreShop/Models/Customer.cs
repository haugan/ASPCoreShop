using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreShop.Models
{
    public class Customer
    {
        [Display(Name="ID")]
        public int CustomerID { get; set; } // PRIMARY KEY

        [Required]
        [Display(Name="Customer #")]
        public string CustomerNumber { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Email { get; set; }

        public ICollection<Order> Orders { get; set; } // ONE CUSTOMER CAN HAVE SEVERAL ORDERS
    }
}
