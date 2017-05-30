using System.Collections.Generic;

namespace CoreShop.Models.ShoppingCartViewModel
{
    public class CartTotalGroup
    {
        public ShoppingCart Cart { get; set; }
        public int TotalProductsQuantity { get; set; }
    }
}
