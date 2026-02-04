namespace LibraryManager.Application.Requests
{
    public class RemoveBookRequest
    {
        public int Id { get; }
        public string? Title { get; } // только для сообщений
        public RemoveBookRequest(int id, string? title = null)
        {
            Id = id;
            Title = title;
        }
    }
}
