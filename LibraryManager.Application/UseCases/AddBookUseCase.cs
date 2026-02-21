using LibraryManager.Application.Models;
using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Domain.ValueObjects;
using LibraryManager.Infrastructure.Repositories.EF;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Application.UseCases
{
    public class AddBookUseCase
    {
        private readonly LibraryDbContext _context;

        public AddBookUseCase(LibraryDbContext context)
        {
            _context = context;
        }

        public OperationResult<AddBookResult> Execute(AddBookRequest addBookRequest)
        {
            try {
                Author? author = _context.Authors.Where(a => a.Id == addBookRequest.AuthorId).Include(a => a.Books).FirstOrDefault();
                if (author is null)
                    return OperationResult<AddBookResult>.NotFound($"Author not found");

                Isbn? isbn = null;
                if (addBookRequest.Isbn is not null) {
                    isbn = Isbn.Parse(addBookRequest.Isbn);
                    Book? bookByIsbn = _context.Books.Where(b => b.Isbn != null && b.Isbn.Value == isbn.Value).Include(b => b.Author).FirstOrDefault();
                    if (bookByIsbn is not null)
                        return OperationResult<AddBookResult>.Conflict($"Book ISBN:{addBookRequest.Isbn} already exists");
                }
                
                Book book = new Book(addBookRequest.Title, addBookRequest.Description, author, author.Id, isbn);
                _context.Books.Add(book);
                _context.SaveChanges();

                AddBookResult addBookResult = new AddBookResult(book.Id, book.Title, book.Description, book.Author.Name, book.Isbn?.Value);
                return OperationResult<AddBookResult>.Ok(addBookResult);
            }
            catch (Exception ex) {
                // Любые неожиданные исключения централизованно обрабатываем
                return OperationResult<AddBookResult>.Error(ex.Message);
            }
        }
    }
}
