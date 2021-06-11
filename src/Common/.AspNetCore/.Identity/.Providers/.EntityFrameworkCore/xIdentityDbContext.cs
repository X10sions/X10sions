using Microsoft.EntityFrameworkCore;
using System;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {

  public class xIdentityDbContext : xIdentityDbContext<string> {
    public xIdentityDbContext(DbContextOptions options) : base(options) { }
    protected xIdentityDbContext() { }
  }

  public abstract class xIdentityDbContext<TKey> : xIdentityUserDbContext<TKey>, IIdentityDbContext<TKey> where TKey : IEquatable<TKey> {

    public xIdentityDbContext(DbContextOptions options) : base(options) { }
    protected xIdentityDbContext() { }

    public virtual DbSet<IIdentityRole<TKey>> AspNetRoles { get; set; }
    public virtual DbSet<IIdentityRoleClaim<TKey>> AspNetRoleClaims { get; set; }
    public virtual DbSet<IIdentityUserRole<TKey>> AspNetUserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
      base.OnModelCreating(builder);
      builder.BuildIdentityRole<TKey>();
      builder.BuildIdentityRoleClaim<TKey>();
      builder.BuildIdentityUserRole<TKey>();
    }

  }
}