using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RCommon.Entities;
using System.Linq.Expressions;

namespace RCommon.Persistence.Crud;

public abstract class LinqRepositoryBase<T, TDataStore, TTable, TLoadTable> : DisposableResource, ILinqRepository<T>
  where T : IBusinessEntity
  where TDataStore : IDataStore
  where TTable : IQueryable<T>
  where TLoadTable : IQueryable<T> {

  public LinqRepositoryBase(IDataStoreFactory dataStoreFactory, IEntityEventTracker eventTracker, IOptions<DefaultDataStoreOptions> defaultDataStoreOptions) {
    if (defaultDataStoreOptions is null) throw new ArgumentNullException(nameof(defaultDataStoreOptions));
    EventTracker = eventTracker ?? throw new ArgumentNullException(nameof(eventTracker));
    if (defaultDataStoreOptions != null && defaultDataStoreOptions.Value != null && !defaultDataStoreOptions.Value.DefaultDataStoreName.IsNullOrEmpty()) {
      DataStoreName = defaultDataStoreOptions.Value.DefaultDataStoreName;
    }
    Database = dataStoreFactory is null ? throw new ArgumentNullException(nameof(dataStoreFactory)) : dataStoreFactory.Resolve<TDataStore>(DataStoreName);
  }

  protected TLoadTable _includableQueryable;//=null;
  protected IQueryable<T> _repositoryQuery;// = null;
  //protected readonly IDataStoreFactory _dataStoreFactory;
  protected TDataStore Database { get; }
  protected abstract TTable Table { get; }

  public IQueryable<T> Queryable {
    get {
      _repositoryQuery = _includableQueryable != null ? _includableQueryable : _repositoryQuery ?? Table.AsQueryable();
      return _repositoryQuery;
    }
  }

  public IQueryable<T> GetQueryable(Expression<Func<T, bool>> expression) {
    IQueryable<T> queryable;
    try {
      Guard.Against<NullReferenceException>(Queryable == null, "RepositoryQuery is null");
      queryable = Queryable.Where(expression);
    } catch (ApplicationException exception) {
      Logger.LogError(exception, "Error in {0}.FindCore while executing a query on the Context.", GetType().FullName);
      throw;
    }
    return queryable;
  }

  #region ReadOnly
  public abstract Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);
  public abstract Task<long> CountAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);
  public abstract Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);
  public abstract Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);
  public abstract Task<T> GetByPrimaryKeyAsync<TKey>(TKey primaryKey, CancellationToken token = default);
  #endregion


  #region WriteOnly
  public abstract Task InsertAsync(T entity, CancellationToken token = default);
  public abstract Task DeleteAsync(T entity, CancellationToken token = default);
  public abstract Task UpdateAsync(T entity, CancellationToken token = default);
  #endregion

  public abstract IEagerLoadableQueryable<T> Include(Expression<Func<T, object>> path);
  public abstract IEagerLoadableQueryable<T> ThenInclude<TPreviousProperty, TProperty>(Expression<Func<object, TProperty>> path);
  public ILogger Logger { get; set; }
  public IEntityEventTracker EventTracker { get; }
  public string DataStoreName { get; set; }


  #region Rows
  public async Task<int> DeleteAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var rowCount = 0;
    foreach (var row in rows) {
      await DeleteAsync(row, token);
      rowCount++;
    }
    return rowCount;
  }

  public async Task<int> UpdateAsync(IEnumerable<T> rows, CancellationToken token = default) {
    var count = 0;
    foreach (var row in rows) {
      await UpdateAsync(row, token);
      count++;
    }
    return count;
  }

  #endregion

}
