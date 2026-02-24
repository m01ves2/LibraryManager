namespace LibraryManager.Application.Commands.Interfaces
{
    public interface IUnitOfWork
    {
        IBookCommandRepository Books { get; }
        IAuthorCommandRepository Authors { get; }
        void Commit();
    }
}
