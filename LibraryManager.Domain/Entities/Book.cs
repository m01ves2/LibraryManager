using LibraryManager.Domain.Exceptions;
using LibraryManager.Domain.ValueObjects;

namespace LibraryManager.Domain.Entities
{
    public class Book
    {
        public int Id { get; private set; } // технический идентификатор, не для бизнес-логики
        public string Title { get; } // обязательное поле, нельзя менять
        public Author Author { get; } // обязательное поле, нельзя менять
        public string? Description { get; private set; } //необязательное поле, можно менять
        public Isbn  ISBN { get; }
        
        public Book(string title, string description, string authorName, string isbn)
        {
            Title = title;      
            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidBookException("InvalidBookException: Title cannot be empty.");

            Description = description;

            if(string.IsNullOrEmpty(authorName)) 
                throw new InvalidBookException("InvalidBookException: Author is required.");
            Author = new Author(authorName);

            ISBN = Isbn.Parse(isbn);
        }

        public bool UpdateDescription(string? newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
                return false;
            
            Description = newDescription;
            return true;
        }
    }
}
