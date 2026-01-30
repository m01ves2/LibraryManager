using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Domain.Results;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Application.UseCases
{
    public class RemoveBookUseCase
    {
        private readonly ILibraryRepository _repository;

        public RemoveBookUseCase(ILibraryRepository repository)
        {
            _repository = repository;
        }

        public OperationResult<Book> Execute(Isbn isbn)
        {
            OperationResult<Book> result = _repository.FindBook(b => b.Isbn == isbn);
            if (!result.IsSuccess)
                return OperationResult<Book>.Fail(ResultStatus.NotFound, $"Book to delete with {isbn.Value} does not exist");

            return _repository.RemoveBook(result.Data);
        }
    }
}
