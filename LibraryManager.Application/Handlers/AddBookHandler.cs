using LibraryManager.Application.Requests;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Exceptions;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Domain.Results;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Application.Handlers
{
    public class AddBookHandler
    {
        private readonly ILibraryRepository _repository;
        private readonly AddBookUseCase _useCase;

        public AddBookHandler(ILibraryRepository repository)
        {
            _repository = repository;
            _useCase = new AddBookUseCase(_repository);
        }

        public OperationResult<Book> Handle(AddBookRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title)) {
                return OperationResult<Book>.Fail(ResultStatus.BookMissing, $"Book title is incorrect");
            }

            OperationResult<Isbn> isbnResult = Isbn.TryParse(dto.Isbn);
            if (!isbnResult.IsSuccess)
                return OperationResult<Book>.Fail(ResultStatus.InvalidIsbn, isbnResult.Message);

            OperationResult<Author> authorResult = _repository.FindAuthor(a => a.Id == dto.AuthorId);
            if (!authorResult.IsSuccess)
                return OperationResult<Book>.Fail(ResultStatus.NotFound, authorResult.Message);

            return _useCase.Execute(dto.Title, dto.Description, authorResult.Data, isbnResult.Data);
        }
    }
}
