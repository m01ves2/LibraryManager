namespace LibraryManager.Application.Commands.RemoveAuthor
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
