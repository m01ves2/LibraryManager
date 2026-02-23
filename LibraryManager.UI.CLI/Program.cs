using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Entities;
using LibraryManager.Infrastructure.Repositories.EF;
using LibraryManager.Infrastructure.Repositories.EF.Queries;
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


            var books = new EfRepository<Book>(libraryDbContext);
            var authors = new EfRepository<Author>(libraryDbContext);
            
            var getBooksPagedQuery = new GetBooksPagedQuery(libraryDbContext);
            var getBooksCountQuery = new GetBooksCountQuery(libraryDbContext);
            var getAuthorsPagedQuery = new GetAuthorsPagedQuery(libraryDbContext);
            var getAuthorsCountQuery = new GetAuthorsCountQuery(libraryDbContext);
            var getBookByIsbnQuery = new GetBookByIsbnQuery(libraryDbContext);

            var listBooksUseCase = new ListBooksUseCase(getBooksPagedQuery, getBooksCountQuery);
            var listAuthorsUseCase = new ListAuthorsUseCase(getAuthorsPagedQuery, getAuthorsCountQuery);
            var hasBooksByAuthorIdQuery = new HasBooksByAuthorIdQuery(libraryDbContext);

            var addAuthorUseCase = new AddAuthorUseCase(books, authors);
            var addBookUseCase = new AddBookUseCase(books, authors, getBookByIsbnQuery);
            var removeAuthorUseCase = new RemoveAuthorUseCase(books, authors, hasBooksByAuthorIdQuery );
            var removeBookUseCase = new RemoveBookUseCase(books, authors);

            AddMockData(books, authors);


            ScreenFactory factory = new ScreenFactory(addAuthorUseCase, addBookUseCase, listAuthorsUseCase, listBooksUseCase, removeAuthorUseCase, removeBookUseCase);

            Screen? currentScreen = new MainMenuScreen(factory, null);
            while(currentScreen != null) {
                currentScreen = currentScreen.Run();
            }
        }
        private static void AddMockData( EfRepository<Book> books, EfRepository<Author> authors )
        {
            var author1 = new Author("Martin");
            var author2 = new Author("Hanonov");
            var author3 = new Author("Fauler");
            var author4 = new Author("Seemann");
            var author5 = new Author("Martin");

            authors.Add(author1);
            authors.Add(author2);
            authors.Add(author3);
            authors.Add(author4);
            authors.Add(author5);
            
            books.Add(new Book("Clean Code", "clean code book", author1, author1.Id, Domain.ValueObjects.Isbn.Parse("1234567890") ));
            books.Add(new Book("How to cook", "cook book", author1, author1.Id, Domain.ValueObjects.Isbn.Parse("1243567890")));
            books.Add(new Book("House holding", "house", author1, author1.Id, Domain.ValueObjects.Isbn.Parse("1243567980")));
            books.Add(new Book("Clean Architecture", "coding", author1, author1.Id, Domain.ValueObjects.Isbn.Parse("2143567980")));
            books.Add(new Book("DDD Learning", "DDD book", author2, author2.Id, Domain.ValueObjects.Isbn.Parse("2134567890")));
            books.Add(new Book("DI", "book about DI", author4, author4.Id, Domain.ValueObjects.Isbn.Parse("9734567890")));
            books.Add(new Book("Refactoring", "book about refactoring", author3, author3.Id, Domain.ValueObjects.Isbn.Parse("8901234763")));
            books.Add(new Book("Shipping", "book about shipping", author5, author5.Id, Domain.ValueObjects.Isbn.Parse("8901237463")));
        }
    }
}