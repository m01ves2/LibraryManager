using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Interfaces;

namespace LibraryManager.UI.CLI.Screens
{
    public class AddBookScreen : Screen
    {
        protected override string Title => "ADD AUTHOR";
        private AddBookUseCase _useCase;
        private AddBookResult? _data;
        private string? _error;
        private ResultStatus _status;

        public AddBookScreen(IUnitOfWork uow) : base(uow)
        {
            _useCase = new AddBookUseCase(uow);
        }

        protected override void LoadData()
        {
            Console.Write("Input Title: ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Input Description (or leave empty): ");
            string description = Console.ReadLine() ?? "";

            Console.Write("Input AuthorName: ");
            string authorName = Console.ReadLine() ?? "";

            Console.Write("Input ISBN: ");
            string isbn = Console.ReadLine() ?? "";

            var request = new AddBookRequest(title, description, authorName, isbn);
            var result = _useCase.Execute(request);

            if (result.Status == ResultStatus.Success)
                _data = result.Data;
            else {
                _error = result.Message;
                _status = result.Status;
            }
        }

        protected override void RenderBody()
        {
            if (_status == ResultStatus.Success) {
                Console.WriteLine($"Book {_data?.Title} by {_data?.AuthorName}, ISBN: {_data?.Isbn}  added\n");
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
            if (_status == ResultStatus.AuthorMissing) {
                Console.WriteLine("[1] - Add author");
            }

            Console.WriteLine("[0] - Main menu\n");
            Console.Write("\nSelect option: ");
        }

        protected override Screen HandleInput(string input)
        {
            switch (input) {
                case "1":
                    if (_status == ResultStatus.AuthorMissing)
                        return new AddAuthorScreen(_uow);
                    else
                        return this;

                case "0":
                    return new MainMenuScreen(_uow);
                
                default:
                    return this;
            }
        }
    }
}
