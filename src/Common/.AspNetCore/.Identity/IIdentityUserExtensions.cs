namespace Common.AspNetCore.Identity;
public static class IIdentityUserExtensions {
  #region IQueryable
  public static async Task<IIdentityUser<TKey>> xFindByIdAsync<TKey>(this IQueryable<IIdentityUser<TKey>> users, string userId, CancellationToken cancellationToken) where TKey : IEquatable<TKey>
     => await users.xFindByIdAsync(userId.ConvertIdFromString<TKey>(), cancellationToken);
  #endregion

  #region RoleStoreBase: https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/Extensions.Stores/src/RoleStoreBase.cs

  public static async Task<string?> xGetUserIdAsync<TKey>(this IIdentityUser<TKey> user, CancellationToken cancellationToken = default) where TKey : IEquatable<TKey> {
    cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
    return await Task.FromResult(user.Id.ConvertIdToString());
  }

  #endregion
}
