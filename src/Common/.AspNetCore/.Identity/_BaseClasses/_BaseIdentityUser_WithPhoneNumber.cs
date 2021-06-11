using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUser_WithPhoneNumber<TKey> : _BaseIdentityUser_WithConcurrency<TKey>, IIdentityUserWithPhoneNumber<TKey>
    where TKey : IEquatable<TKey> {
    [ProtectedPersonalData] public string PhoneNumber { get; set; }
    [PersonalData] public bool IsPhoneNumberConfirmed { get; set; }
  }


}