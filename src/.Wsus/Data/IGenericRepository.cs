using System.Linq.Expressions;

namespace X10sions.Wsus.Data;

public interface IGenericRepository<T> where T : class {
  Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true);
  Task<T?> GetByIdAsync<TId>(TId id);
  Task<List<T>> GetListAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "", bool tracked = true);
  Task DeleteAsync(T entity);
  Task DeleteByIdAsync<TId>(TId id);
  Task InsertAsync(T entity);
  Task UpdateAsync(T entity);
  Task SaveAsync();
}
