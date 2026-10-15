namespace LibraryManager.UI.CLI.Contexts
{
    public class ListAuthorsContext
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        // REVIEW: Id/Name serve both as the currently highlighted author
        // and as the result returned to the calling screen.
        // This causes AddBookScreen to treat navigation (including Back)
        // as an explicit author selection.
        // Separate UI highlight state from confirmed selection result.
        public int? Id { get; set; }
        public string? Name { get; set; }
        public ListAuthorsContext(int pageNumber = 1, int pageSize = 4)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
