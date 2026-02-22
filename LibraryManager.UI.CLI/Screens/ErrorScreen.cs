using LibraryManager.Domain.Interfaces;

namespace LibraryManager.UI.CLI.Screens
{
    public class ErrorScreen : Screen
    {
        private string _message;
        public ErrorScreen(string message, ScreenFactory factory, Screen? previous = null) : base(factory, previous)
        {
            _message = message;
        }

        protected override string Title => "ERROR!";

        protected override void RenderBody()
        {
            Console.WriteLine(_message);
        }

        protected override void RenderPrompt()
        {
            Console.WriteLine("[Q] - Exit");

            Console.Write("\nSelect option: ");
        }

        protected override Screen HandleInput(string input)
        {
            if (IsExitRequested(input))
                return GetMainMenuScreen();

            switch (input) {
                default: 
                    return this;
            }
        }
    }
}
