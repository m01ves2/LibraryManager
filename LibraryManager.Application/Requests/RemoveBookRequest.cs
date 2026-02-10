namespace LibraryManager.Application.Requests
{
    public class RemoveBookRequest
    {
        public int Id { get; }
        public RemoveBookRequest(int id)
        {
            Id = id;
        }
    }
}
