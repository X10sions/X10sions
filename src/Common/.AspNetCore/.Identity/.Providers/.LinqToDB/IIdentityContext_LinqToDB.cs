using LinqToDB;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public interface IIdentityContext_LinqToDB : IIdentityContext, IDataContext {
    //  IDataContext DataContext { get; }
  }

  public static class ILinqToDBIdentityContextExtensions {
    public static async Task<int> xDbDeleteAsync<T>(this IIdentityContext_LinqToDB context, T data, CancellationToken cancellationToken = default) => await context.DeleteAsync(data, token: cancellationToken);
    public static IQueryable<T> xDbGetTable<T>(this IIdentityContext_LinqToDB context) where T : class => context.GetTable<T>();
    public static async Task<int> xDbInsertAsync<T>(this IIdentityContext_LinqToDB context, T data, CancellationToken cancellationToken = default) => await context.InsertAsync(data, token: cancellationToken);
    public static async Task<int> xDbUpdateAsync<T>(this IIdentityContext_LinqToDB context, T data, CancellationToken cancellationToken = default) => await context.UpdateAsync(data, token: cancellationToken);
  }

}
