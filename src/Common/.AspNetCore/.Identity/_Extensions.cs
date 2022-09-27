using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Common.AspNetCore.Identity {
  public static class CommonIdentityBuilderExtensions {
    public static void AddIdentity_Custom<TContext, TKey, TUser, TRole, TUserStore, TRoleStore>(this IServiceCollection services)
      where TContext : class, IIdentityContext
      where TUser : class, IIdentityUserWithConcurrency<TKey>
      where TRole : class, IIdentityRoleWithConcurrency<TKey>
      where TRoleStore : class, IIdentityRoleStoreWithContext<TContext, TRole, TKey>
      where TUserStore : class, IIdentityUserStoreWithContext<TContext, TUser, TKey>
      where TKey : IEquatable<TKey> {
      services.AddScoped<IIdentityContext, TContext>();

      
      services.AddIdentity<TUser, TRole>()
        .AddUserStore<TUserStore>()
        .AddRoleStore<TRoleStore>()
        .AddDefaultTokenProviders();

      //before services.AddMvc();
    }


    public static IdentityBuilder AddCustomUserStore<TContext, TKey>(this IdentityBuilder builder)
      where TContext : IIdentityContext
      where TKey : IEquatable<TKey> {
      var userStoreType = typeof(IIdentityUserStoreWithContext<,,>).MakeGenericType(typeof(TContext), builder.UserType, typeof(TKey));
      return builder.AddCustomUserStore<TKey>(userStoreType);
    }

    public static IdentityBuilder AddCustomUserStore<TKey>(this IdentityBuilder builder, Type userStoreType)
      where TKey : IEquatable<TKey> {
      builder.Services.AddScoped(typeof(IUserStore<>).MakeGenericType(builder.UserType), userStoreType);
      return builder;
    }

    public static IdentityBuilder AddCustomRoleStore<TContext, TKey>(this IdentityBuilder builder)
      where TContext : IIdentityContext
      where TKey : IEquatable<TKey> {
      var roleStoreType = typeof(IIdentityUserStoreWithContext<,,>).MakeGenericType(typeof(TContext), builder.UserType, typeof(TKey));
      return builder.AddCustomRoleStore<TKey>(roleStoreType);
    }

    public static IdentityBuilder AddCustomRoleStore<TKey>(this IdentityBuilder builder, Type roleStoreType)
      where TKey : IEquatable<TKey> {
      builder.Services.AddScoped(typeof(IRoleStore<>).MakeGenericType(builder.RoleType), roleStoreType);
      return builder;
    }

  }
}
