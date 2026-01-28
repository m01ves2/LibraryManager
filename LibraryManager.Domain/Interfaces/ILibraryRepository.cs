using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Results;

namespace LibraryManager.Domain.Interfaces
{
    public interface ILibraryRepository
    {
        OperationResult<Book> AddBook(Book book); 
        OperationResult<Book> FindBook(Func<Book, bool> func); 
        OperationResult<List<Book>> FindAllBooks(Func<Book, bool> func); 
        OperationResult<Book> RemoveBook(Book book); 
        
        OperationResult<Author> AddAuthor(Author author); 
        OperationResult<Author> FindAuthor(Func<Author, bool> func); 
        OperationResult<List<Author>> FindAllAuthors(Func<Author, bool> func);
        OperationResult<Author> RemoveAuthor(Author author);
    }
}
