using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using static Common.AspNetCore.Identity.IdentityConstants;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public class IdentityUserRoleConfiguration<TKey> : IEntityTypeConfiguration<IIdentityUserRole<TKey>>
    where TKey : IEquatable<TKey> {
    public IdentityUserRoleConfiguration(string tableName = UserRoleTableName) {
      this.tableName = tableName;
    }
    private string tableName;
    public void Configure(EntityTypeBuilder<IIdentityUserRole<TKey>> builder) => builder.BuildIdentityUserRole(tableName);
  }

}
