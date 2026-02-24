using LibraryManager.Application.Models;
using LibraryManager.Application.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Infrastructure.Repositories.EF.Repositories
{
    public class EfAuthorQueryRepository : IAuthorQueryRepository
    {
        private readonly LibraryDbContext _context;

        public EfAuthorQueryRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Authors.AsNoTracking().Count();
        }

        public IReadOnlyList<AuthorPreview> GetPaged(int skip, int take, string? nameContains = null)
        {
            var query = _context.Authors.AsNoTracking();

            if (!string.IsNullOrEmpty(nameContains)) {
                query = query.Where(a => a.Name.Contains(nameContains));
            }

            return query.OrderBy(a => a.Id)
                 .Skip(skip)
                 .Take(take)
                 .Select(a => new AuthorPreview(
                     a.Id,
                     a.Name,
                     a.Books.Count(),
                     a.Books
                         .OrderBy(b => b.Id)
                         .Take(3)
                         .Select(b => b.Title)
                         .ToList()
                 )).ToList();
        }
    }
}
