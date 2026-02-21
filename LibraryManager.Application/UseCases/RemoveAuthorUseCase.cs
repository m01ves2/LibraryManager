using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Infrastructure.Repositories.EF;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Application.UseCases
{
    public class RemoveAuthorUseCase
    {
        private readonly LibraryDbContext _context;

        public RemoveAuthorUseCase(LibraryDbContext context)
        {
            _context = context;
        }

        public OperationResult<bool> Execute(RemoveAuthorRequest removeAuthorRequest)
        {
            try {
                Author? author = _context.Authors.Where(a => a.Id == removeAuthorRequest.Id).FirstOrDefault();

                if (author is null) {
                    return OperationResult<bool>.NotFound($"Author not found");
                }

                if ( _context.Books.Where(b => b.AuthorId == removeAuthorRequest.Id).Any()) {
                    return OperationResult<bool>.Conflict($"There are author's books. Remove books first");
                }

                _context.Authors.Remove(author);
                _context.SaveChanges();
                return OperationResult<bool>.Ok(true);
            }
            catch (Exception ex) {
                return OperationResult<bool>.Error(ex.Message);
            }
        }
    }
}
