using System;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithConcurrency<TKey> : IIdentityUser<TKey>, IConcurrency<TKey> where TKey : IEquatable<TKey> {
  }
}