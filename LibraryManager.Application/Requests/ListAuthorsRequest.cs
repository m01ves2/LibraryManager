namespace LibraryManager.Application.Requests
{
    public class ListAuthorsRequest
    {
        private const int MaxPageSize = 10;

        public int PageNumber { get; }
        public int PageSize { get; }
        public string? NameContains { get; }

        public ListAuthorsRequest(int pageNumber, int pageSize, string? nameContains = null)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "PageNumber must be >= 1");

            if (pageSize < 1 || pageSize > MaxPageSize)
                throw new ArgumentOutOfRangeException(nameof(pageSize), $"PageSize must be between 1 and {MaxPageSize}");

            PageNumber = pageNumber;
            PageSize = pageSize;
            NameContains = nameContains;
        }
    }
}
