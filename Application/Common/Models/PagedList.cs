using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class PagedList<T>
    {
        public List<T> Items { get; }
        public int TotalRecords { get; }
        public int PageNumber { get; }  // page number
        public int PageSize { get; } // page size
        public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
        public int Count => Items.Count;
        public bool HasNext => PageNumber < TotalPages;
        public bool HasPrev => PageNumber > 1;

        public int[] Pages
        {
            get
            {
                var pages = new List<int>();
           
                if (TotalPages <= 10)
                {
                    for (int i = 1; i <= TotalPages; i++)
                    {
                        pages.Add(i);
                    }
                }
                else
                {                
                    pages.Add(1);
                    int start = Math.Max(2, PageNumber - 2); 
                    int end = Math.Min(PageNumber + 2, TotalPages); 

                    for (int i = start; i <= end; i++)
                    {
                        pages.Add(i);
                    }

                    pages.Add(TotalPages);
                }
                return pages.Distinct().ToArray();
            }
        }

        public PagedList(List<T> items, int totalRecords, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1)
            {
                throw new ApplicationException("page number minimum value is 1.");
            }

            if (pageSize < 1)
            {
                throw new ApplicationException("page size minimum value is 1.");
            }

            Items = items;
            TotalRecords = totalRecords;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public static PagedList<T> FromList(List<T> source, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1)
            {
                throw new ApplicationException("Page number minimum value is 1.");
            }

            if (pageSize < 1)
            {
                throw new ApplicationException("page size minimum value is 1.");
            }

            var pagedItems = source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedList<T>(pagedItems, source.Count, pageNumber, pageSize);
        }
    }
}
