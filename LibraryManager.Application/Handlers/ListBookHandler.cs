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
    public class ListBookHandler
    {
        private readonly ILibraryRepository _repository;
        private readonly ListBookUseCase _useCase;

        public ListBookHandler(ILibraryRepository repository)
        {
            _repository = repository;
            _useCase = new ListBookUseCase(repository);
        }

        public ViewResult<List<ViewBook>> Handle(ListBookRequest dto)
        {
            Author? author = null;
            if (dto.AuthorId.HasValue) {
                OperationResult<Author> authorResult = _repository.FindAuthor(a => a.Id == dto.AuthorId);
                author = authorResult.Data;
            }

            Isbn? isbn = null;
            if (dto.Isbn is not null) {
                OperationResult<Isbn> isbnResult = Isbn.TryParse(dto.Isbn);
                isbn = isbnResult.Data;
            }

            OperationResult<List<Book>> operationResult = _useCase.Execute(dto.Title, author, isbn);
            ViewResult<List<ViewBook>> viewResult = ResultMapper.ToViewResult(operationResult, BookMapper.ToViewBooks);
            return viewResult;
        }
    }
}
