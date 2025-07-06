using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithEmail<TKey> : IIdentityUserWithConcurrency<TKey> where TKey : IEquatable<TKey> {
    //[ProtectedPersonalData]
    string EmailAddress { get; set; }
    //[PersonalData]
    bool IsEmailConfirmed { get; set; }
    string NormalizedEmailAddress { get; set; }
  }

  public static class IIdentityUserEmailExtensions {

    public static async Task<IIdentityUserWithEmail<TKey>> xFindByEmailAsync<TKey>(this IQueryable<IIdentityUserWithEmail<TKey>> users, string normalizedEmail, CancellationToken cancellationToken = default)
      where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequested();
      return await users.xFirstOrDefaultAsync(u => u.NormalizedEmailAddress == normalizedEmail, cancellationToken);
    }

    public static async Task<string> xGetEmailAsync<TKey>(this IIdentityUserWithEmail<TKey> user, CancellationToken cancellationToken = default)
      where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
      return await Task.FromResult(user.EmailAddress);
    }

    public static async Task<bool> xGetEmailConfirmedAsync<TKey>(this IIdentityUserWithEmail<TKey> user, CancellationToken cancellationToken = default)
      where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
      return await Task.FromResult(user.IsEmailConfirmed);
    }

    public static async Task<string> xGetNormalizedEmailAsync<TKey>(this IIdentityUserWithEmail<TKey> user, CancellationToken cancellationToken = default)
      where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
      return await Task.FromResult(user.NormalizedEmailAddress);
    }

    public static async Task xSetEmailAsync<TKey>(this IIdentityUserWithEmail<TKey> user, string email, CancellationToken cancellationToken = default)
      where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
      await Task.FromResult(user.EmailAddress = email);
    }

    public static async Task xSetEmailConfirmedAsync<TKey>(this IIdentityUserWithEmail<TKey> user, bool confirmed, CancellationToken cancellationToken = default)
      where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
      await Task.FromResult(user.IsEmailConfirmed = confirmed);
    }

    public static async Task xSetNormalizedEmailAsync<TKey>(this IIdentityUserWithEmail<TKey> user, string normalizedEmail, CancellationToken cancellationToken = default)
      where TKey : IEquatable<TKey> {
      cancellationToken.ThrowIfCancellationRequestedOrUserNull(user);
      await Task.FromResult(user.NormalizedEmailAddress = normalizedEmail);
    }


  }
}