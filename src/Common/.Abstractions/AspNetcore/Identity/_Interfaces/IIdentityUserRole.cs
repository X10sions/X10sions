using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserRole<TKey> where TKey : IEquatable<TKey> {
    TKey UserId { get; set; }
    TKey RoleId { get; set; }

    IIdentityUser<TKey> User { get; set; }
    IIdentityRole<TKey> Role { get; set; }
  }


  public static class IIdentityUserRoleExtensions {
    #region IQueryable

    public static async Task<IIdentityUserRole<TKey>> xFindAsync<TKey>(this IQueryable<IIdentityUserRole<TKey>> userRoles, TKey userId, TKey roleId, CancellationToken cancellationToken)
      where TKey : IEquatable<TKey>
      => await userRoles.xSingleOrDefaultAsync(u => u.UserId.Equals(userId) && u.RoleId.Equals(roleId), cancellationToken);

    #endregion

  }
}