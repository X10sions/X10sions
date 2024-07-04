using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Common.NetCoreApp.AspNetCore.Identity;
public class MsEmpDbContext : IdentityDbContext<MsEmpApplicationUser, MsEmpApplicationRole, int> {
  public MsEmpDbContext(DbContextOptions<MsEmpDbContext> options)
      : base(options) {
  }
  protected override void OnModelCreating(ModelBuilder builder) => base.OnModelCreating(builder);
}
