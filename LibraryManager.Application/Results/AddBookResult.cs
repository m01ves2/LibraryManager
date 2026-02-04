namespace LibraryManager.Application.Results
{
    public class AddBookResult
    {
        public int Id { get; set; }
        public string Title { get; }
        public string? Description { get; }
        public string AuthorName { get; }
        public string? Isbn { get; }

        public AddBookResult(int id, string title, string? description, string authorName, string? isbn)
        {
            Id = id;
            Title = title;
            Description = description;
            AuthorName = authorName;
            Isbn = isbn;
        }
    }
}
