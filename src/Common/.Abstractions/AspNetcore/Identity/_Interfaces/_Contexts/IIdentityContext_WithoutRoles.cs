using System;

namespace Common.AspNetCore.Identity {
  public interface IIdentityContext_WithoutRoles<TUser, TKey, TUserClaim, TUserLogin, TUserToken>
    : IIdentityContext_WithUserClaims<TUserClaim, TKey>
    , IIdentityContext_WithUserLogins<TUserLogin, TKey>
    , IIdentityContext_WithUserTokens<TUserToken, TKey>
    where TUser : IIdentityUser<TKey>
    where TUserClaim : IIdentityUserClaim<TKey>
    where TUserLogin : IIdentityUserLogin<TKey>
    where TUserToken : IIdentityUserToken<TKey>
    where TKey : IEquatable<TKey> { }

}