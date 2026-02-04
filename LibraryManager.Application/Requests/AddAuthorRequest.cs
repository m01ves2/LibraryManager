namespace LibraryManager.Application.Requests
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
