using LibraryManager.Domain.Entities;

namespace LibraryManager.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        // Authors
        void Add(Author author);
        int Count();
        Author? GetById(int id);
        Author? GetByName(string name);
        IReadOnlyList<Author> GetPaged(int skip, int take);
        void Remove(Author author);
        void RemoveById(int id);
    }
}
