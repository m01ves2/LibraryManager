using LibraryManager.Domain.Entities;

namespace LibraryManager.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Book> Books { get; }
        IRepository<Author> Authors { get; }
        void Commit();
    }
}
