using Common.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserRoleStoreWithContext<TContext, TUser, TKey, TUserRole, TRole> : IUserRoleStore<TUser>, IIdentityUserStoreWithContext<TContext, TUser, TKey>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoles<TKey>
    where TKey : IEquatable<TKey>
    where TRole : class, IIdentityRole<TKey>
    where TUser : class, IIdentityUserWithRoles<TKey>
    where TUserRole : class, IIdentityUserRole<TKey> {
    //IQueryable<TUserRole> UserRoleQueryable { get; set; }
    //IIdentityDatabaseTable<TUserRole> UserRoleDatabaseTable { get; set; }
  }

  public static class IIdentityUserRoleStoreWithContextExtensions {

    #region https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/EntityFrameworkCore/src/UserStore.cs

    public static async Task xAddToRoleAsync<TContext, TUser, TKey, TUserRole, TRole>(this IIdentityUserRoleStoreWithContext<TContext, TUser, TKey, TUserRole, TRole> store, TUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoles<TKey>
      where TKey : IEquatable<TKey>
      where TUserRole : class, IIdentityUserRole<TKey>, new()
      where TRole : class, IIdentityRole<TKey>
      where TUser : class, IIdentityUserWithRoles<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      if (string.IsNullOrWhiteSpace(normalizedRoleName)) {
        throw new ArgumentException(Resources.ValueCannotBeNullOrEmpty, nameof(normalizedRoleName));
      }
      var roleEntity = await store.Context.DbGetQueryable<TRole>().xFindByNameAsync(normalizedRoleName, cancellationToken);
      if (roleEntity == null) {
        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.RoleNotFound, normalizedRoleName));
      }
      await store.Context.DbInsertAsync(user.xCreateUserRole<TKey, TUserRole>(roleEntity));
    }

    public static async Task<IList<string>> xGetRolesAsync<TContext, TUser, TKey, TUserRole, TRole>(this IIdentityUserRoleStoreWithContext<TContext, TUser, TKey, TUserRole, TRole> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoles<TKey>
      where TKey : IEquatable<TKey>
      where TUserRole : class, IIdentityUserRole<TKey>
      where TRole : class, IIdentityRole<TKey>
      where TUser : class, IIdentityUserWithRoles<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      var userId = user.Id;
      var query = from userRole in store.Context.DbGetQueryable<TUserRole>()
                    //join role in store.GetRoleQueryable() on userRole.RoleId equals role.Id
                  where userRole.UserId.Equals(userId)
                  select userRole.Role.Name;
      return await query.xToListAsync(cancellationToken);
    }

    public static async Task<IList<TUser>> xGetUsersInRoleAsync<TContext, TUser, TKey, TUserRole, TRole>(this IIdentityUserRoleStoreWithContext<TContext, TUser, TKey, TUserRole, TRole> store, string normalizedRoleName, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoles<TKey>
      where TKey : IEquatable<TKey>
      where TUserRole : class, IIdentityUserRole<TKey>
      where TRole : class, IIdentityRole<TKey>
      where TUser : class, IIdentityUserWithRoles<TKey> {
      store.ThrowIfCancelledRequestOrDisposed(cancellationToken);
      if (string.IsNullOrEmpty(normalizedRoleName)) {
        throw new ArgumentNullException(nameof(normalizedRoleName));
      }
      var role = await store.Context.DbGetQueryable<TRole>().xFindByNameAsync(normalizedRoleName, cancellationToken);
      if (role != null) {
        var query = from userrole in store.Context.DbGetQueryable<TUserRole>()
                      //join user in store.GetUserQueryable() on userrole.UserId equals user.Id
                    where userrole.RoleId.Equals(role.Id)
                    select (TUser)userrole.User;
        return await query.xToListAsync(cancellationToken);
      }
      return new List<TUser>();
    }

    public static async Task<bool> xIsInRoleAsync<TContext, TUser, TKey, TUserRole, TRole>(this IIdentityUserRoleStoreWithContext<TContext, TUser, TKey, TUserRole, TRole> store, TUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoles<TKey>
      where TKey : IEquatable<TKey>
      where TUserRole : class, IIdentityUserRole<TKey>
      where TRole : class, IIdentityRole<TKey>
      where TUser : class, IIdentityUserWithRoles<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      if (string.IsNullOrWhiteSpace(normalizedRoleName)) {
        throw new ArgumentException(Resources.ValueCannotBeNullOrEmpty, nameof(normalizedRoleName));
      }
      var role = await store.Context.DbGetQueryable<TRole>().xFindByNameAsync(normalizedRoleName, cancellationToken);
      if (role != null) {
        var userRole = await store.Context.DbGetQueryable<TUserRole>().xFindAsync(user.Id, role.Id, cancellationToken);
        return userRole != null;
      }
      return false;
    }

    public static async Task xRemoveFromRoleAsync<TContext, TUser, TKey, TUserRole, TRole>(this IIdentityUserRoleStoreWithContext<TContext, TUser, TKey, TUserRole, TRole> store, TUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoles<TKey>
      where TKey : IEquatable<TKey>
      where TRole : class, IIdentityRole<TKey>
      where TUserRole : class, IIdentityUserRole<TKey>
      where TUser : class, IIdentityUserWithRoles<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      if (string.IsNullOrWhiteSpace(normalizedRoleName)) {
        throw new ArgumentException(Resources.ValueCannotBeNullOrEmpty, nameof(normalizedRoleName));
      }
      var roleEntity = await store.Context.DbGetQueryable<TRole>().xFindByNameAsync(normalizedRoleName, cancellationToken);
      if (roleEntity != null) {
        var userRole = await store.Context.DbGetQueryable<TUserRole>().xFindAsync(user.Id, roleEntity.Id, cancellationToken);
        if (userRole != null) {
          await store.Context.DbDeleteAsync(userRole);
        }
      }
    }

    #endregion

  }
}