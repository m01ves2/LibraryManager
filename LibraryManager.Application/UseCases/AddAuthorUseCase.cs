using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;

namespace LibraryManager.Application.UseCases
{
    public class AddAuthorUseCase
    {
        private readonly IUnitOfWork _uow;

        public AddAuthorUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public OperationResult<AddAuthorResult> Execute(AddAuthorRequest addAuthorRequest)
        {
            try {
                Author? author = new Author(addAuthorRequest.Name);
                _uow.Authors.Add(author);
                _uow.Commit();
                AddAuthorResult addAuthorResult = new AddAuthorResult(author.Id, author.Name);
                return OperationResult<AddAuthorResult>.Ok(addAuthorResult);
            }
            catch (Exception ex) {
                // Любые неожиданные исключения централизованно обрабатываем
                return OperationResult<AddAuthorResult>.Error(ex.Message);
            }
        }
    }
}
