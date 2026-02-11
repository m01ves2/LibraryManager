using LibraryManager.Domain.Interfaces;
using LibraryManager.UI.CLI.Contexts;

namespace LibraryManager.UI.CLI.Screens
{
    public class MainMenuScreen : Screen
    {
        protected override string Title => "MAIN MENU";
        public MainMenuScreen(IUnitOfWork uow, Screen? previous = null) : base(uow, previous)
        {
        }

        protected override void RenderBody()
        {
            Console.WriteLine("Welcome to Library Manager");
        }

        protected override void RenderPrompt()
        {
            Console.WriteLine("[1] - List books");
            Console.WriteLine("[2] - List authors");
            Console.WriteLine("[3] - Add book");
            Console.WriteLine("[4] - Add author");
            Console.WriteLine("[Q] - Exit");
            
            Console.Write("\nSelect option: ");
        }

        protected override Screen? HandleInput(string input)
        {
            switch (input.ToUpper()) {
                case "1":
                    return new ListBooksScreen(new ListBooksContext(), _uow, this);
                case "2":
                    return new ListAuthorsScreen(new ListAuthorsContext(), _uow, this);
                case "3":
                    return new AddBookScreen(new AddBookContext(), _uow, this );
                case "4":
                    return new AddAuthorScreen(new AddAuthorContext(), _uow, this);
                //case "5":
                //    return new RemoveBookScreen(_uow);
                //case "6":
                //    return new RemoveAuthorScreen(_uow);
                case "Q":
                    return null;
                default:
                    return this;
            }
        }
    }
}
