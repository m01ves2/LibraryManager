using System.Linq.Expressions;

namespace LibraryManager.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Remove(T entity);
        T? GetById(int id);

        IReadOnlyList<T> Find(Expression<Func<T, bool>> predicate, int skip, int take);
        bool Any(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate);
    }
}
