using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace Common.EntityFrameworkCore {
  public interface IDbContext
    : IDisposable, IAsyncDisposable, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbContextPoolable {
    // https://github.com/zarxor/Example.EntityFramework.Testing/blob/master/Example.EntityFramework.Testing.Data/Abstract/IDbContext.cs

    //ChangeTracker ChangeTracker { get; }
    //DatabaseFacade Database { get; }
    //EntityEntry Add(object entity);

    //EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
    //Task<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);
    //Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
    //Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default);
    //Task AddRangeAsync(params object[] entities);
    //EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;
    //EntityEntry Attach(object entity);
    //EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    //EntityEntry Entry(object entity);
    //bool Equals(object obj);
    //object Find(Type entityType, params object[] keyValues);
    //TEntity Find<TEntity>(params object[] keyValues) where TEntity : class;
    //Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class;
    //Task<object> FindAsync(Type entityType, object[] keyValues, CancellationToken cancellationToken);
    //Task<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken) where TEntity : class;
    //Task<object> FindAsync(Type entityType, params object[] keyValues);
    //int GetHashCode();
    //DbQuery<TQuery> Query<TQuery>() where TQuery : class;
    //EntityEntry Remove(object entity);
    //EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;
    //int SaveChanges(bool acceptAllChangesOnSuccess);
    //int SaveChanges();
    //Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    //string ToString();
    //EntityEntry Update(object entity);
    //EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;

    //void AddRange(IEnumerable<object> entities);
    //void AddRange(params object[] entities); void AttachRange(params object[] entities);
    //void AttachRange(IEnumerable<object> entities);
    //void RemoveRange(IEnumerable<object> entities);
    //void RemoveRange(params object[] entities);
    //void UpdateRange(params object[] entities);
    //void UpdateRange(IEnumerable<object> entities);
  }


  public static class IDbContextExtensions {

    public static EntityEntry<T> xAdd<T>(this IDbContext context, T entity) where T : class => context.Set<T>().Add(entity);
    public static EntityEntry<T> xRemove<T>(this IDbContext context, T entity) where T : class => context.Set<T>().Remove(entity);

    #region Identity

    public static IQueryable<T> GetQueryable<T>(this IDbContext context) where T : class => context.Set<T>();

    public static async Task SavedDeleteAsync<T>(this IDbContext context, T data, CancellationToken cancellationToken = default) where T : class {
      context.xRemove(data);
      await context.SaveChangesAsync(cancellationToken);
    }

    public static async Task SavedInsertAsync<T>(this IDbContext context, T data, CancellationToken cancellationToken = default) where T : class {
      context.xAdd(data);
      await context.SaveChangesAsync(cancellationToken);
    }

    public static async Task SavedUpdateAsync<T>(this IDbContext context, T data, CancellationToken cancellationToken = default) where T : class {
      context.Set<T>().GetDbContext().Entry(data).State = EntityState.Modified;
      await context.SaveChangesAsync(cancellationToken);
    }

    #endregion



  }
}