using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class PageList<T> : List<T>
    {

        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; } = 30;
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PageList(List<T> items, int count, int pageNumber)
        {
            TotalCount = count;
            PageSize = 30;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            AddRange(items);
        }

        public static PageList<T> ToPagedList(IQueryable<T> source, int pageNumber)
        {
            var pageSize = 30;
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PageList<T>(items, count, pageNumber);
        }
       
    }
}
