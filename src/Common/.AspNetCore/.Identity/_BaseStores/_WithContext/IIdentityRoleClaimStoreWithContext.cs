using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {

  public interface IIdentityRoleClaimStoreWithContext<TContext, TRole, TKey, TRoleClaim> : IRoleClaimStore<TRole>, IIdentityRoleStoreWithContext<TContext, TRole, TKey>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoleClaims<TKey>
    where TKey : IEquatable<TKey>
    where TRole : class, IIdentityRoleWithClaims<TKey>
    where TRoleClaim : class, IIdentityRoleClaim<TKey> {
    //IQueryable<TRoleClaim> RoleClaimQueryable { get; set; }
    //IIdentityDatabaseTable<TRoleClaim> RoleClaimDatabaseTable{ get; set; }
  }

  public static class IIdentityRoleClaimStoreWithContextExtensions {

    #region RoleStore: https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/EntityFrameworkCore/src/RoleStore.cs

    public static async Task xAddClaimAsync<TContext, TRole, TKey, TRoleClaim>(this IIdentityRoleClaimStoreWithContext<TContext, TRole, TKey, TRoleClaim> store, TRole role, Claim claim, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoleClaims<TKey>
      where TKey : IEquatable<TKey>
      where TRole : class, IIdentityRoleWithClaims<TKey>
      where TRoleClaim : class, IIdentityRoleClaim<TKey>, new() {
      store.ThrowIfCancelledRequestOrDisposedOrRoleNull(role, cancellationToken);
      if (claim == null) {
        throw new ArgumentNullException(nameof(claim));
      }
      var rc = role.xCreateRoleClaim<TKey, TRoleClaim>(claim);
      await store.Context.DbInsertAsync(rc, cancellationToken);
      await Task.CompletedTask;
    }

    public static async Task<IList<Claim>> xGetClaimsAsync<TContext, TRole, TKey, TRoleClaim>(this IIdentityRoleClaimStoreWithContext<TContext, TRole, TKey, TRoleClaim> store, TRole role, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoleClaims<TKey>
      where TKey : IEquatable<TKey>
      where TRole : class, IIdentityRoleWithClaims<TKey>
      where TRoleClaim : class, IIdentityRoleClaim<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrRoleNull(role, cancellationToken);
      return await store.Context.DbGetQueryable<TRoleClaim>().Where(rc => rc.RoleId.Equals(role.Id)).Select(c => new Claim(c.ClaimType, c.ClaimValue)).xToListAsync(cancellationToken);
    }

    // xTask RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default);

    public static async Task xRemoveClaimAsync<TContext, TRole, TKey, TRoleClaim>(this IIdentityRoleClaimStoreWithContext<TContext, TRole, TKey, TRoleClaim> store, TRole role, Claim claim, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoleClaims<TKey>
      where TKey : IEquatable<TKey>
      where TRole : class, IIdentityRoleWithClaims<TKey>
      where TRoleClaim : class, IIdentityRoleClaim<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrRoleNull(role, cancellationToken);
      if (claim == null) {
        throw new ArgumentNullException(nameof(claim));
      }
      var claims = await store.Context.DbGetQueryable<TRoleClaim>().Where(rc => rc.RoleId.Equals(role.Id) && rc.ClaimValue == claim.Value && rc.ClaimType == claim.Type).xToListAsync(cancellationToken);
      foreach (var c in claims) {
        await store.Context.DbDeleteAsync(c);
      }
    }

    #endregion

  }
}