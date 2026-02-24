using LibraryManager.Application.Commands.Interfaces;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Application.Commands.AddBook
{
    public class AddBookUseCase
    {
        private readonly IUnitOfWork _uow;

        public AddBookUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public OperationResult<AddBookResult> Execute(AddBookRequest addBookRequest)
        {
            try {
                Author? author = _uow.Authors.GetById(addBookRequest.AuthorId);
                if (author is null)
                    return OperationResult<AddBookResult>.NotFound($"Author not found");

                Isbn? isbn = null;
                if (addBookRequest.Isbn is not null) {
                    isbn = Isbn.Parse(addBookRequest.Isbn);
                    Book? bookByIsbn = _uow.Books.GetByIsbn(isbn);
                    if (bookByIsbn is not null)
                        return OperationResult<AddBookResult>.Conflict($"Book ISBN:{addBookRequest.Isbn} already exists");
                }
                
                Book book = new Book(addBookRequest.Title, addBookRequest.Description, author, author.Id, isbn);
                _uow.Books.Add(book);
                _uow.Commit();

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
