using System;
using System.Linq;

namespace Common.AspNetCore.Identity {
  public interface IIdentityContext_WithUserAndRoles<TRole, TKey, TUserRole> : IIdentityContext_WithUsers<IIdentityUser<TKey>, TKey>
    where TRole : IIdentityRole<TKey>
    where TUserRole : IIdentityUserRole<TKey>
    where TKey : IEquatable<TKey> {

    //IQueryable<IIdentityRole<TKey>> RoleQueryable { get; set; }
    IIdentityDatabaseTable<IIdentityRole<TKey>> AspNetRoles { get; set; }

    //IQueryable<IIdentityUserRole<TKey>> UserRoleQueryable { get; set; }
    IIdentityDatabaseTable<IIdentityUserRole<TKey>> AspNetUserRoles { get; set; }
  }

  public static class IIdentityContext_WithUserAndRolesExtensions {

    public static IQueryable<TRole> GetRoleQueryable<TRole, TKey>(this IIdentityContext_WithUserAndRoles<TRole, TKey, IIdentityUserRole<TKey>> context)
      where TRole : class, IIdentityRoleWithConcurrency<TKey>
      where TKey : IEquatable<TKey>
      => context.DbGetQueryable<TRole>();

    public static IIdentityDatabaseTable<TRole> GetRoleDatabaseTable<TRole, TKey>(this IIdentityContext_WithUserAndRoles<TRole, TKey, IIdentityUserRole<TKey>> context)
      where TRole : class, IIdentityRoleWithConcurrency<TKey>
      where TKey : IEquatable<TKey>
      => (IIdentityDatabaseTable<TRole>)context.GetRoleQueryable();

    public static IQueryable<TUserRole> GetUserRoleQueryable<TKey, TUserRole>(this IIdentityContext_WithUserAndRoles<IIdentityRole<TKey>, TKey, TUserRole> context)
      where TKey : IEquatable<TKey>
      where TUserRole : class, IIdentityUserRole<TKey>
      => context.DbGetQueryable<TUserRole>();

    public static IIdentityDatabaseTable<TUserRole> GetUserRoleDatabaseTable<TKey, TUserRole>(this IIdentityContext_WithUserAndRoles<IIdentityRole<TKey>, TKey, TUserRole> context)
      where TKey : IEquatable<TKey>
      where TUserRole : class, IIdentityUserRole<TKey>
      => (IIdentityDatabaseTable<TUserRole>)context.GetUserRoleQueryable();

  }
}