namespace LibraryManager.Application.Response
{
    public class ListBookResponse
    {
        public int TotalCount {  get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public IReadOnlyList<BookSummary> Items { get; set; }
    }
}
