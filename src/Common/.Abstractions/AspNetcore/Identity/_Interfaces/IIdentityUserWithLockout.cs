using System;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithLockout<TKey> : IIdentityUserWithConcurrency<TKey> where TKey : IEquatable<TKey> {
    int AccessFailedCount { get; set; }
    bool IsLockoutEnabled { get; set; }
    DateTimeOffset? LockoutEndDateUtc { get; set; }
  }
}