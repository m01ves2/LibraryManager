using LibraryManager.Application.Interfaces;
using LibraryManager.Application.Models;
using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using System.Linq.Expressions;

namespace LibraryManager.Application.UseCases
{
    public class ListBooksUseCase
    {
        private readonly IUnitOfWork _uow;

        public ListBooksUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public OperationResult<ListBooksResult> Execute(ListBooksRequest listBooksRequest)
        {
            try {
                int skip = (listBooksRequest.PageNumber - 1) * listBooksRequest.PageSize;
                int take = listBooksRequest.PageSize;

                var books = _uow.Books.Find( GetPredicate(listBooksRequest), skip, take);
                var totalCount = _uow.Books.Count( GetPredicate(listBooksRequest) );

                var bookSummaries = books
                    .Select(b => new BookPreview(
                        b.Id,
                        b.Title,
                        b.Author?.Name,
                        b.Isbn?.Value))
                    .ToList();

                var data = new ListBooksResult(totalCount, listBooksRequest.PageNumber, listBooksRequest.PageSize, bookSummaries);

                return OperationResult<ListBooksResult>.Ok(data);
            }
            catch (Exception ex) {
                return OperationResult<ListBooksResult>.Error(ex.Message);
            }
        }

        private Expression<Func<Book, bool>> GetPredicate(ListBooksRequest request)
        {

            Expression<Func<Book, bool>> predicate = b => 
            (string.IsNullOrWhiteSpace(request.AuthorNameContains) || b.Author.Name.Contains(request.AuthorNameContains)) &&
            (string.IsNullOrWhiteSpace(request.TitleContains) || b.Title.Contains(request.TitleContains)) &&
            (string.IsNullOrWhiteSpace(request.IsbnContains)  || (b.Isbn != null && b.Isbn.Value.Contains(request.IsbnContains)));

            return predicate;
        }
    }
}
