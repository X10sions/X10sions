using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Odbc;

namespace Common.AspNetCore.Identity.Providers.Dapper {
  public static class _Extensions {

    public static IServiceCollection AddDapperIdentity(IServiceCollection services, IConfiguration configuration) {
      // Add identity types
      services.AddIdentity<DapperIdentityUser, DapperIdentityRole>()
        .AddDefaultTokenProviders();
      // Identity Services
      services.AddTransient<IUserStore<IIdentityUser<Guid>>, DapperUserStore<Guid, IIdentityUser<Guid>>>();
      services.AddTransient<IRoleStore<IIdentityRole<Guid>>, DapperRoleStore<Guid>>();
      services.AddTransient(x => {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        return new OdbcConnection(connectionString);
      });

      //services.AddTransient<DapperUsersTable<Guid>>();

      //services.AddTransient(serviceProvider => {
      //  var dapperUsersTableName = configuration["DapperUsersTableName"];
      //  var connection = serviceProvider.GetService<OdbcConnection>();
      //  return new DapperUsersTable<Guid>(connection, dapperUsersTableName);
      //});

      //before
      //services.AddMvc();
      //// Add application services.
      //services.AddTransient<IEmailSender, AuthMessageSender>();
      //services.AddTransient<ISmsSender, AuthMessageSender>();
      return services;
    }

  }
}
