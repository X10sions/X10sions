using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Common.AspNetCore.Identity {
  public interface IIdentityRoleWithClaims<TKey> : IIdentityRoleWithConcurrency<TKey> where TKey : IEquatable<TKey> {
    ICollection<IIdentityRoleClaim<TKey>> RoleClaims { get; set; }
  }

  public static class IIdentityRoleWithClaimsExtensions {
    #region IQueryable

    #endregion

    #region https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/Extensions.Stores/src/RoleStoreBase.cs

    public static TRoleClaim xCreateRoleClaim<TKey, TRoleClaim>(this IIdentityRoleWithClaims<TKey> role, Claim claim)
      where TKey : IEquatable<TKey>
      where TRoleClaim : IIdentityRoleClaim<TKey>, new() => new TRoleClaim {
        RoleId = role.Id,
        ClaimType = claim.Type,
        ClaimValue = claim.Value
      };

    #endregion
  }

}