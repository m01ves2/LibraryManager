using LibraryManager.Domain.Interfaces;
using LibraryManager.Infrastructure.Repositories.EF.Repositories;

namespace LibraryManager.Infrastructure.Repositories.EF
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        public IBookRepository Books { get; }
        public IAuthorRepository Authors { get; }

        public EfUnitOfWork(LibraryDbContext context, IBookRepository books, IAuthorRepository authors)
        {
            _context = context;

            Books = books;
            Authors = authors;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
