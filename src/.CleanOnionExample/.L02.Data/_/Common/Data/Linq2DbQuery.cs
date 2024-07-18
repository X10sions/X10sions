using Common.Domain;
using LinqToDB;
using System.Linq.Expressions;

namespace Common.Data;

public class Linq2DbQuery<T> : IQuery<T> where T : class {
  public Linq2DbQuery(ITable<T> queryable) {
    Queryable = queryable;
  }
  public IQueryable<T> Queryable { get; }

  public  async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.AnyAsync(predicate, token);
  public  async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.CountAsync(predicate, token);
  public  async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.FirstOrDefaultAsync(predicate, token);
  public  async Task<List<T>> ToListAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.Where(predicate).ToListAsync(token);
}
