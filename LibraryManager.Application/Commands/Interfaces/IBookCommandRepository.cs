using LibraryManager.Domain.Entities;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Application.Commands.Interfaces
{
    public interface IBookCommandRepository
    {
        void Add(Book book);
        void Remove(Book book);
        Book? GetById(int id);
        Book? GetByIsbn(Isbn isbn);
        bool HasBooksByAuthorId(int id);
    }
}
