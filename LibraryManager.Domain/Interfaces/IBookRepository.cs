using LibraryManager.Domain.Entities;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Domain.Interfaces
{
    public interface IBookRepository
    {
        // Books
        void Add(Book book);
        int Count();
        Book? GetById(int id);
        Book? GetByIsbn(Isbn isbn);
        IReadOnlyList<Book> GetPaged(int skip, int take);
        void Remove(Book book);
        void RemoveById(int id);
    }
}
