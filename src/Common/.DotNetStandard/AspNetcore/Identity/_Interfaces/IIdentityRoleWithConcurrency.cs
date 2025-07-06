using System;

namespace Common.AspNetCore.Identity {
  public interface IIdentityRoleWithConcurrency<TKey> : IIdentityRole<TKey>, IConcurrency<TKey> where TKey : IEquatable<TKey> { }

}