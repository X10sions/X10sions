using LinqToDB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RCommon.Entities;
using RCommon.Persistence.Crud;
using System.Linq.Expressions;

namespace RCommon.Persistence.Linq2Db.Crud;

public interface ILinq2DbRepository<T> :ILinqRepository<T> where T: IBusinessEntity{ }

public partial class Linq2DbRepository<T> : LinqRepositoryBase<T, RCommonDataConnection, ITable<T>, ILoadWithQueryable<T, object>> where T : class, IBusinessEntity {

  public Linq2DbRepository(IDataStoreFactory dataStoreFactory, ILoggerFactory logger, IEntityEventTracker eventTracker, IOptions<DefaultDataStoreOptions> defaultDataStoreOptions) : base(dataStoreFactory, eventTracker, defaultDataStoreOptions) {
    Logger = logger is null ? throw new ArgumentNullException(nameof(logger)) : logger.CreateLogger(GetType().Name);
  }
  protected override ITable<T> Table => Database.GetTable<T>();

  #region ReadOnly
  public async override Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) => await Queryable.AnyAsync(expression, token);
  public async override Task<long> CountAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) => await Queryable.CountAsync(expression, token);
  public async override Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) => await Queryable.SingleOrDefaultAsync(expression, token);
  public async override Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) => await GetQueryable(expression).ToListAsync(token);
  public async override Task<T> GetByPrimaryKeyAsync<TKey>(TKey primaryKey, CancellationToken token = default) => throw new NotImplementedException();
  #endregion

  #region WriteOnly
  public async override Task InsertAsync(T entity, CancellationToken token = default) {
    EventTracker.AddEntity(entity);
    await Database.InsertAsync(entity, token: token);
  }

  public async override Task DeleteAsync(T entity, CancellationToken token = default) {
    EventTracker.AddEntity(entity);
    await Database.DeleteAsync(entity);
  }

  public async override Task UpdateAsync(T entity, CancellationToken token = default) {
    EventTracker.AddEntity(entity);
    await Database.UpdateAsync(entity, token: token);
  }

  #endregion

  public override IEagerLoadableQueryable<T> Include(Expression<Func<T, object>> path) {
    _includableQueryable = Queryable.LoadWith(path);
    return this;
  }

  public override IEagerLoadableQueryable<T> ThenInclude<TPreviousProperty, TProperty>(Expression<Func<object, TProperty>> path) {
    _repositoryQuery = _includableQueryable.ThenLoad(path);
    return this;
  }

}

public static  class Linq2DbRepositoryExtensions { 


//  public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => await   Table.FirstOrDefaultAsync(predicate, cancellationToken);
//  public async Task<IEnumerable<T>> GetListAsync( Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => await Table.Where(predicate).ToListAsync(cancellationToken);
//  public async Task<TKey> InsertWithIdAsync<TKey>( T row, Func<T, TKey>? idSelector = null, CancellationToken token = default)    => (TKey)await Database.InsertWithIdentityAsync(row, token: token);
}