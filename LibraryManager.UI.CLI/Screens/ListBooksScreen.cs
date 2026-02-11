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
        private readonly RemoveBookUseCase _removeBookUseCase;

        private ListBooksStep _currentStep;

        private ListBooksResult? _data;
        private ResultStatus _status;
        private string? _error;

        private int _selectedIndex = 0;

        public ListBooksScreen(ListBooksContext context, IUnitOfWork uow, Screen? previous) : base(uow, previous)
        {
            _context = context;
            _useCase = new ListBooksUseCase(uow);
            _removeBookUseCase = new RemoveBookUseCase(uow);
            _currentStep = ListBooksStep.Confirm;

            _error = null;
            _data = null;
        }

        protected override void LoadData()
        {
            if (_currentStep == ListBooksStep.Done)
                return;
            else if(_currentStep == ListBooksStep.Confirm) {
                var request = new ListBooksRequest(_context.PageNumber, _context.PageSize);
                var result = _useCase.Execute(request);
                _data = result.Data;
                _error = result.Message;
                _status = result.Status;

                _currentStep = ListBooksStep.Done;
                _selectedIndex = 0;
                SelectBook(_selectedIndex);
            }

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

                    if(_data.TotalPages > 0)
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
                case ListBooksStep.Confirm:
                    break;

                case ListBooksStep.Done:
                    if (_data is not null) {
                        if (_data.HasPreviousPage || _data.HasNextPage) {
                            Console.WriteLine("[P] - previous page, [N] - next page");
                        }
                        Console.WriteLine("[W] - select prev, [S] - select next");
                    }
                    break;
            }
        }

        protected override void RenderPrompt()
        {
            Console.WriteLine("[1] - Add book");
            Console.WriteLine("[2] - Delete book");
            Console.WriteLine("[3] - Modify Book");
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
                            _currentStep = ListBooksStep.Confirm;
                            LoadData();
                        }
                        break;
                    case "P":
                        if (_data is not null && _data.HasPreviousPage) {
                            _context.PageNumber--;
                            _currentStep = ListBooksStep.Confirm;
                            LoadData();
                        }
                        break;
                    case "W":
                        if (_data is not null && _selectedIndex > 0)
                            SelectBook(--_selectedIndex);
                        break;
                    case "S":
                        if (_data is not null && _data.Books.Count - 1 > _selectedIndex)
                            SelectBook(++_selectedIndex);
                        break;

                    case "1":
                        return new AddBookScreen(new AddBookContext(), _uow, this);
                    case "2":
                        DeleteBook();
                        break;
                    case "3"://TODO Modify book
                        //return new UpdateBookScreen(new UpdateBookContext(), _uow, this);
                        break;
                }
            }
            return this;
        }

        private void ResetScreen()
        {
            _currentStep = ListBooksStep.Confirm;

            _data = null;
            _error = null;
            _status = ResultStatus.Success;

            _context.PageNumber = 1;
            _context.Title = null;
            _context.Isbn = null;
            _context.AuthorName = null;
            _context.Description = null;
        }

        private void SelectBook(int i)
        {
            if (_currentStep == ListBooksStep.Done && _data is not null && i < _data.Books.Count && i >= 0) {
                _context.Id = _data.Books[i].Id;
                _context.Title = _data.Books[i].Title;
                _context.Isbn = _data.Books[i].Isbn;
                _context.AuthorName = _data.Books[i].AuthorName;
                //_context.Description = _data.Books[i].Description;
            }
        }

        private void DeleteBook()
        {
            if (_context.Id is not null) {
                var deleteRequest = new RemoveBookRequest((int)_context.Id);
                var deleteResult = _removeBookUseCase.Execute(deleteRequest);

                if (deleteResult.Status == ResultStatus.Success) {
                    Console.WriteLine($"Book {_context.Id} removed");
                    ResetScreen();
                }
                else {
                    Console.WriteLine(deleteResult.Message);
                }
            }
            else {
                Console.WriteLine("Select book to delete.");
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
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
