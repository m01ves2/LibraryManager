using LibraryManager.Application.Models;
using LibraryManager.Application.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LibraryManager.Infrastructure.Repositories.EF.Queries
{
    public class GetBooksPagedQuery : IGetBooksPagedQuery
    {
        private readonly LibraryDbContext _context;

        public GetBooksPagedQuery(LibraryDbContext context)
        {
            _context = context;
        }

        public List<BookPreview> Execute(int skip, int take, string? searchTerm = null)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm)) {
                query = query.Where(b => b.Title.Contains(searchTerm));
            }

            return query
                .OrderBy(b => b.Id)
                .Skip(skip)
                .Take(take)
                .Select(b => new BookPreview(
                    b.Id,
                    b.Title,
                    b.Author.Name,
                    b.Isbn.Value
                )).ToList();
        }
    }
}
