using CoreShop.Data;
using CoreShop.Models.ShoppingCartViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CoreShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _ctx;

        public HomeController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public ViewResult Index()
        {
            var viewmodel = new ProductsOnSaleViewModel
            {
                ProductsOnSale = _ctx.Products
                                     .Where(p => p.OnSale)
                                     .OrderBy(p => p.Name)
            };

            return View(viewmodel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
