using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using static Common.AspNetCore.Identity.IdentityConstants;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public class IdentityUserConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
    where TKey : IEquatable<TKey>
    where TEntity : class
    , IIdentityUser<TKey>
    , IIdentityUserWithClaims<TKey>
    , IIdentityUserWithConcurrency<TKey>
    , IIdentityUserWithEmail<TKey>
    , IIdentityUserWithLockout<TKey>
    , IIdentityUserWithLogins<TKey>
    , IIdentityUserWithPassword<TKey>
    , IIdentityUserWithPhoneNumber<TKey>
    , IIdentityUserWithRoles<TKey>
    , IIdentityUserWithSecurityStamp<TKey>
    , IIdentityUserWithTokens<TKey>
    , IIdentityUserWithTwoFactor<TKey> {

    public IdentityUserConfiguration(StoreOptions storeOptions, PersonalDataConverter personalDataConverter, string tableName = UserTableName) {
      this.storeOptions = storeOptions;
      this.tableName = tableName;
      this.personalDataConverter = personalDataConverter;
    }
    protected StoreOptions storeOptions;
    protected string tableName { get; }
    protected PersonalDataConverter personalDataConverter;

    public virtual void Configure(EntityTypeBuilder<TEntity> builder) {
      builder.BuildIdentityUser<TEntity, TKey>(storeOptions, personalDataConverter, tableName);
      builder.BuildIdentityUserWithClaims<TEntity, TKey>();
      builder.BuildIdentityUserWithEmail<TEntity, TKey>();
      builder.BuildIdentityUserWithLogins<TEntity, TKey>();
      builder.BuildIdentityUserWithRoles<TEntity, TKey>();
      builder.BuildIdentityUserWithTokens<TEntity, TKey>();
    }

  }
}