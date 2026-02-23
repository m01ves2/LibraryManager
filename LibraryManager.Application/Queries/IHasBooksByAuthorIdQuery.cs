namespace LibraryManager.Application.Queries
{
    public interface IHasBooksByAuthorIdQuery
    {
        bool Execute(int authorId);
    }
}
