namespace LibraryManager.UI.CLI.Contexts
{
    public class ListBooksContext
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int? Id { get; set; } 
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AuthorName { get; set; }
        public int? AuthorId { get; set; }
        public string? Isbn { get; set; }

        public ListBooksContext(int pageNumber = 1, int pageSize = 3)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
