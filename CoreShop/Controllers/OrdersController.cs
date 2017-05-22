using CoreShop.Data;
using CoreShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        private readonly ShoppingCart _cart;

        public OrdersController(ApplicationDbContext ctx, ShoppingCart cart)
        {
            _ctx = ctx;
            _cart = cart;
        }

        private bool OrderExists(int id)
        {
            return _ctx.Orders.Any(e => e.OrderID == id);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerNumber"] = new SelectList(_ctx.Customers, "CustomerID", "CustomerNumber");
            return View();
        }

        // GET: Orders
        public async Task<IActionResult> Index(string sortOrder, string filter, string search, int? pageIndex)
        {
            ViewData["SortOrder"] = sortOrder; // SORT ORDER WHILE NAVIGATING "NEXT/PREVIOUS" PAGES
            ViewData["SortOrderNumber"] = String.IsNullOrEmpty(sortOrder) ? "ordnum_desc" : "";
            ViewData["SortCustomerNumber"] = (sortOrder == "cusnum_asc") ? "cusnum_desc" : "cusnum_asc";
            ViewData["SortOrderDate"] = (sortOrder == "date_asc") ? "date_desc" : "date_asc";
            ViewData["SortStatus"] = (sortOrder == "status_asc") ? "status_desc" : "status_asc";

            if (search != null)
            {
                pageIndex = 1; // NEW SEARCH LEADS TO NEW RESULTS, RESET PAGE TO 1
            }
            else
            {
                search = filter;
            }

            ViewData["SearchFilter"] = search;

            var query = _ctx.Orders
                            .Include(i => i.Customer)
                            .Include(i => i.OrderItems)
                            .ThenInclude(i => i.Product);

            var orders = from order in query select order;

            if (!String.IsNullOrEmpty(filter))
            {
                orders = orders.Where(o => o.OrderNumber.ToString().Contains(filter) || 
                                           o.Customer.CustomerNumber.ToString().Contains(filter) || 
                                           o.Status.ToString().Contains(filter.ToUpper()));
            }

            switch (sortOrder)
            {
                case "ordnum_desc":
                    orders = orders.OrderByDescending(c => c.OrderNumber);
                    break;
                case "cusnum_asc":
                    orders = orders.OrderBy(c => c.Customer.CustomerNumber);
                    break;
                case "cusnum_desc":
                    orders = orders.OrderByDescending(c => c.Customer.CustomerNumber);
                    break;
                case "date_asc":
                    orders = orders.OrderBy(c => c.OrderDate);
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(c => c.OrderDate);
                    break;
                case "status_asc":
                    orders = orders.OrderBy(c => c.Status);
                    break;
                case "status_desc":
                    orders = orders.OrderByDescending(c => c.Status);
                    break;
                default:
                    orders = orders.OrderBy(c => c.OrderNumber);
                    break;
            }

            int itemsOnPage = 3;

            // CONVERT QUERY TO SINGLE PAGE OF ITEMS IN COLLECTION THAT SUPPORTS PAGING
            return View(await PageList<Order>.CreateAsync(orders.AsNoTracking(),
                                                          pageIndex ?? 1, // IF NULL, RETURN 1 - ELSE RETURN PAGEINDEX VALUE
                                                          itemsOnPage));
        }

        // GET: Orders/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var order = await _ctx.Orders.Include(o => o.Customer).SingleOrDefaultAsync(m => m.OrderID == id);
            var order = await _ctx.Orders
                                  .Include(o => o.Customer)
                                  .Include(o => o.OrderItems)
                                  .ThenInclude(o => o.Product)
                                  .AsNoTracking()
                                  .SingleOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _ctx.Orders.SingleOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["CustomerNumber"] = new SelectList(_ctx.Customers, "CustomerID", "CustomerNumber", order.CustomerID);
            return View(order);
        }

        // GET: Orders/Delete/
        public async Task<IActionResult> Delete(int? id, bool? saveFailed = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _ctx.Orders
                                  .Include(o => o.Customer)
                                  .Include(o => o.OrderItems)
                                  .AsNoTracking()
                                  .SingleOrDefaultAsync(m => m.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            if (saveFailed.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Something went wrong! Couldn't delete order from database.";
            }

            return View(order);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,CustomerID,OrderNumber,OrderDate,Status")] Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ctx.Add(order);
                    await _ctx.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(
                    "DbUpdateException",
                    $"Unable to save Order model changes because of a database update exception! {ex.Message}");
            }

            ViewData["CustomerNumber"] = new SelectList(_ctx.Customers, "CustomerID", "CustomerNumber", order.CustomerID);
            return View(order);
        }

        // POST: Orders/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,CustomerID,OrderNumber,OrderDate,Status")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ctx.Update(order);
                    await _ctx.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index");
            }

            ViewData["CustomerNumber"] = new SelectList(_ctx.Customers, "CustomerID", "CustomerNumber", order.CustomerID);
            return View(order);
        }

        // POST: Orders/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _ctx.Orders
                                  .AsNoTracking()
                                  .SingleOrDefaultAsync(o => o.OrderID == id);
            if (order == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                _ctx.Orders.Remove(order);
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
