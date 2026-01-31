namespace LibraryManager.Application.Requests
{
    public class ListBookRequest
    {
        public string? Title { get; set; }
        public int? AuthorId { get; set; }
        public string? Isbn { get; set; }
    }
}
