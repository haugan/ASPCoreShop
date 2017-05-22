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
    public class OrderItemsController : Controller
    {
        private readonly ApplicationDbContext _ctx;

        public OrderItemsController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        private bool OrderItemExists(int id)
        {
            return _ctx.OrderItems.Any(e => e.OrderItemID == id);
        }

        // GET: OrderItems/Create
        public IActionResult Create()
        {
            ViewData["OrderNumber"] = new SelectList(_ctx.Orders, "OrderID", "OrderNumber");
            ViewData["ProductNumber"] = new SelectList(_ctx.Products, "ProductID", "ProductNumber");
            return View();
        }

        // GET: OrderItems
        public async Task<IActionResult> Index(string sortOrder, string filter, string search, int? pageIndex)
        {
            ViewData["SortOrder"] = sortOrder; // SORT ORDER WHILE NAVIGATING "NEXT/PREVIOUS" PAGES
            ViewData["SortOrderNumber"] = String.IsNullOrEmpty(sortOrder) ? "ordnum_desc" : "";
            ViewData["SortProductNumber"] = (sortOrder == "prodnum_asc") ? "prodnum_desc" : "prodnum_asc";
            ViewData["SortPrice"] = (sortOrder == "price_asc") ? "price_desc" : "price_asc";
            ViewData["SortQuantity"] = (sortOrder == "quantity_asc") ? "quantity_desc" : "quantity_asc";

            if (search != null)
            {
                pageIndex = 1; // NEW SEARCH LEADS TO NEW RESULTS, RESET PAGE TO 1
            }
            else
            {
                search = filter;
            }

            ViewData["SearchFilter"] = search;

            var query = _ctx.OrderItems
                            .Include(o => o.Order)
                            .Include(o => o.Product);

            var orderItems = from orderItem in query select orderItem;

            if (!String.IsNullOrEmpty(filter))
            {
                orderItems = orderItems.Where(o => o.Order.OrderNumber.ToString().Contains(filter) || 
                                                   o.Product.ProductNumber.ToString().Contains(filter));
            }

            switch (sortOrder)
            {
                case "ordnum_desc":
                    orderItems = orderItems.OrderByDescending(c => c.Order.OrderNumber);
                    break;
                case "prodnum_asc":
                    orderItems = orderItems.OrderBy(c => c.Product.ProductNumber);
                    break;
                case "prodnum_desc":
                    orderItems = orderItems.OrderByDescending(c => c.Product.ProductNumber);
                    break;
                case "price_asc":
                    orderItems = orderItems.OrderBy(c => c.Product.Price);
                    break;
                case "price_desc":
                    orderItems = orderItems.OrderByDescending(c => c.Product.Price);
                    break;
                case "quantity_asc":
                    orderItems = orderItems.OrderBy(c => c.Quantity);
                    break;
                case "quantity_desc":
                    orderItems = orderItems.OrderByDescending(c => c.Quantity);
                    break;
                default:
                    orderItems = orderItems.OrderBy(c => c.Order.OrderNumber);
                    break;
            }

            int itemsOnPage = 3;

            // CONVERT QUERY TO SINGLE PAGE OF ITEMS IN COLLECTION THAT SUPPORTS PAGING
            return View(await PageList<OrderItem>.CreateAsync(orderItems.AsNoTracking(),
                                                              pageIndex ?? 1, // IF NULL, RETURN 1 - ELSE RETURN PAGEINDEX VALUE
                                                              itemsOnPage));

            //return View(await orderItems.AsNoTracking().ToListAsync());
        }

        // GET: OrderItems/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _ctx.OrderItems
                                      .Include(o => o.Order)
                                      .Include(o => o.Product)
                                      .SingleOrDefaultAsync(m => m.OrderItemID == id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // GET: OrderItems/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _ctx.OrderItems.SingleOrDefaultAsync(m => m.OrderItemID == id);
            if (orderItem == null)
            {
                return NotFound();
            }
            ViewData["OrderNumber"] = new SelectList(_ctx.Orders, "OrderID", "OrderNumber", orderItem.OrderID);
            ViewData["ProductNumber"] = new SelectList(_ctx.Products, "ProductID", "ProductNumber", orderItem.ProductID);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/
        public async Task<IActionResult> Delete(int? id, bool? saveFailed = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _ctx.OrderItems
                                      .Include(o => o.Order)
                                      .Include(o => o.Product)
                                      .AsNoTracking()
                                      .SingleOrDefaultAsync(m => m.OrderItemID == id);

            if (orderItem == null)
            {
                return NotFound();
            }

            if (saveFailed.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Something went wrong! Couldn't delete order item from database.";
            }

            return View(orderItem);
        }

        // POST: OrderItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderItemID,OrderID,ProductID,Quantity")] OrderItem orderItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ctx.Add(orderItem);
                    await _ctx.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(
                    "DbUpdateException",
                    $"Unable to save Order Item model changes because of a database update exception! {ex.Message}");
            }

            ViewData["OrderNumber"] = new SelectList(_ctx.Orders, "OrderID", "OrderNumber", orderItem.OrderID);
            ViewData["ProductNumber"] = new SelectList(_ctx.Products, "ProductID", "ProductNumber", orderItem.ProductID);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderItemID,OrderID,ProductID,Quantity")] OrderItem orderItem)
        {
            if (id != orderItem.OrderItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ctx.Update(orderItem);
                    await _ctx.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.OrderItemID))
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

            ViewData["OrderNumber"] = new SelectList(_ctx.Orders, "OrderID", "OrderNumber", orderItem.OrderID);
            ViewData["ProductNumber"] = new SelectList(_ctx.Products, "ProductID", "ProductNumber", orderItem.ProductID);
            return View(orderItem);
        }

        // POST: OrderItems/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItems = await _ctx.OrderItems
                                       .AsNoTracking()
                                       .SingleOrDefaultAsync(i => i.OrderItemID == id);
            if (orderItems == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                _ctx.OrderItems.Remove(orderItems);
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
