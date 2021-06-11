using System;

namespace Common.AspNetCore.Identity.Providers.Dapper {
  public class DapperIdentityRole : IIdentityRole<Guid> {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string NormalizedName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string ConcurrencyStamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
  }
}
