namespace LibraryManager.Application.ViewModels
{
    public class ViewBook
    {
        public string? Title { get; set; }
        public ViewAuthor? Author { get; set; }
        public ViewIsbn? Isbn { get; set; }
        public string? Description { get; set; }
    }
}
