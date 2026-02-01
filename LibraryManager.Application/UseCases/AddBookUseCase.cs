using LibraryManager.Domain.Entities;
using LibraryManager.Domain.Interfaces;
using LibraryManager.Domain.Results;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Application.UseCases
{
    public class AddBookUseCase
    {
        private readonly  ILibraryRepository _repository;
    
        public AddBookUseCase(ILibraryRepository repository)
        {
            _repository = repository;
        }

        public OperationResult<Book> Execute(string title, string description, Author author, Isbn isbn) //not null, even required!
        {
            Book book = new Book(title, description, author, isbn);
            OperationResult<Book> result = _repository.FindBook(b => b.Isbn == book.Isbn);
            if (result.Status == ResultStatus.Success)
                return new OperationResult<Book>(ResultStatus.Duplicate, $"Book with ISBN{isbn.Value} already exists");
            
            return _repository.AddBook(book);
        }
    }
}
