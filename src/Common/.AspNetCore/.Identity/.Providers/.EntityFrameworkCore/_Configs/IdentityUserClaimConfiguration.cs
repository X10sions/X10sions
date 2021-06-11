using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using static Common.AspNetCore.Identity.IdentityConstants;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public class IdentityUserClaimConfiguration<TKey> : IEntityTypeConfiguration<IIdentityUserClaim<TKey>>
  where TKey : IEquatable<TKey> {
    public IdentityUserClaimConfiguration(string tableName = UserClaimTableName) {
      this.tableName = tableName;
    }
    private string tableName;
    public void Configure(EntityTypeBuilder<IIdentityUserClaim<TKey>> builder) => builder.BuildIdentityUserClaim(tableName);
  }

}