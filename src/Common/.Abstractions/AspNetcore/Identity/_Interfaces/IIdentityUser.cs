using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUser<TKey> : IId<TKey> where TKey : IEquatable<TKey> {
    //https://stackoverflow.com/questions/13614190/internal-property-setters-in-c-sharp
    //[PersonalData]
    //TKey Id { get; }
    //[ProtectedPersonalData]
    string Name { get; set; } //UserName
    string NormalizedName { get; set; } //NormalizedUserName
  }

  public static class IIdentityUserExtensions {
    #region IQueryable

    public static async Task<IIdentityUser<TKey>> xFindByIdAsync<TKey>(this IQueryable<IIdentityUser<TKey>> users, TKey userId, CancellationToken cancellationToken) where TKey : IEquatable<TKey>
      => await users.xSingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);

    public static async Task<IIdentityUser<TKey>> xFindByNameAsync<TKey>(this IQueryable<IIdentityUser<TKey>> users, string normalizedUserName, CancellationToken cancellationToken = default) where TKey : IEquatable<TKey>
      => await users.xSingleOrDefaultAsync(u => u.NormalizedName.Equals(normalizedUserName), cancellationToken);

    #endregion

    #region UserStoreBase: https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/Extensions.Stores/src/UserStoreBase.cs

    public static async Task<string> xGetNormalizedUserNameAsync<TKey>(this IIdentityUser<TKey> user, CancellationToken cancellationToken = default) where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
      return await Task.FromResult(user.NormalizedName);
    }

    public static async Task<string> xGetUserNameAsync<TKey>(this IIdentityUser<TKey> user, CancellationToken cancellationToken = default) where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
      return await Task.FromResult(user.Name);
    }

    public static async Task xSetNormalizedUserNameAsync<TKey>(this IIdentityUser<TKey> user, string normalizedName, CancellationToken cancellationToken = default) where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
      await Task.FromResult(user.NormalizedName = normalizedName ?? throw new ArgumentNullException(nameof(normalizedName)));
    }

    public static async Task xSetUserNameAsync<TKey>(this IIdentityUser<TKey> user, string userName, CancellationToken cancellationToken = default) where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
      await Task.FromResult(user.Name = userName ?? throw new ArgumentNullException(nameof(userName)));
    }

    #endregion

  }
}