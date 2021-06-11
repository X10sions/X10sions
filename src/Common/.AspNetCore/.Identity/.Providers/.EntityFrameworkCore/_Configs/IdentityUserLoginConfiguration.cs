using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using static Common.AspNetCore.Identity.IdentityConstants;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public class IdentityUserLoginConfiguration<TKey> : IEntityTypeConfiguration<IIdentityUserLogin<TKey>> where TKey : IEquatable<TKey> {
    public IdentityUserLoginConfiguration(StoreOptions storeOptions, string tableName = UserLoginTableName) {
      this.storeOptions = storeOptions;
      this.tableName = tableName;
    }
    private StoreOptions storeOptions;
    private string tableName;
    public void Configure(EntityTypeBuilder<IIdentityUserLogin<TKey>> builder) => builder.BuildIdentityUserLogin(storeOptions, tableName);
  }

}
