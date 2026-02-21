using LibraryManager.Domain.Interfaces;
using LibraryManager.Infrastructure.Repositories.InMemory.Repositories;

namespace LibraryManager.Infrastructure.Repositories.InMemory
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        public IBookRepository Books { get; }
        public IAuthorRepository Authors { get; }

        public InMemoryUnitOfWork()
        {
            Books = new InMemoryBookRepository();
            Authors = new InMemoryAuthorRepository();
        }

        public void Commit()
        {
            // nothing to do
        }
    }
}
