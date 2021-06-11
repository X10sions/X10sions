using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using static Common.AspNetCore.Identity.IdentityConstants;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public class IdentityRoleClaimConfiguration<TKey> : IEntityTypeConfiguration<IIdentityRoleClaim<TKey>>
    where TKey : IEquatable<TKey> {
    public IdentityRoleClaimConfiguration(string tableName = RoleClaimTableName) {
      this.tableName = tableName;
    }
    private string tableName;
    public void Configure(EntityTypeBuilder<IIdentityRoleClaim<TKey>> builder) => builder.BuildIdentityRoleClaim(tableName);
  }
}
