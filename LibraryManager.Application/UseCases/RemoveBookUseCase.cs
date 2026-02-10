using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;

namespace LibraryManager.Application.UseCases
{
    public class RemoveBookUseCase
    {
        private readonly IUnitOfWork _uow;

        public RemoveBookUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public OperationResult<bool> Execute(RemoveBookRequest removeBookRequest)
        {
            try {
                Book? book = _uow.Books.GetById(removeBookRequest.Id);
                if (book is null) {
                    return OperationResult<bool>.NotFound($"Book not found");
                }
                _uow.Books.Remove(book);
                _uow.Commit();
                return OperationResult<bool>.Ok(true);
            }
            catch (Exception ex) {
                return OperationResult<bool>.Error(ex.Message);
            }
        }
    }
}
