using LibraryManager.Application.Queries;
using LibraryManager.Application.Repositories;
using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Application.UseCases
{
    public class AddBookUseCase
    {
        private readonly IRepository<Book> _efBookRepository;
        private readonly IRepository<Author> _efAuthorRepository;
        private readonly IGetBookByIsbnQuery _getBookByIsbn;

        public AddBookUseCase(IRepository<Book> efBookRepository, IRepository<Author> efAuthorRepository, IGetBookByIsbnQuery getBookByIsbn)
        {
            _efBookRepository = efBookRepository; 
            _efAuthorRepository = efAuthorRepository;
            _getBookByIsbn = getBookByIsbn;
        }

        public OperationResult<AddBookResult> Execute(AddBookRequest addBookRequest)
        {
            try {
                Author? author = _efAuthorRepository.GetById(addBookRequest.AuthorId);
                if (author is null)
                    return OperationResult<AddBookResult>.NotFound($"Author not found");

                Isbn? isbn = null;
                if (addBookRequest.Isbn is not null) {
                    isbn = Isbn.Parse(addBookRequest.Isbn);
                    Book? bookByIsbn = _getBookByIsbn.Execute(isbn); // TODO вот тут возможно еще нужен Query-объект для ISBN
                    if (bookByIsbn is not null)
                        return OperationResult<AddBookResult>.Conflict($"Book ISBN:{addBookRequest.Isbn} already exists");
                }
                
                Book book = new Book(addBookRequest.Title, addBookRequest.Description, author, author.Id, isbn);
                _efBookRepository.Add(book);

                AddBookResult addBookResult = new AddBookResult(book.Id, book.Title, book.Description, book.Author.Name, book.Isbn?.Value);
                return OperationResult<AddBookResult>.Ok(addBookResult);
            }
            catch (Exception ex) {
                // Любые неожиданные исключения централизованно обрабатываем
                return OperationResult<AddBookResult>.Error(ex.Message);
            }
        }
    }
}
