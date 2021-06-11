using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public abstract class xIdentityUserDbContext<TKey> : DbContext, IIdentityUserDbContext<TKey> where TKey : IEquatable<TKey> {

    public xIdentityUserDbContext(DbContextOptions options) : base(options) { }

    protected xIdentityUserDbContext() { }

    protected PersonalDataConverter PersonalDataConverter => new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
    protected StoreOptions StoreOptions => this.zGetStoreOptions();

    public virtual DbSet<IIdentityUser<TKey>> AspNetUsers { get; set; }
    public virtual DbSet<IIdentityUserClaim<TKey>> AspNetUserClaims { get; set; }
    public virtual DbSet<IIdentityUserLogin<TKey>> AspNetUserLogins { get; set; }
    public virtual DbSet<IIdentityUserToken<TKey>> AspNetUserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
      base.OnModelCreating(builder);
      builder.BuildIdentityUser<TKey>(StoreOptions, PersonalDataConverter);
      builder.BuildIdentityUserClaim<TKey>();
      builder.BuildIdentityUserLogin<TKey>(StoreOptions);
      builder.BuildIdentityUserToken<TKey>(StoreOptions, PersonalDataConverter);

    }
  }

}