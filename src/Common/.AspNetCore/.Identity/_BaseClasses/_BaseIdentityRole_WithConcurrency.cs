using System;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityRole_WithConcurrency<TKey> : _BaseIdentityRole_Only<TKey>, IIdentityRoleWithConcurrency<TKey>
    where TKey : IEquatable<TKey> {
    public _BaseIdentityRole_WithConcurrency() { }
    public _BaseIdentityRole_WithConcurrency(string roleName) : base(roleName) { }
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
  }

}