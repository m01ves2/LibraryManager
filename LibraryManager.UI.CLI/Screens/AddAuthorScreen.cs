using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Interfaces;
using LibraryManager.UI.CLI.Contexts;

namespace LibraryManager.UI.CLI.Screens
{
    enum AddAuthorStep
    {
        Name,
        Confirm,
        Done
    };

    public class AddAuthorScreen : Screen
    {
        protected override string Title => "ADD AUTHOR";
        private AddAuthorUseCase _useCase;
        private AddAuthorContext _context;

        private AddAuthorStep _currentStep;

        private AddAuthorResult? _data;
        private string? _error;
        private ResultStatus _status;

        public AddAuthorScreen(AddAuthorContext context, IUnitOfWork uow, Screen? previous) : base(uow, previous)
        {
            _useCase = new AddAuthorUseCase(uow);
            _context = context;
        }

        protected override void LoadData()
        {
            if (_currentStep == AddAuthorStep.Done)
                return;

            if (_context.Name == null) {
                _currentStep = AddAuthorStep.Name;
            }
            else {
                _currentStep = AddAuthorStep.Confirm;
            }

        }

        protected override void RenderBody()
        {
            Console.WriteLine($"Name: {_context.Name ?? "____"}");

            if (_currentStep == AddAuthorStep.Done) {
                // Показываем результат UseCase
                if (_status == ResultStatus.Success) {
                    Console.WriteLine($"Author {_data?.Name} added");
                }
                else {
                    Console.WriteLine($"\nERROR: {_error ?? "Unknown error"}\n");
                }
            }

            Console.WriteLine();
            //RenderPrompt();
        }

        protected override void RenderPrompt()
        {
            switch (_currentStep) {
                case AddAuthorStep.Name:
                    Console.Write("Enter name (Q - Main menu): ");
                    break;
                case AddAuthorStep.Confirm:
                    Console.Write("Create this author? [Y/N] (Q - Main menu): ");
                    break;
                case AddAuthorStep.Done:
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
                return new MainMenuScreen(_uow);
            }
            if (IsBackRequested(input)) {
                return _previous ?? new MainMenuScreen(_uow);
            }

            switch (_currentStep) {
                case AddAuthorStep.Name:
                    if (string.IsNullOrWhiteSpace(input))
                        return this;
                    _context.Name = input;
                    _currentStep = AddAuthorStep.Confirm;
                    return this;

                case AddAuthorStep.Confirm:
                    if (input.ToUpper() != "Y" && input.ToUpper() != "YES")
                        return _previous ?? new MainMenuScreen(_uow);

                    var request = new AddAuthorRequest(_context.Name!);
                    var result = _useCase.Execute(request);
                    _data = result.Data;
                    _error = result.Message;
                    _status = result.Status;

                    _currentStep = AddAuthorStep.Done;
                    break;

                case AddAuthorStep.Done:
                    break;
            }
            return this;
        }

        private void ResetScreen() //TODO
        {
            _currentStep = AddAuthorStep.Name;

            _data = null;
            _error = null;
            _status = ResultStatus.Success;

            _context = new AddAuthorContext(); //TODO - копирование по значению
        }
    }

}
