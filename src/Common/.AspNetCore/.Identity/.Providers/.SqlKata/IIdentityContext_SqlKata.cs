using SqlKata;
using SqlKata.Execution;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.SqlKata {
  public interface IIdentityContext_SqlKata : IIdentityContext {
    QueryFactory QueryFactory { get; }
    Query QueryRole { get; }
    Query QueryUser { get; }
    Query Query { get; }
  }

  public static class ISqlKataIdentityContextExtensions {
    public static async Task<int> xDbDeleteAsync<T, TKey>(this IIdentityContext_SqlKata context, T data, CancellationToken cancellationToken = default) where T : IId<TKey> where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequested();
      return await context.Query.Where(nameof(data.Id), data.Id).DeleteAsync();
    }
    public static IQueryable<T> xDbGetTable<T>(this IIdentityContext_SqlKata context) where T : class => throw new NotImplementedException();

    public static async Task<int> xDbInsertAsync<T>(this IIdentityContext_SqlKata context, T data, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      return await context.Query.InsertAsync(data);
    }

    public static async Task<int> xDbUpdateAsync<T>(this IIdentityContext_SqlKata context, T data, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      return await context.Query.UpdateAsync(data);
    }

  }
}
