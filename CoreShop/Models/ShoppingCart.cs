using CoreShop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShop.Models
{
    public class ShoppingCart
    {
        public string GUID { get; set; }
        private readonly ApplicationDbContext _ctx; // representing the Database
        public List<CartItem> Items { get; set; }

        private ShoppingCart(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                                       .HttpContext
                                       .Session;

            var ctxService = services.GetService<ApplicationDbContext>();
            string newID = session.GetString("ShoppingCartID") ?? Guid.NewGuid().ToString();

            session.SetString("ShoppingCartID", newID);

            return new ShoppingCart(ctxService) { GUID = newID };
        }

        // ADD CHOSEN PRODUCT TO SHOPPING CART
        public void AddItem(Product product, int quantity)
        {
            var cartItem = _ctx.CartItems
                               .SingleOrDefault(i =>
                                                i.ProductID == product.ProductID &&
                                                i.ShoppingCartID == GUID);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ShoppingCartID = GUID,
                    Product = product,
                    Quantity = 1
                };

                _ctx.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            _ctx.SaveChanges();
        }

        // REMOVE CHOSEN PRODUCT FROM SHOPPING CART
        public int RemoveItem(Product product)
        {
            var item = _ctx.CartItems
                          .SingleOrDefault(i =>
                                           i.ProductID == product.ProductID &&
                                           i.ShoppingCartID == GUID);
            var cartAmount = 0;
            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                    cartAmount = item.Quantity;
                }
                else
                {
                    _ctx.CartItems.Remove(item);
                }
            }

            _ctx.SaveChanges();
            return cartAmount;
        }

        // GET ALL ITEMS FROM SHOPPING CART
        public List<CartItem> GetItems()
        {
            return Items ?? (Items = _ctx.CartItems
                                                 .Where(i => i.ShoppingCartID == GUID)
                                                 .Include(i => i.Product)
                                                 .ToList());
        }

        // CLEAR SHOPPING CART FOR ITEMS
        public void Clear()
        {
            var items = _ctx.CartItems
                            .Where(cart =>
                                   cart.ShoppingCartID == GUID);

            _ctx.CartItems.RemoveRange(items);
            _ctx.SaveChanges();
        }

        // GET TOTAL COST OF ITEMS IN SHOPPING CART
        public decimal GetTotal()
        {
            var total = _ctx.CartItems
                                  .Where(i => i.ShoppingCartID == GUID)
                                  .Select(i => i.Product.Price * i.Quantity)
                                  .Sum();
            return total;
        }
    }
}
