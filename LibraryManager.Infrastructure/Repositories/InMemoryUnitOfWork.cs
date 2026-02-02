using LibraryManager.Domain.Interfaces;

namespace LibraryManager.Infrastructure.Repositories
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
