namespace LibraryManager.Application.Models
{
    public class BookSummary
    {
        public int BookId { get; }
        public string Title { get; }
        public string AuthorName { get; }
        public string? Isbn { get; }

        public BookSummary(int bookId, string title, string authorName, string? isbn)
        {
            BookId = bookId;
            Title = title;
            AuthorName = authorName;
            Isbn = isbn;
        }
    }
}
