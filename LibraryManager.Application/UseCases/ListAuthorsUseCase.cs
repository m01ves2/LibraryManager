using LibraryManager.Application.Interfaces;
using LibraryManager.Application.Models;
using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using System.Linq.Expressions;

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
                //var authors = _uow.Authors.GetPaged(skip, take, listAuthorsRequest.NameContains);

                //var authors = _uow.Authors.Find(a => true, skip, take);

                var authors = _uow.Authors.Find( GetPredicate(listAuthorsRequest), skip, take);
                int totalCount = _uow.Authors.Count(GetPredicate(listAuthorsRequest));

                var authorPreviews = authors.Select(a => new AuthorPreview(a.Id, a.Name, a.Books.Count, a.Books.Select(b => b.Title).Take(3).ToList())).ToList();

                ListAuthorsResult data = new ListAuthorsResult(totalCount, listAuthorsRequest.PageNumber, listAuthorsRequest.PageSize, authorPreviews);
                return OperationResult<ListAuthorsResult>.Ok(data);
            }
            catch (Exception ex) {
                // Любые неожиданные исключения централизованно обрабатываем
                return OperationResult<ListAuthorsResult>.Error(ex.Message);
            }
        }

        private Expression<Func<Author, bool>> GetPredicate(ListAuthorsRequest request)
        {
            Expression<Func<Author, bool>> predicate = a =>
            (string.IsNullOrWhiteSpace(request.NameContains) || a.Name.Contains(request.NameContains));

            return predicate;
        }
    }
}
