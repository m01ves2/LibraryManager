using LibraryManager.Application.Repositories;
using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;

namespace LibraryManager.Application.UseCases
{
    public class RemoveBookUseCase
    {
        private readonly IRepository<Book> _efBookRepository;
        private readonly IRepository<Author> _efAuthorRepository;

        public RemoveBookUseCase(IRepository<Book> efBookRepository, IRepository<Author> efAuthorRepository)
        {
            _efBookRepository = efBookRepository;
            _efAuthorRepository = efAuthorRepository;
        }

        public OperationResult<bool> Execute(RemoveBookRequest removeBookRequest)
        {
            try {
                Book? book = _efBookRepository.GetById(removeBookRequest.Id);
                if (book is null) {
                    return OperationResult<bool>.NotFound($"Book not found");
                }
                _efBookRepository.Remove(book);
                return OperationResult<bool>.Ok(true);
            }
            catch (Exception ex) {
                return OperationResult<bool>.Error(ex.Message);
            }
        }
    }
}
