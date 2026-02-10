using LibraryManager.Domain.Interfaces;

namespace LibraryManager.UI.CLI.Screens
{
    public class ErrorScreen : Screen
    {
        private string _message;
        public ErrorScreen(IUnitOfWork uow, string message, Screen? previous = null) : base(uow, previous)
        {
            _message = message;
        }

        protected override string Title => "ERROR!";

        protected override void RenderBody()
        {
            Console.WriteLine(_message);
        }

        protected override void RenderControls()
        {
            Console.WriteLine("[Q] - Main menu\n");
        }

        protected override Screen HandleInput(string input)
        {
            if (IsExitRequested(input))
                return new MainMenuScreen(_uow);

            switch (input) {
                default: 
                    return this;
            }
        }
    }
}
