using LibraryManager.Application.Commands.AddAuthor;
using LibraryManager.Application.Commands.AddBook;
using LibraryManager.Application.Commands.RemoveAuthor;
using LibraryManager.Application.Commands.RemoveBook;
using LibraryManager.Application.Queries.ListAuthors;
using LibraryManager.Application.Queries.ListBooks;
using LibraryManager.UI.CLI.Contexts;

namespace LibraryManager.UI.CLI.Screens
{
    public class ScreenFactory
    {
        private readonly AddAuthorUseCase _addAuthorUseCase;
        private readonly AddBookUseCase _addBookUseCase;
        private readonly ListAuthorsUseCase _listAuthorsUseCase;
        private readonly ListBooksUseCase _listBooksUseCase;
        private readonly RemoveAuthorUseCase _removeAuthorUseCase;
        private readonly RemoveBookUseCase _removeBookUseCase;

        public ScreenFactory(
                               AddAuthorUseCase addAuthorUseCase,
                               AddBookUseCase addBookUseCase,
                               ListAuthorsUseCase listAuthorsUseCase,
                               ListBooksUseCase listBooksUseCase,
                               RemoveAuthorUseCase removeAuthorUseCase,
                               RemoveBookUseCase removeBookUseCase)
        {
            _addAuthorUseCase = addAuthorUseCase;
            _addBookUseCase = addBookUseCase;
            _listAuthorsUseCase = listAuthorsUseCase;
            _listBooksUseCase = listBooksUseCase;
            _removeAuthorUseCase = removeAuthorUseCase;
            _removeBookUseCase = removeBookUseCase;
        }

        public Screen CreateAddAuthorScreen(AddAuthorContext addAuthorContext, Screen? prev)
        {
            return new AddAuthorScreen(addAuthorContext, _addAuthorUseCase, this, prev);
        }

        public Screen CreateAddBookScreen(AddBookContext addBookContext, Screen? prev)
        {
            return new AddBookScreen(addBookContext, _addBookUseCase, this, prev);
        }

        public Screen CreateListAuthorsScreen(ListAuthorsContext listAuthorsContext, Screen? prev)
        {
            return new ListAuthorsScreen(listAuthorsContext, _listAuthorsUseCase, _removeAuthorUseCase, this, prev);
        }

        public Screen CreateListBooksScreen(ListBooksContext listBooksContext, Screen? prev)
        {
            return new ListBooksScreen(listBooksContext, _listBooksUseCase, _removeBookUseCase, this, prev);
        }
    }
}
