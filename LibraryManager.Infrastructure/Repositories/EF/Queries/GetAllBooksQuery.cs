using LibraryManager.Application.Queries;
using LibraryManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Infrastructure.Repositories.EF.Queries
{
    public class GetAllBooksQuery : IGetAllBooksQuery
    {
        private readonly LibraryDbContext _context;
        public GetAllBooksQuery(LibraryDbContext context) => _context = context;
        public List<Book> Execute() => _context.Books.Include(b => b.Author).ToList();
    }
}
