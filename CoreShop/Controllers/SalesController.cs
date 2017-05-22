using CoreShop.Data;
using CoreShop.Models.ShoppingCartViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShop.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _ctx;

        public SalesController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        // GET: /Sales
        public async Task<IActionResult> Index(string sortOrder, string filter, string search, int? pageIndex)
        {
            ViewData["SortOrder"] = sortOrder; // SORT ORDER WHILE NAVIGATING "NEXT/PREVIOUS" PAGES
            ViewData["SortCustomerNumber"] = String.IsNullOrEmpty(sortOrder) ? "cusnum_desc" : "";
            ViewData["SortOrderCount"] = (sortOrder == "ordercount_asc") ? "ordercount_desc" : "ordercount_asc";

            if (search != null)
            {
                pageIndex = 1; // NEW SEARCH LEADS TO NEW RESULTS, RESET PAGE TO 1
            }
            else
            {
                search = filter;
            }

            ViewData["SearchFilter"] = search;  // KEEP SEARCH FILTER WHILE "PAGING"

            IQueryable<CustomerOrdersViewModel> groupQuery = 
            from item in _ctx.Orders
            group item by item.Customer.CustomerNumber into orderGroup
            select new CustomerOrdersViewModel()
            {
                CustomerNumber = orderGroup.Key,
                OrderCount = orderGroup.Count(),
            };
                                                              
            if (!String.IsNullOrEmpty(filter))
            {
                groupQuery = groupQuery.Where(p => p.CustomerNumber.ToString().Contains(filter));
            }

            switch (sortOrder)
            {
                case "cusnum_desc":
                    groupQuery = groupQuery.OrderByDescending(p => p.CustomerNumber);
                    break;
                case "ordercount_asc":
                    groupQuery = groupQuery.OrderBy(i => i.OrderCount);
                    break;
                case "ordercount_desc":
                    groupQuery = groupQuery.OrderByDescending(i => i.OrderCount);
                    break;
                default:
                    groupQuery = groupQuery.OrderBy(p => p.CustomerNumber);
                    break;
            }

            int itemsOnPage = 6;

            // CONVERT QUERY TO SINGLE PAGE OF ITEMS IN COLLECTION THAT SUPPORTS PAGING
            return View(await PageList<CustomerOrdersViewModel>.CreateAsync(groupQuery.AsNoTracking(),
                                                                        pageIndex ?? 1, // IF NULL, RETURN 1 - ELSE RETURN PAGEINDEX VALUE
                                                                        itemsOnPage));

            //return View(await groupQuery.AsNoTracking().ToListAsync());
        }
    }
}
