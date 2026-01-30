namespace LibraryManager.Application.Requests
{
    public class AddBookRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public string Isbn { get; set; }

        public AddBookRequest( string title, string description, int authorId, string isbn)
        {
            Title = title;
            Description = description;
            AuthorId = authorId;
            Isbn = isbn;
        }
    }
}
