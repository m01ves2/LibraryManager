using LibraryManager.Application.Models;

namespace LibraryManager.Application.Results
{
    public class ListAuthorsResult
    {
        public int TotalCount { get; }
        public int PageNumber { get; }
        public int PageSize { get; }

        public IReadOnlyList<AuthorSummary> Authors { get; }

        public ListAuthorsResult(int totalCount, int pageNumber, int pageSize, IReadOnlyList<AuthorSummary> authors)
        {
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Authors = authors;
        }
    }
}
