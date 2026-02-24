using LibraryManager.Application.Models;
using LibraryManager.Application.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryManager.Infrastructure.Repositories.EF.Repositories
{
    public class EfBookQueryRepository : IBookQueryRepository
    {
        private readonly LibraryDbContext _context;

        public EfBookQueryRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Books.AsNoTracking().Count();
        }

        public IReadOnlyList<BookPreview> GetPaged(int skip, int take, string? titleContains = null, string? authorNameContains = null, string? isbnContains = null)
        {
            var query = _context.Books.Include(b=> b.Author).AsNoTracking();
            
            //.Where(...)
            if( !string.IsNullOrEmpty(titleContains)) {
                query = query.Where(b => b.Title.Contains(titleContains));
            }

            if (!string.IsNullOrEmpty(authorNameContains)) {
                query = query.Where(b => b.Author.Name.Contains(authorNameContains));
            }

            if (!string.IsNullOrEmpty(isbnContains)) {
                query = query.Where(b => b.Isbn != null && b.Isbn.Value.Contains(isbnContains));
            }

            return query.OrderBy(b => b.Id)
                .Skip(skip)
                .Take(take)
                .Select(b => new BookPreview(
                    b.Id,
                    b.Title,
                    b.Author.Name,
                    (b.Isbn == null? "" : b.Isbn.Value)
                )).ToList();
        }
    }
}
