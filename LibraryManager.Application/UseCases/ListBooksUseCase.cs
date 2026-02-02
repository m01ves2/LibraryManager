using LibraryManager.Application.Models;
using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;

namespace LibraryManager.Application.UseCases
{
    public class ListBooksUseCase
    {
        private readonly IUnitOfWork _uow;

        public ListBooksUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public OperationResult<ListBooksResult> Execute(ListBooksRequest listBookRequest)//int? id, string? title, Author? author, Isbn? isbn
        {
            try {
                int skip = (listBookRequest.PageNumber - 1) * listBookRequest.PageSize;
            int take = listBookRequest.PageSize;
            List<Book> books =  _uow.Books.GetPaged(skip, take).ToList();
            int totalCount = _uow.Books.Count();

            // Маппинг Entity -> DTO
            var bookSummaries = books.Select(book => new BookSummary(book.Id, book.Title, book.Author.Name, book.Isbn.Value)).ToList();

            ListBooksResult response = new ListBooksResult(totalCount, listBookRequest.PageNumber, listBookRequest.PageSize, bookSummaries);
            return OperationResult<ListBooksResult>.Ok(response);
            }
            catch (Exception ex) {
                // Любые неожиданные исключения централизованно обрабатываем
                return OperationResult<ListBooksResult>.Fail(ex.Message);
            }
        }
    }
}
