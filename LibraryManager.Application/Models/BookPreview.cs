namespace LibraryManager.Application.Models
{
    public class BookPreview
    {
        public int Id { get; }
        public string Title { get; }
        public string AuthorName { get; }
        public string Isbn { get; }

        public BookPreview(int id, string title, string authorName, string isbn)
        {
            Id = id;
            Title = title;
            AuthorName = authorName;
            Isbn = isbn;
        }
    }
}
