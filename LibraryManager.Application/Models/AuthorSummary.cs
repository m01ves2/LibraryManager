namespace LibraryManager.Application.Models
{
    public class AuthorSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public AuthorSummary(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
