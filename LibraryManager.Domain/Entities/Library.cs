using LibraryManager.Domain.Interfaces;
using LibraryManager.Domain.Results;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Domain.Entities
{
    public class Library
    {
        //private readonly List<Book> _books = new(); // внутреннее состояние агрегата
        private readonly ILibraryRepository _repo;
        //public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

        public Library(ILibraryRepository repo)
        {
            _repo = repo;
        }

        public OperationResult<IEnumerable<Book>> AddBooks(IEnumerable<Book> newBooks)
        {
            var added = new List<Book>();

            foreach (var book in newBooks) {
                if (_repo.FindBook(b => b.Isbn == book.Isbn).Data != null)
                    continue; // пропускаем дубликат

                _repo.AddBook(book);
                added.Add(book);
            }

            return OperationResult<IEnumerable<Book>>.Ok(added); // возвращаем список реально добавленных книг
        }

        public OperationResult<Book> AddBook(Book newBook)
        {
            if (_repo.FindBook(b => b.Isbn == newBook.Isbn).Data != null)
                return OperationResult<Book>.Fail(ResultStatus.Duplicate, $"Book with ISBN {newBook.Isbn} already exists in the library.");

            _repo.AddBook(newBook);
            return OperationResult<Book>.Ok(newBook);
        }

        public OperationResult<Book> GetBookByIsbn(Isbn isbn)
        {
            Book? book = _repo.FindBook(b =>  b.Isbn == isbn).Data;
            if (book == null)
                return OperationResult<Book>.Fail(ResultStatus.NotFound, $"Book with ISBN {isbn} not found");
            return OperationResult<Book>.Ok(book);
        }

        public OperationResult<List<Book>> GetBooksByTitle(string title)
        {
            List<Book> books = _repo.FindAllBooks(b => b.Title == title).Data?.ToList() ?? new List<Book>();
            if (books.Count == 0)
                return OperationResult<List<Book>>.Fail(ResultStatus.NotFound, $"Book with title '{title}' not found");
            return OperationResult<List<Book>>.Ok(books);
        }

        public OperationResult<List<Book>> GetBooksByAuthor(Author author)
        {
            List<Book> books = _repo.FindAllBooks(b => b.Author == author).Data?.ToList() ?? new List<Book>();
            if (books.Count == 0)
                return OperationResult<List<Book>>.Fail(ResultStatus.NotFound, $"Books with author '{author.Name}' not found");
            return OperationResult<List<Book>>.Ok(books);
        }
        
        public OperationResult<Book> UpdateBookById(Book book)
        {
            Book? bookFound = _repo.FindBook(b => b.Id == book.Id).Data;
            if(bookFound == null) {
                return OperationResult<Book>.Fail(ResultStatus.NotFound, $"Book with id '{book.Id}' not found");
            }
            return bookFound.UpdateBook(book);
        }

        public OperationResult<Book> UpdateBookByIsbn(Book book)
        {
            Book? bookFound = _repo.FindBook(b => b.Isbn == book.Isbn).Data;
            if (bookFound == null) {
                return OperationResult<Book>.Fail(ResultStatus.NotFound, $"Book with ISBN '{book.Isbn}' not found");
            }
            return bookFound.UpdateBook(book);
        }

        public OperationResult<Book> DeleteBookById(int id)
        {
            Book? bookFound = _repo.FindBook(b => b.Id == id).Data;
            if (bookFound == null) {
                return OperationResult<Book>.Fail(ResultStatus.NotFound, $"Book with id '{id}' not found");
            }
            
            if(!_repo.RemoveBook(bookFound).IsSuccess)
                return OperationResult<Book>.Fail(ResultStatus.Fail, $"Cannot delete book with id '{id}'");
            return OperationResult<Book>.Ok(bookFound);
        }

        public OperationResult<Book> DeleteBookByIsbn(Isbn isbn)
        {
            Book? bookFound = _repo.FindBook(b => b.Isbn == isbn).Data;
            if (bookFound == null) {
                return OperationResult<Book>.Fail(ResultStatus.NotFound, $"Book with ISBN '{isbn}' not found");
            }

            if (!_repo.RemoveBook(bookFound).IsSuccess)
                return OperationResult<Book>.Fail(ResultStatus.Fail, $"Cannot delete book with ISBN '{isbn}'");
            return OperationResult<Book>.Ok(bookFound);
        }
    }
}
