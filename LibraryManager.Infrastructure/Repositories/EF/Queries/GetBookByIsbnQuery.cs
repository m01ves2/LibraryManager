using LibraryManager.Application.Queries;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Infrastructure.Repositories.EF.Queries
{
    public class GetBookByIsbnQuery : IGetBookByIsbnQuery
    {
        private readonly LibraryDbContext _context;
        public GetBookByIsbnQuery(LibraryDbContext context) => _context = context;

        public Book? Execute(Isbn isbn)
        {
            return _context.Books.Where(b => b.Isbn != null && b.Isbn.Value == isbn.Value).Include(b => b.Author).FirstOrDefault();
        }
    }
}
