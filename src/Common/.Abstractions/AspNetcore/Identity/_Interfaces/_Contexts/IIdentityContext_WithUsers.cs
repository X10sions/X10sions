using System;
using System.Linq;

namespace Common.AspNetCore.Identity {

  public interface IIdentityContext_WithUsers<TUser, TKey> : IIdentityContext
    where TUser : IIdentityUser<TKey>
    where TKey : IEquatable<TKey> {
    //IQueryable<IIdentityUser<TKey>> UserQueryable { get; set; }
    IIdentityDatabaseTable<TUser> AspNetUsers { get; }
  }

  public static class IIdentityContext_WithUsersExtensions {

    public static IQueryable<TUser> GetUserQueryable<TUser, TKey>(this IIdentityContext_WithUsers<TUser, TKey> context)
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithConcurrency<TKey>
      => context.DbGetQueryable<TUser>();

    public static IIdentityDatabaseTable<TUser> GetUserDatabaseTable<TUser, TKey>(this IIdentityContext_WithUsers<TUser, TKey> context)
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithConcurrency<TKey>
      => (IIdentityDatabaseTable<TUser>)context.GetUserQueryable();

  }

}