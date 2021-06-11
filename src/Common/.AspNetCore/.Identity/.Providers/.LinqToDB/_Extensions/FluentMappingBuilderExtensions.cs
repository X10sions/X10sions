using LinqToDB.Mapping;
using Microsoft.AspNetCore.Identity;
using System;
using static Common.AspNetCore.Identity.IdentityConstants;
namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public static class FluentMappingBuilderExtensions {

    public static void BuildIdentityRole<TKey>(this FluentMappingBuilder builder, string tableName = RoleTableName)
      where TKey : IEquatable<TKey>
      => builder.Entity<IIdentityRole<TKey>>().BuildIdentityRole<IIdentityRole<TKey>, TKey>();

    public static void BuildIdentityRoleClaim<TKey>(this FluentMappingBuilder builder, string tableName = RoleClaimTableName) where TKey : IEquatable<TKey>
      => builder.Entity<IIdentityRoleClaim<TKey>>().BuildIdentityRoleClaim(tableName);

    public static void BuildIdentityUser<TKey>(this FluentMappingBuilder builder, StoreOptions storeOptions, DefaultPersonalDataProtector personalDataConverter, string tableName = UserTableName)
      where TKey : IEquatable<TKey>
      => builder.Entity<IIdentityUser<TKey>>().BuildIdentityUser<IIdentityUser<TKey>, TKey>(storeOptions, personalDataConverter, tableName);

    public static void BuildIdentityUserClaim<TKey>(this FluentMappingBuilder builder, string tableName = UserClaimTableName)
      where TKey : IEquatable<TKey>
      => builder.Entity<IIdentityUserClaim<TKey>>().BuildIdentityUserClaim(tableName);

    public static void BuildIdentityUserLogin<TKey>(this FluentMappingBuilder builder, StoreOptions storeOptions, string tableName = UserLoginTableName)
      where TKey : IEquatable<TKey>
      => builder.Entity<IIdentityUserLogin<TKey>>().BuildIdentityUserLogin(storeOptions, tableName);

    public static void BuildIdentityUserRole<TKey>(this FluentMappingBuilder builder, string tableName = UserRoleTableName)
      where TKey : IEquatable<TKey>
      => builder.Entity<IIdentityUserRole<TKey>>().BuildIdentityUserRole(tableName);

    public static void BuildIdentityUserToken<TKey>(this FluentMappingBuilder builder, StoreOptions storeOptions, DefaultPersonalDataProtector personalDataConverter, string tableName = UserTokenTableName)
      where TKey : IEquatable<TKey>
      => builder.Entity<IIdentityUserToken<TKey>>().BuildIdentityUserToken<IIdentityUserToken<TKey>, TKey>(storeOptions, personalDataConverter);

    //public static void BuildIdentityModelsForUserOnly<TKey>(this FluentMappingBuilder builder, StoreOptions storeOptions, DefaultPersonalDataProtector personalDataConverter)
    //  where TKey : IEquatable<TKey> {
    //  builder.BuildIdentityUser<TKey>(storeOptions, personalDataConverter);
    //  builder.BuildIdentityUserClaim<TKey>();
    //  builder.BuildIdentityUserLogin<TKey>(storeOptions);
    //  builder.BuildIdentityUserToken<TKey>(storeOptions, personalDataConverter);
    //}

    //public static void BuildIdentityModelsForUserAndRole<TKey>(this FluentMappingBuilder builder, StoreOptions storeOptions, DefaultPersonalDataProtector personalDataConverter)
    //  where TKey : IEquatable<TKey> {
    //  builder.BuildIdentityModelsForUserOnly<TKey>(storeOptions, personalDataConverter);
    //  builder.BuildIdentityUserAndRole<TKey>(storeOptions, personalDataConverter);
    //  builder.BuildIdentityRole<TKey>();
    //  builder.BuildIdentityRoleClaim<TKey>();
    //  builder.BuildIdentityUserRole<TKey>();
    //}

  }
}