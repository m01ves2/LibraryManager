using LibraryManager.Domain.Entities;

namespace LibraryManager.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        // Authors
        void AddAuthor(Author author);
        Author? GetAuthorById(int id);
        Author? GetAuthorByName(string name);
        IReadOnlyList<Author> GetAllAuthors();
        void RemoveAuthor(Author author);
    }
}
