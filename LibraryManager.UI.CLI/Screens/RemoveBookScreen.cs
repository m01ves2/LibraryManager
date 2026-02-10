using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Interfaces;

namespace LibraryManager.UI.CLI.Screens
{
    public class RemoveBookScreen : Screen
    {
        protected override string Title => "REMOVE BOOK";
        private RemoveBookUseCase _useCase;
        
        private string? _error;
        private ResultStatus _status;

        public RemoveBookScreen(IUnitOfWork uow, Screen? previous) : base(uow, previous)
        {
            _useCase = new RemoveBookUseCase(uow);
        }

        protected override void LoadData()
        {
            _status = ResultStatus.Success;
            _error = null;

            Console.WriteLine("Input BookId:");
            string idInput = Console.ReadLine() ?? "";

            if (!int.TryParse(idInput, out int id)) {
                _status = ResultStatus.InvalidInput;
                _error = "Invalid number";
                return;
            }


            var request = new RemoveBookRequest(id);
            var result = _useCase.Execute(request);

            if (result.Status != ResultStatus.Success) {
                _error = result.Message;
                _status = result.Status;
            }
        }

        protected override void RenderBody()
        {
            if (_status == ResultStatus.Success) {
                Console.WriteLine($"Book removed");
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
            Console.WriteLine("[Q] - Main menu\n");
            Console.Write("\nSelect option: ");
        }

        protected override Screen HandleInput(string input)
        {
            switch (input.ToUpper()) {
                case "Q":
                    return new MainMenuScreen(_uow, null);
                default:
                    return this;
            }
        }
    }
}
