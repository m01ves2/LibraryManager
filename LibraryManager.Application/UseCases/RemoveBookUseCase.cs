using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Infrastructure.Repositories.EF;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Application.UseCases
{
    public class RemoveBookUseCase
    {
        private readonly LibraryDbContext _context;

        public RemoveBookUseCase(LibraryDbContext context)
        {
            _context = context;
        }

        public OperationResult<bool> Execute(RemoveBookRequest removeBookRequest)
        {
            try {
                Book? book = _context.Books.Where(b => b.Id == removeBookRequest.Id).FirstOrDefault();
                if (book is null) {
                    return OperationResult<bool>.NotFound($"Book not found");
                }
                _context.Books.Remove(book);
                _context.SaveChanges();
                return OperationResult<bool>.Ok(true);
            }
            catch (Exception ex) {
                return OperationResult<bool>.Error(ex.Message);
            }
        }
    }
}
