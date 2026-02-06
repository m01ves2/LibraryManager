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
                    return new OperationResult<bool>(ResultStatus.NotFound, $"Author {removeAuthorRequest.Name} not found");
                }

                if (_uow.Books.HasBooksByAuthorId(removeAuthorRequest.Id)) {
                    return new OperationResult<bool>( ResultStatus.BookMissing, $"Author {removeAuthorRequest.Name} has books. Remove his books");
                }

                _uow.Authors.Remove(author);
                _uow.Commit();
                return OperationResult<bool>.Ok(true);
            }
            catch (Exception ex) {
                return OperationResult<bool>.Fail(ex.Message);
            }
        }
    }
}
