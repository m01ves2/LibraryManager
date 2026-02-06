using LibraryManager.Domain.Interfaces;
using LibraryManager.Infrastructure.Repositories;

namespace LibraryManager.UI.CLI.Screens
{
    public abstract class Screen
    {
        protected readonly IUnitOfWork _uow;
        protected abstract string Title { get; }
        public Screen(IUnitOfWork uow) 
        {
            _uow = uow;
        }
        
        public Screen Run()
        {
            Console.Clear();
            RenderHeader();
            LoadData();
            RenderBody();
            RenderControls();

            var input = ReadInput();
            return HandleInput(input);
        }

        protected virtual void LoadData() { }
        protected virtual void RenderHeader()
        {
            Console.WriteLine("============================");
            Console.WriteLine($"\t{Title}");
            RenderHeaderDetails();
            Console.WriteLine("============================");
            Console.WriteLine();
        }

        protected virtual void RenderHeaderDetails() { }
        protected abstract void RenderBody();
        protected virtual void RenderControls() { }

        protected virtual string ReadInput()
        {
            return Console.ReadLine() ?? string.Empty;
        }
        protected abstract Screen? HandleInput(string input);
    }
}
