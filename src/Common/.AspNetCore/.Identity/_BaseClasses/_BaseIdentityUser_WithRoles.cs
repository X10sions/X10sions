using System;
using System.Collections.Generic;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUser_WithRoles<TKey> : _BaseIdentityUser_WithConcurrency<TKey>, IIdentityUserWithRoles<TKey>
  where TKey : IEquatable<TKey> {
    public ICollection<IIdentityUserRole<TKey>> UserRoles { get; set; }
  }


}