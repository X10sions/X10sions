using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithLockoutStoreWithContext<TContext, TUser, TKey>
    : IUserLockoutStore<TUser>, IIdentityUserStoreWithContext<TContext, TUser, TKey>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUserWithLockout<TKey> {
  }

  public static class IIdentityUserWithLockoutStoreWithContextExtensions {

    public static Task<int> xGetAccessFailedCountAsync<TContext, TUser, TKey>(this IIdentityUserWithLockoutStoreWithContext<TContext, TUser, TKey> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithLockout<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.AccessFailedCount);
    }

    public static Task<bool> xGetLockoutEnabledAsync<TContext, TUser, TKey>(this IIdentityUserWithLockoutStoreWithContext<TContext, TUser, TKey> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithLockout<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.IsLockoutEnabled);
    }

    public static Task<DateTimeOffset?> xGetLockoutEndDateAsync<TContext, TUser, TKey>(this IIdentityUserWithLockoutStoreWithContext<TContext, TUser, TKey> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithLockout<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.LockoutEndDateUtc);
    }

    public static Task<int> xIncrementAccessFailedCountAsync<TContext, TUser, TKey>(this IIdentityUserWithLockoutStoreWithContext<TContext, TUser, TKey> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithLockout<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      user.AccessFailedCount++;
      return Task.FromResult(user.AccessFailedCount);
    }

    public static Task xResetAccessFailedCountAsync<TContext, TUser, TKey>(this IIdentityUserWithLockoutStoreWithContext<TContext, TUser, TKey> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithLockout<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.AccessFailedCount = 0);
    }

    public static Task xSetLockoutEnabledAsync<TContext, TUser, TKey>(this IIdentityUserWithLockoutStoreWithContext<TContext, TUser, TKey> store, TUser user, bool enabled, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithLockout<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.IsLockoutEnabled = enabled);
    }

    public static Task xSetLockoutEndDateAsync<TContext, TUser, TKey>(this IIdentityUserWithLockoutStoreWithContext<TContext, TUser, TKey> store, TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithLockout<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.LockoutEndDateUtc = lockoutEnd);
    }

  }

}