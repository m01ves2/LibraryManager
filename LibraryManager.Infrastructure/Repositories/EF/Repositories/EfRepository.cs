using LibraryManager.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryManager.Infrastructure.Repositories.EF.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        protected readonly LibraryDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public EfRepository(LibraryDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity) => _dbSet.Add(entity);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public T? GetById(int id) => _dbSet.Find(id);

        public IReadOnlyList<T> Find( Expression<Func<T, bool>> predicate, int skip, int take)
        {
            return _dbSet
                .Where(predicate)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Count(predicate);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }
    }
}
