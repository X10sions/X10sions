using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUserRole<TKey> : IdentityUserRole<TKey>, IIdentityUserRole<TKey> where TKey : IEquatable<TKey> {
    public virtual IIdentityUser<TKey> User { get; set; }
    public virtual IIdentityRole<TKey> Role { get; set; }
  }
}