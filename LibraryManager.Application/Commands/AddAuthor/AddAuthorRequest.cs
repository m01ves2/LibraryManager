namespace LibraryManager.Application.Commands.AddAuthor
{
    public class AddAuthorRequest
    {
        public string Name { get; }
        public AddAuthorRequest(string name) 
        { 
            Name = name;
        }
    }
}
