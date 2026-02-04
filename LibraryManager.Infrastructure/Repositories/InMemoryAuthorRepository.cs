using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;

namespace LibraryManager.Infrastructure.Repositories
{
    public class InMemoryAuthorRepository : IAuthorRepository
    {
        private readonly List<Author> _authors = new List<Author>();
        private int nextAuthorIndex = 0;

        public void Add(Author author)
        {
            author.Id = ++nextAuthorIndex;
            _authors.Add(author);
        }
        public IReadOnlyList<Author> GetPaged(int skip, int take)
        {
            return _authors.Skip(skip).Take(take).ToList();
        }

        public Author? GetById(int id)
        {
            return _authors.FirstOrDefault(b => b.Id == id);
        }

        public Author? GetByName(string name)
        {
            return _authors.FirstOrDefault(b => b.Name == name);
        }

        public void Remove(Author author)
        {
            if (author is null)
                throw new ArgumentNullException(nameof(author));

            if (!_authors.Remove(author))
                throw new InvalidOperationException("Author to delete not found");
        }

        public int Count()
        {
            return _authors.Count;
        }
    }
}
