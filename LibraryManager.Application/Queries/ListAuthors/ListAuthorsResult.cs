using LibraryManager.Application.Models;

namespace LibraryManager.Application.Queries.ListAuthors
{
    public class ListAuthorsResult
    {
        public int TotalCount { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public IReadOnlyList<AuthorPreview> Authors { get; }

        //public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public int TotalPages => TotalCount / PageSize + (TotalCount % PageSize == 0 ? 0 : 1);
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;

        public ListAuthorsResult(int totalCount, int pageNumber, int pageSize, IReadOnlyList<AuthorPreview> authors)
        {
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Authors = authors;
        }
    }
}
