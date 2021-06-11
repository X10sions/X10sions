using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityRole<TKey> : IId<TKey> where TKey : IEquatable<TKey> {
    //[PersonalData]
    //TKey Id { get; }
    string Name { get; set; }
    string NormalizedName { get; set; }
  }

  public static class IIdentityRoleExtensions {
    #region IQueryable

    public static async Task<IIdentityRole<TKey>> xFindByIdAsync<TKey>(this IQueryable<IIdentityRole<TKey>> roles, TKey roleId, CancellationToken cancellationToken) where TKey : IEquatable<TKey>
      => await roles.xSingleOrDefaultAsync(u => u.Id.Equals(roleId), cancellationToken);

    public static async Task<IIdentityRole<TKey>> xFindByNameAsync<TKey>(this IQueryable<IIdentityRole<TKey>> roles, string normalizedRoleName, CancellationToken cancellationToken) where TKey : IEquatable<TKey>
      => await roles.xSingleOrDefaultAsync(r => r.NormalizedName.Equals(normalizedRoleName), cancellationToken);

    #endregion

    #region RoleStoreBase: https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/Extensions.Stores/src/RoleStoreBase.cs

    public static async Task<string> xGetNormalizedRoleNameAsync<TKey>(this IIdentityRole<TKey> role, CancellationToken cancellationToken = default) where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrRoleNull(role);
      return await Task.FromResult(role.NormalizedName);
    }


    public static async Task<string> xGetRoleNameAsync<TKey>(this IIdentityRole<TKey> role, CancellationToken cancellationToken = default) where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrRoleNull(role);
      return await Task.FromResult(role.Name);
    }

    public static async Task xSetNormalizedRoleNameAsync<TKey>(this IIdentityRole<TKey> role, string normalizedName, CancellationToken cancellationToken = default) where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrRoleNull(role);
      await Task.FromResult(role.NormalizedName = normalizedName ?? throw new ArgumentNullException(nameof(normalizedName)));
    }

    public static async Task xSetRoleNameAsync<TKey>(this IIdentityRole<TKey> role, string roleName, CancellationToken cancellationToken = default) where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrRoleNull(role);
      await Task.FromResult(role.Name = roleName ?? throw new ArgumentNullException(nameof(roleName)));
    }

    #endregion

  }
}