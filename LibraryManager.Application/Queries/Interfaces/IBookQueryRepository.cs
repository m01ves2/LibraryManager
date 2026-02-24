using LibraryManager.Application.Models;

namespace LibraryManager.Application.Queries.Interfaces
{
    public interface IBookQueryRepository
    {
        IReadOnlyList<BookPreview> GetPaged(int skip, int take, string? titleContains = null, string? authorNameContains = null, string? isbnContains = null );
        int Count();
    }
}
