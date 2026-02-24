using LibraryManager.Application.Commands.Interfaces;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Infrastructure.Repositories.EF.Repositories
{
    public class EfBookCommandRepository : IBookCommandRepository
    {
        private readonly LibraryDbContext _context;
        public EfBookCommandRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public void Add(Book book)
        {
            _context.Books.Add(book);
        }

        public Book? GetById(int id)
        {
            return _context.Books.FirstOrDefault(a => a.Id == id);
        }

        public Book? GetByIsbn(Isbn isbn)
        {
            return _context.Books.FirstOrDefault(a => a.Isbn.Value == isbn.Value);
        }

        public bool HasBooksByAuthorId(int id)
        {
            return _context.Books.Any(a => a.AuthorId == id);
        }

        public void Remove(Book book)
        {
            _context.Books.Remove(book);
        }
    }
}
