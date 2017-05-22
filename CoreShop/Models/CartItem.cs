using System.ComponentModel.DataAnnotations;

namespace CoreShop.Models
{
    public class CartItem
    {
        [Display(Name="ID")]
        public int CartItemID { get; set; } // PRIMARY KEY
        public int ProductID { get; set; } // FOREIGN KEY

        public string ShoppingCartID { get; set; } // CART GUID

        [Required]
        [Range(1, 999)]
        public int Quantity { get; set; }

        public Product Product { get; set; } // A CART ITEM HOLDS ONE PRODUCT (PRICE CALCULATED FROM ITS PRICE * THE QUANTITY)
    }
}
