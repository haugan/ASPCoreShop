using System.ComponentModel.DataAnnotations;

namespace CoreShop.Models
{
    public class OrderItem
    {
        [Display(Name = "ID")]
        public int OrderItemID { get; set; } // PRIMARY KEY
        public int OrderID { get; set; } // FOREIGN KEY
        public int ProductID { get; set; } // FOREIGN KEY

        [Required]
        [Range(1,999)]
        public int Quantity { get; set; }

        public Order Order { get; set; } // EACH ORDER ITEM BELONGS TO AN ORDER
        public Product Product { get; set; } // ONE ORDER ITEM CAN HAVE A SINGLE PRODUCT
    }
}
