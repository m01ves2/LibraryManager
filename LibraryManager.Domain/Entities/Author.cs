using LibraryManager.Domain.Exceptions;

namespace LibraryManager.Domain.Entities
{
    public class Author
    {
        public int Id { get; internal set; } //technical id for DB
        public string Name { get; private set; }

        public Author(string name) 
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainValidationException("Author name cannot be empty");

            Name = name;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainValidationException("Author name cannot be empty");
            Name = newName;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) 
                return true;
            if (obj is not Author other) 
                return false;

            return Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }

        public static bool operator ==(Author left, Author right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);
            return left.Equals(right);
        }

        public static bool operator !=(Author left, Author right)
        {
            return !(left == right);
        }
    }
}
