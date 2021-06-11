using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserClaim<TKey> : IClaimConverter where TKey : IEquatable<TKey> {
    int Id { get; set; }
    TKey UserId { get; set; }
    IIdentityUser<TKey> User { get; set; }
  }

  public static class IIdentityUserClaimExtensions {

    public static async Task<IList<Claim>> xGetClaimsAsync<TKey, TUser>(this IQueryable<IIdentityUserClaim<TKey>> userClaims, TUser user, CancellationToken cancellationToken = default)
      where TUser : class, IIdentityUser<TKey>
      where TKey : IEquatable<TKey> {
      if (user == null) {
        throw new ArgumentNullException(nameof(user));
      }
      return await (from uc in userClaims.Where(user) select uc.ToClaim()).xToListAsync(cancellationToken);
    }

    public static async Task<IList<TUser>> xGetUsersForClaimAsync<TKey, TUser>(this IQueryable<IIdentityUserClaim<TKey>> userClaims, IQueryable<TUser> users, Claim claim, CancellationToken cancellationToken = default)
      where TUser : IIdentityUser<TKey>
      where TKey : IEquatable<TKey> {
      if (claim == null) {
        throw new ArgumentNullException(nameof(claim));
      }
      var query = from uc in userClaims.Where(claim)
                  join user in users on uc.UserId equals user.Id
                  select user;
      return await query.xToListAsync(cancellationToken);
    }

    public static IQueryable<IIdentityUserClaim<TKey>> Where<TKey, TUser>(this IQueryable<IIdentityUserClaim<TKey>> userClaims, TUser user)
      where TUser : class, IIdentityUser<TKey>
      where TKey : IEquatable<TKey>
      => userClaims.Where(uc => uc.UserId.Equals(user.Id));

    public static IQueryable<IIdentityUserClaim<TKey>> Where<TKey>(this IQueryable<IIdentityUserClaim<TKey>> userClaims, Claim claim)
      where TKey : IEquatable<TKey>
      => userClaims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type);

    public static IQueryable<IIdentityUserClaim<TKey>> Where<TKey, TUser>(this IQueryable<IIdentityUserClaim<TKey>> userClaims, TUser user, Claim claim)
      where TUser : class, IIdentityUser<TKey>
      where TKey : IEquatable<TKey>
      => userClaims.Where(user).Where(claim);

  }
}