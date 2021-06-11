using Common.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public interface IIdentityContext_EntityFrameworkCore : IIdentityContext, IDbContext { }

  public static class IEntityFrameworkCoreIdentityContextExtensions {

    public static async Task xDbDeleteAsync<T>(this IIdentityContext_EntityFrameworkCore context, T data, CancellationToken cancellationToken = default) where T : class => await context.SavedDeleteAsync(data, cancellationToken);
    public static IQueryable<T> xDbGetQueryable<T>(this IIdentityContext_EntityFrameworkCore context) where T : class => context.GetQueryable<T>();
    public static async Task xDbInsertAsync<T>(this IIdentityContext_EntityFrameworkCore context, T data, CancellationToken cancellationToken = default) where T : class => await context.SavedInsertAsync(data, cancellationToken);
    public static async Task xDbUpdateAsync<T>(this IIdentityContext_EntityFrameworkCore context, T data, CancellationToken cancellationToken = default) where T : class => await context.SavedUpdateAsync(data, cancellationToken);

  }
}
