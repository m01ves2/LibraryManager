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
    public class AddBookHandler
    {
        private readonly ILibraryRepository _repository;
        private readonly AddBookUseCase _useCase;

        public AddBookHandler(ILibraryRepository repository)
        {
            _repository = repository;
            _useCase = new AddBookUseCase(_repository);
        }

        public ViewResult<ViewBook> Handle(AddBookRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title)) {
                return new ViewResult<ViewBook>() { Status = ViewResultStatus.BookMissing, Message = $"Book title is incorrect" };
            }

            OperationResult<Isbn> isbnResult = Isbn.TryParse(dto.Isbn);
            if (!isbnResult.IsSuccess)
                return new ViewResult<ViewBook>() { Status = ViewResultStatus.InvalidIsbn, Message = isbnResult.Message };

            OperationResult<Author> authorResult = _repository.FindAuthor(a => a.Id == dto.AuthorId);
            if (!authorResult.IsSuccess)
                return new ViewResult<ViewBook>() { Status = ViewResultStatus.NotFound, Message = authorResult.Message };

            OperationResult<Book> result = _useCase.Execute(dto.Title, dto.Description, authorResult.Data, isbnResult.Data);
            
            //ViewResult<ViewBook> viewResult = ResultMapper.ToViewResult<ViewBook, Book>(result, BookMapper.ToViewBook );
            ViewResult<ViewBook> viewResult = ResultMapper.ToViewResult(result, BookMapper.ToViewBook);

            return viewResult;
        }
    }
}
