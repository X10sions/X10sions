using System;
using System.Collections.Generic;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithRoles<TKey> : IIdentityUserWithConcurrency<TKey> where TKey : IEquatable<TKey> {
    ICollection<IIdentityUserRole<TKey>> UserRoles { get; set; }
  }

  public static class IIdentityUserWithRolesExtensions {

    #region https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/Extensions.Stores/src/UserStoreBase.cs

    public static TUserRole xCreateUserRole<TKey, TUserRole>(this IIdentityUserWithRoles<TKey> user, IIdentityRole<TKey> role)
      where TKey : IEquatable<TKey>
      where TUserRole : class, IIdentityUserRole<TKey>, new()
      => new TUserRole() {
        UserId = user.Id,
        RoleId = role.Id
      };

    #endregion

  }

}