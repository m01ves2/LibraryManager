using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Entities;
using LibraryManager.Infrastructure.Repositories.EF;
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


            //var books = new EfBookRepository(libraryDbContext);
            //var authors = new EfAuthorRepository(libraryDbContext);
            //var uow = new EfUnitOfWork(libraryDbContext, books, authors);

            //var listBooksUseCase = new ListBooksUseCase(libraryDbContext);
            //var listAuthorsUseCase = new ListAuthorsUseCase(libraryDbContext);
            //var addAuthorUseCase = new AddAuthorUseCase(libraryDbContext);
            //var addBookUseCase = new AddBookUseCase(libraryDbContext);
            //var removeAuthorUseCase = new RemoveAuthorUseCase(libraryDbContext);
            //var removeBookUseCase = new RemoveBookUseCase(libraryDbContext);

            AddMockData(libraryDbContext);


            Screen? currentScreen = new MainMenuScreen(libraryDbContext);
            while(currentScreen != null) {
                currentScreen = currentScreen.Run();
            }
        }
        private static void AddMockData(LibraryDbContext context)
        {
            var author1 = new Author("Martin");
            var author2 = new Author("Hanonov");
            var author3 = new Author("Fauler");
            var author4 = new Author("Seemann");
            var author5 = new Author("Martin");

            context.Authors.Add(author1);
            context.Authors.Add(author3);
            context.Authors.Add(author2);
            context.Authors.Add(author4);
            context.Authors.Add(author5);
            
            context.Books.Add(new Book("Clean Code", "clean code book", author1, author1.Id, Domain.ValueObjects.Isbn.Parse("1234567890") ));
            context.Books.Add(new Book("How to cook", "cook book", author1, author1.Id, Domain.ValueObjects.Isbn.Parse("1243567890")));
            context.Books.Add(new Book("House holding", "house", author1, author1.Id, Domain.ValueObjects.Isbn.Parse("1243567980")));
            context.Books.Add(new Book("Clean Architecture", "coding", author1, author1.Id, Domain.ValueObjects.Isbn.Parse("2143567980")));
            context.Books.Add(new Book("DDD Learning", "DDD book", author2, author2.Id, Domain.ValueObjects.Isbn.Parse("2134567890")));
            context.Books.Add(new Book("DI", "book about DI", author4, author4.Id, Domain.ValueObjects.Isbn.Parse("9734567890")));
            context.Books.Add(new Book("Refactoring", "book about refactoring", author3, author3.Id, Domain.ValueObjects.Isbn.Parse("8901234763")));
            context.Books.Add(new Book("Shipping", "book about shipping", author5, author5.Id, Domain.ValueObjects.Isbn.Parse("8901237463")));
            
            context.SaveChanges();
        }
    }
}