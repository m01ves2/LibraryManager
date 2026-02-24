using LibraryManager.Domain.Entities;

namespace LibraryManager.Application.Commands.Interfaces
{
    public interface IAuthorCommandRepository
    {
        void Add(Author author);
        void Remove(Author author);
        Author? GetById(int id);
    }
}
