using CoreShop.Data;
using CoreShop.Models;
using CoreShop.Models.ShoppingCartViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

/*

    Denne klassen bør oppdatere Order, OrderItem, 
    hver gang et produkt legges til eller fjernes fra ShoppingCart.

    Når Checkout trykkes i index-View - bør Ordre lagres i database.
 
*/

namespace CoreShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        private readonly ShoppingCart _cart;

        private Customer _customer;
        private Order _order;
        private OrderItem _orderItem;

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

            var cartVM = new CartTotalViewModel
            {
                Cart = _cart,
                CartTotal = _cart.GetTotal()
            };

            return View(cartVM);
        }

        // ADD CHOSEN PRODUCT TO SHOPPING CART
        public async Task<RedirectToActionResult> AddToCart(int id)
        {
            var product = _ctx.Products
                              .FirstOrDefault(p => p.ProductID == id);

            if (product != null)
            {
                _cart.AddItem(product, 1);

                // CREATION OF CUSTOMER, ORDER, AND ORDER ITEM DUMMY DATA
                //await CreateNewCustomer();
                //await CreateNewOrder();
                //await CreateNewOrderItem(product, 1);
                //await UpdateOrderItemQuantity(product, 1);
            }

            return RedirectToAction("Index");
        }

        private async Task CreateNewCustomer()
        {
            _customer = new Customer
            {
                CustomerNumber = "CR-0000",
                Firstname = "xxxx",
                Lastname = "xxxx",
                Email = "xxx@xxx.net"
            };
            _ctx.Customers.Add(_customer);
            await _ctx.SaveChangesAsync();
        }

        private async Task CreateNewOrder()
        {
            _order = new Order
            {
                CustomerID = _customer.CustomerID,
                OrderNumber = "OR-0000",
                OrderDate = DateTime.Today,
                Status = Status.PENDING
            };
            _ctx.Orders.Add(_order);
            await _ctx.SaveChangesAsync();
        }

        private async Task CreateNewOrderItem(Product product, int quantity)
        {
            _orderItem = new OrderItem
            {
                OrderID = _order.OrderID,
                ProductID = product.ProductID,
                Quantity = quantity
            };
            _ctx.OrderItems.Add(_orderItem);
            await _ctx.SaveChangesAsync();
        }

        private async Task UpdateOrderItemQuantity(Product product, int quantity)
        {
            _orderItem.Quantity = quantity;
            _ctx.OrderItems.Update(_orderItem);
            await _ctx.SaveChangesAsync();
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
    }
}