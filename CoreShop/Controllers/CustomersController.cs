using CoreShop.Data;
using CoreShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShop.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _ctx;

        public CustomersController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        private bool CustomerExists(int id)
        {
            return _ctx.Customers.Any(e => e.CustomerID == id);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Customers
        public async Task<IActionResult> Index(string sortOrder, string filter, string search, int? pageIndex)
        {
            ViewData["SortOrder"] = sortOrder; // SORT ORDER WHILE NAVIGATING "NEXT/PREVIOUS" PAGES
            ViewData["SortCustomerNumber"] = String.IsNullOrEmpty(sortOrder) ? "cusnum_desc" : "";
            ViewData["SortFirstname"] = (sortOrder == "first_asc") ? "first_desc" : "first_asc";
            ViewData["SortLastname"] = (sortOrder == "last_asc") ? "last_desc" : "last_asc";

            if (search != null)
            {
                pageIndex = 1; // NEW SEARCH LEADS TO NEW RESULTS, RESET PAGE TO 1
            }
            else
            {
                search = filter;
            }

            ViewData["SearchFilter"] = search;  // KEEP SEARCH FILTER WHILE "PAGING"

            var query = _ctx.Customers
                            .Include(i => i.Orders);

            var customers = from customer in query select customer;

            if (!String.IsNullOrEmpty(filter))
            {
                customers = customers.Where(c => c.CustomerNumber.ToString().Contains(filter) || 
                                                 c.Firstname.Contains(filter) || 
                                                 c.Lastname.Contains(filter));
            }

            switch (sortOrder)
            {
                case "cusnum_desc":
                    customers = customers.OrderByDescending(c => c.CustomerNumber);
                    break;
                case "first_asc":
                    customers = customers.OrderBy(c => c.Firstname);
                    break;
                case "first_desc":
                    customers = customers.OrderByDescending(c => c.Firstname);
                    break;
                case "last_asc":
                    customers = customers.OrderBy(c => c.Lastname);
                    break;
                case "last_desc":
                    customers = customers.OrderByDescending(c => c.Lastname);
                    break;
                default:
                    customers = customers.OrderBy(c => c.CustomerNumber);
                    break;
            }

            int itemsOnPage = 3;

            // CONVERT QUERY TO SINGLE PAGE OF ITEMS IN COLLECTION THAT SUPPORTS PAGING
            return View(await PageList<Customer>.CreateAsync(customers.AsNoTracking(), 
                                                             pageIndex ?? 1, // IF NULL, RETURN 1 - ELSE RETURN PAGEINDEX VALUE
                                                             itemsOnPage)); 
        }

        // GET: Customers/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var customer = await _ctx.Customers.SingleOrDefaultAsync(m => m.CustomerID == id);
            var customer = await _ctx.Customers
                                     .Include(c => c.Orders)
                                     .ThenInclude(o => o.OrderItems)
                                     .AsNoTracking()
                                     .SingleOrDefaultAsync(c => c.CustomerID == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        
        // GET: Customers/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _ctx.Customers.SingleOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Delete/
        public async Task<IActionResult> Delete(int? id, bool? saveFailed = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _ctx.Customers
                                     .AsNoTracking()
                                     .SingleOrDefaultAsync(m => m.CustomerID == id);

            if (customer == null)
            {
                return NotFound();
            }

            if (saveFailed.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Something went wrong! Couldn't delete customer from database.";
            }

            return View(customer);
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,CustomerNumber,Firstname,Lastname,Email")] Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ctx.Add(customer);
                    await _ctx.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(
                    "DbUpdateException",
                    $"Unable to create Customer model because of a database update exception! {ex.Message}");
            }

            return View(customer);
        }

        // POST: Customers/Edit/
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _ctx.Customers.SingleOrDefaultAsync(c => c.CustomerID == id);

            if (await TryUpdateModelAsync<Customer>(
                customer,
                "",
                c => c.CustomerNumber, 
                c => c.Firstname,
                c => c.Lastname,
                c => c.Email)) 
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
                    $"Unable to update Customer model because of a database update exception! {ex.Message}");
                }
            }

            return View(customer);
        }

        // POST: Customers/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _ctx.Customers
                                     .AsNoTracking()
                                     .SingleOrDefaultAsync(c => c.CustomerID == id);
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                _ctx.Customers.Remove(customer);
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
