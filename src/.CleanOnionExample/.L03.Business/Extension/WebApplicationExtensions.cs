using CleanOnionExample.Data.Entities;
using CleanOnionExample.Data.Seeds;
using Microsoft.AspNetCore.Identity;

namespace CleanOnionExample.Infrastructure.Extension {
  public static class WebApplicationExtensions {

    public async static System.Threading.Tasks.Task CleanArch_ProgramMain<TProgram>(this WebApplication app) {
      using (var scope = app.Services.CreateScope()) {
        var services = scope.ServiceProvider;
        try {
          var context = services.GetRequiredService<ApplicationDbContext>();
          if (context.Database.IsSqlServer()) {
            context.Database.Migrate();
          }
          var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
          var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
          await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
          await ApplicationDbContextSeed.SeedSampleDataAsync(context);
        } catch (Exception ex) {
          var logger = scope.ServiceProvider.GetRequiredService<ILogger<TProgram>>();
          logger.LogError(ex, "An error occurred while migrating or seeding the database.");
          throw;
        }
      }
    }

    public static void ConfigureWebApi(this WebApplication app, WebApplicationBuilder builder) {
      if (builder.Environment.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        SwaggerBuilderExtensions.UseSwagger(app);//   app.UseSwagger();
        app.UseSwaggerUI();
      }
      app.UseCors(options => options.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod());
      app.ConfigureCustomExceptionMiddleware();
      //app.ConfigureHealthCheck();
      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      app.ConfigureSwagger();
      app.UseHealthChecks("/healthz", new HealthCheckOptions {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        ResultStatusCodes = {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
      },
      }).UseHealthChecksUI(setup => {
        setup.ApiPath = "/healthcheck";
        setup.UIPath = "/healthcheck-ui";
        //setup.AddCustomStylesheet("Customization/custom.css");
      });
      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
      //app.MapControllers();
    }

  }
}
