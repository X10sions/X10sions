using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUser_Microsoft : _BaseIdentityUser_Microsoft<string> {
    public _BaseIdentityUser_Microsoft() {
      Id = Guid.NewGuid().ToString();
    }
  }

  public class _BaseIdentityUser_Microsoft<TKey>
    : IdentityUser<TKey>
    , IIdentityUserWithClaims<TKey>
    , IIdentityUserWithEmail<TKey>
    , IIdentityUserWithLockout<TKey>
    , IIdentityUserWithLogins<TKey>
    , IIdentityUserWithPassword<TKey>
    , IIdentityUserWithPhoneNumber<TKey>
    , IIdentityUserWithRoles<TKey>
    , IIdentityUserWithSecurityStamp<TKey>
    , IIdentityUserWithTokens<TKey>
    , IIdentityUserWithTwoFactor<TKey>
    where TKey : IEquatable<TKey> {
    public virtual ICollection<IIdentityUserClaim<TKey>> UserClaims { get; set; }
    public virtual ICollection<IIdentityUserLogin<TKey>> UserLogins { get; set; }
    public virtual ICollection<IIdentityUserRole<TKey>> UserRoles { get; set; }
    public virtual ICollection<IIdentityUserToken<TKey>> UserTokens { get; set; }

    string IIdentityUser<TKey>.Name { get => UserName; set => UserName = value; }
    string IIdentityUser<TKey>.NormalizedName { get => NormalizedUserName; set => NormalizedUserName = value; }

    string IIdentityUserWithEmail<TKey>.EmailAddress { get => Email; set => Email = value; }
    bool IIdentityUserWithEmail<TKey>.IsEmailConfirmed { get => EmailConfirmed; set => EmailConfirmed = value; }
    string IIdentityUserWithEmail<TKey>.NormalizedEmailAddress { get => NormalizedEmail; set => NormalizedEmail = value; }

    bool IIdentityUserWithLockout<TKey>.IsLockoutEnabled { get => LockoutEnabled; set => LockoutEnabled = value; }
    DateTimeOffset? IIdentityUserWithLockout<TKey>.LockoutEndDateUtc { get => LockoutEnd; set => LockoutEnd = value; }

    bool IIdentityUserWithPhoneNumber<TKey>.IsPhoneNumberConfirmed { get => PhoneNumberConfirmed; set => PhoneNumberConfirmed = value; }
    bool IIdentityUserWithTwoFactor<TKey>.IsTwoFactorEnabled { get => TwoFactorEnabled; set => TwoFactorEnabled = value; }

    public _BaseIdentityUser_Microsoft() { }

    public _BaseIdentityUser_Microsoft(string userName) : this() {
      UserName = userName ?? throw new ArgumentNullException(nameof(userName));
    }

  }
}