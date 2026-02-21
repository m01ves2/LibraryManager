using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Infrastructure.Repositories.EF;

namespace LibraryManager.Application.UseCases
{
    public class AddAuthorUseCase
    {
        private readonly LibraryDbContext _context;

        public AddAuthorUseCase(LibraryDbContext context)
        {
            _context = context;
        }

        public OperationResult<AddAuthorResult> Execute(AddAuthorRequest addAuthorRequest)
        {
            try {
                Author? author = new Author(addAuthorRequest.Name);
                _context.Authors.Add(author);
                _context.SaveChanges();
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
