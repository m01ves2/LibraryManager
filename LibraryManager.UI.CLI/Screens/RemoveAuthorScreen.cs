using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Interfaces;

namespace LibraryManager.UI.CLI.Screens
{
    public class RemoveAuthorScreen : Screen
    {
        protected override string Title => "REMOVE AUTHOR";
        private RemoveAuthorUseCase _useCase;
        
        private string? _error;
        private ResultStatus _status;

        public RemoveAuthorScreen(IUnitOfWork uow, Screen? previous) : base(uow, previous)
        {
            _useCase = new RemoveAuthorUseCase(uow);
        }

        protected override void LoadData()
        {
            _status = ResultStatus.Success;
            _error = null;

            //int index = int.Parse(input) - 1;
            //if (index < 0 || index >= _data.Books.Count)
            //    return this;

            //var bookId = _data.Books[index].Id;
            //_removeBookUseCase.Execute(new RemoveBookRequest(bookId));


            Console.WriteLine("Input AuthorId:");
            string idInput = Console.ReadLine() ?? "";

            if (!int.TryParse(idInput, out int id)) {
                _status = ResultStatus.InvalidInput;
                _error = "Invalid number";
                return;
            }

            var request = new RemoveAuthorRequest(id);
            var result = _useCase.Execute(request);

            if (result.Status != ResultStatus.Success) {
                _error = result.Message;
                _status = result.Status;
            }
        }

        protected override void RenderBody()
        {
            if (_status == ResultStatus.Success) {
                Console.WriteLine($"Author removed");
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
            if (_status == ResultStatus.NotFound) {
                Console.WriteLine("[1] - Remove book");
            }

            Console.WriteLine("[Q] - Main menu\n");
            Console.Write("\nSelect option: ");
        }

        protected override Screen HandleInput(string input)
        {
            switch (input.ToUpper()) {
                case "1":
                    if (_status == ResultStatus.NotFound)
                        return new RemoveBookScreen(_uow);
                    else
                        return this;
                case "Q":
                    return new MainMenuScreen(_uow);

                default:
                    return this;
            }
        }
    }
}
