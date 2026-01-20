
using LibraryManager.Domain.Exceptions;
using LibraryManager.Domain.Results;
using LibraryManager.Domain.ValueObjects;
using System.Collections.Generic;

namespace LibraryManager.Domain.Entities
{
    public class Library
    {
        private readonly List<Book> _books = new(); // внутреннее состояние агрегата
        public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

        public Library(List<Book> books)
        {

            //books.ForEach(b => AddBook(b));
            foreach (var book in books) {
                if(_books.Any(b => b.Isbn == book.Isbn))
                    continue;
                _books.Add(book);
            }
        }

        public OperationResult<IEnumerable<Book>> AddBooks(IEnumerable<Book> newBooks)
        {
            var added = new List<Book>();

            foreach (var book in newBooks) {
                if (_books.Any(b => b.Isbn == book.Isbn))
                    continue; // пропускаем дубликат

                _books.Add(book);
                added.Add(book);
            }

            return OperationResult<IEnumerable<Book>>.Ok(added); // возвращаем список реально добавленных книг
        }

        public OperationResult<Book> AddBook(Book newBook)
        {
            if (_books.Any(b => b.Isbn == newBook.Isbn))
                return OperationResult<Book>.Fail(ResultStatus.Duplicate, $"Book with ISBN {newBook.Isbn} already exists in the library.");

            _books.Add(newBook);
            return OperationResult<Book>.Ok(newBook);
        }

        public OperationResult<Book> GetBookByIsbn(Isbn isbn)
        {
            Book? book = _books.Find(b =>  b.Isbn == isbn);
            if (book == null)
                return OperationResult<Book>.Fail(ResultStatus.NotFound, $"Book with ISBN {isbn} not found");
            return OperationResult<Book>.Ok(book);
        }

        public OperationResult<List<Book>> GetBooksByTitle(string title)
        {
            List<Book> books = _books.FindAll(b => b.Title == title);
            if (books.Count == 0)
                return OperationResult<List<Book>>.Fail(ResultStatus.NotFound, $"Book with title '{title}' not found");
            return OperationResult<List<Book>>.Ok(books);
        }

        public OperationResult<List<Book>> GetBooksByAuthor(Author author)
        {
            List<Book> books = _books.FindAll(b => b.Author == author);
            if (books.Count == 0)
                return OperationResult<List<Book>>.Fail(ResultStatus.NotFound, $"Books with author '{author.Name}' not found");
            return OperationResult<List<Book>>.Ok(books);
        }
        
        public OperationResult<Book> UpdateBookById(Book book)
        {
            Book? bookFound = _books.Find(b => b.Id == book.Id);
            if(bookFound == null) {
                return OperationResult<Book>.Fail(ResultStatus.NotFound, $"Book with id '{book.Id}' not found");
            }
            return bookFound.UpdateBook(book);
        }

        public OperationResult<Book> UpdateBookByIsbn(Book book)
        {
            Book? bookFound = _books.Find(b => b.Isbn == book.Isbn);
            if (bookFound == null) {
                return OperationResult<Book>.Fail(ResultStatus.NotFound, $"Book with ISBN '{book.Isbn}' not found");
            }
            return bookFound.UpdateBook(book);
        }

        public OperationResult<Book> DeleteBookById(int id)
        {
            Book? bookFound = _books.Find(b => b.Id == id);
            if (bookFound == null) {
                return OperationResult<Book>.Fail(ResultStatus.NotFound, $"Book with id '{id}' not found");
            }
            
            if(!_books.Remove(bookFound))
                return OperationResult<Book>.Fail(ResultStatus.Fail, $"Cannot delete book with id '{id}'");
            return OperationResult<Book>.Ok(bookFound);
        }

        public OperationResult<Book> DeleteBookByIsbn(Isbn isbn)
        {
            Book? bookFound = _books.Find(b => b.Isbn == isbn);
            if (bookFound == null) {
                return OperationResult<Book>.Fail(ResultStatus.NotFound, $"Book with ISBN '{isbn}' not found");
            }

            if (!_books.Remove(bookFound))
                return OperationResult<Book>.Fail(ResultStatus.Fail, $"Cannot delete book with ISBN '{isbn}'");
            return OperationResult<Book>.Ok(bookFound);
        }
    }
}
