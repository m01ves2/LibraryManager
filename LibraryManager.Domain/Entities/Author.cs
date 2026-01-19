using LibraryManager.Domain.Exceptions;

namespace LibraryManager.Domain.Entities
{
    public class Author
    {
        public int Id { get; private set; } //technical id for DB
        public string Name { get; private set; }

        public Author(string name) 
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainValidationException("Author name cannot be empty");
            Name = name;
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainValidationException("Author name cannot be empty");
            Name = newName;
        }
    }
}
