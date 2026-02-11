namespace LibraryManager.UI.CLI.Contexts
{
    public class ListAuthorsContext
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public ListAuthorsContext(int pageNumber = 1, int pageSize = 4)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
