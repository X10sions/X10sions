using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithPassword<TKey> : IIdentityUserWithConcurrency<TKey> where TKey : IEquatable<TKey> {
    string PasswordHash { get; set; }
  }

  public static class IIdentityUserWithPasswordExtensions {

    public static async Task<string> xGetPasswordHashAsync<TKey>(this IIdentityUserWithPassword<TKey> user, CancellationToken cancellationToken = default)
      where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
      return await Task.FromResult(user.PasswordHash);
    }

    public static async Task<bool> xHasPasswordAsync<TKey>(this IIdentityUserWithPassword<TKey> user, CancellationToken cancellationToken = default)
      where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
      return await Task.FromResult(user.PasswordHash != null);
    }

    public static async Task xSetPasswordHashAsync<TKey>(this IIdentityUserWithPassword<TKey> user, string passwordHash, CancellationToken cancellationToken = default)
      where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
      await Task.FromResult(user.PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash)));
    }

  }
}