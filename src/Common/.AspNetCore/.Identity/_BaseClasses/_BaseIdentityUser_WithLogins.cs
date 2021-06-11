using System;
using System.Collections.Generic;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUser_WithLogins<TKey> : _BaseIdentityUser_WithConcurrency<TKey>, IIdentityUserWithLogins<TKey>
    where TKey : IEquatable<TKey> {
    public ICollection<IIdentityUserLogin<TKey>> UserLogins { get; set; }
  }
}