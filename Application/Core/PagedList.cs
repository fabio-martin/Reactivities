using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Core
{
    // Because it will be used for any kind of list in the application it uses a generic 
    // derives from the list class e that means that inherits everything from the list class
    // but it is extended with other properties 
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        // how many items to show per page
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize )
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double) pageSize);
            PageSize = pageSize;
            TotalCount = count;
            // Add the items that we get passed in as a parameter into the class that it is returned
            AddRange(items);
        }

        // to create a paged list 
        /*
        *  @params : source, receives a list of items before they are executed to a list on the database 
         * it's a query that's going to the database
        */
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            // we need to make this query to establish how many items are in our database 
            var count = await source.CountAsync();
            // get the items , give the items to skip 
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

  
    }
}