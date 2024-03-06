//https://medium.com/@codebob75/repository-pattern-c-ultimate-guide-entity-framework-core-clean-architecture-dtos-dependency-6a8d8b444dcb

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace X10sions.Wsus.Data;

public class GenericRepository<T>(SusdbDbContext databaseContext) : IGenericRepository<T> where T : class {
  private readonly DbSet<T> dbSet = databaseContext.Set<T>();

  public async virtual Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true) {
    IQueryable<T> query = dbSet;
    if (!tracked) {
      query = query.AsNoTracking();
    }
    if (filter != null) {
      query = query.Where(filter);
    }
    return await query.FirstOrDefaultAsync();
  }

  public async Task<T?> GetByIdAsync<TId>(TId id) => await dbSet.FindAsync(id);

  public async virtual Task<List<T>> GetListAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "", bool tracked = true) {
    IQueryable<T> query = dbSet;
    if (!tracked) {
      query = query.AsNoTracking();
    }
    if (filter != null) {
      query = query.Where(filter);
    }
    foreach (var includeProperty in includeProperties.Split
        (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
      query = query.Include(includeProperty);
    }
    return orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
  }

  public async virtual Task DeleteAsync(T entity) {
    if (entity != null) {
      dbSet.Remove(entity);
      await SaveAsync();
    }
  }

  public async Task DeleteByIdAsync<TId>(TId id) {
    var entity = await dbSet.FindAsync(id);
    if (entity is null) { return; }
    if (databaseContext.Entry(entity).State == EntityState.Detached) {
      dbSet.Attach(entity);
    }
    await DeleteAsync(entity);
  }

  public async Task InsertAsync(T entity) {
    await dbSet.AddAsync(entity);
    await SaveAsync();
  }

  //public virtual void Update(T entityToUpdate) {
  //  dbSet.Attach(entityToUpdate);
  //  databaseContext.Entry(entityToUpdate).State = EntityState.Modified;
  //}

  public async Task UpdateAsync(T entity) {
    dbSet.Update(entity);
    await SaveAsync();
  }

  public async Task SaveAsync() => await databaseContext.SaveChangesAsync();

}