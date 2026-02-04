using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Application.UseCases
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
                Author? author = _uow.Authors.GetByName(addBookRequest.AuthorName);
                if (author is null)
                    return new OperationResult<AddBookResult>(ResultStatus.AuthorMissing, $"Author {addBookRequest.AuthorName} not found");

                Isbn isbn = Isbn.Parse(addBookRequest.Isbn);

                Book book = new Book(addBookRequest.Title, addBookRequest.Description, author, isbn);
                _uow.Books.Add(book);
                _uow.Commit();

                AddBookResult addBookResult = new AddBookResult(book.Id, book.Title, book.Description, book.Author.Name, book.Isbn.Value);
                return OperationResult<AddBookResult>.Ok(addBookResult);
            }
            catch (Exception ex) {
                // Любые неожиданные исключения централизованно обрабатываем
                return OperationResult<AddBookResult>.Fail(ex.Message);
            }
        }
    }
}
