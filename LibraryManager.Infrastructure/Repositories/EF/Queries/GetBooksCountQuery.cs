using LibraryManager.Application.Queries;

namespace LibraryManager.Infrastructure.Repositories.EF.Queries
{
    public class GetBooksCountQuery : IGetBooksCountQuery
    {
        private readonly LibraryDbContext _context;

        public GetBooksCountQuery(LibraryDbContext context)
        {
            _context = context;
        }

        public int Execute()
        {
            return _context.Books.Count();
        }
    }
}
