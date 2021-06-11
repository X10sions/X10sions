using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUser_WithTwoFactor<TKey> : _BaseIdentityUser_WithConcurrency<TKey>, IIdentityUserWithTwoFactor<TKey>
    where TKey : IEquatable<TKey> {
    [PersonalData] public bool IsTwoFactorEnabled { get; set; }
  }
}