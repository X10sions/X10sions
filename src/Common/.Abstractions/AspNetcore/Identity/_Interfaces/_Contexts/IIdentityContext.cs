using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityContext {

    Task DbDeleteAsync<T>(T data, CancellationToken cancellationToken = default) where T : class;
    Task DbInsertAsync<T>(T data, CancellationToken cancellationToken = default) where T : class;
    Task DbUpdateAsync<T>(T data, CancellationToken cancellationToken = default) where T : class;

    //IEntityEntry Add(object entity);
    //IEntityEntry<T> Add<T>(T entity) where T : class;
    //IEntityEntry<T> Attach<T>(T entity) where T : class;
    //IEntityEntry Remove(object entity);
    //IEntityEntry Update(object entity);

    //IIdentityDatabaseTable<T> DbGetDatabaseTable<T>() where T : class;// DbSet or ITable
    IQueryable<T> DbGetQueryable<T>() where T : class;

    //Task DbSaveChangesAsync(CancellationToken cancellationToken = default);

  }
}