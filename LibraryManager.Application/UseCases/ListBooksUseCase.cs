using LibraryManager.Application.Queries;
using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;

namespace LibraryManager.Application.UseCases
{
    public class ListBooksUseCase
    {
        private readonly IGetBooksPagedQuery _getBooksPagedQuery;
        private readonly IGetBooksCountQuery _countQuery;

        public ListBooksUseCase(IGetBooksPagedQuery getBooksQuery, IGetBooksCountQuery countQuery)
        {
            _getBooksPagedQuery = getBooksQuery;
            _countQuery = countQuery;
        }

        public OperationResult<ListBooksResult> Execute(ListBooksRequest listBooksRequest)//"int? id, string? title, Author? author, Isbn? isbn" filters
        {
            try {
                int skip = (listBooksRequest.PageNumber - 1) * listBooksRequest.PageSize;
                int take = listBooksRequest.PageSize;

                var bookSummaries = _getBooksPagedQuery.Execute(skip, take); //уже внутри маппинг - проекция
                int totalCount = _countQuery.Execute();

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
