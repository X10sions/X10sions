using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Common.AspNetCore.Identity {

  public class _BaseIdentityRole_Microsoft : _BaseIdentityRole_Microsoft<string> {
    public _BaseIdentityRole_Microsoft() {
      Id = Guid.NewGuid().ToString();
    }
  }

  public class _BaseIdentityRole_Microsoft<TKey> : IdentityRole<TKey>
    , IIdentityRoleWithClaims<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityRole_Microsoft() { }

    public _BaseIdentityRole_Microsoft(string roleName) : this() {
      Name = roleName ?? throw new ArgumentNullException(nameof(roleName));
    }

    public virtual ICollection<IIdentityRoleClaim<TKey>> RoleClaims { get; set; }
    public virtual ICollection<IIdentityUserRole<TKey>> UserRoles { get; set; }
  }
}