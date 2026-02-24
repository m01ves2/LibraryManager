using LibraryManager.Domain.Entities;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Domain.Interfaces
{
    public interface IBookRepository
    {
        // Books
        void Add(Book book);
        int Count();
        int CountByAuthorId(int authorId);

        IReadOnlyList<Book> GetAll();
        Book? GetById(int id);
        Book? GetByIsbn(Isbn isbn);
        public IReadOnlyList<Book> GetByAuthorId(int authorId);
        IReadOnlyList<Book> GetPaged(int skip, int take, string? titleContains = null, string? authorNameContains = null, string? isbnContains = null);
        IReadOnlyList<Book> GetPagedByAuthorId(int skip, int take, int authorId);
        bool HasBooksByAuthorId(int authorId);
        void Remove(Book book);
    }
}
