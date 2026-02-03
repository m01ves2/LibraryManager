using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Infrastructure.Repositories;

namespace LibraryManager.UI.CLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var uow = new InMemoryUnitOfWork(); //TODO сделать по уму CR! сейчас у нас транзитивная зависимость от Infrastructure через другие слои!!
            var listBooksUseCase = new ListBooksUseCase(uow);

            AddMockData(uow);

            while (true) {
                //Console.Clear();
                Console.WriteLine("Input command (add author|book\nremove author|book\nlist authors|books\nexit):");
                var command = Console.ReadLine();
                switch (command) {
                    case "list books":
                        // выводим все книги из репозитория
                        ListBooksRequest listBookRequest = ListBookRequestMenu();
                        //ViewResult<List<ViewBook>> listResult = listHandler.Handle(listBookRequest);
                        OperationResult<ListBooksResult> listResult = listBooksUseCase.Execute(listBookRequest);

                        DisplayResult<ListBooksResult>(listResult, books => string.Join(Environment.NewLine, books.Books.Select(b => $"({b.Title} by {b.AuthorName}, ISBN: {b.Isbn})")));

                        break;
                    case "exit":
                        return;
                }
            }
        }

        //private static AddBookRequest AddBookMenu()
        //{
        //    Console.Clear();

        //    Console.WriteLine("Input Title: ");
        //    string? title = Console.ReadLine();

        //    Console.WriteLine("Input Description (or leave empty): ");
        //    string? description = Console.ReadLine();

        //    Console.WriteLine("Input AuthorId: ");
        //    string? idInput = Console.ReadLine();
        //    int.TryParse(idInput, out var authorId);

        //    Console.WriteLine("Input ISBN: ");
        //    string isbn = Console.ReadLine() ?? "";

        //    return new AddBookRequest() { Title = title, Description = description, Isbn = isbn, AuthorId = authorId };
        //}

        //private static RemoveBookRequest RemoveBookRequestMenu()
        //{
        //    Console.Clear();

        //    Console.WriteLine("Input ISBN (or leave empty):");
        //    string isbn = Console.ReadLine() ?? "";

        //    if (!string.IsNullOrEmpty(isbn)) {
        //        return new RemoveBookRequest() { Isbn = isbn };
        //    }

        //    Console.WriteLine("Input BookId (or leave empty):");
        //    string? idInput = Console.ReadLine();
        //    if (int.TryParse(idInput, out int bookId))
        //        return new RemoveBookRequest { BookId = bookId };

        //    Console.WriteLine("Input Title:");
        //    string? title = Console.ReadLine();
        //    return new RemoveBookRequest { Title = title };
        //}

        private static ListBooksRequest ListBookRequestMenu()
        {
            //throw new NotImplementedException();
            ListBooksRequest listBooksRequest = new ListBooksRequest(1, 50);
            return listBooksRequest;
        }

        private static void DisplayResult<T>(OperationResult<T> result, Func<T, string> formatter)
        {
            if (result.Status != ResultStatus.Success) {
                Console.WriteLine(result.Message);
                return;
            }

            if(result.Data != null)
                Console.WriteLine(formatter(result.Data));
        }


        private static void AddMockData(IUnitOfWork uow)
        {
            var author1 = new Author("Martin");
            var author2 = new Author("Hanonov");
            var author3 = new Author("Fauler");
            var author4 = new Author("Seemann");

            uow.Authors.Add(author1);
            uow.Authors.Add(author2);
            uow.Authors.Add(author3);
            uow.Authors.Add(author4);
            
            uow.Books.Add(new Book("Clean Code", "clean code book", author1, Domain.ValueObjects.Isbn.Parse("1234567890") ));
            uow.Books.Add(new Book("DDD Learning", "DDD book", author2, Domain.ValueObjects.Isbn.Parse("2134567890")));
            uow.Books.Add(new Book("DI", "book about DI", author4, Domain.ValueObjects.Isbn.Parse("9734567890")));
            uow.Books.Add(new Book("Refactoring", "book about refactoring", author3, Domain.ValueObjects.Isbn.Parse("8901234763")));

        }
    }
}