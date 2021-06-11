using System;
using System.Collections.Generic;

namespace Common.AspNetCore.Identity {

  public class _BaseIdentityRole_Only : _BaseIdentityRole_Only<string> {
    public _BaseIdentityRole_Only() {
      Id = Guid.NewGuid().ToString();
    }
  }

  public class _BaseIdentityRole_Only<TKey> : IIdentityRole<TKey>
    where TKey : IEquatable<TKey> {
    public _BaseIdentityRole_Only() { }
    public _BaseIdentityRole_Only(string roleName) : this() {
      Name = roleName ?? throw new ArgumentNullException(nameof(roleName));
    }
    public virtual TKey Id { get; internal set; }
    public virtual string Name { get; set; }
    public virtual string NormalizedName { get; set; }

    public ICollection<IIdentityUserRole<TKey>> Users { get; set; }


  }

}