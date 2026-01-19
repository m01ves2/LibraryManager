
using LibraryManager.Domain.Exceptions;

namespace LibraryManager.Domain.Entities
{
    public class Library
    {
        private readonly List<Book> _books = new(); // внутреннее состояние агрегата
        public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

        public Library(List<Book> books)
        {
            
            books.ForEach(b => AddBook(b));

        }

        public IEnumerable<Book> AddBooks(IEnumerable<Book> newBooks)
        {
            var added = new List<Book>();

            foreach (var book in newBooks) {
                if (_books.Any(b => b.Isbn == book.Isbn))
                    continue; // пропускаем дубликат

                _books.Add(book);
                added.Add(book);
            }

            return added; // возвращаем список реально добавленных книг
        }

        public void AddBook(Book newBook)
        {
            if (_books.Any(b => b.Isbn == newBook.Isbn))
                throw new DomainValidationException($"Book with ISBN {newBook.Isbn} already exists in the library.");

            _books.Add(newBook);
        }
    }
}
