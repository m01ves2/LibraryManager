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

        public OperationResult<ListBooksResult> Execute(ListBooksRequest listBooksRequest)//"int? id, string? title, Author? author, Isbn? isbn" filters
        {
            try {
                int skip = (listBooksRequest.PageNumber - 1) * listBooksRequest.PageSize;
                int take = listBooksRequest.PageSize;
                List<Book> books = _uow.Books.GetPaged(skip, take).ToList();
                int totalCount = _uow.Books.Count();

                // Маппинг Entity -> DTO
                var bookSummaries = books.Select(book => new BookPreview(book.Id, book.Title, book.Author.Name, book.Isbn?.Value)).ToList();

                ListBooksResult data = new ListBooksResult(totalCount, listBooksRequest.PageNumber, listBooksRequest.PageSize, bookSummaries);
                return OperationResult<ListBooksResult>.Ok(data);
            }
            catch (Exception ex) {
                // Любые неожиданные исключения централизованно обрабатываем
                return OperationResult<ListBooksResult>.Error(ex.Message);
            }
        }
    }
}
