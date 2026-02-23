using LibraryManager.Application.Models;

namespace LibraryManager.Application.Queries
{
    public interface IGetAuthorsPagedQuery
    {
        List<AuthorPreview> Execute(int skip, int take, string? searchTerm = null);
    }
}
