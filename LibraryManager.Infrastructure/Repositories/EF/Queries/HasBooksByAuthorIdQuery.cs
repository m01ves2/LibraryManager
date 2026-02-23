using LibraryManager.Application.Queries;

namespace LibraryManager.Infrastructure.Repositories.EF.Queries
{
    public class HasBooksByAuthorIdQuery : IHasBooksByAuthorIdQuery
    {
        private readonly LibraryDbContext _context;
        public HasBooksByAuthorIdQuery(LibraryDbContext context)
        {
            _context = context;
        }
        public bool Execute(int authorId)
        {
            return _context.Books.Where(b => b.AuthorId == authorId).Any();
        }
    }
}
