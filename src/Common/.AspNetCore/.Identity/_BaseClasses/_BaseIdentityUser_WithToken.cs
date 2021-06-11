using System;
using System.Collections.Generic;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUser_WithToken<TKey> : _BaseIdentityUser_WithConcurrency<TKey>, IIdentityUserWithTokens<TKey>
    where TKey : IEquatable<TKey> {
    public ICollection<IIdentityUserToken<TKey>> UserTokens { get; set; }
  }
}