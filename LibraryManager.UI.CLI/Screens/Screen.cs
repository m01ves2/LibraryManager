using LibraryManager.Application.Results;
using LibraryManager.Domain.Interfaces;

namespace LibraryManager.UI.CLI.Screens
{
    public abstract class Screen
    {
        protected readonly Screen? _previous;
        protected readonly ScreenFactory _factory;
        protected abstract string Title { get; }

        public Screen(ScreenFactory factory, Screen? previous = null) 
        {
            _factory = factory;
            _previous = previous;
        }
        
        public virtual Screen? Run()
        {
            LoadData();
            
            Console.Clear();
            RenderHeader();
            RenderBreadScrumbs();
            RenderBody();
            RenderPrompt();

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
                current = current._previous;
            }
            Console.WriteLine(breadScrumbs);
            Console.WriteLine("----------------------------");
        }

        protected virtual Screen GetMainMenuScreen()
        {
            Screen current = this;
            while(current._previous != null) {
                current = current._previous;
            }
            return current;
        }

        protected abstract void RenderBody();
        protected void RenderError(ResultStatus status, string? error)
        {
            if (status == ResultStatus.Success) return;
            Console.WriteLine("ERROR: " + (error ?? "UNDEFINED ERROR"));
        }

        protected virtual void RenderPrompt() { }
        protected virtual string ReadInput()
        {
            return Console.ReadLine() ?? string.Empty;
        }
        protected abstract Screen? HandleInput(string input);

        protected bool IsExitRequested(string input)
        {
            return input.Equals("Q", StringComparison.OrdinalIgnoreCase);
        }

        protected bool IsBackRequested(string input)
        {
            return input.Equals("B", StringComparison.OrdinalIgnoreCase);
        }
    }
}
