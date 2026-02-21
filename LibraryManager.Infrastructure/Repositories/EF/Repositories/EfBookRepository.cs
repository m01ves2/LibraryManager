using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Infrastructure.Repositories.EF.Repositories
{
    public class EfBookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public EfBookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
        }

        public int Count()
        {
            return _context.Books.Count();
        }

        public int CountByAuthorId(int authorId)
        {
            return _context.Books.Count(b => b.AuthorId == authorId);
        }

        public IReadOnlyList<Book> GetAll()
        {
            return _context.Books.Include(b => b.Author).ToList();
        }

        public IReadOnlyList<Book> GetByAuthorId(int authorId)
        {
            return _context.Books.Where(b => b.AuthorId == authorId).Include(b => b.Author).ToList();
        }

        public Book? GetById(int id)
        {
            return _context.Books.Where(b => b.Id == id).Include(b => b.Author).FirstOrDefault();
        }

        public Book? GetByIsbn(Isbn isbn)
        {
            return _context.Books.Where(b => b.Isbn != null && b.Isbn.Value == isbn.Value).Include(b => b.Author).FirstOrDefault();
        }

        public IReadOnlyList<Book> GetPaged(int skip, int take)
        {
            return _context.Books.OrderBy(b => b.Id).Skip(skip).Take(take).Include(b => b.Author).ToList();
        }

        public IReadOnlyList<Book> GetPagedByAuthorId(int skip, int take, int authorId)
        {
            return _context.Books.OrderBy(b => b.Id).Where(b => b.AuthorId == authorId).Skip(skip).Take(take).Include(b => b.Author).ToList();
        }

        public bool HasBooksByAuthorId(int authorId)
        {
            return _context.Books.Where(b => b.AuthorId == authorId).Any();
        }

        public void Remove(Book book)
        {
           _context.Books.Remove(book);
        }
    }
}
