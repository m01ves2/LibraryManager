namespace LibraryManager.Application.Commands.AddAuthor
{
    public class AddAuthorResult
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public AddAuthorResult(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
