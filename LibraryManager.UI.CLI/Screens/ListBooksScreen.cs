using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.UI.CLI.Contexts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryManager.UI.CLI.Screens
{
    public class ListBooksScreen : Screen
    {
        private ListBooksUseCase _useCase;
        private ListBooksContext _context;

        private ListBooksResult? _data;
        private string? _error;

        public ListBooksScreen( ListBooksContext context, IUnitOfWork uow) : base(uow)
        {
            _context = context;
            _useCase = new ListBooksUseCase(uow);
        }

        protected override string Title => "BOOKS";

        protected override void LoadData()
        {
            var request = new ListBooksRequest(_context.PageNumber, _context.PageSize);
            var result = _useCase.Execute(request);

            if (result.Status == ResultStatus.Success)
                _data = result.Data;
            else
                _error = result.Message;
        }

        protected override void RenderBody()
        {
            if (_data is null ) {
                if(_error is not null)
                    Console.WriteLine(_error);
                return;
            }

            var books = _data.Books;
            for (int i = 1; i <= books.Count; i++) {
                Console.WriteLine($"{i}. {books[i - 1].Title} by {books[i - 1].AuthorName}, ISBN: {books[i - 1].Isbn}");
            }

            for(int i = 0; i < _data.PageSize - books.Count; i++) {
                Console.WriteLine();
            }
        }

        protected override void RenderControls()
        {
            if (_data is null || _data.Books.Count == 0) {
                //Console.WriteLine("Press any key to go for main menu\n");
                //return;
            }
            else {
                Console.WriteLine($"Page: {_data.PageNumber}/{_data.TotalPages}\n");

                if (_data.HasPreviousPage || _data.HasNextPage) {
                    Console.WriteLine("[1] - previous page");
                    Console.WriteLine("[2] - next page");
                }
            }

            Console.WriteLine("[0] - Main menu\n");
            Console.Write("Select option: ");
        }

        protected override Screen HandleInput(string input)
        {
            if(_data is null) {
                return new MainMenuScreen(_uow);
            }

            switch (input) {
                case "1" when _data.HasPreviousPage:
                    var newContextPrev = new ListBooksContext(_data.PageNumber - 1, _context.PageSize);
                    return new ListBooksScreen(newContextPrev, _uow);


                case "2" when _data.HasNextPage:
                    var newContextNext = new ListBooksContext(_data.PageNumber + 1, _context.PageSize);
                    return new ListBooksScreen(newContextNext, _uow);

                case "0":
                    return new MainMenuScreen(_uow);
                default:
                    return this;
            }
        }
    }
}
