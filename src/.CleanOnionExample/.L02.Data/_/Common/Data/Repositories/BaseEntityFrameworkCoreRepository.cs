using Common.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Data.Repositories;
public class BaseEntityFrameworkCoreRepository<TDbContext, TEntity, TId> : IRepository<TEntity, TId>
  where TDbContext : DbContext
  where TEntity : class, IEntityWithId<TId>
  where TId : IEquatable<TId> {
  protected readonly TDbContext _dbContext;

  public BaseEntityFrameworkCoreRepository(TDbContext dbContext) {
    _dbContext = dbContext;
  }

  public DbSet<TEntity> GetDbSet() => _dbContext.Set<TEntity>();

  public IQueryable<TEntity> Queryable => GetDbSet();
  public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate) => GetDbSet().Where(predicate);


  public bool Any() {
    throw new NotImplementedException();
  }

  public bool Any(Expression<Func<TEntity, bool>> where) {
    throw new NotImplementedException();
  }

  public Task<bool> AnyAsync(CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public long Count() {
    throw new NotImplementedException();
  }

  public long Count(Expression<Func<TEntity, bool>> where) {
    throw new NotImplementedException();
  }

  public Task<long> CountAsync(CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public Task<long> CountAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public void Delete(TId key) {
    throw new NotImplementedException();
  }

  public void Delete(Expression<Func<TEntity, bool>> where) {
    throw new NotImplementedException();
  }

  public async Task DeleteAsync(TId id, CancellationToken cancellationToken = default) {
    var entity = await GetDbSet().FindAsync(id);
    if (entity == null) {
      throw new Exception($"{nameof(id)} could not be found.");
    }
    GetDbSet().Remove(entity);
    await _dbContext.SaveChangesAsync(cancellationToken);
  }

  public Task DeleteAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public IEnumerable<TEntity> GetAll() {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) {
    try {
      return await GetDbSet().AsNoTracking().ToListAsync(cancellationToken);
    } catch (Exception ex) {
      throw new Exception($"Couldn't retrieve entities: {ex.Message}");
    }
  }

  public TEntity GetById(TId key) {
    throw new NotImplementedException();
  }

  public async Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken = default) {
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

  public void Insert(TEntity item) {
    throw new NotImplementedException();
  }

  public async Task InsertAsync(TEntity data, CancellationToken cancellationToken = default) {
    if (data == null) {
      throw new ArgumentNullException(nameof(data));
    }
    try {
      await GetDbSet().AddAsync(data, cancellationToken);
      await _dbContext.SaveChangesAsync(cancellationToken);
    } catch (Exception ex) {
      throw new Exception($"{nameof(data)} could not be saved: {ex.Message}");
    }
  }

  public void InsertRange(IEnumerable<TEntity> items) {
    throw new NotImplementedException();
  }

  public Task InsertRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public void Update(TId key, TEntity item) {
    throw new NotImplementedException();
  }

  public async Task<TEntity> UpdateAsync(TEntity data, CancellationToken cancellationToken = default) {
    if (data == null) {
      throw new ArgumentNullException(nameof(data));
    }
    try {
      GetDbSet().Update(data);
      await _dbContext.SaveChangesAsync(cancellationToken);
      return data;
    } catch (Exception ex) {
      throw new Exception($"{nameof(data)} could not be updated: {ex.Message}");
    }
  }

  public Task UpdateAsync(TId key, TEntity item, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public void UpdatePartial(TId key, object item) {
    throw new NotImplementedException();
  }

  public Task UpdatePartialAsync(TId key, object item, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public void UpdateRange(IEnumerable<TEntity> items) {
    throw new NotImplementedException();
  }

  public Task UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  //public async Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
  //  => await GetQueryable(predicate).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

  //public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
  //  => await GetQueryable(predicate).AsNoTracking().ToListAsync(cancellationToken);

}