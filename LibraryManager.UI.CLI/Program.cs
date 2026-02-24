using LibraryManager.Application.Commands.AddAuthor;
using LibraryManager.Application.Commands.AddBook;
using LibraryManager.Application.Commands.Interfaces;
using LibraryManager.Application.Commands.RemoveAuthor;
using LibraryManager.Application.Commands.RemoveBook;
using LibraryManager.Application.Queries.ListAuthors;
using LibraryManager.Application.Queries.ListBooks;
using LibraryManager.Domain.Entities;
using LibraryManager.Infrastructure.Repositories.EF;
using LibraryManager.Infrastructure.Repositories.EF.Repositories;
using LibraryManager.UI.CLI.Screens;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.UI.CLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //services.AddDbContext<LibraryDbContext>(options => options.UseSqlite("Data Source=library.db"));
            var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
            var options = optionsBuilder.UseSqlite("Data Source=library.db").Options;

            //var uow = new InMemoryUnitOfWork(); //TODO сделать по уму CR! сейчас у нас транзитивная зависимость от Infrastructure через другие слои!!
            var libraryDbContext = new LibraryDbContext(options);
            libraryDbContext.Database.EnsureDeleted();
            libraryDbContext.Database.EnsureCreated();


            var bookCommandRepository = new EfBookCommandRepository(libraryDbContext);
            var authorCommandRepository = new EfAuthorCommandRepository(libraryDbContext);
            var bookQueryRepository = new EfBookQueryRepository(libraryDbContext);
            var authorQueryRepository = new EfAuthorQueryRepository(libraryDbContext);


            var uow = new EfUnitOfWork(libraryDbContext, bookCommandRepository, authorCommandRepository);
            
            var listBooksUseCase = new ListBooksUseCase(bookQueryRepository, authorQueryRepository);
            var listAuthorsUseCase = new ListAuthorsUseCase(bookQueryRepository, authorQueryRepository);
            var addAuthorUseCase = new AddAuthorUseCase(uow);
            var addBookUseCase = new AddBookUseCase(uow);
            var removeAuthorUseCase = new RemoveAuthorUseCase(uow);
            var removeBookUseCase = new RemoveBookUseCase(uow);

            AddMockData(uow);


            ScreenFactory factory = new ScreenFactory(addAuthorUseCase, addBookUseCase, listAuthorsUseCase, listBooksUseCase, removeAuthorUseCase, removeBookUseCase);

            Screen? currentScreen = new MainMenuScreen(factory, null);
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
            
            uow.Books.Add(new Book("Clean Code", "clean code book", author1, author1.Id, Domain.ValueObjects.Isbn.Parse("1234567890") ));
            uow.Books.Add(new Book("How to cook", "cook book", author1, author1.Id, Domain.ValueObjects.Isbn.Parse("1243567890")));
            uow.Books.Add(new Book("House holding", "house", author1, author1.Id, Domain.ValueObjects.Isbn.Parse("1243567980")));
            uow.Books.Add(new Book("Clean Architecture", "coding", author1, author1.Id, Domain.ValueObjects.Isbn.Parse("2143567980")));
            uow.Books.Add(new Book("DDD Learning", "DDD book", author2, author2.Id, Domain.ValueObjects.Isbn.Parse("2134567890")));
            uow.Books.Add(new Book("DI", "book about DI", author4, author4.Id, Domain.ValueObjects.Isbn.Parse("9734567890")));
            uow.Books.Add(new Book("Refactoring", "book about refactoring", author3, author3.Id, Domain.ValueObjects.Isbn.Parse("8901234763")));
            uow.Books.Add(new Book("Shipping", "book about shipping", author5, author5.Id, Domain.ValueObjects.Isbn.Parse("8901237463")));

            uow.Commit();
        }
    }
}