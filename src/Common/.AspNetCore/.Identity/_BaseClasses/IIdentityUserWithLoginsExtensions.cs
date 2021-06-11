using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public static class IIdentityUserWithLoginsExtensions {

    #region https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/Extensions.Stores/src/UserStoreBase.cs

    public static TUserLogin xCreateUserLogin<TKey, TUserLogin>(this IIdentityUserWithLogins<TKey> user, UserLoginInfo login)
      where TKey : IEquatable<TKey>
      where TUserLogin : class, IIdentityUserLogin<TKey>, new() => new TUserLogin {
        UserId = user.Id,
        ProviderKey = login.ProviderKey,
        LoginProvider = login.LoginProvider,
        ProviderDisplayName = login.ProviderDisplayName
      };

    #endregion

  }
}