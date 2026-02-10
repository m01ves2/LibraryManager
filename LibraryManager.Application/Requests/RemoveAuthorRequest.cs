namespace LibraryManager.Application.Requests
{
    public class RemoveAuthorRequest
    {
        public int Id { get; }
        public RemoveAuthorRequest(int id)
        {
            Id = id;
        }
    }
}
