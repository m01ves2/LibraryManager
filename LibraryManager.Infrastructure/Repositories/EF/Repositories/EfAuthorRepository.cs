using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Infrastructure.Repositories.EF.Repositories
{
    public class EfAuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;

        public EfAuthorRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(Author author)
        {
            _context.Add(author);
        }

        public int Count(string? nameContains = null)
        {
            if (string.IsNullOrEmpty(nameContains))
                return _context.Authors.Count();
            else
                return _context.Authors.Count(a => a.Name.Contains(nameContains));
        }

        public IReadOnlyList<Author> GetAll(string? nameContains = null)
        {
            if(string.IsNullOrEmpty(nameContains))
                return _context.Authors.Include(a => a.Books).ToList();
            else
                return _context.Authors.Where(a => a.Name.Contains(nameContains)).Include(a => a.Books).ToList();
        }

        public Author? GetById(int id)
        {
            return _context.Authors.Where(a => a.Id == id).Include(a => a.Books).FirstOrDefault();
        }

        public IReadOnlyList<Author> GetByName(string name)
        {
            return _context.Authors.Where(a =>a.Name == name).Include(a => a.Books).ToList();
        }

        public IReadOnlyList<Author> GetPaged(int skip, int take, string? nameContains = null)
        {
            if (string.IsNullOrEmpty(nameContains))
                return _context.Authors.OrderBy(a => a.Id).Skip(skip).Take(take).Include(a => a.Books).ToList();
            else
                return _context.Authors.Where(a => a.Name.Contains(nameContains)).OrderBy(a => a.Id).Skip(skip).Take(take).Include(a => a.Books).ToList();
        }

        public void Remove(Author author)
        {
            _context.Authors.Remove(author);
        }
    }
}
