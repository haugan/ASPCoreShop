using CoreShop.Models;
using CoreShop.Models.ShoppingCartViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CoreShop.Components
{
    public class ShoppingCartItemsCount : ViewComponent
    {
        private readonly ShoppingCart _cart;

        public ShoppingCartItemsCount(ShoppingCart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            var cartItems = _cart.GetItems();
            _cart.Items = cartItems;

            var quantity = 0;
            foreach (var item in cartItems)
            {
                quantity += item.Quantity;
            }

            var shoppingCartViewModel = new CartTotalGroup
            {
                Cart = _cart,
                TotalProductsQuantity = quantity
            };

            return View(shoppingCartViewModel);
        }
    }
}
