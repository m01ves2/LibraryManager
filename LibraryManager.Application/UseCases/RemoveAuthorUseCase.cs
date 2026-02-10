using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;

namespace LibraryManager.Application.UseCases
{
    public class RemoveAuthorUseCase
    {
        private readonly IUnitOfWork _uow;

        public RemoveAuthorUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public OperationResult<bool> Execute(RemoveAuthorRequest removeAuthorRequest)
        {
            try {
                Author? author = _uow.Authors.GetById(removeAuthorRequest.Id);

                if(author is null) {
                    return OperationResult<bool>.NotFound($"Author not found");
                }

                if (_uow.Books.HasBooksByAuthorId(removeAuthorRequest.Id)) {
                    return OperationResult<bool>.Conflict($"There are author's books. Remove books first");
                }

                _uow.Authors.Remove(author);
                _uow.Commit();
                return OperationResult<bool>.Ok(true);
            }
            catch (Exception ex) {
                return OperationResult<bool>.Error(ex.Message);
            }
        }
    }
}
