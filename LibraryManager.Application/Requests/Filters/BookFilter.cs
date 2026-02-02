using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Application.Requests.Filters
{
    public class BookFilter
    {
        public int? AuthorId { get; }
        public string? TitleContains { get; }
        public Isbn? Isbn { get; }

        public BookFilter(
            int? authorId,
            string? titleContains,
            Isbn? isbn)
        {
            AuthorId = authorId;
            TitleContains = titleContains;
            Isbn = isbn;
        }
    }
}
