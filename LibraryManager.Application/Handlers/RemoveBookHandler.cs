using LibraryManager.Application.Requests;
using LibraryManager.Application.UseCases;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Domain.Results;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Application.Handlers
{
    public class RemoveBookHandler
    {
        private readonly ILibraryRepository _repository;
        private readonly RemoveBookUseCase _useCase;

        public RemoveBookHandler(ILibraryRepository repository)
        {
            _repository = repository;
            _useCase = new RemoveBookUseCase(_repository);
        }

        public OperationResult<Book> Handle(RemoveBookRequest dto)
        {
            var isbnResult = ResolveIsbn(dto);
            if (!isbnResult.IsSuccess)
                return OperationResult<Book>.Fail(isbnResult.Status, isbnResult.Message);

            return _useCase.Execute(isbnResult.Data);
        }

        private OperationResult<Isbn> ResolveIsbn(RemoveBookRequest dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Isbn))
                return Isbn.TryParse(dto.Isbn);

            if (dto.BookId.HasValue) {
                var bookResult = _repository.FindBook(b => b.Id == dto.BookId.Value);
                if (!bookResult.IsSuccess)
                    return OperationResult<Isbn>.Fail(ResultStatus.NotFound, "Book not found");

                return OperationResult<Isbn>.Ok(bookResult.Data.Isbn);
            }

            if (!string.IsNullOrWhiteSpace(dto.Title)) {
                var books = _repository.FindAllBooks(b => b.Title == dto.Title);
                if (!books.IsSuccess || books.Data.Count != 1)
                    return OperationResult<Isbn>.Fail(ResultStatus.Ambiguous, "Title is ambiguous");

                return OperationResult<Isbn>.Ok(books.Data[0].Isbn);
            }

            return OperationResult<Isbn>.Fail(ResultStatus.InvalidRequest, "No identifier provided");
        }
    }
}
