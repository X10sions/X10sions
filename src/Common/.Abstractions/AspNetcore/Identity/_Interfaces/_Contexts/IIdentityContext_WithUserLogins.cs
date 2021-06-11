using System;
using System.Linq;

namespace Common.AspNetCore.Identity {
  public interface IIdentityContext_WithUserLogins<TUserLogin, TKey> : IIdentityContext_WithUsers<IIdentityUser<TKey>, TKey>
    where TUserLogin : IIdentityUserLogin<TKey>
    where TKey : IEquatable<TKey> {
    //IQueryable<IIdentityUserLogin<TKey>> UserLoginQueryable { get; set; }
    IIdentityDatabaseTable<IIdentityUserLogin<TKey>> AspNetUserLogins { get; set; }
  }

  public static class IIdentityContext_WithUserLoginsExtensions {

    public static IQueryable<TUserLogin> GetUserLoginQueryable<TUserLogin, TKey>(this IIdentityContext_WithUserLogins<TUserLogin, TKey> context)
      where TKey : IEquatable<TKey>
      where TUserLogin : class, IIdentityUserLogin<TKey>
      => context.DbGetQueryable<TUserLogin>();

    public static IIdentityDatabaseTable<TUserLogin> GetUserLoginDatabaseTable<TUserLogin, TKey>(this IIdentityContext_WithUserLogins<TUserLogin, TKey> context)
      where TKey : IEquatable<TKey>
      where TUserLogin : class, IIdentityUserLogin<TKey>
      => (IIdentityDatabaseTable<TUserLogin>)context.GetUserLoginQueryable();
  }
}
