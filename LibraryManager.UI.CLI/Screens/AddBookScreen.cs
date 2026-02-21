using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Infrastructure.Repositories.EF;
using LibraryManager.UI.CLI.Contexts;

namespace LibraryManager.UI.CLI.Screens
{
    enum AddBookStep
    {
        Title,
        Description,
        Author,
        ISBN,
        Confirm,
        Done
    };

    public class AddBookScreen : Screen
    {
        protected override string Title => "ADD BOOK";
        private AddBookUseCase _useCase;
        private AddBookContext _context;
        private ListAuthorsContext _listAuthorsContext;

        private AddBookStep _currentStep;

        private AddBookResult? _data;
        private string? _error;
        private ResultStatus _status;

        public AddBookScreen(AddBookContext context, LibraryDbContext dbContext, Screen? previous) : base(dbContext, previous)
        {
            _useCase = new AddBookUseCase(dbContext);
            _listAuthorsContext = new ListAuthorsContext();
            _context = context;

            _currentStep = AddBookStep.Title;
            _data = null;
            _error = null;
        }

        public override Screen Run()
        {

            LoadData();

            Console.Clear();
            RenderHeader();
            RenderBreadScrumbs();
            RenderBody();
            RenderPrompt();

            var input = ReadInput();
            return HandleInput(input); // переключает _currentStep
        }

        protected override void LoadData()
        {
            if (_currentStep == AddBookStep.Done)
                return;

            if (_context.Title == null) {
                _currentStep = AddBookStep.Title;
            }
            else if (_context.Description == null) {
                _currentStep = AddBookStep.Description;
            }
            else if (_context.AuthorId == null) {
                if (_listAuthorsContext.Id != null) { //попытка получить данные из возможно вызванных экранов 
                    _context.AuthorId = _listAuthorsContext.Id;
                    _context.AuthorName = _listAuthorsContext.Name;
                    _currentStep = AddBookStep.ISBN;
                }
                else
                    _currentStep = AddBookStep.Author;
            }
            else {
                _currentStep = AddBookStep.Confirm;
            }
        }

        protected override void RenderBody()
        {
            Console.WriteLine($"Title: {_context.Title ?? "____"}");
            Console.WriteLine($"Description: {_context.Description ?? "____"}");
            Console.WriteLine($"Author: {_context.AuthorName ?? "____"}");
            Console.WriteLine($"ISBN: {_context.Isbn ?? "____"}");

            if (_currentStep == AddBookStep.Done) {
                // Показываем результат UseCase
                if (_status == ResultStatus.Success) {
                    Console.WriteLine($"\nBook '{_data?.Title}' by {_data?.AuthorName}, ISBN: {_data?.Isbn} added successfully.\n");
                }
                else {
                    Console.WriteLine($"\nERROR: {_error ?? "Unknown error"}\n");
                }
            }

            Console.WriteLine();
        }


        protected override void RenderPrompt()
        {
            switch (_currentStep) {
                case AddBookStep.Title:
                    Console.Write("Enter title (Q - Main menu): ");
                    break;

                case AddBookStep.Description:
                    Console.Write("Enter description (Q - Main menu): ");
                    break;

                case AddBookStep.Author:
                    Console.Write("Enter author name (Q - Main menu): ");
                    break;

                case AddBookStep.ISBN:
                    Console.Write("Enter ISBN (Q - Main menu): ");
                    break;

                case AddBookStep.Confirm:
                    Console.Write("Create this book? [Y/N] (Q - Main menu): ");
                    break;

                case AddBookStep.Done:
                    Console.WriteLine("[B] - Go back");
                    Console.WriteLine("[Q] - Exit");

                    Console.Write("\nSelect option: ");
                    break;
            }
        }

        protected override Screen HandleInput(string input)
        {
            if (IsExitRequested(input)) {
                ResetScreen();
                return new MainMenuScreen(_dbContext);
            }
            if (IsBackRequested(input)) {
                return _previous ?? new MainMenuScreen(_dbContext);
            }

            switch (_currentStep) {
                case AddBookStep.Title:
                    if (string.IsNullOrWhiteSpace(input))
                        return this;
                    _context.Title = input;
                    _currentStep = AddBookStep.Description;
                    return this;

                case AddBookStep.Description:
                    _context.Description = input ?? ""; //not requirable field
                    _currentStep = AddBookStep.Author;
                    break;

                case AddBookStep.Author:
                    if (string.IsNullOrWhiteSpace(input))
                        return this;
                    _context.AuthorName = input;

                    _listAuthorsContext.Name = input;
                    return new ListAuthorsScreen(_listAuthorsContext, _dbContext, this);

                case AddBookStep.ISBN:
                    _context.Isbn = string.IsNullOrWhiteSpace(input) ? null : input;
                    _currentStep = AddBookStep.Confirm;
                    break;

                case AddBookStep.Confirm:
                    if (input.ToUpper() != "Y" && input.ToUpper() != "YES")
                        return _previous ?? new MainMenuScreen(_dbContext);

                    var request = new AddBookRequest(_context.Title!, _context.Description, _context.AuthorId!.Value, _context.Isbn);
                    var result = _useCase.Execute(request);
                    _data = result.Data;
                    _error = result.Message;
                    _status = result.Status;

                    _currentStep = AddBookStep.Done;
                    break;

                case AddBookStep.Done:
                    break;
            }

            return this;
        }

        private void ResetScreen()
        {
            _currentStep = AddBookStep.Title;

            _data = null;
            _error = null;
            _status = ResultStatus.Success;

            _context = new AddBookContext(); //TODO - копирование по значению
            _listAuthorsContext = new ListAuthorsContext();
        }

    }
}
