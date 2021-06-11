using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {

  public class _BaseIdentityUser_Only : _BaseIdentityUser_Only<string> {
    public _BaseIdentityUser_Only() {
      Id = Guid.NewGuid().ToString();
    }
  }

  public class _BaseIdentityUser_Only<TKey> : IIdentityUser<TKey>
    where TKey : IEquatable<TKey> {
    public _BaseIdentityUser_Only() { }
    public _BaseIdentityUser_Only(string userName) : this() {
      Name = userName ?? throw new ArgumentNullException(nameof(userName));
    }
    [PersonalData] public TKey Id { get; internal set; }
    [ProtectedPersonalData] public string Name { get; set; }
    public string NormalizedName { get; set; }
  }

}