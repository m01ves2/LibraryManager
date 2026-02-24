using LibraryManager.Application.Models;
using LibraryManager.Application.Queries.Interfaces;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;

namespace LibraryManager.Application.Queries.ListBooks
{
    public class ListBooksUseCase
    {
        private readonly IBookQueryRepository _bookQueryRepository;
        private readonly IAuthorQueryRepository _authorQueryRepository;

        public ListBooksUseCase(IBookQueryRepository bookQueryRepository, IAuthorQueryRepository authorQueryRepository)
        {
            _bookQueryRepository = bookQueryRepository;
            _authorQueryRepository = authorQueryRepository;
        }

        public OperationResult<ListBooksResult> Execute(ListBooksRequest listBooksRequest)//"int? id, string? title, Author? author, Isbn? isbn" filters
        {
            try {
                int skip = (listBooksRequest.PageNumber - 1) * listBooksRequest.PageSize;
                int take = listBooksRequest.PageSize;
                var bookSummaries = _bookQueryRepository.GetPaged(skip, take/*, listBooksRequest.TitleContains, listBooksRequest.AuthorNameContains, listBooksRequest.IsbnContains*/).ToList();
                int totalCount = _bookQueryRepository.Count();

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
