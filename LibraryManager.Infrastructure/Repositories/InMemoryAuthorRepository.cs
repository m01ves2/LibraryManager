using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using System.Collections.Generic;

namespace LibraryManager.Infrastructure.Repositories
{
    public class InMemoryAuthorRepository : IAuthorRepository
    {
        private readonly List<Author> _authors = new List<Author>();
        private int nextAuthorId = 0;

        public void Add(Author author)
        {
            author.Id = ++nextAuthorId;
            _authors.Add(author);
        }

        public IReadOnlyList<Author> GetAll(string? NameContains = null)
        {
            IReadOnlyList<Author> getFiltered = _authors;
            if(!string.IsNullOrEmpty(NameContains))
                getFiltered = _authors.Where(a => a.Name.Contains(NameContains)).ToList();
            return getFiltered;
        }
        public IReadOnlyList<Author> GetPaged(int skip, int take, string? NameContains = null)
        {
            IReadOnlyList<Author> _authorsSelected = _authors;
            if (!string.IsNullOrEmpty(NameContains))
                _authorsSelected = _authors.Where(a => a.Name.Contains(NameContains)).ToList();
            return _authorsSelected.Skip(skip).Take(take).ToList();
        }

        public Author? GetById(int id)
        {
            return _authors.FirstOrDefault(b => b.Id == id);
        }

        public IReadOnlyList<Author> GetByName(string name)
        {
            return _authors.Where(b => b.Name == name).ToList();
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
