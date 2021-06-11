using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithTwoFactorStoreWithContext<TContext, TUser, TKey>
    : IUserTwoFactorStore<TUser>, IIdentityUserStoreWithContext<TContext, TUser, TKey>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUserWithTwoFactor<TKey> {
  }

  public static class IIdentityUserWithTwoFactorStoreWithContextExtensions {

    public static Task<bool> xGetTwoFactorEnabledAsync<TContext, TUser, TKey>(this IIdentityUserWithTwoFactorStoreWithContext<TContext, TUser, TKey> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithTwoFactor<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.IsTwoFactorEnabled);
    }

    public static Task xSetTwoFactorEnabledAsync<TContext, TUser, TKey>(this IIdentityUserWithTwoFactorStoreWithContext<TContext, TUser, TKey> store, TUser user, bool enabled, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithTwoFactor<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.IsTwoFactorEnabled = enabled);
    }

  }

}