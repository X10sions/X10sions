using LinqToDB;
using System.Linq.Expressions;

namespace Common.Data;

public interface ILinq2DBRepository<T> : IRepository<T, IDataContext, ITable<T>> where T : class { }

public class Linq2DbQuery<T> : IQuery<T> where T : class {
  public Linq2DbQuery(ITable<T> queryable) {
    Queryable = queryable;
  }
  public IQueryable<T> Queryable { get; }

  public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.AnyAsync(predicate, token);
  public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.CountAsync(predicate, token);
  public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.FirstOrDefaultAsync(predicate, token);
  public virtual async Task<List<T>> ToListAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.Where(predicate).ToListAsync(token);
}

public  class Linq2DbRepository<T> : ILinq2DBRepository<T> where T : class {
  public Linq2DbRepository(IDataContext dataContext) {
    Database = dataContext;
    Table = dataContext.GetTable<T>();
    Query = new Linq2DbQuery<T>(Table);
  }

  public IDataContext Database { get; }
  public ITable<T> Table { get; }
  public IQuery<T> Query { get; }

  #region IWriteRepository
  public virtual async Task<int> DeleteAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var rowCount = 0;
    foreach (var row in rows) {
      await Database.DeleteAsync(row, token: token);
      rowCount++;
    }
    return rowCount;
  }

  public virtual async Task<int> InsertAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var rowCount = 0;
    foreach (var row in rows) {
      await Database.InsertAsync(row, token: token);
      rowCount++;
    }
    return rowCount;
  }

  public virtual async Task<TKey> InsertWithIdAsync<TKey>(T row, Func<T, TKey>? idSelector = null, CancellationToken token = default)
    => (TKey)await Database.InsertWithIdentityAsync(row, token: token);

  public virtual async Task<int> UpdateAsync(T row, CancellationToken token = default) => await Database.UpdateAsync(row, token: token);

  #endregion
}
public static class Linq2DbRepositoryExtensions {

}