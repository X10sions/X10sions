using LinqToDB;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public class _BaseIdentityContext_LinqToDBDataContext : DataContext, IIdentityContext_LinqToDB {
    public IDataContext DataContext { get; }
    public async Task DbDeleteAsync<T>(T data, CancellationToken cancellationToken = default) where T : class => await DataContext.DeleteAsync(data, token: cancellationToken);
    public async Task DbInsertAsync<T>(T data, CancellationToken cancellationToken = default) where T : class => await DataContext.InsertAsync(data, token: cancellationToken);
    public async Task DbUpdateAsync<T>(T data, CancellationToken cancellationToken = default) where T : class => await DataContext.UpdateAsync(data, token: cancellationToken);
    public IQueryable<T> DbGetQueryable<T>() where T : class => DataContext.GetTable<T>();
    //public async Task DbSaveChangesAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();

  }
}
