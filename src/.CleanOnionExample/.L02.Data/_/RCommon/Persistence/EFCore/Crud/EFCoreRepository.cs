using Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RCommon.Entities;
using RCommon.Persistence.Crud;
using System.Linq.Expressions;

namespace RCommon.Persistence.EFCore.Crud;
public interface IEFCoreRepository<T> : IGraphRepository<T> where T : class, IBusinessEntity { }
public partial class EFCoreRepository<T> : GraphRepositoryBase<T, RCommonDbContext, DbSet<T>, IIncludableQueryable<T, object>> where T : class, IBusinessEntity {
  public EFCoreRepository(IDataStoreFactory dataStoreFactory, ILoggerFactory logger, IEntityEventTracker eventTracker, IOptions<DefaultDataStoreOptions> defaultDataStoreOptions) : base(dataStoreFactory, eventTracker, defaultDataStoreOptions) {
    Logger = logger is null ? throw new ArgumentNullException(nameof(logger)) : logger.CreateLogger(GetType().Name);
    Tracking = true;
  }
  protected override DbSet<T> Table => Database.Set<T>();

  #region ReadOnly
  public async override Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) => await Table.AnyAsync(expression, token);
  public async override Task<long> CountAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) => await Table.CountAsync(expression, token);
  public override async Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) => await GetQueryable(expression).SingleOrDefaultAsync(token);
  public async override Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) => await GetQueryable(expression).ToListAsync(token);
  public async override Task<T> GetByPrimaryKeyAsync<TKey>(TKey primaryKey, CancellationToken token = default) => await Table.FindAsync([primaryKey], token);
  #endregion

  #region WriteOnly
  public async override Task DeleteAsync(T entity, CancellationToken token = default) {
    EventTracker.AddEntity(entity);
    Table.Remove(entity);
    await Database.SaveChangesAsync(token);
  }

  public override async Task InsertAsync(T entity, CancellationToken token = default) {
    EventTracker.AddEntity(entity);
    await Table.AddAsync(entity, token);
    await Database.SaveChangesAsync(token);
  }

  public async override Task UpdateAsync(T entity, CancellationToken token = default) {
    EventTracker.AddEntity(entity);
    Table.Update(entity);
    await Database.SaveChangesAsync(token);
  }
  #endregion

  public override IEagerLoadableQueryable<T> Include(Expression<Func<T, object>> path) {
    _includableQueryable = Queryable.Include(path);
    return this;
  }
  public override IEagerLoadableQueryable<T> ThenInclude<TPreviousProperty, TProperty>(Expression<Func<object, TProperty>> path) {
    _repositoryQuery = _includableQueryable.ThenInclude(path);
    return this;
  }

  #region ByRows

  public async Task<int> DeleteRowsAsync(IEnumerable<T> rows, CancellationToken cancellationToken = default) {
    Table.RemoveRange(rows);
    return await Database.SaveChangesAsync(cancellationToken);
  }

  public async Task<int> InsertRowsAsync(IEnumerable<T> rows, CancellationToken cancellationToken = default) {
    await Table.AddRangeAsync(rows, cancellationToken);
    return await Database.SaveChangesAsync(cancellationToken);
  }

  public async Task<int> UpdateRowsAsync(IEnumerable<T> rows, CancellationToken cancellationToken = default) {
    Table.UpdateRange(rows);
    return await Database.SaveChangesAsync(cancellationToken);
  }

  #endregion

}

public class EFCoreRepository<T, TKey> : EFCoreRepository<T>, IReadOnlyRepository<T, TKey>, IWriteOnlyRepository<T, TKey> 
  where T : class, IBusinessEntity , IEntityWithId<TKey>
  where TKey : IEquatable<TKey> {
  public EFCoreRepository(IDataStoreFactory dataStoreFactory, ILoggerFactory logger, IEntityEventTracker eventTracker, IOptions<DefaultDataStoreOptions> defaultDataStoreOptions) : base(dataStoreFactory, logger, eventTracker, defaultDataStoreOptions) { }

  public async Task DeleteByPrimaryKeyAsync(TKey id, CancellationToken cancellationToken = default) {
    var entity = await Table.FindAsync(id);
    if (entity != null) {
      Table.Remove(entity);
      await Database.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task<T?> GetByPrimaryKeyAsync(TKey key, CancellationToken token = default) => await Table.FindAsync([key], token);

}


public static class EFCoreRepositoryExtensions {
  //    public async Task<IEnumerable<T>> GetNotTrackedListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => await GetQueryable(predicate).AsNoTracking().ToListAsync(cancellationToken);
  //    public async Task<T?> GetNotTrackedAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => await GetQueryable(predicate).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
  //    public IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate) => Table.Where(predicate);
  //    public async Task InsertAsync(T data, CancellationToken cancellationToken = default) => await DoAndSaveAsync(async () => await Table.AddAsync(data, cancellationToken), cancellationToken);
}