using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Interfaces;
using LibraryManager.UI.CLI.Contexts;

namespace LibraryManager.UI.CLI.Screens
{
    public class ListBooksScreen : Screen
    {
        protected override string Title => "BOOKS";
        private ListBooksUseCase _useCase;
        private ListBooksContext _context;
        private ResultStatus _status;

        private ListBooksResult? _data;
        private string? _error;

        public ListBooksScreen(ListBooksContext context, IUnitOfWork uow, Screen? previous) : base(uow, previous)
        {
            _context = context;
            _useCase = new ListBooksUseCase(uow);
        }

        protected override void LoadData()
        {
            _status = ResultStatus.Success;
            _error = null;
            _data = null;

            var request = new ListBooksRequest(_context.PageNumber, _context.PageSize);
            var result = _useCase.Execute(request);

            if (result.Status == ResultStatus.Success)
                _data = result.Data;
            else {
                _status = result.Status;
                _error = result.Message;
            }
        }

        protected override void RenderBody()
        {
            if (_status != ResultStatus.Success) {
                if (_error is not null)
                    Console.WriteLine("ERROR: " + _error);
                else {
                    Console.WriteLine("UNDEFINED ERROR");
                }
                return;
            }

            if (_data == null)
                return;

            var books = _data.Books;
            for (int i = 1; i <= books.Count; i++) {
                //Console.WriteLine($"{i + (_context.PageNumber - 1)*_context.PageSize}. {books[i - 1].Title} by {books[i - 1].AuthorName}, ISBN: {books[i - 1].Isbn}");
                Console.WriteLine($"{books[i - 1].Id}. {books[i - 1].Title} by {books[i - 1].AuthorName}, ISBN: {books[i - 1].Isbn}"); //we have to use Id's for CLI (Clean Architecture)
            }

            for (int i = 0; i < _data.PageSize - books.Count; i++) {
                Console.WriteLine();
            }
        }

        protected override void RenderControls()
        {
            if (_data is null || _data.Books.Count == 0) {

            }
            else {
                Console.WriteLine($"Page: {_data.PageNumber}/{_data.TotalPages}\n");

                if (_data.HasPreviousPage || _data.HasNextPage) {
                    Console.WriteLine("[P] - previous page");
                    Console.WriteLine("[N] - next page");
                }
            }

            Console.WriteLine("[Q] - Main menu\n");
            Console.Write("\nSelect option: ");
        }

        protected override Screen HandleInput(string input)
        {
            if (_data is null) {
                    return this;
            }

            switch (input.ToUpper()) {
                case "P" when _data.HasPreviousPage:
                    var newContextPrev = new ListBooksContext(_data.PageNumber - 1, _context.PageSize);
                    return new ListBooksScreen(newContextPrev, _uow);
                case "N" when _data.HasNextPage:
                    var newContextNext = new ListBooksContext(_data.PageNumber + 1, _context.PageSize);
                    return new ListBooksScreen(newContextNext, _uow);
                case "Q":
                    return new MainMenuScreen(_uow);
                default:
                    return this;
            }
        }
    }
}
