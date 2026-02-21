using LibraryManager.Application.Models;
using LibraryManager.Application.Requests;
using LibraryManager.Application.Results;
using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Infrastructure.Repositories.EF;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Application.UseCases
{
    public class ListAuthorsUseCase
    {
        private readonly LibraryDbContext _context;

        public ListAuthorsUseCase(LibraryDbContext context)
        {
            _context = context;
        }

        public OperationResult<ListAuthorsResult> Execute(ListAuthorsRequest listAuthorsRequest)//"string? Name" filter
        {
            try {
                int skip = (listAuthorsRequest.PageNumber - 1) * listAuthorsRequest.PageSize;
                int take = listAuthorsRequest.PageSize;
                var authors = _context.Authors.OrderBy(a => a.Id).Skip(skip).Take(take).Include(a => a.Books).ToList();
                int totalCount = _context.Authors.Count();

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
