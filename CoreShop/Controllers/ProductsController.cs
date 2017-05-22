using CoreShop.Data;
using CoreShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _ctx;

        public ProductsController(ApplicationDbContext ctx)
        {
            _ctx = ctx;    
        }

        private bool ProductExists(int id)
        {
            return _ctx.Products.Any(e => e.ProductID == id);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Products
        public async Task<IActionResult> Index(string sortOrder, string filter, string search, int? pageIndex)
        {
            ViewData["SortOrder"] = sortOrder; // SORT ORDER WHILE NAVIGATING "NEXT/PREVIOUS" PAGES
            ViewData["SortProductNumber"] = String.IsNullOrEmpty(sortOrder) ? "prodnum_desc" : "";
            ViewData["SortPrice"] = (sortOrder == "price_asc") ? "price_desc" : "price_asc";
            ViewData["SortName"] = (sortOrder == "name_asc") ? "name_desc" : "name_asc";

            if (search != null)
            {
                pageIndex = 1; // NEW SEARCH LEADS TO NEW RESULTS, RESET PAGE TO 1
            }
            else
            {
                search = filter;
            }

            ViewData["SearchFilter"] = search;

            var query = from product in _ctx.Products select product;

            if (!String.IsNullOrEmpty(filter))
            {
                query = query.Where(p => p.ProductNumber.ToString().Contains(filter) || p.Name.Contains(filter));
            }

            switch (sortOrder)
            {
                case "prodnum_desc":
                    query = query.OrderByDescending(p => p.ProductNumber);
                    break;
                case "price_asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case "name_asc":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(p => p.Name);
                    break;
                default:
                    query = query.OrderBy(p => p.ProductNumber);
                    break;
            }

            int itemsOnPage = 3;

            // CONVERT QUERY TO SINGLE PAGE OF ITEMS IN COLLECTION THAT SUPPORTS PAGING
            return View(await PageList<Product>.CreateAsync(query.AsNoTracking(),
                                                            pageIndex ?? 1, // IF NULL, RETURN 1 - ELSE RETURN PAGEINDEX VALUE
                                                            itemsOnPage));
        }

        // GET: Products/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _ctx.Products
                                    .SingleOrDefaultAsync(m => m.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _ctx.Products.SingleOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        
        // GET: Products/Delete/
        public async Task<IActionResult> Delete(int? id, bool? saveFailed = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _ctx.Products
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(m => m.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            if (saveFailed.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Something went wrong! Couldn't delete product from database.";
            }

            return View(product);
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductNumber,Price,OnSale,Name,ImgURL1,ImgURL2,ImgURL3,DescriptionShort,DescriptionLong")] Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ctx.Add(product);
                    await _ctx.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(
                    "DbUpdateException",
                    $"Unable to save Product model changes because of a database update exception! {ex.Message}");
            }

            return View(product);
        }

        // POST: Products/Edit/
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _ctx.Products.SingleOrDefaultAsync(p => p.ProductID == id);

            if (await TryUpdateModelAsync<Product>(
                product,
                "",
                p => p.ProductNumber,
                p => p.Price,
                p => p.OnSale,
                p => p.Name,
                p => p.ImgURL1,
                p => p.ImgURL2,
                p => p.ImgURL3,
                p => p.DescriptionShort,
                p => p.DescriptionLong))
            {
                try
                {
                    await _ctx.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(
                    "DbUpdateException",
                    $"Unable to update Product model because of a database update exception! {ex.Message}");
                }
            }

            return View(product);
        }

        // POST: Products/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _ctx.Products
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(p => p.ProductID == id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                _ctx.Products.Remove(product);
                await _ctx.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                System.Console.WriteLine($"Database update exception! {ex.Message}");
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }
    }
}
