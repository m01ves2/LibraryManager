using LibraryManager.Domain.Exceptions;
using LibraryManager.Domain.Results;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Domain.Entities
{
    public class Book
    {
        public int Id { get; private set; } // технический идентификатор, не для бизнес-логики
        public string Title { get; private set; }
        public Author Author { get; private set; }
        public string? Description { get; private set; } //необязательное поле, можно менять
        public Isbn  Isbn { get; private set; }
        
        public Book(string title, string description, Author author, string isbn)
        {
            Title = title;      
            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidBookException("InvalidBookException: Title cannot be empty.");

            Description = description;

            Author = author ?? throw new DomainValidationException("Author cannot be null");

            Isbn = Isbn.Parse(isbn); //защита уже есть внутри VO.
        }

        public OperationResult<Book> ChangeTitle(string newTitle)
        {
            if (string.IsNullOrEmpty(newTitle)) {
                return OperationResult<Book>.Fail(ResultStatus.BookMissing, "Book title cannot be empty");
            }
            Title = newTitle;
            return OperationResult<Book>.Ok(this);
        }

        public OperationResult<Book> ChangeDescription(string? newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
                return OperationResult<Book>.Fail(ResultStatus.BookMissing, "Book description is empty");
            
            Description = newDescription;
            return OperationResult<Book>.Ok(this);
        }

        public OperationResult<Book> ChangeIsbn(string newIsbn)
        {
            if (!Isbn.IsValid(newIsbn))
                return OperationResult<Book>.Fail(ResultStatus.InvalidIsbn, "ISBN is invalid");

            Isbn = Isbn.Parse(newIsbn); // теперь безопасно, т.к. IsValid проверили
            return OperationResult<Book>.Ok(this);
        }

        public OperationResult<Book> ChangeAuthor(Author newAuthor)
        {
            if(newAuthor == null) {
                return OperationResult<Book>.Fail(ResultStatus.BookMissing, "Author is empty");
            }
            Author = newAuthor;
            return OperationResult<Book>.Ok(this);
        }
    }
}
