using NHibernate;
using NHibernate.Linq;
using Microsoft.Extensions.Options;
using RCommon.Entities;
using RCommon.Persistence.Crud;
using System.Linq.Expressions;
using Common.Domain.Entities;

namespace RCommon.Persistence.NHibernate.Crud;

public interface INHibernateRepository<T> : ILinqRepository<T> where T : IBusinessEntity { }
public class NHibernateRepository<T> : LinqRepositoryBase<T, RCommonNHContext, IQueryable<T>, INhFetchRequest<T, object>> where T : class, IBusinessEntity {
  public NHibernateRepository(IDataStoreFactory dataStoreFactory, Microsoft.Extensions.Logging.ILoggerFactory logger, IEntityEventTracker eventTracker, IOptions<DefaultDataStoreOptions> defaultDataStoreOptions) : base(dataStoreFactory, eventTracker, defaultDataStoreOptions) {
    Logger = (logger is null) ? throw new ArgumentNullException(nameof(logger)) : logger.CreateLogger(GetType().Name);
  }
  protected override IQueryable<T> Table => Database.Session.Query<T>();

  #region ReadOnly
  public override async Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) => await Table.AnyAsync(expression, token);
  public override async Task<long> CountAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) => await Table.CountAsync(expression, token);
  public override async Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) => await Table.FirstOrDefaultAsync(expression, token);
  public override async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) => await Table.Where(expression).ToListAsync(token);
  public async override Task<T> GetByPrimaryKeyAsync<TKey>(TKey primaryKey, CancellationToken token = default) => await Database.Session.GetAsync<T>(primaryKey, token);
  #endregion

  #region WriteOnly
  public override async Task InsertAsync(T entity, CancellationToken token = default) => await Database.Session.SaveAsync(entity, token);
  public override async Task DeleteAsync(T entity, CancellationToken token = default) => await Database.Session.DeleteAsync(entity, token);
  public override async Task UpdateAsync(T entity, CancellationToken token = default) => await Database.Session.UpdateAsync(entity, token);
  #endregion

  public override IEagerLoadableQueryable<T> Include(Expression<Func<T, object>> path) {
    _includableQueryable = Queryable.Fetch(path);
    return this;
  }
  public override IEagerLoadableQueryable<T> ThenInclude<TPreviousProperty, TProperty>(Expression<Func<object, TProperty>> path) {
    _repositoryQuery = _includableQueryable.ThenFetch(path);
    return this;
  }

  public async Task<TKey> InsertWithIdAsync<TKey>(T row, Func<T, TKey>? idSelector = null, CancellationToken token = default) => (TKey)await Database.Session.SaveAsync(row, token);
}

public class NHibernateRepository<T, TKey> : NHibernateRepository<T>, IReadOnlyRepository<T, TKey>, IWriteOnlyRepository<T, TKey>
  where T : class, IBusinessEntity, IEntityWithId<TKey>
  where TKey : IEquatable<TKey> {
  public NHibernateRepository(IDataStoreFactory dataStoreFactory, Microsoft.Extensions.Logging.ILoggerFactory logger, IEntityEventTracker eventTracker, IOptions<DefaultDataStoreOptions> defaultDataStoreOptions) : base(dataStoreFactory, logger, eventTracker, defaultDataStoreOptions) { }

  public async Task DeleteByPrimaryKeyAsync(TKey primaryKey, CancellationToken cancellationToken = default)  => await Database.Session.DeleteAsync(await GetByPrimaryKeyAsync(primaryKey, cancellationToken), cancellationToken);
  public async Task<T?> GetByPrimaryKeyAsync(TKey primaryKey, CancellationToken cancellationToken = default) => await Database.Session.GetAsync<T>(primaryKey, cancellationToken);

}