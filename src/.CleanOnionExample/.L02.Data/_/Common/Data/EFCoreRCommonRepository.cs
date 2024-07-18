﻿using Common.Data.Specification;
using Common.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NHibernate.Linq;
using System.Linq.Expressions;

namespace Common.Data;

public class EFCoreRCommonRepository<TEntity> : GraphRepositoryBase<TEntity>
      where TEntity : class, IBusinessEntity {
  private IQueryable<TEntity> _repositoryQuery;
  private bool _tracking;
  private IIncludableQueryable<TEntity, object> _includableQueryable;
  private readonly IDataStoreFactory _dataStoreFactory;



  /// <summary>
  /// The default constructor for the repository. 
  /// </summary>
  /// <param name="dbContext">The <see cref="TDataStore"/> is injected with scoped lifetime so it will always return the same instance of the <see cref="DbContext"/>
  /// througout the HTTP request or the scope of the thread.</param>
  /// <param name="logger">Logger used throughout the application.</param>
  public EFCoreRepository(IDataStoreFactory dataStoreFactory, ILoggerFactory logger, IEntityEventTracker eventTracker, IOptions<DefaultDataStoreOptions> defaultDataStoreOptions) : base(dataStoreFactory, eventTracker, defaultDataStoreOptions) {
    if (logger is null) {
      throw new ArgumentNullException(nameof(logger));
    }
    if (eventTracker is null) {
      throw new ArgumentNullException(nameof(eventTracker));
    }
    if (defaultDataStoreOptions is null) {
      throw new ArgumentNullException(nameof(defaultDataStoreOptions));
    }
    Logger = logger.CreateLogger(GetType().Name);
    _tracking = true;
    _dataStoreFactory = dataStoreFactory ?? throw new ArgumentNullException(nameof(dataStoreFactory));
  }

  protected DbSet<TEntity> ObjectSet {
    get {
      return ObjectContext.Set<TEntity>();
    }
  }

  public override bool Tracking { get => _tracking; set => _tracking = value; }

  public override IEagerLoadableQueryable<TEntity> Include(Expression<Func<TEntity, object>> path) {
    _includableQueryable = RepositoryQuery.Include(path);
    return this;
  }

  public override IEagerLoadableQueryable<TEntity> ThenInclude<TPreviousProperty, TProperty>(Expression<Func<object, TProperty>> path) {
    _repositoryQuery = _includableQueryable.ThenInclude(path);
    return this;
  }

  protected override IQueryable<TEntity> RepositoryQuery {
    get {
      if (_repositoryQuery == null) {
        _repositoryQuery = ObjectSet.AsQueryable<TEntity>();
      }
      // Start Eagerloading
      if (_includableQueryable != null) {
        _repositoryQuery = _includableQueryable;
      }
      return _repositoryQuery;
    }
  }

  public override async Task AddAsync(TEntity entity, CancellationToken token = default) {
    EventTracker.AddEntity(entity);
    await ObjectSet.AddAsync(entity, token);
    await SaveAsync(token);
  }


  public async override Task DeleteAsync(TEntity entity, CancellationToken token = default) {
    EventTracker.AddEntity(entity);
    ObjectSet.Remove(entity);
    await SaveAsync();
  }

  public async override Task UpdateAsync(TEntity entity, CancellationToken token = default) {
    EventTracker.AddEntity(entity);
    ObjectSet.Update(entity);
    await SaveAsync(token);
  }

  private IQueryable<TEntity> FindCore(Expression<Func<TEntity, bool>> expression) {
    IQueryable<TEntity> queryable;
    try {
      Guard.Against<NullReferenceException>(RepositoryQuery == null, "RepositoryQuery is null");
      queryable = RepositoryQuery.Where(expression);
    } catch (ApplicationException exception) {
      Logger.LogError(exception, "Error in {0}.FindCore while executing a query on the Context.", GetType().FullName);
      throw;
    }
    return queryable;
  }

  public async override Task<long> GetCountAsync(ISpecification<TEntity> selectSpec, CancellationToken token = default) {
    return await FindCore(selectSpec.Predicate).CountAsync(token);
  }

  public async override Task<long> GetCountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default) {
    return await FindCore(expression).CountAsync(token);
  }

  public override IQueryable<TEntity> FindQuery(ISpecification<TEntity> specification) {
    return FindCore(specification.Predicate);
  }

  public override IQueryable<TEntity> FindQuery(Expression<Func<TEntity, bool>> expression) {
    return FindCore(expression);
  }

  public override async Task<TEntity> FindAsync(object primaryKey, CancellationToken token = default) {
    return await ObjectSet.FindAsync(new object[] { primaryKey }, token);
  }

  public async override Task<ICollection<TEntity>> FindAsync(ISpecification<TEntity> specification, CancellationToken token = default) {
    return await FindCore(specification.Predicate).ToListAsync(token);
  }

  public async override Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default) {
    return await FindCore(expression).ToListAsync(token);
  }

  public async override Task<IPaginatedList<TEntity>> FindAsync(IPagedSpecification<TEntity> specification, CancellationToken token = default) {
    IQueryable<TEntity> query;
    if (specification.OrderByAscending) {
      query = FindCore(specification.Predicate).OrderBy(specification.OrderByExpression);
    } else {
      query = FindCore(specification.Predicate).OrderByDescending(specification.OrderByExpression);
    }
    return await Task.FromResult(query.ToPaginatedList(specification.PageIndex, specification.PageSize));
  }

  public async override Task<IPaginatedList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderByExpression,
      bool orderByAscending, int? pageIndex, int pageSize = 1,
      CancellationToken token = default) {
    IQueryable<TEntity> query;
    if (orderByAscending) {
      query = FindCore(expression).OrderBy(orderByExpression);
    } else {
      query = FindCore(expression).OrderByDescending(orderByExpression);
    }
    return await Task.FromResult(query.ToPaginatedList(pageIndex, pageSize));
  }

  public override async Task<TEntity> FindSingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default) {
    return await FindCore(expression).SingleOrDefaultAsync(token);
  }

  public override async Task<TEntity> FindSingleOrDefaultAsync(ISpecification<TEntity> specification, CancellationToken token = default) {
    return await FindCore(specification.Predicate).SingleOrDefaultAsync(token);
  }

  public async override Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default) {
    return await FindCore(expression).AnyAsync(token);
  }

  public async override Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken token = default) {
    return await FindCore(specification.Predicate).AnyAsync(token);
  }

  protected internal RCommonDbContext ObjectContext {
    get {
      return this._dataStoreFactory.Resolve<RCommonDbContext>(this.DataStoreName);
    }
  }

  private async Task<int> SaveAsync(CancellationToken token = default) {
    int affected = 0;
    try {
      affected = await ObjectContext.SaveChangesAsync(true, token);
    } catch (ApplicationException ex) {
      var persistEx = new PersistenceException($"Error in {this.GetGenericTypeName()}.SaveAsync while executing on the Context.", ex);
      throw persistEx;
    }

    return affected;
  }
}
