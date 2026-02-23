using LibraryManager.Application.Queries;
using LibraryManager.Application.Repositories;
using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;

namespace LibraryManager.Application.UseCases
{
    public class RemoveAuthorUseCase
    {
        private readonly IRepository<Book> _efBookRepository;
        private readonly IRepository<Author> _efAuthorRepository;
        private readonly IHasBooksByAuthorIdQuery _hasBooksByAuthorId;

        public RemoveAuthorUseCase(IRepository<Book> efBookRepository, IRepository<Author> efAuthorRepository, IHasBooksByAuthorIdQuery hasBooksByAuthorId)
        {
            _efBookRepository = efBookRepository;
            _efAuthorRepository = efAuthorRepository;
            _hasBooksByAuthorId = hasBooksByAuthorId;
        }

        public OperationResult<bool> Execute(RemoveAuthorRequest removeAuthorRequest)
        {
            try {
                Author? author = _efAuthorRepository.GetById(removeAuthorRequest.Id);

                if(author is null) {
                    return OperationResult<bool>.NotFound($"Author not found");
                }

                if (_hasBooksByAuthorId.Execute(removeAuthorRequest.Id)) {
                    return OperationResult<bool>.Conflict($"There are author's books. Remove books first");
                }

                _efAuthorRepository.Remove(author);
                return OperationResult<bool>.Ok(true);
            }
            catch (Exception ex) {
                return OperationResult<bool>.Error(ex.Message);
            }
        }
    }
}
