using LibraryManager.Application.Models;
using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;

namespace LibraryManager.Application.UseCases
{
    public class ListAuthorsUseCase
    {
        private readonly IUnitOfWork _uow;

        public ListAuthorsUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public OperationResult<ListAuthorsResult> Execute(ListAuthorsRequest listAuthorsRequest)//"string? Name" filter
        {
            try {
                int skip = (listAuthorsRequest.PageNumber - 1) * listAuthorsRequest.PageSize;
                int take = listAuthorsRequest.PageSize;
                var authors = _uow.Authors.GetPaged(skip, take, listAuthorsRequest.NameContains);
                int totalCount = _uow.Authors.Count(listAuthorsRequest.NameContains);

                var authorPreviews = authors.Select(a => new AuthorPreview(a.Id, a.Name, a.Books.Count, a.Books.Select(b => b.Title).Take(3).ToList())).ToList();

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
