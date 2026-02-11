using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Interfaces;
using LibraryManager.UI.CLI.Contexts;

namespace LibraryManager.UI.CLI.Screens
{
    enum ListAuthorsStep
    {
        Confirm,
        Done
    };

    public class ListAuthorsScreen : Screen
    {
        protected override string Title => "AUTHORS";
        private ListAuthorsUseCase _useCase;
        private ListAuthorsContext _context;
        private readonly RemoveAuthorUseCase _removeAuthorUseCase;

        private ListAuthorsStep _currentStep;

        private ListAuthorsResult? _data;
        private ResultStatus _status;
        private string? _error;

        private int _selectedIndex = 0;

        public ListAuthorsScreen(ListAuthorsContext context, IUnitOfWork uow, Screen? previous) : base(uow, previous)
        {
            _context = context;
            _useCase = new ListAuthorsUseCase(uow);
            _removeAuthorUseCase = new RemoveAuthorUseCase(uow);
            _currentStep = ListAuthorsStep.Confirm;

            _error = null;
            _data = null;
        }

        protected override void LoadData()
        {
            if (_currentStep == ListAuthorsStep.Done)
                return;
            else if (_currentStep == ListAuthorsStep.Confirm) {
                var request = new ListAuthorsRequest(_context.PageNumber, _context.PageSize);
                var result = _useCase.Execute(request);
                _data = result.Data;
                _error = result.Message;
                _status = result.Status;

                _currentStep = ListAuthorsStep.Done;
                _selectedIndex = 0;
                SelectAuthor(_selectedIndex);
            }
        }

        protected override void RenderBody()
        {
            if (_currentStep == ListAuthorsStep.Done) {
                // Показываем результат UseCase
                if (_status == ResultStatus.Success && _data is not null) {
                    var authors = _data.Authors;
                    for (int i = 1; i <= authors.Count; i++) {
                        var ellipsis = authors[i - 1].BooksCount > 3 ? "..." : "";
                        string output = $"{authors[i - 1].Id}. {authors[i - 1].Name} - " +
                                            $"({authors[i - 1].BooksCount}) books - " +
                                            $"{string.Join(", ", authors[i - 1].SampleBooksTitles.Select(t => $"\"{t}\""))}{ellipsis}";

                        if (_context.Id is not null && authors[i - 1].Id == _context.Id) {
                            PrintSelected(output);
                        }
                        else
                            Console.WriteLine(output);
                    }

                    for (int i = 0; i < _data.PageSize - authors.Count; i++) {
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
                case ListAuthorsStep.Confirm:
                    break;

                case ListAuthorsStep.Done:
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
            Console.WriteLine("[1] - Add author");
            Console.WriteLine("[2] - Delete selected");
            Console.WriteLine("[3] - Modify selected");
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

            if (_currentStep == ListAuthorsStep.Done) {
                switch (input.ToUpper()) {
                    case "N":
                        if (_data is not null && _data.HasNextPage) {
                            _context.PageNumber++;
                            _currentStep = ListAuthorsStep.Confirm;
                            LoadData();
                        }
                        break;
                    case "P":
                        if (_data is not null && _data.HasPreviousPage) {
                            _context.PageNumber--;
                            _currentStep = ListAuthorsStep.Confirm;
                            LoadData();
                        }
                        break;
                    case "W":
                        if (_data is not null && _selectedIndex > 0)
                            SelectAuthor(--_selectedIndex);
                        break;
                    case "S":
                        if (_data is not null && _data.Authors.Count - 1 > _selectedIndex)
                            SelectAuthor(++_selectedIndex);
                        break;
                    case "1":   //Add author
                        return new AddAuthorScreen(new AddAuthorContext(), _uow, this);
                    case "2":   //Delete author
                        DeleteAuthor();
                        break;
                    case "3":   //TODO Modify author
                        //return new UpdateAuthorScreen(new UpdateAuthorContext(), _uow, this);
                        break;
                }
            }
            return this;
        }

        private void ResetScreen()
        {
            _currentStep = ListAuthorsStep.Confirm;

            _data = null;
            _error = null;
            _status = ResultStatus.Success;

            _context.PageNumber = 1;
            _context.Name = null;
            _context.Id = null;
        }

        private void SelectAuthor(int i)
        {
            if (_currentStep == ListAuthorsStep.Done && _data is not null && i < _data.Authors.Count && i >= 0) {
                _context.Id = _data.Authors[i].Id;
                _context.Name = _data.Authors[i].Name;
            }
        }

        private void DeleteAuthor()
        {
            if (_context.Id is not null) {
                var deleteRequest = new RemoveAuthorRequest((int)_context.Id);
                var deleteResult = _removeAuthorUseCase.Execute(deleteRequest);

                if (deleteResult.Status == ResultStatus.Success) {
                    Console.WriteLine($"Author {_context.Id} removed");
                    ResetScreen();
                }
                else {
                    Console.WriteLine(deleteResult.Message);
                }
            }
            else {
                Console.WriteLine("Select author to delete.");
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
