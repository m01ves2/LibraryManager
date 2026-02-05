namespace LibraryManager.UI.CLI.Contexts
{
    public class ListBooksContext
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public ListBooksContext(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
