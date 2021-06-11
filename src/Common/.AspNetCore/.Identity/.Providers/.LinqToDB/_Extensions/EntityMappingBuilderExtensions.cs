using LinqToDB.Mapping;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using static Common.AspNetCore.Identity.IdentityConstants;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public static class EntityMappingBuilderExtensions {

    #region Role

    public static void BuildIdentityRole<TEntity, TKey>(this EntityMappingBuilder<TEntity> builder, string tableName = RoleTableName)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityRole<TKey> {
      builder.HasTableName(tableName);
      builder.HasPrimaryKey(r => r.Id);
      builder.Property(u => u.Name).HasLength(256);
      builder.Property(u => u.NormalizedName).HasLength(256);
      //builder.HasIndex(r => r.NormalizedName).HasDatabaseName(normalizedNameIndex).IsUnique();
    }

    public static void BuildIdentityRoleWithClaims<TEntity, TKey>(this EntityMappingBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityRoleWithClaims<TKey> {
      builder.Association(x => x.RoleClaims, k => k.Id, rc => rc.RoleId, false);
    }

    //public static void BuildIdentityRoleWithConcurrency<TEntity, TKey>(this EntityMappingBuilder<TEntity> builder)
    //  where TKey : IEquatable<TKey>
    //  where TEntity : class, IIdentityRoleWithConcurrency<TKey> => builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

    public static void BuildIdentityRoleWithUsers<TEntity, TKey>(this EntityMappingBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityRoleWithUsers<TKey> => builder.Association(x => x.UserRoles, k => k.Id, ur => ur.RoleId, false);

    #endregion

    public static void BuildIdentityRoleClaim<TKey>(this EntityMappingBuilder<IIdentityRoleClaim<TKey>> builder, string tableName = RoleClaimTableName)
      where TKey : IEquatable<TKey> {
      builder.HasTableName(tableName);
      builder.HasPrimaryKey(rc => rc.Id);
      builder.Association(x => x.Role, k => k.RoleId, other => other.Id, true);
    }

    #region User

    public static void BuildIdentityUser<TEntity, TKey>(this EntityMappingBuilder<TEntity> builder, StoreOptions storeOptions, DefaultPersonalDataProtector personalDataConverter, string tableName = UserTableName)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityUser<TKey> {
      builder.HasTableName(tableName);
      builder.HasPrimaryKey (u => u.Id);
      builder.Property(u => u.NormalizedName).HasLength(256);
      builder.Property(u => u.Name).HasLength(256);
      ///builder.HasIndex(u => u.NormalizedName).HasDatabaseName(normalizedNameIndex).IsUnique();
      storeOptions.EncryptIdentityPersonalData<TEntity>(personalDataConverter);
    }

    public static void BuildIdentityUserWithClaims<TEntity, TKey>(this EntityMappingBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityUserWithClaims<TKey> => builder.Association(x => x.UserClaims, k => k.Id, other => other.UserId, false);

    public static void BuildIdentityUserWithEmail<TEntity, TKey>(this EntityMappingBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityUserWithEmail<TKey> {
      //builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
      builder.Property(u => u.EmailAddress).HasLength(256);
      builder.Property(u => u.NormalizedEmailAddress).HasLength(256);
      ///builder.HasIndex(u => u.NormalizedEmailAddress).HasDatabaseName("EmailIndex");
    }

    public static void BuildIdentityUserWithLogins<TEntity, TKey>(this EntityMappingBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityUserWithLogins<TKey> => builder.Association(x => x.UserLogins, k => k.Id, other => other.UserId, false);

    public static void BuildIdentityUserWithRoles<TEntity, TKey>(this EntityMappingBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityUserWithRoles<TKey> => builder.Association(x => x.UserRoles, k => k.Id, other => other.UserId, false);

    public static void BuildIdentityUserWithTokens<TEntity, TKey>(this EntityMappingBuilder<TEntity> builder)
      where TKey : IEquatable<TKey>
      where TEntity : class, IIdentityUserWithTokens<TKey> => builder.Association(x => x.UserTokens, k => k.Id, other => other.UserId, false);

    #endregion

    public static void BuildIdentityUserClaim<TKey>(this EntityMappingBuilder<IIdentityUserClaim<TKey>> builder, string tableName = UserClaimTableName)
      where TKey : IEquatable<TKey> {
      builder.HasTableName(tableName);
      builder.HasPrimaryKey(uc => uc.Id);
      builder.Association(x => x.User, k => k.UserId, other => other.Id, false);
    }

    public static void BuildIdentityUserLogin<TKey>(this EntityMappingBuilder<IIdentityUserLogin<TKey>> builder, StoreOptions storeOptions, string tableName = UserLoginTableName)
      where TKey : IEquatable<TKey> {
      builder.HasTableName(tableName);
      builder.HasPrimaryKey(l => new { l.LoginProvider, l.ProviderKey });
      var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
      if (maxKeyLength > 0) {
        builder.Property(l => l.LoginProvider).HasLength(maxKeyLength);
        builder.Property(l => l.ProviderKey).HasLength(maxKeyLength);
      }
      builder.Association(x => x.User, k => k.UserId, other => other.Id, false);
    }

    public static void BuildIdentityUserRole<TKey>(this EntityMappingBuilder<IIdentityUserRole<TKey>> builder, string tableName = UserRoleTableName)
      where TKey : IEquatable<TKey> {
      builder.HasTableName(tableName);
      builder.HasPrimaryKey(r => new { r.UserId, r.RoleId });
      builder.Association(x => x.User, k => k.UserId, other => other.Id, false);
    }

    public static void BuildIdentityUserToken<TUser, TKey>(this EntityMappingBuilder<TUser> builder, StoreOptions storeOptions, DefaultPersonalDataProtector personalDataConverter, string tableName = UserTokenTableName)
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserToken<TKey> {
      builder.HasTableName(tableName);
      builder.HasPrimaryKey(t => new { t.UserId, t.LoginProvider, t.Name });
      var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
      if (maxKeyLength > 0) {
        builder.Property(t => t.LoginProvider).HasLength(maxKeyLength);
        builder.Property(t => t.Name).HasLength(maxKeyLength);
      }
      storeOptions.EncryptIdentityPersonalData<TUser>(personalDataConverter);
    }

    public static void EncryptIdentityPersonalData<T>(this StoreOptions storeOptions, DefaultPersonalDataProtector personalDataConverter)
      where T : class {
      var encryptPersonalData = storeOptions?.ProtectPersonalData ?? false;
      if (encryptPersonalData) {
        var personalDataProps = typeof(T).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
        foreach (var p in personalDataProps) {
          if (p.PropertyType != typeof(string)) {
            throw new InvalidOperationException("Can Only Protect Strings");
          }
          //builder.Property(e => Sql.Property<string>(e, p.Name)).HasConversion(personalDataConverter);
          throw new NotImplementedException();
        }
      }

    }

  }

}