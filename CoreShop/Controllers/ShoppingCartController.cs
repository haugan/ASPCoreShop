using CoreShop.Data;
using CoreShop.Models;
using CoreShop.Models.StoreViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        private readonly ShoppingCart _cart;

        public ShoppingCartController(ApplicationDbContext ctx, ShoppingCart cart)
        {
            _ctx = ctx;
            _cart = cart;
        }

        // SHOW ALL ITEMS IN SHOPPING CART
        public ViewResult Index()
        {
            var currentCartItems = _cart.GetItems();
            _cart.Items = currentCartItems;

            var cartVM = new ShoppingCartTotalGroup
            {
                Cart = _cart,
                CartTotal = _cart.GetTotal()
            };

            return View(cartVM);
        }

        // ADD CHOSEN PRODUCT TO SHOPPING CART
        public RedirectToActionResult AddToCart(int id)
        {
            var product = _ctx.Products.FirstOrDefault(p => p.ProductID == id);

            if (product != null)
            {
                _cart.AddItem(product, 1);
            }

            return RedirectToAction("Index");
        }

        // REMOVE CHOSEN PRODUCT FROM SHOPPING CART
        public RedirectToActionResult RemoveFromCart(int id)
        {
            var product = _ctx.Products
                              .FirstOrDefault(p => p.ProductID == id);

            if (product != null)
            {
                _cart.RemoveItem(product);
            }

            return RedirectToAction("Index");
        }

        // REMOVE ALL PRODUCTS FROM SHOPPING CART
        public RedirectToActionResult Clear()
        {
            var queryResult = from rows in _ctx.OrderItems select rows;
            foreach (var row in queryResult)
            {
                _ctx.OrderItems.Remove(row);
            }

            _cart.Clear();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout()
        {
            // GET THE CUSTOMER ID
            var cartID = _cart.GUID.ToString();

            // CREATE RANDOM ORDER #
            var orderNumber = "OR-" + Path.GetRandomFileName().Replace(".", "").Substring(0, 6).ToUpper();

            // ADD NEW ORDER TO DATABASE
            _ctx.Orders.Add(new Order
            {
                CustomerID = 0, // get customer id
                OrderDate = DateTime.Today,
                OrderNumber = orderNumber,
                Status = Status.PENDING
            });
            await _ctx.SaveChangesAsync();

            // GET ITEMS (PRODUCT + QUANTITY) FROM SHOPPING CART
            var cartItems = _cart.GetItems();

            // GET ORDER ID

            // CREATE ORDER ITEMS
            foreach (var item in cartItems)
            {
                _ctx.OrderItems.Add(new OrderItem
                {
                    OrderID = 0, // get order id
                    ProductID = item.ProductID, 
                    Quantity = item.Quantity 
                });
                await _ctx.SaveChangesAsync();
            }

            return View();
        }
    }
}