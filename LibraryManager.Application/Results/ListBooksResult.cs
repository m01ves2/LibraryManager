using LibraryManager.Application.Models;

namespace LibraryManager.Application.Results
{
    public class ListBooksResult
    {
        public int TotalCount {  get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public IReadOnlyList<BookSummary> Books { get; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;

        public ListBooksResult(int totalCount, int pageNumber, int pageSize, IReadOnlyList<BookSummary> books)
        {
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Books = books;
        }
    }
}
