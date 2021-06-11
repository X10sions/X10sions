using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserClaimStoreWithContext<TContext, TUser, TKey, TUserClaim>
    : IUserClaimStore<TUser>, IIdentityUserStoreWithContext<TContext, TUser, TKey>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUserClaims<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUserWithClaims<TKey>
    where TUserClaim : class, IIdentityUserClaim<TKey> {
    //IQueryable<TUserClaim> UserClaims { get; set; }
    //IIdentityDatabaseTable<TUserClaim> UserClaimsSet { get; set; }
  }

  public static class IIdentityUserClaimStoreWithContextExtensions {

    #region UserStore: https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/EntityFrameworkCore/src/UserStore.cs

    public static Task xAddClaimsAsync<TContext, TUser, TKey, TUserClaim>(this IIdentityUserClaimStoreWithContext<TContext, TUser, TKey, TUserClaim> store, TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserClaims<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithClaims<TKey>
      where TUserClaim : class, IIdentityUserClaim<TKey>, new() {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      if (claims == null) {
        throw new ArgumentNullException(nameof(claims));
      }
      foreach (var claim in claims) {
        store.Context.DbInsertAsync(user.xCreateUserClaim<TKey, TUserClaim>(claim));
      }
      return Task.FromResult(false);
    }

    public static async Task<IList<Claim>> xGetClaimsAsync<TContext, TUser, TKey, TUserClaim>(this IIdentityUserClaimStoreWithContext<TContext, TUser, TKey, TUserClaim> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserClaims<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithClaims<TKey>
      where TUserClaim : class, IIdentityUserClaim<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return await store.Context.DbGetQueryable<TUserClaim>().xGetClaimsAsync(user, cancellationToken);
    }

    public static async Task<IList<TUser>> xGetUsersForClaimAsync<TContext, TUser, TKey, TUserClaim>(this IIdentityUserClaimStoreWithContext<TContext, TUser, TKey, TUserClaim> store, Claim claim, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserClaims<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithClaims<TKey>
      where TUserClaim : class, IIdentityUserClaim<TKey> {
      store.ThrowIfCancelledRequestOrDisposed(cancellationToken);
      if (claim == null) {
        throw new ArgumentNullException(nameof(claim));
      }
      return await store.Context.DbGetQueryable<TUserClaim>().xGetUsersForClaimAsync(store.Context.DbGetQueryable<TUser>(), claim, cancellationToken);
    }

    public static async Task xRemoveClaimsAsync<TContext, TUser, TKey, TUserClaim>(this IIdentityUserClaimStoreWithContext<TContext, TUser, TKey, TUserClaim> store, TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserClaims<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithClaims<TKey>
      where TUserClaim : class, IIdentityUserClaim<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      if (claims == null) {
        throw new ArgumentNullException(nameof(claims));
      }
      foreach (var claim in claims) {
        var matchedClaims = await store.Context.DbGetQueryable<TUserClaim>().Where(user, claim).xToListAsync(cancellationToken);
        foreach (var c in matchedClaims) {
          await store.Context.DbDeleteAsync(c);
        }
      }
    }

    public static async Task xReplaceClaimAsync<TContext, TUser, TKey, TUserClaim>(this IIdentityUserClaimStoreWithContext<TContext, TUser, TKey, TUserClaim> store, TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserClaims<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithClaims<TKey>
      where TUserClaim : class, IIdentityUserClaim<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      if (claim == null) {
        throw new ArgumentNullException(nameof(claim));
      }
      if (newClaim == null) {
        throw new ArgumentNullException(nameof(newClaim));
      }
      var matchedClaims = await (store.Context.DbGetQueryable<TUserClaim>().Where(user, claim).Where(user, claim)).xToListAsync(cancellationToken);
      foreach (var matchedClaim in matchedClaims) {
        matchedClaim.ClaimValue = newClaim.Value;
        matchedClaim.ClaimType = newClaim.Type;
      }
    }

    #endregion

  }
}