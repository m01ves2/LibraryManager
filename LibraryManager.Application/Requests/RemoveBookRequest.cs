using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Application.Requests
{
    public class RemoveBookRequest
    {
        public string? Isbn { get; init; }
        public int? BookId { get; init; }
        public string? Title { get; init; }
    }
}
