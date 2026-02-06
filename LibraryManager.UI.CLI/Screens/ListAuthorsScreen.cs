using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Interfaces;
using LibraryManager.UI.CLI.Contexts;

namespace LibraryManager.UI.CLI.Screens
{
    public class ListAuthorsScreen : Screen
    {
        protected override string Title => "AUTHORS";
        private ListAuthorsUseCase _useCase;
        private ListAuthorsContext _context;

        private ListAuthorsResult? _data;
        private ResultStatus _status;
        private string? _error;

        public ListAuthorsScreen(ListAuthorsContext context, IUnitOfWork uow) : base(uow)
        {
            _context = context;
            _useCase = new ListAuthorsUseCase(uow);
        }
        protected override void LoadData()
        {
            var request = new ListAuthorsRequest(_context.PageNumber, _context.PageSize);
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

            var authors = _data.Authors;
            for (int i = 1; i <= authors.Count; i++) {
                //Console.WriteLine($"{i + (_context.PageNumber - 1) * _context.PageSize}. {authors[i - 1].Name}");
                Console.WriteLine($"{authors[i-1].Id}. {authors[i - 1].Name}"); //we have to use Id's for CLI (Clean Architecture)
            }

            for (int i = 0; i < _data.PageSize - authors.Count; i++) {
                Console.WriteLine();
            }
        }

        protected override void RenderControls()
        {
            if (_data is null || _data.Authors.Count == 0) {

            }
            else {
                Console.WriteLine($"Page: {_data.PageNumber}/{_data.TotalPages}\n");

                if (_data.HasPreviousPage || _data.HasNextPage) {
                    Console.WriteLine("[1] - previous page");
                    Console.WriteLine("[2] - next page");
                }
            }

            Console.WriteLine("[0] - Main menu\n");
            Console.Write("\nSelect option: ");
        }

        protected override Screen HandleInput(string input)
        {
            if (_data is null) {
                if (input == "0")
                    return new MainMenuScreen(_uow);
                else
                    return this;
            }

            switch (input) {
                case "1" when _data.HasPreviousPage:
                    var newContextPrev = new ListAuthorsContext(_data.PageNumber - 1, _context.PageSize);
                    return new ListAuthorsScreen(newContextPrev, _uow);


                case "2" when _data.HasNextPage:
                    var newContextNext = new ListAuthorsContext(_data.PageNumber + 1, _context.PageSize);
                    return new ListAuthorsScreen(newContextNext, _uow);

                case "0":
                    return new MainMenuScreen(_uow);
                default:
                    return this;
            }
        }
    }
}
