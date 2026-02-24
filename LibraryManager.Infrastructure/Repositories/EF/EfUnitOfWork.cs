using LibraryManager.Application.Interfaces;
using LibraryManager.Domain.Entities;
using LibraryManager.Infrastructure.Repositories.EF.Repositories;

namespace LibraryManager.Infrastructure.Repositories.EF
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        public IRepository<Book> Books { get; }
        public IRepository<Author> Authors { get; }

        public EfUnitOfWork(LibraryDbContext context)
        {
            _context = context;
            Books = new EfRepository<Book>(_context);
            Authors = new EfRepository<Author>(_context);
        }

        public void Commit() => _context.SaveChanges();
    }
}
