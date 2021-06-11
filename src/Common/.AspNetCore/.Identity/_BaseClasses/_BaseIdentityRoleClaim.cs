using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityRoleClaim<TKey> : IdentityRoleClaim<TKey>, IIdentityRoleClaim<TKey> where TKey : IEquatable<TKey> {
    public virtual IIdentityRole<TKey> Role { get; set; }
  }
}