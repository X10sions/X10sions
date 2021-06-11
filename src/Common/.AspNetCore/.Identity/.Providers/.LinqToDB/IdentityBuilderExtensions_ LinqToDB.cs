using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public static class IdentityBuilderExtensions_LinqToDB {

    public static void AddIdentity_BaseLinqToDBWithContext<TContext, TKey>(this IServiceCollection services)
      where TContext : class, IIdentityContext_LinqToDB//, IIdentityContext_WithUsers<_BaseIdentityUser_WithConcurrency<TKey>, TKey>
      where TKey : IEquatable<TKey> {
      services.AddScoped<IIdentityContext_LinqToDB, TContext>();

      services.AddIdentity<_BaseIdentityUser_WithConcurrency<TKey>, _BaseIdentityRole_WithConcurrency<TKey>>()
        .AddUserStore<_BaseIdentityUserStoreWithContext_LinqToDB<IIdentityContext_LinqToDB, _BaseIdentityUser_WithConcurrency<TKey>, TKey>>()
        .AddRoleStore<_BaseIdentityRoleStoreWithContext_LinqToDB<IIdentityContext_LinqToDB, _BaseIdentityRole_WithConcurrency<TKey>, TKey>>()
        .AddDefaultTokenProviders();

      //before services.AddMvc();
    }

    public static void AddIdentity_BaseLinqToDBDataContext<TKey>(this IServiceCollection services)
      where TKey : IEquatable<TKey>
      => services.AddIdentity_BaseLinqToDBWithContext<_BaseIdentityContext_LinqToDBDataContext, TKey>();

    public static void AddIdentity_BaseLinqToDBDataConnection<TKey>(this IServiceCollection services)
      where TKey : IEquatable<TKey>
      => services.AddIdentity_BaseLinqToDBWithContext<_BaseIdentityContext_LinqToDBDataConnection, TKey>();

  }
}