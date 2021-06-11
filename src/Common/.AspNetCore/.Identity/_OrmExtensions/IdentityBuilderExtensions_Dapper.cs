using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
namespace Common.AspNetCore.Identity {

  public static class IdentityBuilderExtensions_Dapper {

    public static void ConfigureServices(this IServiceCollection services) {
      // Add framework services.
      //services.AddDbContext<ApplicationDbContext>(options =>
      //        options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

      services.AddSingleton(DapperDocumentStoreHolder.Store);

      services.AddIdentity<DapperIdentityUser_Only, DapperIdentityRole_Only>()
          .UseDapperDataStoreAdaptor<IDocumentStore>()
          .AddDefaultTokenProviders();

      //services.AddMvc();
      //services.AddTransient<IEmailSender, AuthMessageSender>();
      //services.AddTransient<ISmsSender, AuthMessageSender>();
    }


    private static IdentityBuilder UseDapperDataStoreAdaptor<TDocumentStore>(this IdentityBuilder builder) where TDocumentStore : class, IDocumentStore
      => builder
      .AddDapperUserStore<TDocumentStore>()
      .AddDapperRoleStore<TDocumentStore>();

    private static IdentityBuilder AddDapperUserStore<TDocumentStore>(this IdentityBuilder builder) {
      var userStoreType = typeof(DapperIdentityUser_OnlyStore<,>).MakeGenericType(builder.UserType, typeof(TDocumentStore));
      return builder.AddCustomUserStore(userStoreType);
    }

    private static IdentityBuilder AddDapperRoleStore<TDocumentStore>(this IdentityBuilder builder) {
      var roleStoreType = typeof(DapperIdentityRole_OnlyStore<,>).MakeGenericType(builder.RoleType, typeof(TDocumentStore));
      return builder.AddCustomRoleStore(roleStoreType);
    }

  }
}
