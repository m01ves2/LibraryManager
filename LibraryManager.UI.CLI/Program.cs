using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Infrastructure.Repositories;
using LibraryManager.UI.CLI.Screens;
using System.Net;

namespace LibraryManager.UI.CLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var uow = new InMemoryUnitOfWork(); //TODO сделать по уму CR! сейчас у нас транзитивная зависимость от Infrastructure через другие слои!!
            var listBooksUseCase = new ListBooksUseCase(uow);
            var listAuthorsUseCase = new ListAuthorsUseCase(uow);
            var addAuthorUseCase = new AddAuthorUseCase(uow);
            var addBookUseCase = new AddBookUseCase(uow);
            var removeAuthorUseCase = new RemoveAuthorUseCase(uow);
            var removeBookUseCase = new RemoveBookUseCase(uow);

            AddMockData(uow);

            //    while (true) {
            //        //Console.Clear();
            //        Console.WriteLine("Input command:\n list authors|books\n add author|book\n remove author|book\nexit:");
            //        var command = Console.ReadLine();
            //        switch (command) {
            //            case "list books":
            //                // выводим все книги из репозитория
            //                ListBooksRequest listBookRequest = ListBooksRequestMenu();
            //                OperationResult<ListBooksResult> listBooksResult = listBooksUseCase.Execute(listBookRequest);
            //                DisplayResult<ListBooksResult>(listBooksResult, books => string.Join(Environment.NewLine, books.Books.Select(b => $"({b.Title} by {b.AuthorName}, ISBN: {b.Isbn})")));
            //                break;

            //            case "list authors":
            //                ListAuthorsRequest listAuthorsRequest = ListAuthorsRequestMenu();
            //                OperationResult<ListAuthorsResult> listAuthorsResult = listAuthorsUseCase.Execute(listAuthorsRequest);
            //                DisplayResult<ListAuthorsResult>(listAuthorsResult, authors => string.Join(Environment.NewLine, authors.Authors.Select(a => $"{a.Name}")));
            //                break;

            //            case "add author":
            //                AddAuthorRequest addAuthorRequest = AddAuthorRequestMenu();
            //                OperationResult<AddAuthorResult> addAuthorResult = addAuthorUseCase.Execute(addAuthorRequest);
            //                DisplayResult<AddAuthorResult>(addAuthorResult, a => $"Author {a.Name} added");
            //                break;

            //            case "add book":
            //                AddBookRequest addBookRequest = AddBookRequestMenu();
            //                OperationResult<AddBookResult> addBookResult = addBookUseCase.Execute(addBookRequest);
            //                DisplayResult<AddBookResult>(addBookResult, b => $"Book {b.Title} added");
            //                break;

            //            case "remove author":
            //                RemoveAuthorRequest removeAuthorRequest = RemoveAuthorRequestMenu();
            //                OperationResult<bool> removeAuthorResult = removeAuthorUseCase.Execute(removeAuthorRequest);
            //                DisplayResult<bool>(removeAuthorResult, a => $"Author {removeAuthorRequest.Name} removed");
            //                break;

            //            case "remove book":
            //                RemoveBookRequest removeBookRequest = RemoveBookRequestMenu();
            //                OperationResult<bool> removeBookResult = removeBookUseCase.Execute(removeBookRequest);
            //                DisplayResult<bool>(removeBookResult, a => $"Book {removeBookRequest.Title} removed");
            //                break;

            //            case "exit":
            //                return;
            //        }
            //        Console.WriteLine("===");
            //    }

            Screen currentScreen = new MainMenuScreen(uow);
            while(currentScreen != null) {
                currentScreen = currentScreen.Run();
            }
        }

        //private static ListBooksRequest ListBooksRequestMenu()
        //{
        //    ListBooksRequest listBooksRequest = new ListBooksRequest(1, 50);
        //    return listBooksRequest;
        //}

        //private static ListAuthorsRequest ListAuthorsRequestMenu()
        //{
        //    //throw new NotImplementedException();
        //    ListAuthorsRequest listAuthorsRequest = new ListAuthorsRequest(1, 50);
        //    return listAuthorsRequest;
        //}

        //private static AddAuthorRequest AddAuthorRequestMenu()
        //{    
        //    Console.WriteLine("Input Name: ");
        //    string? name = Console.ReadLine() ?? "";

        //    return new AddAuthorRequest(name);
        //}

        //private static AddBookRequest AddBookRequestMenu()
        //{
        //    Console.WriteLine("Input Title: ");
        //    string title = Console.ReadLine() ?? "";

        //    Console.WriteLine("Input Description (or leave empty): ");
        //    string description = Console.ReadLine() ?? "";

        //    Console.WriteLine("Input AuthorName: ");
        //    string authorName = Console.ReadLine() ?? "";

        //    Console.WriteLine("Input ISBN: ");
        //    string isbn = Console.ReadLine() ?? "";

        //    return new AddBookRequest(title, description, authorName, isbn);
        //}

        //private static RemoveBookRequest RemoveBookRequestMenu()
        //{
        //    Console.WriteLine("Input BookId:");
        //    string idInput = Console.ReadLine() ?? "";
        //    int bookId = int.Parse(idInput);

        //    Console.WriteLine("Input Title:");
        //    string? title = Console.ReadLine();
        //    return new RemoveBookRequest(bookId, title);
        //}

        //private static RemoveAuthorRequest RemoveAuthorRequestMenu()
        //{
        //    Console.WriteLine("Input AuthorId:");
        //    string idInput = Console.ReadLine() ?? "";
        //    int authorId = int.Parse(idInput);

        //    Console.WriteLine("Input Name:");
        //    string? name = Console.ReadLine();
        //    return new RemoveAuthorRequest(authorId, name);
        //}

        //private static void DisplayResult<T>(OperationResult<T> result, Func<T, string> formatter)
        //{
        //    if (result.Status != ResultStatus.Success) {
        //        Console.WriteLine(result.Message);
        //        return;
        //    }

        //    if(result.Data != null)
        //        Console.WriteLine(formatter(result.Data));
        //}


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