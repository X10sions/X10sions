//using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserToken<TKey> where TKey : IEquatable<TKey> {
    TKey UserId { get; set; }
    string LoginProvider { get; set; }
    string Name { get; set; }
    //[ProtectedPersonalData]
    string Value { get; set; }

    IIdentityUser<TKey> User { get; set; }
  }

  public static class IIdentityTokenExtensions {

    public static async Task<IIdentityUserToken<TKey>> xFindAsync<TKey>(this IQueryable<IIdentityUserToken<TKey>> userTokens, IIdentityUser<TKey> user, string loginProvider, string name, CancellationToken cancellationToken)
      where TKey : IEquatable<TKey>
      => await userTokens.xSingleOrDefaultAsync(x => x.UserId.Equals(user.Id) && x.LoginProvider.Equals(loginProvider) && x.Name.Equals(name), cancellationToken);

  }
}