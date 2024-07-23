using Dommel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RCommon.Entities;
using RCommon.Persistence.Crud;
using System.Data;
using System.Linq.Expressions;

namespace RCommon.Persistence.Dapper.Crud;
public class DapperRepository<T> : SqlRepositoryBase<T>
    where T : class, IBusinessEntity {

  public DapperRepository(IDataStoreFactory dataStoreFactory, ILoggerFactory logger, IEntityEventTracker eventTracker, IOptions<DefaultDataStoreOptions> defaultDataStoreOptions) : base(dataStoreFactory, logger, eventTracker, defaultDataStoreOptions) {
    Logger = logger.CreateLogger(GetType().Name);
  }

  #region ReadOnly
  public override async Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) {
    await using (var db = DataStore.GetDbConnection()) {
      try {
        if (db.State == ConnectionState.Closed) {
          await db.OpenAsync();
        }

        var results = await db.AnyAsync(expression);
        return results;
      } catch (ApplicationException exception) {
        Logger.LogError(exception, "Error in {0}.AnyAsync while executing on the DbConnection.", GetType().FullName);
        throw;
      } finally {
        if (db.State == ConnectionState.Open) {
          await db.CloseAsync();
        }
      }
    }
  }

  public override async Task<long> CountAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) {
    await using (var db = DataStore.GetDbConnection()) {
      try {
        if (db.State == ConnectionState.Closed) {
          await db.OpenAsync();
        }

        var results = await db.CountAsync(expression);
        return results;
      } catch (ApplicationException exception) {
        Logger.LogError(exception, "Error in {0}.GetCountAsync while executing on the DbConnection.", GetType().FullName);
        throw;
      } finally {
        if (db.State == ConnectionState.Open) {
          await db.CloseAsync();
        }
      }
    }
  }

  public override async Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) {
    var result = await GetAllAsync (expression, token);
    return result.SingleOrDefault();
  }

  public override async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken token = default) {
    await using (var db = DataStore.GetDbConnection()) {
      try {
        if (db.State == ConnectionState.Closed) {
          await db.OpenAsync();
        }
        var results = await db.SelectAsync(expression, cancellationToken: token);
        return results.ToList();
      } catch (ApplicationException exception) {
        Logger.LogError(exception, "Error in {0}.FindAsync while executing on the DbConnection.", GetType().FullName);
        throw;
      } finally {
        if (db.State == ConnectionState.Open) {
          await db.CloseAsync();
        }
      }
    }
  }

  public override async Task<T> GetByPrimaryKeyAsync<TKey>(TKey primaryKey, CancellationToken token = default) {
    await using (var db = DataStore.GetDbConnection()) {
      try {
        if (db.State == ConnectionState.Closed) {
          await db.OpenAsync();
        }
        var result = await db.GetAsync<T>(primaryKey, cancellationToken: token);
        return result;
      } catch (ApplicationException exception) {
        Logger.LogError(exception, "Error in {0}.FindAsync while executing on the DbConnection.", GetType().FullName);
        throw;
      } finally {
        if (db.State == ConnectionState.Open) {
          await db.CloseAsync();
        }
      }
    }
  }

  #endregion

  #region WriteOnly
  public override async Task InsertAsync(T entity, CancellationToken token = default) {
    await using (var db = DataStore.GetDbConnection()) {
      try {
        if (db.State == ConnectionState.Closed) {
          await db.OpenAsync();
        }
        EventTracker.AddEntity(entity);
        await db.InsertAsync(entity, cancellationToken: token);

      } catch (ApplicationException exception) {
        Logger.LogError(exception, "Error in {0}.AddAsync while executing on the DbConnection.", GetType().FullName);
        throw;
      } finally {
        if (db.State == ConnectionState.Open) {
          await db.CloseAsync();
        }
      }

    }
  }

  public override async Task DeleteAsync(T entity, CancellationToken token = default) {
    await using (var db = DataStore.GetDbConnection()) {
      try {
        if (db.State == ConnectionState.Closed) {
          await db.OpenAsync();
        }

        EventTracker.AddEntity(entity);
        await db.DeleteAsync(entity, cancellationToken: token);
      } catch (ApplicationException exception) {
        Logger.LogError(exception, "Error in {0}.DeleteAsync while executing on the DbConnection.", GetType().FullName);
        throw;
      } finally {
        if (db.State == ConnectionState.Open) {
          await db.CloseAsync();
        }
      }

    }
  }

  public override async Task UpdateAsync(T entity, CancellationToken token = default) {
    await using (var db = DataStore.GetDbConnection()) {
      try {
        if (db.State == ConnectionState.Closed) {
          await db.OpenAsync();
        }

        EventTracker.AddEntity(entity);
        await db.UpdateAsync(entity, cancellationToken: token);
      } catch (ApplicationException exception) {
        Logger.LogError(exception, "Error in {0}.UpdateAsync while executing on the DbConnection.", GetType().FullName);
        throw;
      } finally {
        if (db.State == ConnectionState.Open) {
          await db.CloseAsync();
        }
      }
    }
  }
  #endregion

}
