using LibraryManager.Domain.Exceptions;
using LibraryManager.Domain.Results;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Domain.Entities
{
    public class Book
    {
        public int Id { get; internal set; } // технический идентификатор, не для бизнес-логики
        public string Title { get; private set; }
        public Author Author { get; private set; }
        public string? Description { get; private set; } //необязательное поле, можно менять
        public Isbn  Isbn { get; private set; }
        
        public Book(string title, string description, Author author, Isbn isbn)
        {
            Title = title;      
            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidBookException("InvalidBookException: Title cannot be empty.");

            Description = description;

            Author = author ?? throw new DomainValidationException("Author cannot be null");

            Isbn = isbn;
        }

        public OperationResult<Book> UpdateBook(Book other)
        {
            // Используем уже существующие методы ChangeX
            var titleResult = UpdateTitle(other.Title);
            var descResult = UpdateDescription(other.Description);
            var isbnResult = UpdateIsbn(other.Isbn);
            var authorResult = UpdateAuthor(other.Author);

            // Проверяем результаты
            if (titleResult.Status != ResultStatus.Success) return titleResult;
            if (descResult.Status != ResultStatus.Success) return descResult;
            if (isbnResult.Status != ResultStatus.Success) return isbnResult;
            if (authorResult.Status != ResultStatus.Success) return authorResult;

            return OperationResult<Book>.Ok(this);
        }

        public OperationResult<Book> UpdateTitle(string newTitle)
        {
            if (string.IsNullOrEmpty(newTitle)) {
                return new OperationResult<Book>(ResultStatus.BookMissing, "Book title cannot be empty");
            }
            Title = newTitle;
            return OperationResult<Book>.Ok(this);
        }

        public OperationResult<Book> UpdateDescription(string? newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
                return new OperationResult<Book>(ResultStatus.BookMissing, "Book description is empty");
            
            Description = newDescription;
            return OperationResult<Book>.Ok(this);
        }

        public OperationResult<Book> UpdateIsbn(Isbn newIsbn)
        {
            //if (!Isbn.IsValid(newIsbn))
            //    return OperationResult<Book>.Fail(ResultStatus.InvalidIsbn, "ISBN is invalid");
            //Isbn = Isbn.Parse(newIsbn); // теперь безопасно, т.к. IsValid проверили
            
            if(newIsbn is null) {
                return new OperationResult<Book>(ResultStatus.InvalidIsbn, "ISBN is invalid");
            }
            Isbn = newIsbn;
            return OperationResult<Book>.Ok(this);
        }

        public OperationResult<Book> UpdateAuthor(Author newAuthor)
        {
            if(newAuthor is null) {
                return new OperationResult<Book>(ResultStatus.BookMissing, "Author is empty");
            }
            Author = newAuthor;
            return OperationResult<Book>.Ok(this);
        }
    }
}
