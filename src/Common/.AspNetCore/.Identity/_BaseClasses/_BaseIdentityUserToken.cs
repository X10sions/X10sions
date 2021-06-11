using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUserToken<TKey> : IdentityUserToken<TKey>, IIdentityUserToken<TKey> where TKey : IEquatable<TKey> {
    public virtual IIdentityUser<TKey> User { get; set; }
  }
}