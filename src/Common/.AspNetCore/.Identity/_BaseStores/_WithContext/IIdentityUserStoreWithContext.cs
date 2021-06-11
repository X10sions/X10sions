using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserStoreWithContext<TContext, TUser, TKey>
    : IQueryableUserStore<TUser>
    , IIdentityStoreWithContext<TContext> //<IIdentityContext_WithUsers<TKey>>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUserWithConcurrency<TKey> {
    //IQueryable<TUser> UserQueryable { get; }
    //IIdentityDatabaseTable<TUser> UserDatabaseTable { get; set; }
  }

  public static class IIdentityUserStoreWithContextExtensions {

    #region UserStore: https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/EntityFrameworkCore/src/UserStore.cs

    public static async Task<IdentityResult> xCreateAsync<TContext, TUser, TKey>(this IIdentityUserStoreWithContext<TContext, TUser, TKey> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithConcurrency<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      await store.Context.DbInsertAsync(user, cancellationToken);
      return IdentityResult.Success;
    }

    public static async Task<IdentityResult> xDeleteAsync<TContext, TUser, TKey>(this IIdentityUserStoreWithContext<TContext, TUser, TKey> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithConcurrency<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      var stored = await store.Context.DbGetQueryable<TUser>().FindByIdAndConcurrencyStampAsync(user, cancellationToken);
      if (stored == null) {
        return IdentityResult.Failed(store.ErrorDescriber.ConcurrencyFailure());
      }
      await store.Context.DbDeleteAsync(user, cancellationToken);
      return IdentityResult.Success;
    }

    public static async Task<TUser> xFindByIdAsync<TContext, TUser, TKey>(this IIdentityUserStoreWithContext<TContext, TUser, TKey> store, string id, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithConcurrency<TKey> {
      store.ThrowIfCancelledRequestOrDisposed(cancellationToken);
      return (TUser)await store.Context.DbGetQueryable<TUser>().xFindByIdAsync(id, cancellationToken);
    }

    public static async Task<TUser> xFindByNameAsync<TContext, TUser, TKey>(this IIdentityUserStoreWithContext<TContext, TUser, TKey> store, string normalizedUserName, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithConcurrency<TKey> {
      store.ThrowIfCancelledRequestOrDisposed(cancellationToken);
      return (TUser)await store.Context.DbGetQueryable<TUser>().xFindByNameAsync(normalizedUserName, cancellationToken);
    }

    public static async Task<IdentityResult> xUpdateAsync<TContext, TUser, TKey>(this IIdentityUserStoreWithContext<TContext, TUser, TKey> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithConcurrency<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      var dbUser = await store.Context.DbGetQueryable<TUser>().FindByIdAndConcurrencyStampAsync(user, cancellationToken);
      if (dbUser == null) {
        return IdentityResult.Failed(store.ErrorDescriber.ConcurrencyFailure());
      }
      dbUser.ConcurrencyStamp = Guid.NewGuid().ToString();
      await store.Context.DbUpdateAsync(dbUser, cancellationToken);
      return IdentityResult.Success;
    }

    #endregion

  }
}