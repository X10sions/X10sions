using System;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithSecurityStamp<TKey> : IIdentityUserWithConcurrency<TKey> where TKey : IEquatable<TKey> {
    string SecurityStamp { get; set; }
  }

}