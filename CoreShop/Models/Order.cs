using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreShop.Models
{
    public enum Status
    {
        PENDING, CANCELED, PAYED, SHIPPING, DELIVERED
    }

    public class Order
    {
        [Display(Name="ID")]
        public int OrderID { get; set; } // PRIMARY KEY
        public int CustomerID { get; set; } // FOREIGN KEY

        [Required]
        [Display(Name="Order #")]
        public string OrderNumber { get; set; }

        [Required]
        [Display(Name="Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}", ApplyFormatInEditMode=true)]
        public DateTime OrderDate { get; set; }

        [Required]
        public Status? Status { get; set; } 

        public ICollection<OrderItem> OrderItems { get; set; } // ONE ORDER CAN INCLUDE SEVERAL ITEMS
        public Customer Customer { get; set; } // AN ORDER IS MADE BY A SINGLE CUSTOMER
    }
}
