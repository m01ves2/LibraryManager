namespace LibraryManager.Application.Requests
{
    public class AddBookRequest
    {
        public string Title { get; }
        public string? Description { get; }
        public int AuthorId { get; }
        public string? Isbn { get; }

        public AddBookRequest(string title, string? description, int authorId, string? isbn)
        {
            Title = title;
            Description = description;
            AuthorId = authorId;
            Isbn = isbn;
        }
    }
}
