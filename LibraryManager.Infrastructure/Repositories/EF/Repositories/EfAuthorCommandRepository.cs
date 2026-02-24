using LibraryManager.Application.Commands.Interfaces;
using LibraryManager.Domain.Entities;

namespace LibraryManager.Infrastructure.Repositories.EF.Repositories
{
    public class EfAuthorCommandRepository : IAuthorCommandRepository
    {
        private readonly LibraryDbContext _context;
        public EfAuthorCommandRepository(LibraryDbContext context) 
        { 
            _context = context;
        }

        public void Add(Author author)
        {
            _context.Authors.Add(author);
        }

        public Author? GetById(int id)
        {
            return _context.Authors.FirstOrDefault(a => a.Id == id);
        }

        public void Remove(Author author)
        {
            _context.Authors.Remove(author);
        }
    }
}
