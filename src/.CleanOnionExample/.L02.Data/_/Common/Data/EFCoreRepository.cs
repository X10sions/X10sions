using Common.Data.Specification;
using Common.Domain;
using Common.Domain.Entities;
using Common.Domain.Repositories;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using NHibernate.Linq;
using System;
using System.Linq.Expressions;

namespace Common.Data;

public interface IEFCoreRepository<T> : IRepositoryAsync<T,  DbContext, DbSet<T>> where T : class { }


// where TDbContext : DbContext
public class EFCoreRepository<T> : IEFCoreRepository<T> where T : class {
  public EFCoreRepository(DbContext dbContext) {
    Database = dbContext;
    Table = dbContext.Set<T>();
    Query = new EFCoreQuery<T>(Table);
  }

  public DbContext Database { get; }
  public DbSet<T> Table { get; }
  public IQuery<T> Query { get; }
  public IQueryable<T> Queryable { get; }

  #region IReadRepository

  #endregion

  #region IWriteRepository
  public  async Task<int> DeleteAsync(IEnumerable<T> rows, CancellationToken cancellationToken = default) {
    Table.RemoveRange(rows);
    return await Database.SaveChangesAsync(cancellationToken);
  }

  public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate , CancellationToken cancellationToken = default) 
    => await GetQueryable(predicate).AsNoTracking().ToListAsync(cancellationToken);

  public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    => await GetQueryable(predicate).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

  public IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate) => Table.Where(predicate);

  public async Task<int> InsertAsync(IEnumerable<T> rows, CancellationToken cancellationToken = default) {
    await Table.AddRangeAsync(rows, cancellationToken);
    return await Database.SaveChangesAsync(cancellationToken);
  }

  public async Task InsertAsync(T data, CancellationToken cancellationToken = default) {
      var newRow = await Table.AddAsync(data, cancellationToken);
      await Database..SaveChangesAsync(cancellationToken);
    } catch (Exception ex) {
      throw new Exception($"{nameof(data)} could not be saved: {ex.Message}");
    }
  }


  public  async Task<TKey> InsertWithIdAsync<TKey>(T row, Func<T, TKey> idSelector, CancellationToken token = default) {
    var newRow = await Table.AddAsync(row);
    await Database.SaveChangesAsync(token);
    return idSelector(row);
  }

  public  async Task<int> UpdateAsync(T row, CancellationToken token = default) {
    Table.UpdateRange(row);
    return await Database.SaveChangesAsync(token);
  }

  public async Task<int> UpdateAsync(IEnumerable<T> rows, CancellationToken token = default) {
    Table.UpdateRange(rows);
    return await Database.SaveChangesAsync(token);
  }

  //public async Task<T> UpdateAsync(T data, CancellationToken cancellationToken = default) {
  //  if (data == null) {
  //    throw new ArgumentNullException(nameof(data));
  //  }
  //  try {
  //    Table.Update(data);
  //    await Database.SaveChangesAsync(cancellationToken);
  //    return data;
  //  } catch (Exception ex) {
  //    throw new Exception($"{nameof(data)} could not be updated: {ex.Message}");
  //  }
  //}

  #endregion
}

public class EFCoreRepository<T, TKey> : EFCoreRepository<T>, IRepositoryAsync<T, TKey>  where T : class, IEntityWithId<TKey> where TKey : IEquatable<TKey> {
  public EFCoreRepository(DbContext dbContext) :base(dbContext){ }
  #region IReadRepository

  public async Task<T> GetByIdAsync(TKey id, CancellationToken cancellationToken = default) {
    try {
      var item = await GetQueryable(x => x.Id.Equals(id)).AsNoTracking().FirstOrDefaultAsync();
      if (item == null) {
        throw new Exception($"Couldn't find entity with id={id}");
      }
      return item;
    } catch (Exception ex) {
      throw new Exception($"Couldn't retrieve entity with id={id}: {ex.Message}");
    };
  }

  #endregion

  #region IWriteRepository

  public async Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default) {
    var entity = await Table.FindAsync(id);
    if (entity == null) {
      throw new Exception($"{nameof(id)} could not be found.");
    }
    Table.Remove(entity);
    await Database.SaveChangesAsync(cancellationToken);
  }

  public async Task UpdateByIdAsync(TKey key, T item, CancellationToken cancellationToken = default) {
    throw new not;
    var entity = await Table.FindAsync(key);
    if (entity == null) {
      throw new Exception($"{nameof(key)} could not be found.");
    }
    Table.UpdateAsync(entity, cancellationToken);
    await Database.SaveChangesAsync(cancellationToken);
  }

  #endregion

}

public static class EFCoreRepositoryExtensions {
  public static async Task<T?> GetByIdAsync<T, TId>(this DbSet<T> dbSet, TId key, CancellationToken token = default) where T : class where TId : notnull => await dbSet.FindAsync(new object[] { key }, token);
  public static async Task<T?> GetByIdAsync<T>(this DbSet<T> dbSet, object[] key, CancellationToken token = default) where T : class => await dbSet.FindAsync(key, token);
}