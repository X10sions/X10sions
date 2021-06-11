using System;
using System.Linq;

namespace Common.AspNetCore.Identity {
  public interface IIdentityContext_WithUserAndRoleClaims<TRoleClaim, TKey>
    : IIdentityContext_WithUserAndRoles<IIdentityRole<TKey>, TKey, IIdentityUserRole<TKey>>
    , IIdentityContext_WithUserClaims<IIdentityUserClaim<TKey>, TKey>
    where TRoleClaim : IIdentityRoleClaim<TKey>
    where TKey : IEquatable<TKey> {

    //IQueryable<IIdentityRoleClaim<TKey>> RoleClaimQueryable { get; set; }
    IIdentityDatabaseTable<IIdentityRoleClaim<TKey>> AspNetRoleClaims { get; set; }
  }

  public static class IIdentityContext_WithUserAndRoleClaimsExtensions {

    public static IQueryable<IIdentityRoleClaim<TKey>> GetRoleClaimQueryable<TRoleClaim, TKey>(this IIdentityContext_WithUserAndRoleClaims<TRoleClaim, TKey> context)
      where TKey : IEquatable<TKey>
      where TRoleClaim : class, IIdentityRoleClaim<TKey>
      => context.DbGetQueryable<IIdentityRoleClaim<TKey>>();

    public static IIdentityDatabaseTable<TRoleClaim> GetRoleClaimDatabaseTable<TRoleClaim, TKey>(this IIdentityContext_WithUserAndRoleClaims<TRoleClaim, TKey> context)
      where TKey : IEquatable<TKey>
      where TRoleClaim : class, IIdentityRoleClaim<TKey>
      => (IIdentityDatabaseTable<TRoleClaim>)context.GetRoleClaimQueryable();

  }
}