using System;

namespace Common.AspNetCore.Identity {
  public interface IIdentityContext_Microsoft<TKey>
    : IIdentityContext
    , IIdentityContext_WithUserAndRoleClaims<IIdentityRoleClaim<TKey>, TKey>
    , IIdentityContext_WithUserLogins<IIdentityUserLogin<TKey>, TKey>
    , IIdentityContext_WithUserTokens<IIdentityUserToken<TKey>, TKey>
    where TKey : IEquatable<TKey> { }

}