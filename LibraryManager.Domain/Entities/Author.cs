using LibraryManager.Domain.Exceptions;
using LibraryManager.Domain.Results;

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

        public OperationResult<Author> ChangeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) {
                return OperationResult<Author>.Fail(ResultStatus.AuthorMissing, "Author name cannot be empty");
            }
            Name = newName;
            return OperationResult<Author>.Ok(this);
        }
    }
}
