using System;
using System.Collections.Generic;

namespace Common.AspNetCore.Identity {
  public interface IIdentityRoleWithUsers<TKey> : IIdentityRoleWithConcurrency<TKey> where TKey : IEquatable<TKey> {
    ICollection<IIdentityUserRole<TKey>> UserRoles { get; set; }
  }

  public static class IIdentityRoleWithUsersExtensions {
  }
}