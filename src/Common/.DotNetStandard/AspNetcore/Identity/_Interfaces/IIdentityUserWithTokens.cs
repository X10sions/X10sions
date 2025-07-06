using System;
using System.Collections.Generic;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithTokens<TKey> : IIdentityUserWithConcurrency<TKey> where TKey : IEquatable<TKey> {
    ICollection<IIdentityUserToken<TKey>> UserTokens { get; set; }
  }

  public static class IIdentityUserWithTokensExtensions {

    #region https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/Extensions.Stores/src/UserStoreBase.cs

    public static TUserToken xCreateUserToken<TKey, TUserToken>(this IIdentityUserWithTokens<TKey> user, string loginProvider, string name, string value)
      where TKey : IEquatable<TKey>
      where TUserToken : class, IIdentityUserToken<TKey>, new()
      => new TUserToken {
        UserId = user.Id,
        LoginProvider = loginProvider,
        Name = name,
        Value = value
      };

    #endregion

  }

}