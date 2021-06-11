using System;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUser_WithConcurrency<TKey> : _BaseIdentityUser_Only<TKey>, IIdentityUserWithConcurrency<TKey>
    where TKey : IEquatable<TKey> {
    public _BaseIdentityUser_WithConcurrency() { }
    public _BaseIdentityUser_WithConcurrency(string userName) : base(userName) { }
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
  }
}