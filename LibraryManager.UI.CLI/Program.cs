using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Infrastructure.Repositories;
using LibraryManager.UI.CLI.Screens;

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

            Screen? currentScreen = new MainMenuScreen(uow);
            while(currentScreen != null) {
                currentScreen = currentScreen.Run();
            }
        }
        private static void AddMockData(IUnitOfWork uow)
        {
            var author1 = new Author("Martin");
            var author2 = new Author("Hanonov");
            var author3 = new Author("Fauler");
            var author4 = new Author("Seemann");
            var author5 = new Author("Martin");

            uow.Authors.Add(author1);
            uow.Authors.Add(author2);
            uow.Authors.Add(author3);
            uow.Authors.Add(author4);
            uow.Authors.Add(author5);
            
            uow.Books.Add(new Book("Clean Code", "clean code book", author1, Domain.ValueObjects.Isbn.Parse("1234567890") ));
            uow.Books.Add(new Book("How to cook", "cook book", author1, Domain.ValueObjects.Isbn.Parse("1243567890")));
            uow.Books.Add(new Book("House holding", "house", author1, Domain.ValueObjects.Isbn.Parse("1243567980")));
            uow.Books.Add(new Book("Clean Architecture", "coding", author1, Domain.ValueObjects.Isbn.Parse("2143567980")));
            uow.Books.Add(new Book("DDD Learning", "DDD book", author2, Domain.ValueObjects.Isbn.Parse("2134567890")));
            uow.Books.Add(new Book("DI", "book about DI", author4, Domain.ValueObjects.Isbn.Parse("9734567890")));
            uow.Books.Add(new Book("Refactoring", "book about refactoring", author3, Domain.ValueObjects.Isbn.Parse("8901234763")));
            uow.Books.Add(new Book("Shipping", "book about shipping", author5, Domain.ValueObjects.Isbn.Parse("8901237463")));
        }
    }
}