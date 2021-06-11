using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {

  public interface IIdentityUserWithEmailStoreWithContext<TContext, TUser, TKey>
    : IUserEmailStore<TUser>, IIdentityUserStoreWithContext<TContext, TUser, TKey>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUserWithEmail<TKey> {
  }

  public static class IIdentityUserWithEmailStoreWithContextExtensions {

    public static async Task<TUser> xFindByEmailAsync<TContext, TUser, TKey>(this IIdentityUserWithEmailStoreWithContext<TContext, TUser, TKey> store, string normalizedEmail, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithEmail<TKey> {
      store.ThrowIfCancelledRequestOrDisposed(cancellationToken);
      return (TUser)await store.Context.DbGetQueryable<TUser>().xFindByEmailAsync(normalizedEmail, cancellationToken);
    }

  }

}