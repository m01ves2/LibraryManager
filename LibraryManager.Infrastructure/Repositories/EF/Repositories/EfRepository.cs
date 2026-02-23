using LibraryManager.Application.Repositories;

namespace LibraryManager.Infrastructure.Repositories.EF.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly LibraryDbContext _context;

        public EfRepository(LibraryDbContext context) => _context = context;

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
        public T? GetById(int id) => _context.Set<T>().Find(id);
    }
}
