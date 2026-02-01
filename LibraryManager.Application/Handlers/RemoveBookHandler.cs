using LibraryManager.Application.Mappers;
using LibraryManager.Application.Requests;
using LibraryManager.Application.UseCases;
using LibraryManager.Application.ViewModels;
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

        public ViewResult<ViewBook> Handle(RemoveBookRequest dto)
        {
            var idResult = ResolveBookId(dto);
            if (isbnResult.Status != ResultStatus.Success)
                return new ViewResult<ViewBook>()
                {
                    Status = ResultStatusMapper.ToViewStatus(isbnResult.Status),
                    Message = isbnResult.Message
                };

            OperationResult<Book> result = _useCase.Execute(isbnResult.Data);
            ViewResult<ViewBook> viewResult =  ResultMapper.ToViewResult(result, BookMapper.ToViewBook);
            return viewResult;
        }

        private OperationResult<Isbn> ResolveBookId(RemoveBookRequest dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Isbn))
                return Isbn.TryParse(dto.Isbn);

            if (dto.BookId.HasValue) {
                var bookResult = _repository.FindBook(b => b.Id == dto.BookId.Value);
                if (bookResult.Status != ResultStatus.Success)
                    return new OperationResult<Isbn>(ResultStatus.NotFound, "Book not found");

                return OperationResult<Isbn>.Ok(bookResult.Data.Isbn);
            }

            if (!string.IsNullOrWhiteSpace(dto.Title)) {
                var books = _repository.FindAllBooks(b => b.Title == dto.Title);
                if (books.Status != ResultStatus.Success || books.Data.Count != 1)
                    return new OperationResult<Isbn>(ResultStatus.Ambiguous, "Title is ambiguous");

                return OperationResult<Isbn>.Ok(books.Data[0].Isbn);
            }

            return new OperationResult<Isbn>(ResultStatus.InvalidRequest, "No identifier provided");
        }
    }
}
