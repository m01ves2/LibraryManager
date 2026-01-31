using LibraryManager.Application.ViewModels;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Application.Mappers
{
    public class IsbnMapper
    {
        public static ViewIsbn ToViewIsbn(Isbn isbn)
            => new ViewIsbn() { Value = isbn.Value };
    }
}
