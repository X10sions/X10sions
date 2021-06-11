using System;
using System.Collections.Generic;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUser_WithClaims<TKey> : _BaseIdentityUser_WithConcurrency<TKey>, IIdentityUserWithClaims<TKey>
    where TKey : IEquatable<TKey> {
    public ICollection<IIdentityUserClaim<TKey>> UserClaims { get; set; }
  }
}