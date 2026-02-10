using LibraryManager.Application.Results;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Infrastructure.Repositories;

namespace LibraryManager.UI.CLI.Screens
{
    public abstract class Screen
    {
        protected readonly IUnitOfWork _uow;
        protected readonly Screen? _previous;
        protected abstract string Title { get; }
        public Screen(IUnitOfWork uow, Screen? previous = null) 
        {
            _uow = uow;
            _previous = previous;
        }
        
        public virtual Screen? Run()
        {
            LoadData();
            
            Console.Clear();
            RenderHeader();
            RenderBreadScrumbs();
            RenderBody();
            RenderControls();

            var input = ReadInput();
            return HandleInput(input);
        }

        protected virtual void LoadData() { }

        protected virtual void RenderHeader()
        {
            if (!string.IsNullOrWhiteSpace(Title)) {
                Console.WriteLine("============================");
                Console.WriteLine($"\t{Title}");
                Console.WriteLine("============================");
                Console.WriteLine();
            }
        }
        protected virtual void RenderBreadScrumbs() {
            Screen? current = this;
            string breadScrumbs = "";
            while (current != null) {
                breadScrumbs = current.Title + " > " + breadScrumbs;
                current = _previous;
            }
        }
        protected abstract void RenderBody();
        protected void RenderError(ResultStatus status, string? error)
        {
            if (status == ResultStatus.Success) return;
            Console.WriteLine("ERROR: " + (error ?? "UNDEFINED ERROR"));
        }

        protected virtual void RenderControls() { }
        //protected virtual void RenderPrompt(string message) { }
        protected virtual string ReadInput()
        {
            return Console.ReadLine() ?? string.Empty;
        }
        protected abstract Screen? HandleInput(string input);

        protected bool IsExitRequested(string input)
        {
            return input.Equals("Q", StringComparison.OrdinalIgnoreCase);
        }
    }
}
