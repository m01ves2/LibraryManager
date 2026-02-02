namespace LibraryManager.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        void Commit();
    }
}
