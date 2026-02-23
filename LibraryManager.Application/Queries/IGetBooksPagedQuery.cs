using LibraryManager.Application.Models;

namespace LibraryManager.Application.Queries
{
    public interface IGetBooksPagedQuery
    {
        List<BookPreview> Execute(int skip, int take, string? searchTerm = null);
    }
}
