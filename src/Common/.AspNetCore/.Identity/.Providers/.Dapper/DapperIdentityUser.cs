using System;
using System.Security.Principal;

namespace Common.AspNetCore.Identity.Providers.Dapper {
  public class DapperIdentityUser : IIdentityUser<Guid>, IIdentity {
    #region "IIdentity"
    public virtual string AuthenticationType { get; set; }
    public virtual bool IsAuthenticated { get; set; }
    public virtual string Name { get; set; }
    #endregion
    public virtual int AccessFailedCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public virtual bool LockoutEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public virtual DateTimeOffset? LockoutEnd { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public virtual string PasswordHash { get; set; }
    public virtual string NormalizedEmail { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public virtual string NormalizedUserName { get; set; }
    public virtual string NormalizedName{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public virtual string SecurityStamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public virtual Guid Id { get; set; } = Guid.NewGuid();
    public virtual bool EmailConfirmed { get; set; }
    public virtual bool PhoneNumberConfirmed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public virtual bool TwoFactorEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public virtual string Email { get; set; }
    public virtual string PhoneNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public virtual string UserName { get; set; }
    public virtual string ConcurrencyStamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
  }
}
