using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using static Common.AspNetCore.Identity.IdentityConstants;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public static class EntityTypeBuilderExtensions {

    #region Role

    public static void BuildIdentityRole<TEntity, TKey>(this EntityTypeBuilder<TEntity> builder, string tableName = RoleTableName, string normalizedNameIndex = RoleTableNormalizedNameIndex)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityRole<TKey> {
      builder.ToTable(tableName);
      builder.HasKey(r => r.Id);
      builder.Property(u => u.Name).HasMaxLength(256);
      builder.Property(u => u.NormalizedName).HasMaxLength(256);
      builder.HasIndex(r => r.NormalizedName).HasDatabaseName(normalizedNameIndex).IsUnique();
    }

    public static void BuildIdentityRoleWithClaims<TEntity, TKey>(this EntityTypeBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityRoleWithClaims<TKey> {
      builder.HasMany(x => x.RoleClaims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
    }

    public static void BuildIdentityRoleWithConcurrency<TEntity, TKey>(this EntityTypeBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityRoleWithConcurrency<TKey> => builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

    public static void BuildIdentityRoleWithUsers<TEntity, TKey>(this EntityTypeBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityRoleWithUsers<TKey> => builder.HasMany(x => x.UserRoles).WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

    #endregion

    public static void BuildIdentityRoleClaim<TKey>(this EntityTypeBuilder<IIdentityRoleClaim<TKey>> builder, string tableName = RoleClaimTableName)
      where TKey : IEquatable<TKey>{
      builder.ToTable(tableName);
      builder.HasKey(rc => rc.Id);
    }

    #region User

    public static void BuildIdentityUser<TEntity, TKey>(this EntityTypeBuilder<TEntity> builder, StoreOptions storeOptions, PersonalDataConverter personalDataConverter, string tableName = UserTableName, string normalizedNameIndex = UserTableNormalizedNameIndex)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityUser<TKey> {
      builder.ToTable(tableName);
      builder.HasKey(u => u.Id);
      builder.Property(u => u.NormalizedName).HasMaxLength(256);
      builder.Property(u => u.Name).HasMaxLength(256);
      builder.HasIndex(u => u.NormalizedName).HasDatabaseName(normalizedNameIndex).IsUnique();
      builder.EncryptIdentityPersonalData(storeOptions, personalDataConverter);
    }

    public static void BuildIdentityUserWithClaims<TEntity, TKey>(this EntityTypeBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityUserWithClaims<TKey> => builder.HasMany(x => x.UserClaims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

    public static void BuildIdentityUserWithEmail<TEntity, TKey>(this EntityTypeBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityUserWithEmail<TKey> {
      builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
      builder.Property(u => u.EmailAddress).HasMaxLength(256);
      builder.Property(u => u.NormalizedEmailAddress).HasMaxLength(256);
      builder.HasIndex(u => u.NormalizedEmailAddress).HasDatabaseName("EmailIndex");
    }

    public static void BuildIdentityUserWithLogins<TEntity, TKey>(this EntityTypeBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityUserWithLogins<TKey> => builder.HasMany(x => x.UserLogins).WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

    public static void BuildIdentityUserWithRoles<TEntity, TKey>(this EntityTypeBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityUserWithRoles<TKey> => builder.HasMany(x => x.UserRoles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

    public static void BuildIdentityUserWithTokens<TEntity, TKey>(this EntityTypeBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityUserWithTokens<TKey> => builder.HasMany(x => x.UserTokens).WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

    #endregion

    public static void BuildIdentityUserClaim<TKey>(this EntityTypeBuilder<IIdentityUserClaim<TKey>> builder, string tableName = UserClaimTableName)
      where TKey : IEquatable<TKey> {
      builder.ToTable(tableName);
      builder.HasKey(uc => uc.Id);
    }

    public static void BuildIdentityUserLogin<TKey>(this EntityTypeBuilder<IIdentityUserLogin<TKey>> builder, StoreOptions storeOptions, string tableName = UserLoginTableName)
      where TKey : IEquatable<TKey> {
      builder.ToTable(tableName);
      builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });
      var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
      if (maxKeyLength > 0) {
        builder.Property(l => l.LoginProvider).HasMaxLength(maxKeyLength);
        builder.Property(l => l.ProviderKey).HasMaxLength(maxKeyLength);
      }
    }

    public static void BuildIdentityUserRole<TKey>(this EntityTypeBuilder<IIdentityUserRole<TKey>> builder, string tableName = UserRoleTableName)
      where TKey : IEquatable<TKey> {
      builder.ToTable(tableName);
      builder.HasKey(r => new { r.UserId, r.RoleId });
    }

    public static void BuildIdentityUserToken<TKey>(this EntityTypeBuilder<IIdentityUserToken<TKey>> builder, StoreOptions storeOptions, PersonalDataConverter personalDataConverter, string tableName = UserTokenTableName)
      where TKey : IEquatable<TKey> {
      builder.ToTable(tableName);
      builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
      var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
      if (maxKeyLength > 0) {
        builder.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
        builder.Property(t => t.Name).HasMaxLength(maxKeyLength);
      }
      builder.EncryptIdentityPersonalData(storeOptions, personalDataConverter);
    }

    public static void EncryptIdentityPersonalData<T>(this EntityTypeBuilder<T> builder, StoreOptions storeOptions, PersonalDataConverter personalDataConverter) where T : class {
      var encryptPersonalData = storeOptions?.ProtectPersonalData ?? false;
      if (encryptPersonalData) {
        var personalDataProps = typeof(T).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
        foreach (var p in personalDataProps) {
          if (p.PropertyType != typeof(string)) {
            throw new InvalidOperationException(Resources.CanOnlyProtectStrings);
          }
          builder.Property(typeof(string), p.Name).HasConversion(personalDataConverter);
        }
      }
    }

  }
}