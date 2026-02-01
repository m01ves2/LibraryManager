using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Domain.Results;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Application.UseCases
{
    public class ListBookUseCase
    {
        private readonly ILibraryRepository _repository;

        public ListBookUseCase(ILibraryRepository repository)
        {
            _repository = repository;
        }

        public OperationResult<List<Book>> Execute(int? id, string? title, Author? author, Isbn? isbn)
        {
            Func<Book, bool> predicate = book => true;

            if (id is not null)
                predicate = And(predicate, b => b.Id == id);

            if (!string.IsNullOrWhiteSpace(title))
                predicate = And(predicate, b => b.Title == title);

            if (author is not null)
                predicate = And(predicate, b => b.Author.Id == author.Id);

            if (isbn is not null)
                predicate = And(predicate, b => b.Isbn.Equals(isbn));

            return _repository.FindAllBooks(predicate);
        }

        private static Func<T, bool> And<T>(Func<T, bool> left, Func<T, bool> right)
        {
            return x => left(x) && right(x);
        }
    }
}
