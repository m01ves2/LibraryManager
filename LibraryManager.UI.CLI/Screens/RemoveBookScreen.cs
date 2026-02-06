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

        public RemoveBookScreen(IUnitOfWork uow) : base(uow)
        {
            _useCase = new RemoveBookUseCase(uow);
        }

        protected override void LoadData()
        {
            //For the future
            //int index = int.Parse(input) - 1;
            //if (index < 0 || index >= _data.Books.Count)
            //    return this;

            //var bookId = _data.Books[index].Id;
            //_removeBookUseCase.Execute(new RemoveBookRequest(bookId));

            Console.WriteLine("Input BookId:");
            string idInput = Console.ReadLine() ?? "";
            int id = int.Parse(idInput);

            Console.WriteLine("Input Title:");
            string? title = Console.ReadLine();

            var request = new RemoveBookRequest(id, title);
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
            Console.WriteLine("[0] - Main menu\n");
            Console.Write("\nSelect option: ");
        }

        protected override Screen HandleInput(string input)
        {
            switch (input) {
                case "0":
                    return new MainMenuScreen(_uow);
                default:
                    return this;
            }
        }
    }
}
