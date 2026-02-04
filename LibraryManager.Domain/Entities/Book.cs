using LibraryManager.Domain.Exceptions;
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
        
        public Book(string title, string? description, Author author, Isbn isbn)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidBookException("Title cannot be empty");

            Title = title;
            Description = description;
            Author = author ?? throw new DomainValidationException("Author cannot be null");
            Isbn = isbn ?? throw new DomainValidationException("ISBN cannot be null");
        }

        public void UpdateBook(Book other)
        {
            UpdateTitle(other.Title);
            UpdateDescription(other.Description);
            UpdateIsbn(other.Isbn);
            UpdateAuthor(other.Author);
        }

        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new InvalidBookException("Book title cannot be empty");
            Title = newTitle;
        }

        public void UpdateDescription(string? newDescription)
        {
            Description = newDescription; // допускаем пустое описание
        }

        public void UpdateIsbn(Isbn newIsbn)
        {
            Isbn = newIsbn ?? throw new DomainValidationException("ISBN cannot be null");
        }
        public void UpdateAuthor(Author newAuthor)
        {
            Author = newAuthor ?? throw new DomainValidationException("Author cannot be null");
        }
    }
}
