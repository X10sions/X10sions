using CleanOnionExample.Data.Entities;
using CleanOnionExample.Data.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CleanOnionExample.Data.DbContexts;

public class IdentityDbContext : IdentityDbContext<ApplicationUser> {
  public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);
    modelBuilder.HasDefaultSchema("Identity");
    modelBuilder.Entity<ApplicationUser>(entity => { entity.ToTable(name: "User"); });
    modelBuilder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Role"); });
    modelBuilder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
    modelBuilder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
    modelBuilder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
    modelBuilder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });
    modelBuilder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens"); });
    modelBuilder.Seed();
  }
}
