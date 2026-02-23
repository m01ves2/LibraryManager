using LibraryManager.Application.Repositories;
using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;

namespace LibraryManager.Application.UseCases
{
    public class AddAuthorUseCase
    {
        private readonly IRepository<Book> _efBookRepository;
        private readonly IRepository<Author> _efAuthorRepository;

        public AddAuthorUseCase(IRepository<Book> efBookRepository, IRepository<Author> efAuthorRepository)
        {
            _efBookRepository = efBookRepository;
            _efAuthorRepository = efAuthorRepository;
        }

        public OperationResult<AddAuthorResult> Execute(AddAuthorRequest addAuthorRequest)
        {
            try {
                Author? author = new Author(addAuthorRequest.Name);
                _efAuthorRepository.Add(author);

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
