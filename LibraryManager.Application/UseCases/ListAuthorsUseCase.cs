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

        public OperationResult<ListAuthorsResult> Execute(ListAuthorsRequest listAuthorsRequest)//string? Name
        {
            try {
                int skip = (listAuthorsRequest.PageNumber - 1) * listAuthorsRequest.PageSize;
                int take = listAuthorsRequest.PageSize;
                List<Author> authors = _uow.Authors.GetPaged(skip, take).ToList();
                int totalCount = _uow.Authors.Count();

                // Маппинг Entity -> DTO
                var authorSummaries = authors.Select(author => new AuthorSummary(author.Id, author.Name)).ToList();

                ListAuthorsResult data = new ListAuthorsResult(totalCount, listAuthorsRequest.PageNumber, listAuthorsRequest.PageSize,authorSummaries);
                return OperationResult<ListAuthorsResult>.Ok(data);
            }
            catch (Exception ex) {
                // Любые неожиданные исключения централизованно обрабатываем
                return OperationResult<ListAuthorsResult>.Fail(ex.Message);
            }
        }
    }
}
