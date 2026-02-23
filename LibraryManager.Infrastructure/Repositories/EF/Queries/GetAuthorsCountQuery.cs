using LibraryManager.Application.Queries;

namespace LibraryManager.Infrastructure.Repositories.EF.Queries
{
    public class GetAuthorsCountQuery : IGetAuthorssCountQuery
    {
        private readonly LibraryDbContext _context;

        public GetAuthorsCountQuery(LibraryDbContext context)
        {
            _context = context;
        }
        public int Execute()
        {
            return _context.Authors.Count();
        }
    }
}
