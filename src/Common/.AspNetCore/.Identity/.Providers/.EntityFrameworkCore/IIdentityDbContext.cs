using Microsoft.EntityFrameworkCore;
using System;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public interface IIdentityDbContext<TKey> : IIdentityUserDbContext<TKey>
    where TKey : IEquatable<TKey> {
    DbSet<IIdentityRole<TKey>> AspNetRoles { get; set; }
    DbSet<IIdentityRoleClaim<TKey>> AspNetRoleClaims { get; set; }
    DbSet<IIdentityUserRole<TKey>> AspNetUserRoles { get; set; }
  }

}