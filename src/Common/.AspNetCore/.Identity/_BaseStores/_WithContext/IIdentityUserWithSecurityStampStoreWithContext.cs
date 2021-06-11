using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithSecurityStampStoreWithContext<TContext, TUser, TKey> : IUserSecurityStampStore<TUser>, IIdentityUserStoreWithContext<TContext, TUser, TKey>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUserWithSecurityStamp<TKey> {
  }

  public static class IIdentityUserWithSecurityStampWithContextExtensions {

    public static Task<string> xGetSecurityStampAsync<TContext, TUser, TKey>(this IIdentityUserWithSecurityStampStoreWithContext<TContext, TUser, TKey> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithSecurityStamp<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.SecurityStamp);
    }

    public static Task xSetSecurityStampAsync<TContext, TUser, TKey>(this IIdentityUserWithSecurityStampStoreWithContext<TContext, TUser, TKey> store, TUser user, string stamp, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithSecurityStamp<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.SecurityStamp = stamp ?? throw new ArgumentNullException(nameof(stamp)));
    }

  }
}