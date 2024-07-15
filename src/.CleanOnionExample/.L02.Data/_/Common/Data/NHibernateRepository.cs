using Common.Domain.Repositories;
using NHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;

public interface INHibernateRepository<T> : IRepository<T, ISession, IQueryable<T>> where T : class { }

public class NHibernateQuery<T> : NHibernate.IQuery<T> where T : class {
  public NHibernateQuery(IQueryable<T> queryable) {
    Queryable = queryable;
  }
  public IQueryable<T> Queryable { get; }

  public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.AnyAsync(predicate, token);
  public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.CountAsync(predicate, token);
  public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.FirstOrDefaultAsync(predicate, token);
  public virtual async Task<List<T>> ToListAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.Where(predicate).ToListAsync(token);
}

public class NHibernateRepository<T> : INHibernateRepository<T> where T : class {
  public NHibernateRepository(ISession session) {
    Database = session;
    Table = session.Query<T>();
    Query = new NHibernateQuery<T>(Table);
  }

  public ISession Database { get; }
  public IQueryable<T> Table { get; }
  public  IQuery<T> Query { get; }

  #region IWriteRepository
  public virtual async Task<int> DeleteAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var rowCount = 0;
    foreach (var row in rows) {
      await Database.DeleteAsync(row, token);
      rowCount++;
    }
    return rowCount;
  }

  public virtual async Task<int> InsertAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var rowCount = 0;
    foreach (var row in rows) {
      await Database.SaveAsync(row, token);
      rowCount++;
    }
    return rowCount;
  }

  public virtual async Task<TKey> InsertWithIdAsync<TKey>(T row, Func<T, TKey>? idSelector = null, CancellationToken token = default)
    => (TKey)  await Database.SaveAsync(row, token );

  public virtual async Task<int> UpdateAsync(T row, CancellationToken token = default) {
    await Database.UpdateAsync(row, token);
    return 1;
  }

  #endregion

}

public static class NHibernateRepositoryExtensions {
  public static async Task<T> GetByIdAsync<T, TId>(this ISession session, TId id, CancellationToken token = default) => await session.GetAsync<T>(id, token);
  public static async Task DeleteByIdAsync<T, TId>(this ISession session, TId id, CancellationToken token = default) => await session.DeleteAsync(await session.GetByIdAsync<T, TId>(id, token), token);
}