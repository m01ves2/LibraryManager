using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Interfaces;

namespace LibraryManager.UI.CLI.Screens
{
    public class AddAuthorScreen : Screen
    {
        protected override string Title => "ADD AUTHOR";
        private AddAuthorUseCase _useCase;
        private AddAuthorResult? _data;
        private string? _error;
        private ResultStatus _status;

        public AddAuthorScreen(IUnitOfWork uow) : base(uow)
        {
            _useCase = new AddAuthorUseCase(uow);
        }

        protected override void LoadData()
        {
            Console.Write("Input author's Name: ");
            string name = Console.ReadLine() ?? "";

            var request = new AddAuthorRequest(name);
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
            if (_status == ResultStatus.Success) {
                Console.WriteLine($"Author {_data?.Name} added");
            }
            else {
                if (_error is not null) {
                    Console.WriteLine("ERROR: " + _error);
                }
                else {
                    Console.WriteLine("UNDEFINED ERROR");
                }
            }
        }
        protected override void RenderControls()
        {
            Console.WriteLine("[0] - Main menu\n");
            Console.Write("\nSelect option: ");
        }

        protected override Screen HandleInput(string input)
        {
            if (_data is null) {
                return new MainMenuScreen(_uow);
            }

            switch (input) {
                case "0":
                    return new MainMenuScreen(_uow);
                default:
                    return this;
            }
        }
    }
}
