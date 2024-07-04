using Common.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Data;

public interface IEFCoreRepository<T> : IRepository<T, DbContext, DbSet<T>> where T : class { }

public class EFCoreQuery<T> : IQuery<T> where T : class {
  public EFCoreQuery(DbSet<T> queryable) {
    Queryable = queryable;
  }
  public IQueryable<T> Queryable { get; }

  public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.AnyAsync(predicate, token);
  public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.CountAsync(predicate, token);
  public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.FirstOrDefaultAsync(predicate, token);
  public virtual async Task<List<T>> ToListAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default) => await Queryable.Where(predicate).ToListAsync(token);
}

public  class EFCoreRepository<T> : IEFCoreRepository<T> where T : class {
  public EFCoreRepository(DbContext dbContext) {
    Database = dbContext;
    Table = dbContext.Set<T>();
    Query = new EFCoreQuery<T>(Table);
  }

  public DbContext Database { get; }
  public DbSet<T> Table { get; }
  public IQuery<T> Query { get; }


  #region IWriteRepository
  public virtual async Task<int> DeleteAsync(IEnumerable<T> rows, CancellationToken token = default) {
    Table.RemoveRange(rows);
    return await Database.SaveChangesAsync(token);
  }
  public virtual async Task<int> InsertAsync(IEnumerable<T> rows, CancellationToken token = default) {
    Table.AddRange(rows);
    return await Database.SaveChangesAsync(token);
  }

  public virtual async Task<TKey> InsertWithIdAsync<TKey>(T row, Func<T, TKey> idSelector, CancellationToken token = default) {
    Table.Add(row);
    await Database.SaveChangesAsync(token);
    return idSelector(row);
  }

  public virtual async Task<int> UpdateAsync(T row, CancellationToken token = default) {
    Table.Update(row);
    return await Database.SaveChangesAsync(token);
  }
  #endregion
}

public static class EFCoreRepositoryExtensions {
  public static async Task<T?> GetByIdAsync<T, TId>(this DbSet<T> dbSet, TId key, CancellationToken token = default) where T : class where TId : notnull => await dbSet.FindAsync(new object[] { key }, token);
  public static async Task<T?> GetByIdAsync<T>(this DbSet<T> dbSet, object[] key, CancellationToken token = default) where T : class => await dbSet.FindAsync(key, token);

}