using System;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithTwoFactor<TKey> : IIdentityUserWithConcurrency<TKey> where TKey : IEquatable<TKey> {
    //[PersonalData]
    bool IsTwoFactorEnabled { get; set; }
  }
}