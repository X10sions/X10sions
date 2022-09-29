namespace Common.AspNetCore.Identity;

public static class IIdentityRoleExtensions {
  #region IQueryable

  public static async Task<IIdentityRole<TKey>> xFindByIdAsync<TKey>(this IQueryable<IIdentityRole<TKey>> roles, string roleId, CancellationToken cancellationToken) where TKey : IEquatable<TKey>
    => await roles.xFindByIdAsync(roleId.ConvertIdFromString<TKey>(), cancellationToken);

  #endregion

  #region RoleStoreBase: https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/Extensions.Stores/src/RoleStoreBase.cs

  public static async Task<string?> xGetRoleIdAsync<TKey>(this IIdentityRole<TKey> role, CancellationToken cancellationToken = default) where TKey : IEquatable<TKey> {
    cancellationToken.ThrowIfCancellationRequestedOrRoleNull(role);
    return await Task.FromResult(role.Id.ConvertIdToString());
  }

  #endregion

}
