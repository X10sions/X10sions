using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;

namespace Common.AspNetCore.Identity {

  public static class OdbcIdentityBuilderExtensions {

    public static void ConfigureServices(this IServiceCollection services) {
      // Add framework services.
      //services.AddDbContext<ApplicationDbContext>(options =>
      //        options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

      services.AddSingleton(OdbcDocumentStoreHolder.Store);

      services.AddIdentity<OdbcIdentityUser_Only, OdbcIdentityRole_Only>()
          .UseOdbcDataStoreAdaptor<IDocumentStore>()
          .AddDefaultTokenProviders();

      //services.AddMvc();
      //services.AddTransient<IEmailSender, AuthMessageSender>();
      //services.AddTransient<ISmsSender, AuthMessageSender>();
    }


    private static IdentityBuilder UseOdbcDataStoreAdaptor<TDocumentStore>(this IdentityBuilder builder) where TDocumentStore : class, IDocumentStore
      => builder
      .AddOdbcUserStore<TDocumentStore>()
      .AddOdbcRoleStore<TDocumentStore>();

    private static IdentityBuilder AddOdbcUserStore<TDocumentStore>(this IdentityBuilder builder) {
      var userStoreType = typeof(OdbcIdentityUser_OnlyStore<,>).MakeGenericType(builder.UserType, typeof(TDocumentStore));
      return builder.AddCustomUserStore(userStoreType);
    }

    private static IdentityBuilder AddOdbcRoleStore<TDocumentStore>(this IdentityBuilder builder) {
      var roleStoreType = typeof(OdbcIdentityRole_OnlyStore<,>).MakeGenericType(builder.RoleType, typeof(TDocumentStore));
      return builder.AddCustomRoleStore(roleStoreType);
    }

  }
}
