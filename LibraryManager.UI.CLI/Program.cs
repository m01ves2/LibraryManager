using LibraryManager.Application.Handlers;
using LibraryManager.Application.Requests;
using LibraryManager.Application.UseCases;
using LibraryManager.Infrastructure.Repositories;

namespace LibraryManager.UI.CLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var repository = new InMemoryLibraryRepository();
            var addHandler = new AddBookHandler(repository);
            var removeHandler = new RemoveBookHandler(repository);

            while (true) {
                Console.Clear();
                Console.WriteLine("Input command (add/remove/list/exit):");
                var command = Console.ReadLine();
                switch (command) {
                    case "add":
                        // собираем данные из консоли и вызываем addHandler.Handle
                        AddBookRequest addBookRequest = AddBookMenu();
                        addHandler.Handle(addBookRequest);
                        break;
                    case "remove":
                        // собираем данные (BookId, Title или Isbn) и вызываем removeHandler.Handle
                        RemoveBookRequest removeBookRequest = RemoveBookRequestMenu();
                        removeHandler.Handle(removeBookRequest);
                        break;
                    case "list":
                        // выводим все книги из репозитория
                        //TODO
                        break;
                    case "exit":
                        return;
                }
            }
        }

        private static AddBookRequest AddBookMenu()
        {
            Console.Clear();

            Console.WriteLine("Input Title: ");
            string? title = Console.ReadLine();

            Console.WriteLine("Input Description (or leave empty): ");
            string? description = Console.ReadLine();

            Console.WriteLine("Input AuthorId: ");
            string? idInput = Console.ReadLine();
            int.TryParse(idInput, out var authorId);

            Console.WriteLine("Input ISBN: ");
            string isbn = Console.ReadLine() ?? "";

            return new AddBookRequest() { Title = title, Description = description, Isbn = isbn, AuthorId = authorId };
        }

        private static RemoveBookRequest RemoveBookRequestMenu()
        {
            Console.Clear();

            Console.WriteLine("Input ISBN (or leave empty):");
            string isbn = Console.ReadLine() ?? "";

            if (!string.IsNullOrEmpty(isbn)) {
                return new RemoveBookRequest() { Isbn = isbn };
            }

            Console.WriteLine("Input BookId (or leave empty):");
            string? idInput = Console.ReadLine();
            if (int.TryParse(idInput, out int bookId))
                return new RemoveBookRequest { BookId = bookId };

            Console.WriteLine("Input Title:");
            string? title = Console.ReadLine();
            return new RemoveBookRequest { Title = title };
        }

    }
}