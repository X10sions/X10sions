using Common.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public interface IIdentityUserDbContext<TKey> : IDbContext where TKey : IEquatable<TKey> {
    DbSet<IIdentityUser<TKey>> AspNetUsers { get; set; }
    DbSet<IIdentityUserClaim<TKey>> AspNetUserClaims { get; set; }
    DbSet<IIdentityUserLogin<TKey>> AspNetUserLogins { get; set; }
    DbSet<IIdentityUserToken<TKey>> AspNetUserTokens { get; set; }
  }
}