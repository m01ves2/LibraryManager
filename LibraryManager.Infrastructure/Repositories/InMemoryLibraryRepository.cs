using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Domain.Results;

namespace LibraryManager.Infrastructure.Repositories
{
    public class InMemoryLibraryRepository : ILibraryRepository
    {

        private readonly List<Book> _books = new List<Book>();
        private readonly List<Author> _authors = new List<Author>();


        public OperationResult<Book> AddBook(Book newBook)
        {
            _books.Add(newBook);
            return OperationResult<Book>.Ok(newBook);
        }
        public OperationResult<Book> FindBook(Func<Book, bool> func)
        {
            Book? bookFound = _books.Find(func.Invoke);
            return OperationResult<Book>.Ok(bookFound!);
        }
        public OperationResult<List<Book>> FindAllBooks(Func<Book, bool> func)
        {
            List<Book> booksFound = _books.FindAll(func.Invoke);
            return OperationResult<List<Book>>.Ok(booksFound);
        }
        public OperationResult<Book> RemoveBook(Book book)
        {
            bool isSuccess =_books.Remove(book);
            if(isSuccess)
                return OperationResult<Book>.Ok(book);
            else
               return OperationResult<Book>.Fail(ResultStatus.NotFound, $"Book to delete not found");
        }


        public OperationResult<Author> AddAuthor(Author newAuthor)
        {
            _authors.Add(newAuthor);
            return OperationResult<Author>.Ok(newAuthor);
        }

        public OperationResult<Author> FindAuthor(Func<Author, bool> func)
        {
            Author? authorFound = _authors.Find(func.Invoke);
            return OperationResult<Author>.Ok(authorFound!);
        }

        public OperationResult<List<Author>> FindAllAuthors(Func<Author, bool> func)
        {
            List<Author> authorsFound = _authors.FindAll(func.Invoke);
            return OperationResult<List<Author>>.Ok(authorsFound);
        }

        public OperationResult<Author> RemoveAuthor(Author author)
        {
            bool isSuccess =_authors.Remove(author);
            if(isSuccess)
                return OperationResult<Author>.Ok(author);
            else
               return OperationResult<Author>.Fail(ResultStatus.NotFound, $"Author to delete not found"); 
        }

    }
}