using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUserLogin<TKey> : IdentityUserLogin<TKey>, IIdentityUserLogin<TKey> where TKey : IEquatable<TKey> {
    public virtual IIdentityUser<TKey> User { get; set; }
  }
}