using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using static Common.AspNetCore.Identity.IdentityConstants;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public class IdentityUserTokenConfiguration<TKey> : IEntityTypeConfiguration<IIdentityUserToken<TKey>> where TKey : IEquatable<TKey> {
    public IdentityUserTokenConfiguration(StoreOptions storeOptions, PersonalDataConverter personalDataConverter, string tableName = UserTokenTableName) {
      this.storeOptions = storeOptions;
      this.tableName = tableName;
      this.personalDataConverter = personalDataConverter;
    }
    private StoreOptions storeOptions;
    private string tableName;
    private PersonalDataConverter personalDataConverter;
    public void Configure(EntityTypeBuilder<IIdentityUserToken<TKey>> builder) => builder.BuildIdentityUserToken(storeOptions, personalDataConverter, tableName);
  }

}
