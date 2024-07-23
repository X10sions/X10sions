using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RCommon.Entities;
using System.Linq.Expressions;
using RCommon.Persistence.Sql;

namespace RCommon.Persistence.Crud;

public abstract class SqlRepositoryBase<TEntity> : DisposableResource, ISqlMapperRepository<TEntity> where TEntity : class, IBusinessEntity {
  private readonly IDataStoreFactory _dataStoreFactory;

  public SqlRepositoryBase(IDataStoreFactory dataStoreFactory, ILoggerFactory logger, IEntityEventTracker eventTracker, IOptions<DefaultDataStoreOptions> defaultDataStoreOptions) {
    _dataStoreFactory = dataStoreFactory ?? throw new ArgumentNullException(nameof(dataStoreFactory));
    Logger = logger is null ? throw new ArgumentNullException(nameof(logger)) : logger.CreateLogger(GetType().Name);
    EventTracker = eventTracker ?? throw new ArgumentNullException(nameof(eventTracker));

    if (defaultDataStoreOptions is null) {
      throw new ArgumentNullException(nameof(defaultDataStoreOptions));
    }
    if (defaultDataStoreOptions != null && defaultDataStoreOptions.Value != null && !defaultDataStoreOptions.Value.DefaultDataStoreName.IsNullOrEmpty()) {
      DataStoreName = defaultDataStoreOptions.Value.DefaultDataStoreName;
    }

    DataStore = dataStoreFactory.Resolve<RDbConnection>(DataStoreName);
  }


  public string TableName { get; set; }
  protected internal RDbConnection DataStore { get; }

  public string DataStoreName { get; set; }
  public ILogger Logger { get; set; }
  public IEntityEventTracker EventTracker { get; }

  #region ReadOnly
  public abstract Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default);
  public abstract Task<long> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default);
  public abstract Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default);
  public abstract Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default);
  public abstract Task<TEntity> GetByPrimaryKeyAsync<TKey>(TKey primaryKey, CancellationToken token = default);
  #endregion

  #region WriteOnly
  public abstract Task DeleteAsync(TEntity entity, CancellationToken token = default);
  public abstract Task InsertAsync(TEntity entity, CancellationToken token = default);
  public abstract Task UpdateAsync(TEntity entity, CancellationToken token = default);
  #endregion

}
