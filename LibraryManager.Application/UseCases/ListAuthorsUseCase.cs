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
                List<Author> authors = _uow.Authors.GetPaged(skip, take, listAuthorsRequest.NameContains).ToList();
                var totalAuthors = _uow.Authors.GetAll(listAuthorsRequest.NameContains);
                int totalCount = totalAuthors.Count();

                var authorsBooksTitles = authors.ToDictionary(author => author.Id, author => _uow.Books.GetPagedByAuthorId(0, 3, author.Id).Select(b => b.Title).ToList());
                var authorsBooksCount = authors.ToDictionary(author => author.Id, author => _uow.Books.CountByAuthorId(author.Id));

                // Маппинг Entity -> DTO
                var authorSummaries = authors.Select(author => new AuthorSummary(author.Id, author.Name, authorsBooksCount[author.Id], authorsBooksTitles[author.Id])).ToList();

                ListAuthorsResult data = new ListAuthorsResult(totalCount, listAuthorsRequest.PageNumber, listAuthorsRequest.PageSize, authorSummaries);
                return OperationResult<ListAuthorsResult>.Ok(data);
            }
            catch (Exception ex) {
                // Любые неожиданные исключения централизованно обрабатываем
                return OperationResult<ListAuthorsResult>.Error(ex.Message);
            }
        }
    }
}
