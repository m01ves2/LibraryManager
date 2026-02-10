using LibraryManager.Domain.Entities;

namespace LibraryManager.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        // Authors
        void Add(Author author);
        int Count();
        public IReadOnlyList<Author> GetAll(string? NameContains = null);
        Author? GetById(int id);
        public IReadOnlyList<Author> GetByName(string name);
        IReadOnlyList<Author> GetPaged(int skip, int take, string? nameContains);
        void Remove(Author author);
    }
}
