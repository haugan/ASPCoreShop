using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShop
{
    public class PageList<T> : List<T>
    {
        public int Index { get; set; }
        public int TotalPages { get; set; }

        public PageList(List<T> listItems, int itemCount, int pageIndex, int pageSize)
        {
            Index = pageIndex;
            TotalPages = (int) Math.Ceiling(itemCount / (double) pageSize);
            this.AddRange(listItems);
        }

        public static async Task<PageList<T>> CreateAsync(IQueryable<T> collection, int index, int size)
        {
            var count = await collection.CountAsync();
            var items = await collection.Skip((index - 1) * size)
                                        .Take(size).ToListAsync();

            return new PageList<T>(items, count, index, size); // CONSTRUCTORS CAN'T RUN ASYNCHRONOUS CODE
        }

        public bool HasPrevious
        {
            get
            {
                return (Index > 1); // ENABLE PAGING BUTTON; "PREVIOUS"
            }
        }

        public bool HasNext
        {
            get
            {
                return (Index < TotalPages); // ENABLE PAGING BUTTON; "NEXT"
            }
        }
    }
}
