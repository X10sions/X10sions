using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.AspNetCore.Identity.Providers.Raven {

  public static class IdentityBuilderExtensions_RavenDb {

    public static void AddIdentity_BaseRavenDbIdentityContext<TKey>(this IServiceCollection services) where TKey : IEquatable<TKey> {

      // https://www.eximiaco.tech/en/2019/07/27/writing-an-asp-net-core-identity-storage-provider-from-scratch-with-ravendb/

      services.AddSingleton(RavenDbDocumentStoreHolder.Store);
      services.AddScoped<IIdentityContext_RavenDb, _BaseIdentityContext_RavenDb>();

      services.AddIdentity<_BaseIdentityUser_WithConcurrency<string>, _BaseIdentityRole_WithConcurrency<string>>()
        .AddUserStore<_BaseIdentityUserStoreWithContext_RavenDb<_BaseIdentityUser_WithConcurrency<string>, string>>()
        .AddRoleStore<_BaseIdentityRoleStoreWithContext_RavenDb<_BaseIdentityRole_WithConcurrency<string>, string>>()
        .AddDefaultTokenProviders();

      //before services.AddMvc();

    }

  }
}
