using Common.Domain.Repositories;
using NHibernate;
using System.Linq.Expressions;

public interface INHibernateRepository<T> : IRepositoryAsync<T, ISession, IQueryable<T>> where T : class { }

public class NHibernateRepository<T> : INHibernateRepository<T> where T : class {
  public NHibernateRepository(ISession session) {
    Database = session;
    Table = session.Query<T>();
    Query = new NHibernateQuery<T>(Table);
  }

  public ISession Database { get; }
  public IQueryable<T> Table { get; }
  public  Common.Domain.IQuery<T> Query { get; }
  public IQueryable<T> Queryable => Table;

  #region IWriteRepository
  public  async Task<int> DeleteAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var rowCount = 0;
    foreach (var row in rows) {
      await Database.DeleteAsync(row, token);
      rowCount++;
    }
    return rowCount;
  }

  public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => Database.GetAsync(predicate, cancellationToken);

  public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => Queryable.Where(predicate).ToListAsync( cancellationToken);

  public  async Task<int> InsertAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var rowCount = 0;
    foreach (var row in rows) {
      await Database.SaveAsync(row, token);
      rowCount++;
    }
    return rowCount;
  }

  public  async Task<TKey> InsertWithIdAsync<TKey>(T row, Func<T, TKey>? idSelector = null, CancellationToken token = default)
    => (TKey)  await Database.SaveAsync(row, token );

  public  async Task<int> UpdateAsync(T row, CancellationToken token = default) {
    await Database.UpdateAsync(row, token);
    return 1;
  }

  public async Task<int> UpdateAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var count =0;
    foreach (var row in rows) {
      await Database.UpdateAsync(row ,token);
      count++;
    }
    return count;
  }

  #endregion

}

public static class NHibernateRepositoryExtensions {
  public static async Task<T> GetByIdAsync<T, TId>(this ISession session, TId id, CancellationToken token = default) => await session.GetAsync<T>(id, token);
  public static async Task DeleteByIdAsync<T, TId>(this ISession session, TId id, CancellationToken token = default) => await session.DeleteAsync(await session.GetByIdAsync<T, TId>(id, token), token);
}