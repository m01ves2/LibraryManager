using LibraryManager.Application.Models;
using LibraryManager.Application.Queries;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Infrastructure.Repositories.EF.Queries
{
    public class GetAuthorsPagedQuery : IGetAuthorsPagedQuery
    {
        private readonly LibraryDbContext _context;

        public GetAuthorsPagedQuery(LibraryDbContext context)
        {
            _context = context;
        }
        public List<AuthorPreview> Execute(int skip, int take, string? searchTerm = null)
        {
            var query = _context.Authors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm)) {
                query = query.Where(a => a.Name.Contains(searchTerm));
            }

            return query
                .OrderBy(a => a.Id)
                .Include(b => b.Books)
                .Skip(skip)
                .Take(take)
                .Select(a => new AuthorPreview(
                    a.Id,
                    a.Name,
                    a.Books.Count,
                    a.Books.Select(b => b.Title).Take(3).ToList()
                )).ToList();
        }
    }
}
