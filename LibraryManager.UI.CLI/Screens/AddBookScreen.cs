using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Interfaces;
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

        AddBookStep _currentStep;

        private AddBookResult? _data;
        private string? _error;
        private ResultStatus _status;

        public AddBookScreen(AddBookContext context, IUnitOfWork uow, Screen? previous) : base(uow, previous)
        {
            _useCase = new AddBookUseCase(uow);
            _context = context;
            _listAuthorsContext = new ListAuthorsContext(1, 10);
        }

        public override Screen Run() //TODO
        {
            
            LoadData();

            Console.Clear();
            RenderHeader();
            RenderBreadScrumbs();
            RenderBody();
            RenderControls();

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
                if(_listAuthorsContext.AuthorId != null) { //попытка получить данные из возможно вызванных экранов 
                    _context.AuthorId = _listAuthorsContext.AuthorId;
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
            RenderPrompt();
        }

        protected override void RenderControls()
        {
            Console.WriteLine("[Q] - Main menu\n");
        }

        private void RenderPrompt()
        {
            switch (_currentStep) {
                case AddBookStep.Title:
                    Console.Write("Enter title: ");
                    break;

                case AddBookStep.Description:
                    Console.Write("Enter description: ");
                    break;

                case AddBookStep.Author:
                    Console.Write("Enter author name: ");
                    break;

                case AddBookStep.ISBN:
                    Console.Write("Enter ISBN: ");
                    break;

                case AddBookStep.Confirm:
                    Console.Write("Create this book? [Y/N] (Q - Main menu): ");
                    break;

                case AddBookStep.Done:
                    Console.Write("Press Q to return to Main menu: ");
                    break;
            }
        }

        protected override Screen HandleInput(string input)
        {
            if (IsExitRequested(input))
                return new MainMenuScreen(_uow);

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
                    if(string.IsNullOrWhiteSpace(input))
                        return this;
                    _context.AuthorName = input;

                    _listAuthorsContext.AuthorName = input;
                    return new ListAuthorsScreen(_listAuthorsContext, _uow, this);

                case AddBookStep.ISBN:
                    _context.Isbn = string.IsNullOrWhiteSpace(input) ? null : input;
                    _currentStep = AddBookStep.Confirm;
                    break;

                case AddBookStep.Confirm:
                    if (input.ToUpper() != "Y" && input.ToUpper() != "YES")
                        return new MainMenuScreen(_uow);

                    //// тут подтверждение и вызов UseCase
                    //if (_context.Title == null || _context.AuthorId == null) { //проверка на всякий случай
                    //    return new ErrorScreen(_uow, "Title or Author is missing. Can't create a new book", this);
                    //}

                    var request = new AddBookRequest(_context.Title!, _context.Description, _context.AuthorId!.Value, _context.Isbn); //что делать с -1 ??
                    var result = _useCase.Execute(request);
                    _data = result.Data;
                    _error = result.Message;
                    _status = result.Status;

                    _currentStep = AddBookStep.Done;
                    break;

                case AddBookStep.Done: //TODO - возвращаться не в MainMenuScreen, а в родительский экран, с очищением контекстов. чтобы массово CRUD книги, прямо из ListBooksScreen
                    break;
            }

            return this;
        }
    }
}
