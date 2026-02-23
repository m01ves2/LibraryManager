using LibraryManager.Domain.Entities;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Application.Queries
{
    public interface IGetBookByIsbnQuery
    {
        Book? Execute(Isbn isbn);
    }
}
