using NHibernate.Linq;
using System.Linq.Expressions;

public class NHibernateQuery<T> :  Common.Domain.IQuery<T> where T : class {
  public NHibernateQuery(IQueryable<T> queryable) {
    Queryable = queryable;
  }
  public IQueryable<T> Queryable { get; }

  public  async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.AnyAsync(predicate, token);
  public  async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.CountAsync(predicate, token);
  public  async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.FirstOrDefaultAsync(predicate, token);
  public  async Task<List<T>> ToListAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.Where(predicate).ToListAsync(token);
}
