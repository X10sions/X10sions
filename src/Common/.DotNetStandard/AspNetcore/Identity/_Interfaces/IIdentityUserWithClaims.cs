using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithClaims<TKey> : IIdentityUserWithConcurrency<TKey> where TKey : IEquatable<TKey> {
    ICollection<IIdentityUserClaim<TKey>> UserClaims { get; set; }
  }

  public static class IIdentityUserWithClaimsExtensions {

    #region https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/Extensions.Stores/src/UserStoreBase.cs

    public static TUserClaim xCreateUserClaim<TKey, TUserClaim>(this IIdentityUserWithClaims<TKey> user, Claim claim)
      where TKey : IEquatable<TKey>
      where TUserClaim : class, IIdentityUserClaim<TKey>, new() {
      var userClaim = new TUserClaim { UserId = user.Id };
      userClaim.InitializeFromClaim(claim);
      return userClaim;
    }
    #endregion

  }

}