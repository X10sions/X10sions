using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using static Common.AspNetCore.Identity.IdentityConstants;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public static class ModelBuilderExtensions {

    public static void BuildIdentityRole<TKey>(this ModelBuilder builder, string tableName = RoleTableName)
      where TKey : IEquatable<TKey>
      => builder.Entity<IIdentityRole<TKey>>().BuildIdentityRole<IIdentityRole<TKey>,TKey>(tableName);

    public static void BuildIdentityRoleClaim<TKey>(this ModelBuilder builder, string tableName = RoleClaimTableName) where TKey : IEquatable<TKey>
      => builder.Entity<IIdentityRoleClaim<TKey>>().BuildIdentityRoleClaim(tableName);

    public static void BuildIdentityUser<TKey>(this ModelBuilder builder, StoreOptions storeOptions, PersonalDataConverter personalDataConverter, string tableName = UserTableName)
      where TKey : IEquatable<TKey>
      => builder.Entity<IIdentityUser<TKey>>().BuildIdentityUser<IIdentityUser<TKey>,TKey>(storeOptions, personalDataConverter, tableName);

    public static void BuildIdentityUserClaim<TKey>(this ModelBuilder builder, string tableName = UserClaimTableName)
      where TKey : IEquatable<TKey>
      => builder.Entity<IIdentityUserClaim<TKey>>().BuildIdentityUserClaim(tableName);

    public static void BuildIdentityUserLogin<TKey>(this ModelBuilder builder, StoreOptions storeOptions, string tableName = UserLoginTableName)
      where TKey : IEquatable<TKey>
      => builder.Entity<IIdentityUserLogin<TKey>>().BuildIdentityUserLogin(storeOptions, tableName);

    public static void BuildIdentityUserRole<TKey>(this ModelBuilder builder, string tableName = UserRoleTableName)
      where TKey : IEquatable<TKey>
      => builder.Entity<IIdentityUserRole<TKey>>().BuildIdentityUserRole(tableName);

    public static void BuildIdentityUserToken<TKey>(this ModelBuilder builder, StoreOptions storeOptions, PersonalDataConverter personalDataConverter, string tableName = UserTokenTableName)
      where TKey : IEquatable<TKey>
      //=> builder.Entity<IIdentityUserToken<TKey>>().BuildIdentityUserToken(storeOptions, personalDataConverter, tableName);
      => builder.ApplyConfiguration(new IdentityUserTokenConfiguration<TKey>(storeOptions, personalDataConverter, tableName));

    //public static void BuildIdentityModelsForUserOnly<TKey>(this ModelBuilder builder, StoreOptions storeOptions, PersonalDataConverter personalDataConverter)
    //  where TKey : IEquatable<TKey> {
    //  builder.BuildIdentityUser<TKey>(storeOptions, personalDataConverter);
    //  builder.BuildIdentityUserClaim<TKey>();
    //  builder.BuildIdentityUserLogin<TKey>(storeOptions);
    //  builder.BuildIdentityUserToken<TKey>(storeOptions, personalDataConverter);
    //}

    //public static void BuildIdentityModelsForUserAndRole<TKey>(this ModelBuilder builder, StoreOptions storeOptions, PersonalDataConverter personalDataConverter)
    //  where TKey : IEquatable<TKey> {
    //  builder.BuildIdentityModelsForUserOnly<TKey>(storeOptions, personalDataConverter);
    //  builder.BuildIdentityUserAndRole<TKey>(storeOptions, personalDataConverter);
    //  builder.BuildIdentityRole<TKey>();
    //  builder.BuildIdentityRoleClaim<TKey>();
    //  builder.BuildIdentityUserRole<TKey>();
    //}
  }
}