using System;
using System.Linq;

namespace Common.AspNetCore.Identity {
  public interface IIdentityContext_WithUserTokens<TUserToken, TKey> : IIdentityContext_WithUsers<IIdentityUser<TKey>, TKey>
    where TUserToken : IIdentityUserToken<TKey>
    where TKey : IEquatable<TKey> {
    //IQueryable<IIdentityUserToken<TKey>> UserTokenQueryable { get; set; }
    IIdentityDatabaseTable<IIdentityUserToken<TKey>> AspNetUserTokens { get; set; }
  }

  public static class IIdentityContext_WithUserTokensExtensions {

    public static IQueryable<TUserToken> GetUserTokenQueryable<TUserToken, TKey>(this IIdentityContext_WithUserTokens<TUserToken, TKey> context)
      where TKey : IEquatable<TKey>
      where TUserToken : class, IIdentityUserToken<TKey>
      => context.DbGetQueryable<TUserToken>();

    public static IIdentityDatabaseTable<TUserToken> GetUserTokenDatabaseTable<TUserToken, TKey>(this IIdentityContext_WithUserTokens<TUserToken, TKey> context)
      where TKey : IEquatable<TKey>
      where TUserToken : class, IIdentityUserToken<TKey>
      => (IIdentityDatabaseTable<TUserToken>)context.GetUserTokenQueryable();

  }
}