using LibraryManager.Domain.Entities;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Domain.Interfaces
{
    public interface IBookRepository
    {
        // Books
        void AddBook(Book book);
        Book? GetBookById(int id);
        Book? GetBookByIsbn(Isbn isbn);
        IReadOnlyList<Book> GetBooks(int skip, int take);
        int GetBooksCount();
        void RemoveBook(Book book);
    }
}
