//using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithPhoneNumber<TKey> : IIdentityUserWithConcurrency<TKey> where TKey : IEquatable<TKey> {
    //[ProtectedPersonalData]
    string PhoneNumber { get; set; }
    //[PersonalData]
    bool IsPhoneNumberConfirmed { get; set; }
  }
}