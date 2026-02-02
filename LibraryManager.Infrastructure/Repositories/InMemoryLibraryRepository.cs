using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;

namespace LibraryManager.Infrastructure.Repositories
{
    public class InMemoryLibraryRepository : IBookRepository
    {

        private readonly List<Book> _books = new List<Book>();
        private readonly List<Author> _authors = new List<Author>();
        private static int nextBookIndex = 0;
        private static int nextAuthorIndex = 0;

        //public OperationResult<Book> AddBook(Book newBook)
        //{
        //    newBook.Id = ++nextBookIndex;
        //    _books.Add(newBook);
        //    return OperationResult<Book>.Ok(newBook);
        //}
        //public OperationResult<Book> FindBook(Func<Book, bool> func)
        //{
        //    Book? bookFound = _books.Find(func.Invoke);
        //    if (bookFound is not null)
        //        return OperationResult<Book>.Ok(bookFound);
        //    else
        //        return OperationResult<Book>.NotFound("Books not found");
        //}
        //public OperationResult<List<Book>> FindAllBooks(Func<Book, bool> func)
        //{
        //    List<Book> booksFound = _books.FindAll(func.Invoke);
        //    if(booksFound.Count > 0)
        //        return OperationResult<List<Book>>.Ok(booksFound);
        //    else
        //        return OperationResult<List<Book>>.NotFound("Books not found");
        //}
        //public OperationResult<Book> RemoveBook(Book book)
        //{
        //    bool isSuccess =_books.Remove(book);
        //    if(isSuccess)
        //        return OperationResult<Book>.Ok(book);
        //    else
        //       return OperationResult<Book>.NotFound("Book to delete not found");
        //}


        //public OperationResult<Author> AddAuthor(Author newAuthor)
        //{
        //    newAuthor.Id = ++nextAuthorIndex;
        //    _authors.Add(newAuthor);
        //    return OperationResult<Author>.Ok(newAuthor);
        //}

        //public OperationResult<Author> FindAuthor(Func<Author, bool> func)
        //{
        //    Author? authorFound = _authors.Find(func.Invoke);
        //    if (authorFound is not null)
        //        return OperationResult<Author>.Ok(authorFound);
        //    else
        //        return OperationResult<Author>.NotFound("Author not found");
        //}

        //public OperationResult<List<Author>> FindAllAuthors(Func<Author, bool> func)
        //{
        //    List<Author> authorsFound = _authors.FindAll(func.Invoke);

        //    if (authorsFound.Count > 0)
        //        return OperationResult<List<Author>>.Ok(authorsFound);
        //    else
        //        return OperationResult<List<Author>>.NotFound("Authors not found");
        //}

        //public OperationResult<Author> RemoveAuthor(Author author)
        //{
        //    bool isSuccess =_authors.Remove(author);
        //    if(isSuccess)
        //        return OperationResult<Author>.Ok(author);
        //    else
        //       return OperationResult<Author>.NotFound("Author to delete not found"); 
        //}

    }
}