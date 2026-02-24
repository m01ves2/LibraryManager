using LibraryManager.Application.Commands.Interfaces;

namespace LibraryManager.Infrastructure.Repositories.EF
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        public IBookCommandRepository Books { get; }
        public IAuthorCommandRepository Authors { get; }

        public EfUnitOfWork(LibraryDbContext context, IBookCommandRepository books, IAuthorCommandRepository authors)
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
