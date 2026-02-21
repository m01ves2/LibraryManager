using LibraryManager.Domain.Entities;

namespace LibraryManager.Application.Models
{
    public class AuthorPreview
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? BooksCount { get; set; }
        public IReadOnlyList<string> SampleBooksTitles { get; set; }
        public AuthorPreview(int id, string name, int? booksCount, IReadOnlyList<string> sampleBooksTitles)
        {
            Id = id;
            Name = name;
            SampleBooksTitles = sampleBooksTitles;
            BooksCount = booksCount;
        }
    }
}
