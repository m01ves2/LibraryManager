using LibraryManager.Application.Models;
using LibraryManager.Application.Queries;
using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;

namespace LibraryManager.Application.UseCases
{
    public class ListAuthorsUseCase
    {
        private readonly IGetAuthorsPagedQuery _getAuthorsPagedQuery;
        private readonly IGetAuthorssCountQuery _countQuery;


        public ListAuthorsUseCase(IGetAuthorsPagedQuery getAuthorsPagedQuery, IGetAuthorssCountQuery countQuery)
        {
            _getAuthorsPagedQuery = getAuthorsPagedQuery;
            _countQuery = countQuery;
        }

        public OperationResult<ListAuthorsResult> Execute(ListAuthorsRequest listAuthorsRequest)//"string? Name" filter
        {
            try {
                int skip = (listAuthorsRequest.PageNumber - 1) * listAuthorsRequest.PageSize;
                int take = listAuthorsRequest.PageSize;

                var authorPreviews = _getAuthorsPagedQuery.Execute(skip, take, listAuthorsRequest.NameContains);
                int totalCount = _countQuery.Execute();

                ListAuthorsResult data = new ListAuthorsResult(totalCount, listAuthorsRequest.PageNumber, listAuthorsRequest.PageSize, authorPreviews);
                return OperationResult<ListAuthorsResult>.Ok(data);
            }
            catch (Exception ex) {
                // Любые неожиданные исключения централизованно обрабатываем
                return OperationResult<ListAuthorsResult>.Error(ex.Message);
            }
        }
    }
}
