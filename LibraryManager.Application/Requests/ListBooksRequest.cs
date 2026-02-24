namespace LibraryManager.Application.Requests
{
    public class ListBooksRequest
    {
        private const int MaxPageSize = 50;

        public int PageNumber { get; }
        public int PageSize { get; }

        public string? TitleContains { get; set; }
        public string? AuthorNameContains { get; set; }
        public string? IsbnContains { get; set; }

        public ListBooksRequest(int pageNumber, int pageSize, string? titleContains = null, string? authorNameContains = null, string? isbnContains = null)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "PageNumber must be >= 1");

            if (pageSize < 1 || pageSize > MaxPageSize)
                throw new ArgumentOutOfRangeException(nameof(pageSize), $"PageSize must be between 1 and {MaxPageSize}");

            PageNumber = pageNumber;
            PageSize = pageSize;
            TitleContains = titleContains;
            AuthorNameContains = authorNameContains;
            IsbnContains = isbnContains;
        }
    }
}
