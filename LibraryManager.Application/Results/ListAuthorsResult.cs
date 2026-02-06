using LibraryManager.Application.Models;

namespace LibraryManager.Application.Results
{
    public class ListAuthorsResult
    {
        public int TotalCount { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public IReadOnlyList<AuthorSummary> Authors { get; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;

        public ListAuthorsResult(int totalCount, int pageNumber, int pageSize, IReadOnlyList<AuthorSummary> authors)
        {
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Authors = authors;
        }
    }
}
