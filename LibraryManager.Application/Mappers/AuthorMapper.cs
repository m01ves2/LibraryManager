using LibraryManager.Application.ViewModels;
using LibraryManager.Domain.Entities;

namespace LibraryManager.Application.Mappers
{
    public  class AuthorMapper
    {
        public static ViewAuthor ToViewAuthor(Author author)
            => new ViewAuthor
            {
                Id = author.Id,
                Name = author.Name
            };
    }
}
