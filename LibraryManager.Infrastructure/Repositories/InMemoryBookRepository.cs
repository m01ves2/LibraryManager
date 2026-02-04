using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Infrastructure.Repositories
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly List<Book> _books = new List<Book>();
        private int nextBookIndex = 0;
        public void Add(Book book)
        {
            if (book is null)
                throw new ArgumentNullException(nameof(book));

            book.Id = ++nextBookIndex;
            _books.Add(book);
        }

        public Book? GetById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        public Book? GetByIsbn(Isbn isbn)
        {
            return _books.FirstOrDefault(b => b.Isbn == isbn);
        }

        //public IReadOnlyList<Book> GetPagedByAuthor(int skip, int take, Author author)
        //{
        //    return _books.Where(b => b.Author == author).Skip(skip).Take(take).ToList();
        //}

        public IReadOnlyList<Book> GetPaged(int skip, int take)
        {
            return _books.Skip(skip).Take(take).ToList();
        }

        public int Count()
        {
            return _books.Count;
        }

        public bool HasBooksByAuthorId(int authorId)
        {
            return _books.Where(b => b.Author.Id == authorId).Count() > 0;
        }

        public void Remove(Book book)
        {
            if (book is null)
                throw new ArgumentNullException(nameof(book));

            if (!_books.Remove(book))
                throw new InvalidOperationException("Book to delete not found");
        }
    }
}
