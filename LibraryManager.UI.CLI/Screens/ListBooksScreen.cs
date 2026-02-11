using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Interfaces;
using LibraryManager.UI.CLI.Contexts;

namespace LibraryManager.UI.CLI.Screens
{
    enum ListBooksStep
    {
        Confirm,
        Done
    };
    public class ListBooksScreen : Screen
    {
        protected override string Title => "BOOKS";
        private ListBooksUseCase _useCase;
        private ListBooksContext _context;

        private ListBooksStep _currentStep;

        private ListBooksResult? _data;
        private ResultStatus _status;
        private string? _error;

        public ListBooksScreen(ListBooksContext context, IUnitOfWork uow, Screen? previous) : base(uow, previous)
        {
            _context = context;
            _useCase = new ListBooksUseCase(uow);
            _currentStep = ListBooksStep.Confirm;

            _error = null;
            _data = null;
        }

        protected override void LoadData()
        {
            if (_currentStep == ListBooksStep.Done)
                return;
            else if(_currentStep == ListBooksStep.Confirm) {
                LoadPage();
            }

        }

        private void LoadPage()
        {
            var request = new ListBooksRequest(_context.PageNumber, _context.PageSize);
            var result = _useCase.Execute(request);
            _data = result.Data;
            _error = result.Message;
            _status = result.Status;

            _currentStep = ListBooksStep.Done;
        }

        protected override void RenderBody()
        {
            if (_currentStep == ListBooksStep.Done) {
                // Показываем результат UseCase
                if (_status == ResultStatus.Success && _data is not null) {
                    var books = _data.Books;
                    for (int i = 1; i <= books.Count; i++) {
                        //Console.WriteLine($"{i + (_context.PageNumber - 1)*_context.PageSize}. {books[i - 1].Title} by {books[i - 1].AuthorName}, ISBN: {books[i - 1].Isbn}");
                        
                        string output = $"{books[i - 1].Id}. {books[i - 1].Title} by {books[i - 1].AuthorName}, ISBN: {books[i - 1].Isbn}"; //we have to use Id's for CLI (Clean Architecture)

                        if (_context.Id is not null && books[i - 1].Id == _context.Id) {
                            PrintSelected(output);
                        }
                        else
                            Console.WriteLine(output);
                    }

                    for (int i = 0; i < _data.PageSize - books.Count; i++) {
                        Console.WriteLine();
                    }

                    Console.WriteLine($"Page: {_data.PageNumber}/{_data.TotalPages}\n");
                }
                else {
                    Console.WriteLine($"\nERROR: {_error ?? "Unknown error"}\n");
                }
            }

            RenderControls();
            Console.WriteLine();

        }
        private void RenderControls()
        {
            switch (_currentStep) {
                case ListBooksStep.Done:
                    if (_data is not null) {
                        if (_data.HasPreviousPage || _data.HasNextPage) {
                            Console.WriteLine("[P] - previous page");
                            Console.WriteLine("[N] - next page");
                        }
                    }
                    break;
            }
        }

        protected override void RenderPrompt()
        {
            Console.WriteLine("[1] - Select book");
            Console.WriteLine("[2] - Add book");
            Console.WriteLine("[3] - Delete book");
            Console.WriteLine("[4] - Modify Book");
            Console.WriteLine("[B] - Go back");
            Console.WriteLine("[Q] - Main menu");

            Console.Write("\nSelect option: ");
        }

        protected override Screen HandleInput(string input)
        {
            if (IsExitRequested(input)) {
                ResetScreen();
                return new MainMenuScreen(_uow);
            }
            if (IsBackRequested(input)) {
                return _previous ?? new MainMenuScreen(_uow);
            }

            if (_currentStep == ListBooksStep.Done) {
                switch (input.ToUpper()) {
                    case "N":
                        if (_data is not null && _data.HasNextPage) {
                            _context.PageNumber++;
                            LoadPage();
                        }
                        break;
                    case "P":
                        if (_data is not null && _data.HasPreviousPage) {
                            _context.PageNumber--;
                            LoadPage();
                        }
                        break;
                    case "1":
                        //TODO select record
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                }
            }
            return this;
        }

        private void ResetScreen() //TODO
        {
            _currentStep = ListBooksStep.Confirm;

            _data = null;
            _error = null;
            _status = ResultStatus.Success;

            _context = new ListBooksContext();
        }

        private void PrintSelected(string output)
        {
            var defaultColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine(output);
            Console.BackgroundColor = defaultColor;
        }
    }
}
