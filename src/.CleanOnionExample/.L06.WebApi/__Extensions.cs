using Microsoft.AspNetCore.Identity;

namespace CleanOnionExample {
  public static class CleanOnionExample_WebApi_Extensions {
    public static async void AspNetCoreHeroBoilerplate(this WebApplicationBuilder builder) {
      using (var scope = builder.Host.Services.CreateScope()) {
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("app");
        try {
          var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
          var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

          //TODO  await DefaultRoles.SeedAsync(userManager, roleManager);
          //TODO  await DefaultSuperAdminUser.SeedAsync(userManager, roleManager);
          //TODO  await DefaultBasicUser.SeedAsync(userManager, roleManager);
          logger.LogInformation("Finished Seeding Default Data");
          logger.LogInformation("Application Starting");
        } catch (Exception ex) {
          logger.LogWarning(ex, "An error occurred seeding the DB");
        }
      }
    }

  }
}
