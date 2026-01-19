namespace LibraryManager.Domain.Exceptions
{
    public class InvalidBookException : Exception
    {
        public InvalidBookException(string message) : base(message) { }
    }
}
