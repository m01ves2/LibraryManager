namespace LibraryManager.Application.Commands.RemoveBook
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
