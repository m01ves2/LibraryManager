using LibraryManager.Application.Models;
using LibraryManager.Application.Queries.Interfaces;
using LibraryManager.Application.Results;

namespace LibraryManager.Application.Queries.ListAuthors
{
    public class ListAuthorsUseCase
    {
        private readonly IBookQueryRepository _bookQueryRepository;
        private readonly IAuthorQueryRepository _authorQueryRepository;

        public ListAuthorsUseCase(IBookQueryRepository bookQueryRepository, IAuthorQueryRepository authorQueryRepository)
        {
            _bookQueryRepository = bookQueryRepository;
            _authorQueryRepository = authorQueryRepository;
        }

        public OperationResult<ListAuthorsResult> Execute(ListAuthorsRequest listAuthorsRequest)//"string? Name" filter
        {
            try {
                int skip = (listAuthorsRequest.PageNumber - 1) * listAuthorsRequest.PageSize;
                int take = listAuthorsRequest.PageSize;
                var authorPreviews = _authorQueryRepository.GetPaged(skip, take/*, listAuthorsRequest.NameContains*/);
                int totalCount = _authorQueryRepository.Count(/*listAuthorsRequest.NameContains*/);

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
