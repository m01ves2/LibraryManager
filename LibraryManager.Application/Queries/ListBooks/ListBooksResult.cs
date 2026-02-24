using LibraryManager.Application.Models;

namespace LibraryManager.Application.Queries.ListBooks
{
    public class ListBooksResult
    {
        public int TotalCount {  get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public IReadOnlyList<BookPreview> Books { get; }

        //public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public int TotalPages => TotalCount / PageSize + (TotalCount % PageSize == 0 ? 0 : 1);
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;

        public ListBooksResult(int totalCount, int pageNumber, int pageSize, IReadOnlyList<BookPreview> books)
        {
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Books = books;
        }
    }
}
