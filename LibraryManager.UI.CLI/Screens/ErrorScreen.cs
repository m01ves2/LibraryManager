using LibraryManager.Domain.Interfaces;
using LibraryManager.Infrastructure.Repositories.EF;

namespace LibraryManager.UI.CLI.Screens
{
    public class ErrorScreen : Screen
    {
        private string _message;
        public ErrorScreen(LibraryDbContext dbContext, string message, Screen? previous = null) : base(dbContext, previous)
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
                return new MainMenuScreen(_dbContext);

            switch (input) {
                default: 
                    return this;
            }
        }
    }
}
