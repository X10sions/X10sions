using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public class IdentityRoleNav<TKey> : IdentityRole<TKey>
    , IIdentityRole<TKey> where TKey : IEquatable<TKey> { }

  public class IdentityRoleClaimNav<TKey> : IdentityRoleClaim<TKey>
    , IIdentityRoleClaim<TKey> where TKey : IEquatable<TKey> {
    public IIdentityRole<TKey> Role { get; set; }
  }

  public class IdentityUserNav<TKey> : IdentityUser<TKey>
    , IIdentityUser<TKey> where TKey : IEquatable<TKey>
    {
    public string Name { get => UserName;set => UserName = value;}
    public string NormalizedName { get => NormalizedUserName ;set => NormalizedName = value;}
  }

  public class IdentityUserClaimNav<TKey>: IdentityUserClaim<TKey>
    , IIdentityUserClaim<TKey> where TKey : IEquatable<TKey> {
    public IIdentityUser<TKey> User { get; set; }
  }

  public class IdentityUserLoginNav<TKey>: IdentityUserLogin<TKey>
    , IIdentityUserLogin<TKey> where TKey : IEquatable<TKey> {
    public IIdentityUser<TKey> User { get; set; }
  }

  public class IdentityUserRoleNav<TKey> : IdentityUserRole<TKey>
    , IIdentityUserRole<TKey> where TKey : IEquatable<TKey> {
    public IIdentityRole<TKey> Role { get; set; }
    public IIdentityUser<TKey> User { get; set; }
  }

  public class IdentityUserTokenNav<TKey> : IdentityUserToken<TKey>
    , IIdentityUserToken<TKey> where TKey : IEquatable<TKey> {
    public IIdentityUser<TKey> User { get; set; }
  }

}
