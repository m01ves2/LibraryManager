using LibraryManager.Domain.Interfaces;
using LibraryManager.UI.CLI.Contexts;

namespace LibraryManager.UI.CLI.Screens
{
    public class MainMenuScreen : Screen
    {
        protected override string Title => "MAIN MENU";
        public MainMenuScreen(IUnitOfWork uow) : base(uow)
        {
        }

        protected override void RenderBody()
        {
            Console.WriteLine("Welcome to Library Manager");
        }

        protected override void RenderControls()
        {
            Console.WriteLine("[1] - list books");
            Console.WriteLine("[2] - list authors");
            Console.WriteLine("[3] - add book");
            Console.WriteLine("[4] - add author");
            Console.WriteLine("[5] - remove book");
            Console.WriteLine("[6] - remove author");
            Console.WriteLine("[0] - exit");
            
            Console.Write("\nSelect option: ");
        }

        protected override Screen? HandleInput(string input)
        {
            switch (input) {
                case "1":
                    return new ListBooksScreen(new ListBooksContext(1, 3), _uow);
                case "2":
                    return new ListAuthorsScreen(new ListAuthorsContext(1,3), _uow);
                case "3":
                    return new AddBookScreen(_uow);
                case "4":
                    return new AddAuthorScreen(_uow);
                case "5":
                    return new RemoveBookScreen(_uow);
                case "6":
                    return new RemoveAuthorScreen(_uow);
                case "0":
                    return null;
                default:
                    return this;
            }
        }
    }
}
