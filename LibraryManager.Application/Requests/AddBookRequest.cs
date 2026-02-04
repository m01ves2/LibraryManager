namespace LibraryManager.Application.Requests
{
    public class AddBookRequest
    {
        public string Title { get; }
        public string? Description { get; }
        public string AuthorName { get; }
        public string Isbn { get; }

        public AddBookRequest(string title, string description, string authorName, string isbn)
        {
            Title = title;
            Description = description;
            AuthorName = authorName;
            Isbn = isbn;
        }
    }
}
