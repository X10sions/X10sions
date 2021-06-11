using System;
using System.Linq;

namespace Common.AspNetCore.Identity {
  public interface IIdentityContext_WithUserClaims<TUserClaim, TKey> : IIdentityContext_WithUsers<IIdentityUser<TKey>, TKey>
    where TUserClaim : IIdentityUserClaim<TKey>
    //where TUser : IIdentityUser<TKey>
    where TKey : IEquatable<TKey> {
    //IQueryable<IIdentityUserClaim<TKey>> UserClaimQueryable { get; set; }
    IIdentityDatabaseTable<IIdentityUserClaim<TKey>> AspNetUserClaims { get; set; }
  }

  public static class IIdentityContext_WithUserClaimsExtensions {

    public static IQueryable<TUserClaim> GetUserClaimQueryable<TUserClaim, TKey>(this IIdentityContext_WithUserClaims<TUserClaim, TKey> context)
      where TKey : IEquatable<TKey>
      where TUserClaim : class, IIdentityUserClaim<TKey>
      => context.DbGetQueryable<TUserClaim>();

    public static IIdentityDatabaseTable<TUserClaim> GetUserClaimDatabaseTable<TUserClaim, TKey>(this IIdentityContext_WithUserClaims<TUserClaim, TKey> context)
      where TKey : IEquatable<TKey>
      where TUserClaim : class, IIdentityUserClaim<TKey>
      => (IIdentityDatabaseTable<TUserClaim>)context.GetUserClaimQueryable();

  }
}