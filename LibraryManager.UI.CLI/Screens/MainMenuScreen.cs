using LibraryManager.UI.CLI.Contexts;

namespace LibraryManager.UI.CLI.Screens
{
    public class MainMenuScreen : Screen
    {
        protected override string Title => "MAIN MENU";
        public MainMenuScreen(ScreenFactory factory, Screen? previous = null) : base(factory, previous)
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
                    return _factory.CreateListBooksScreen(new ListBooksContext(), this);
                case "2":
                    return _factory.CreateListAuthorsScreen(new ListAuthorsContext(), this);
                case "3":
                    return _factory.CreateAddBookScreen(new AddBookContext(), this );
                case "4":
                    return _factory.CreateAddAuthorScreen(new AddAuthorContext(), this);

                case "Q":
                    return null;
                default:
                    return this;
            }
        }
    }
}
