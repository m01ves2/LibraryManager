using LibraryManager.Application.Models;

namespace LibraryManager.Application.Queries.Interfaces
{
    public interface IAuthorQueryRepository
    {
        IReadOnlyList<AuthorPreview> GetPaged(int skip, int take, string? nameContains = null);
        int Count();
    }
}
