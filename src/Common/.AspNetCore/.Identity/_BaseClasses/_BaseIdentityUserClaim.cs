using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUserClaim<TKey> : IdentityUserClaim<TKey>, IIdentityUserClaim<TKey> where TKey : IEquatable<TKey> {
    public virtual IIdentityUser<TKey> User { get; set; }
  }
}