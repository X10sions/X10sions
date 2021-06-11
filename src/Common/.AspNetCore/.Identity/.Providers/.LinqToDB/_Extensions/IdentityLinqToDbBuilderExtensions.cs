using LinqToDB.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public static class IdentityLinqToDbBuilderExtensions {

    [Obsolete]
    public static IdentityBuilder AddLinqToDBtores<TContext>(this IdentityBuilder builder) where TContext : DataConnection {
      AddStores(builder.Services, builder.UserType, builder.RoleType, typeof(TContext));
      return builder;
    }

    [Obsolete]
    private static void AddStores(IServiceCollection services, Type userType, Type roleType, Type contextType) {
      var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));
      if (identityUserType == null) {
        throw new InvalidOperationException(Resources.NotIdentityUser);
      }
      var keyType = identityUserType.GenericTypeArguments[0];
      if (roleType != null) {
        var identityRoleType = FindGenericBaseType(roleType, typeof(IdentityRole<>));
        if (identityRoleType == null) {
          throw new InvalidOperationException(Resources.NotIdentityRole);
        }
        Type userStoreType;
        Type roleStoreType;
        var identityContext = FindGenericBaseType(contextType, typeof(IIdentityDataConnection<>));
        if (identityContext == null) {
          // If its a custom DbContext, we can only add the default POCOs
          userStoreType = typeof(LinqToDbUserStore<>).MakeGenericType(userType, roleType, contextType, keyType);
          roleStoreType = typeof(LinqToDbRoleStore<>).MakeGenericType(roleType, contextType, keyType);
        } else {
          userStoreType = typeof(LinqToDbUserStore<>).MakeGenericType(userType, roleType, contextType,
              identityContext.GenericTypeArguments[2],
              identityContext.GenericTypeArguments[3],
              identityContext.GenericTypeArguments[4],
              identityContext.GenericTypeArguments[5],
              identityContext.GenericTypeArguments[7],
              identityContext.GenericTypeArguments[6]);
          roleStoreType = typeof(LinqToDbRoleStore<>).MakeGenericType(roleType, contextType,
              identityContext.GenericTypeArguments[2],
              identityContext.GenericTypeArguments[4],
              identityContext.GenericTypeArguments[6]);
        }
        services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
        services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);
      } else {   // No Roles
        Type userStoreType ;
        var identityContext = FindGenericBaseType(contextType, typeof(IIdentityDataConnection<>));
        if (identityContext == null) {
          // If its a custom DbContext, we can only add the default POCOs
          userStoreType = typeof(LinqToDbUserOnlyStore<>).MakeGenericType(userType, contextType, keyType);
        } else {
          userStoreType = typeof(LinqToDbUserOnlyStore<>).MakeGenericType(userType, contextType,
              identityContext.GenericTypeArguments[1],
              identityContext.GenericTypeArguments[2],
              identityContext.GenericTypeArguments[3],
              identityContext.GenericTypeArguments[4]);
        }
        services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), (Type)null);
      }

    }

    private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType) {
      var type = currentType;
      while (type != null) {
        var typeInfo = type.GetTypeInfo();
        var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
        if (genericType != null && genericType == genericBaseType) {
          return typeInfo;
        }
        type = type.BaseType;
      }
      return null;
    }

  }
}
