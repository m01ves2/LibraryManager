namespace LibraryManager.Application.Requests
{
    public class RemoveAuthorRequest
    {
        public int Id { get; }
        public string? Name { get; }
        public RemoveAuthorRequest(int id, string? name = null)
        {
            Id = id;
            Name = name;
        }
    }
}
