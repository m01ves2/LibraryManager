namespace LibraryManager.UI.CLI.Contexts
{
    public class ListAuthorsContext
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public ListAuthorsContext(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
