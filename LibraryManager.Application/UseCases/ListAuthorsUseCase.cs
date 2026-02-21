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

                //var authorsBooksTitles = authors.ToDictionary(author => author.Id, author => _uow.Books.GetPagedByAuthorId(0, 3, author.Id).Select(b => b.Title).ToList());
                //var authorsBooksCount = authors.ToDictionary(author => author.Id, author => _uow.Books.CountByAuthorId(author.Id));

                // Маппинг Entity -> DTO
                //var authorPreviews = authors.Select(author => new AuthorSummary(author.Id, author.Name, authorsBooksCount[author.Id], authorsBooksTitles[author.Id])).ToList();

                //var authorPreviews = authors.Select(author => new AuthorPreview(
                //                            author.Id,
                //                            author.Name,
                //                            _uow.Books.CountByAuthorId(author.Id),
                //                            _uow.Books.GetPagedByAuthorId(0, 3, author.Id).Select(b => b.Title).ToList()
                //                        )).ToList();

                //var authorPreviews = _uow.Books.GetByAuthorIds(authors.Select(a => a.Id))
                //                                .GroupBy(b => b.AuthorId)
                //                                .Select(g => new { AuthorId = g.Key, Books = g.Take(3), Count = g.Count() });

                //var authorPreviews = _uow.Books.GetByAuthorIds(authors.Select(a => a.Id))
                //                                .GroupBy(b => b.AuthorId)
                //                                .Select(g => new AuthorPreview(g.Key,));

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
