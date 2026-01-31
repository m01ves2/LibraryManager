using LibraryManager.Application.ViewModels;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Results;

namespace LibraryManager.Application.Mappers
{
    public class BookMapper
    {
        public static ViewBook ToViewBook(Book book)
            => new ViewBook()
            {
                Title = book.Title,
                Author = AuthorMapper.ToViewAuthor(book.Author),
                Description = book.Description,
                Isbn = IsbnMapper.ToViewIsbn(book.Isbn),
            };

        public static List<ViewBook> ToViewBooks(List<Book> books)
            => books.Select(ToViewBook).ToList();
    }
}
