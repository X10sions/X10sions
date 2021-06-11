using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithPhoneNumberStoreWithContext<TContext, TUser, TKey> : IUserPhoneNumberStore<TUser>, IIdentityUserStoreWithContext<TContext, TUser, TKey>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUserWithPhoneNumber<TKey> {
  }

  public static class IIdentityUserWithPhoneNumberStoreWithContextExtensions {

    public static Task<string> xGetPhoneNumberAsync<TContext, TUser, TKey>(this IIdentityUserWithPhoneNumberStoreWithContext<TContext, TUser, TKey> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithPhoneNumber<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.PhoneNumber);
    }

    public static Task<bool> xGetPhoneNumberConfirmedAsync<TContext, TUser, TKey>(this IIdentityUserWithPhoneNumberStoreWithContext<TContext, TUser, TKey> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithPhoneNumber<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.IsPhoneNumberConfirmed);
    }

    public static Task xSetPhoneNumberAsync<TContext, TUser, TKey>(this IIdentityUserWithPhoneNumberStoreWithContext<TContext, TUser, TKey> store, TUser user, string phoneNumber, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithPhoneNumber<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.PhoneNumber = phoneNumber);
    }

    public static Task xSetPhoneNumberConfirmedAsync<TContext, TUser, TKey>(this IIdentityUserWithPhoneNumberStoreWithContext<TContext, TUser, TKey> store, TUser user, bool confirmed, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithPhoneNumber<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return Task.FromResult(user.IsPhoneNumberConfirmed = confirmed);
    }

  }
}