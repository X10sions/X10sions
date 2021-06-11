using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUser_WithEmail<TKey> : _BaseIdentityUser_WithConcurrency<TKey>, IIdentityUserWithEmail<TKey>
    where TKey : IEquatable<TKey> {
    [ProtectedPersonalData] public string EmailAddress { get; set; }
    [PersonalData] public bool IsEmailConfirmed { get; set; }
    public string NormalizedEmailAddress { get; set; }
  }


}