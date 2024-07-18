using Common.Domain;
using Common.Domain.Repositories;
using LinqToDB;
using System.Linq.Expressions;

namespace Common.Data;
public interface ILinq2DBRepository<T> : IRepositoryAsync<T, IDataContext, ITable<T>> where T : class { }

public  class Linq2DbRepository<T> : ILinq2DBRepository<T> where T : class {
  public Linq2DbRepository(IDataContext dataContext) {
    Database = dataContext;
    Table = dataContext.GetTable<T>();
    Query = new Linq2DbQuery<T>(Table);
  }

  public IDataContext Database { get; }
  public ITable<T> Table { get; }
  public IQuery<T> Query { get; }
  public IQueryable<T> Queryable => Table;

  #region IWriteRepository
  public  async Task<int> DeleteAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var rowCount = 0;
    foreach (var row in rows) {
      await Database.DeleteAsync(row, token: token);
      rowCount++;
    }
    return rowCount;
  }

  public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => await Table.Where(predicate).FirstOrDefaultAsync(cancellationToken);
  public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => await Table.Where(predicate).ToListAsync(cancellationToken);

  public  async Task<int> InsertAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var rowCount = 0;
    foreach (var row in rows) {
      await Database.InsertAsync(row, token: token);
      rowCount++;
    }
    return rowCount;
  }

  public  async Task<TKey> InsertWithIdAsync<TKey>(T row, Func<T, TKey>? idSelector = null, CancellationToken token = default)
    => (TKey)await Database.InsertWithIdentityAsync(row, token: token);

  public  async Task<int> UpdateAsync(T row, CancellationToken token = default) => await Database.UpdateAsync(row, token: token);

  public async Task<int> UpdateAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var count = 0;
    foreach (var row in rows) {
      await Database.UpdateAsync(row);
      count++;
    }
    return count;
   
  }

  #endregion
}
public static class Linq2DbRepositoryExtensions {

}