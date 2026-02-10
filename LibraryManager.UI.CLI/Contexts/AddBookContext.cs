namespace LibraryManager.UI.CLI.Contexts
{
    public class AddBookContext
    {
        public  string? Title { get; set; }
        public string? Description { get; set; }
        public string? AuthorName { get; set; }
        public int? AuthorId { get; set; }
        public string? Isbn { get; set; }

    }
}
