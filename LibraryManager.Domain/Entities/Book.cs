using LibraryManager.Domain.Exceptions;
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

        public void ChangeTitle(string newTitle)
        {
            if (string.IsNullOrEmpty(newTitle))
                throw new DomainValidationException("Book title can't be empty");
            Title = newTitle;
        }

        public void ChangeDescription(string? newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
                return; //silent change
            
            Description = newDescription;
        }

        public void ChangeIsbn(string newIsbn)
        {
            Isbn = Isbn.Parse(newIsbn);
        }

        public void ChangeAuthor(Author newAuthor)
        {
            Author = newAuthor ?? throw new DomainValidationException("Author cannot be null");
        }
    }
}
