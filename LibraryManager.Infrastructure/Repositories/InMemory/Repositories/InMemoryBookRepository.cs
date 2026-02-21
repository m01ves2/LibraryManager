using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Infrastructure.Repositories.InMemory.Repositories
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly List<Book> _books = new List<Book>();
        private int nextBookId = 0;
        public void Add(Book book)
        {
            if (book is null)
                throw new ArgumentNullException(nameof(book));

            book.Id = ++nextBookId;
            _books.Add(book);
        }

        public int Count()
        {
            return _books.Count;
        }
        public int CountByAuthorId(int authorId)
        {
            return _books.Where(b => b.Author.Id == authorId).Count();
        }

        public IReadOnlyList<Book> GetAll()
        {
            return _books;
        }

        public Book? GetById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        public Book? GetByIsbn(Isbn isbn)
        {
            return _books.FirstOrDefault(b => b.Isbn == isbn);
        }

        public IReadOnlyList<Book> GetByAuthorId(int authorId)
        {
            return _books.Where(b => b.Author.Id == authorId).ToList();
        }

        public IReadOnlyList<Book> GetPaged(int skip, int take)
        {
            return _books.Skip(skip).Take(take).ToList();
        }

        public IReadOnlyList<Book> GetPagedByAuthorId(int skip, int take, int authorId)
        {
            return _books.Where(b => b.Author.Id == authorId).Skip(skip).Take(take).ToList();
        }

        public bool HasBooksByAuthorId(int authorId)
        {
            return _books.Any(b => b.Author.Id == authorId);
        }

        public void Remove(Book book)
        {
            if (book is null)
                throw new ArgumentNullException(nameof(book));

            if (!_books.Remove(book))
                throw new InvalidOperationException("Book to delete not found");
        }

        public void RemoveById(int id) {
            Book? book = _books.FirstOrDefault(b => b.Id == id);

            if (book is null)
                throw new InvalidOperationException($"Book with id {id} not found");

            _books.Remove(book);
        }
    }
}
