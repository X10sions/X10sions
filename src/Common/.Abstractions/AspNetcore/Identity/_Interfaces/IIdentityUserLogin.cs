using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserLogin<TKey> where TKey : IEquatable<TKey> {
    string LoginProvider { get; set; }
    string ProviderKey { get; set; }
    string ProviderDisplayName { get; set; }
    TKey UserId { get; set; }

    IIdentityUser<TKey> User { get; set; }
  }

  public static class IIdentityUserLoginExtensions {
    #region IQueryable

    public static async Task<IIdentityUserLogin<TKey>> xFindAsync<TKey>(this IQueryable<IIdentityUserLogin<TKey>> userLogins, TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
      where TKey : IEquatable<TKey>
      => await userLogins.xSingleOrDefaultAsync(userLogin => userLogin.UserId.Equals(userId) && userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);

    #endregion

  }
}