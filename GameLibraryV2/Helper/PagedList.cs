using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Helper
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get;  private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set;}
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> item, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(item);
        }

        public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize) 
        { 
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedList<T>(await items.ToListAsync(), count, pageNumber, pageSize);
        }
    }
}
