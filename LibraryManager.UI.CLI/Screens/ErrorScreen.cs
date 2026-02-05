using LibraryManager.Domain.Interfaces;

namespace LibraryManager.UI.CLI.Screens
{
    public class ErrorScreen : Screen
    {
        public ErrorScreen(IUnitOfWork uow) : base(uow)
        {
        }

        protected override string Title => "ERROR";

        protected override Screen HandleInput(string input)
        {
            throw new NotImplementedException();
        }

        protected override void RenderBody()
        {
            throw new NotImplementedException();
        }
    }
}
