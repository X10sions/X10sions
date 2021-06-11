using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using static Common.AspNetCore.Identity.IdentityConstants;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public class IdentityRoleConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
    where TKey : IEquatable<TKey>
    where TEntity : class, IIdentityRole<TKey>
      , IIdentityRoleWithClaims<TKey>
      , IIdentityRoleWithConcurrency<TKey>
      , IIdentityRoleWithUsers<TKey> {

    public IdentityRoleConfiguration(string tableName = RoleTableName) {
      this.tableName = tableName;
    }

    protected string tableName { get; }

    public void Configure(EntityTypeBuilder<TEntity> builder) {
      builder.BuildIdentityRole<TEntity, TKey>(tableName);
      builder.BuildIdentityRoleWithClaims<TEntity, TKey>();
      builder.BuildIdentityRoleWithConcurrency<TEntity, TKey>();
      builder.BuildIdentityRoleWithUsers<TEntity, TKey>();
    }

  }
}