using LibraryManager.Application.Models;
using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Domain.ValueObjects;
using System.Linq;

namespace LibraryManager.Application.UseCases
{
    public class AddBookUseCase
    {
        private readonly IUnitOfWork _uow;

        public AddBookUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public OperationResult<AddBookResult> Execute(AddBookRequest addBookRequest)
        {
            try {
                Author? author = _uow.Authors.GetById(addBookRequest.AuthorId);
                if (author is null)
                    return OperationResult<AddBookResult>.NotFound($"Author not found");

                Isbn? isbn = null;
                if (addBookRequest.Isbn is not null) {
                    isbn = Isbn.Parse(addBookRequest.Isbn);
                    Book? bookByIsbn = _uow.Books.GetByIsbn(isbn);
                    if (bookByIsbn is not null)
                        return OperationResult<AddBookResult>.Conflict($"Book ISBN:{addBookRequest.Isbn} already exists");
                }
                
                Book book = new Book(addBookRequest.Title, addBookRequest.Description, author, isbn);
                _uow.Books.Add(book);
                _uow.Commit();

                AddBookResult addBookResult = new AddBookResult(book.Id, book.Title, book.Description, book.Author.Name, book.Isbn?.Value);
                return OperationResult<AddBookResult>.Ok(addBookResult);
            }
            catch (Exception ex) {
                // Любые неожиданные исключения централизованно обрабатываем
                return OperationResult<AddBookResult>.Error(ex.Message);
            }
        }

        public OperationResult<IReadOnlyList<AuthorSummary>> GetAuthors(string authorName)
        {
            if (string.IsNullOrWhiteSpace(authorName))
                return OperationResult<IReadOnlyList<AuthorSummary>>.InvalidInput("Author name is empty");

            var authors = _uow.Authors.GetByName(authorName).ToList();
            if (!authors.Any())
                return OperationResult<IReadOnlyList<AuthorSummary>>.NotFound($"Author not found");


            var authorsBooks = authors.ToDictionary(author => author.Id, author => _uow.Books.GetPagedByAuthorId(0, 3, author.Id).Select(b => b.Title).ToList());
            var authorsBooksCount = authors.ToDictionary(author => author.Id, author => _uow.Books.CountByAuthorId(author.Id));
            
            var result = authors.Select(author => new AuthorSummary(author.Id, author.Name, authorsBooksCount[author.Id], authorsBooks[author.Id])).ToList();
            return OperationResult<IReadOnlyList<AuthorSummary>>.Ok(result);
        }
    }
}
