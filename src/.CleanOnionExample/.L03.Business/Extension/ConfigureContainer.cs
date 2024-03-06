using Microsoft.AspNetCore.Hosting;

namespace Microsoft.AspNetCore.Builder;
public static class ConfigureContainer {

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public static IApplicationBuilder Configure_AspNetCoreHeroBoilerplateApi(this IApplicationBuilder app, IWebHostEnvironment env) {
    if (env.IsDevelopment()) {
      app.UseDeveloperExceptionPage();
    }
    app.ConfigureSwagger();
    app.UseHttpsRedirection();
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseRouting();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints => {
      endpoints.MapControllers();
    });
    return app;
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public static void Configure_PereirenWebApi(this IApplicationBuilder app, IWebHostEnvironment env) {
    if (env.IsDevelopment()) {
      app.UseDeveloperExceptionPage();
    }
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthorization();
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseEndpoints(endpoints => {
      endpoints.MapControllers();
    });
    app.UseStaticFiles();
    SwaggerBuilderExtensions.UseSwagger(app);//   app.UseSwagger();
    app.UseSwaggerUI(c => {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Tasks API V1");
    });
  }

  public static void Configure_CleanArch(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration) {
    // https://github.com/jasontaylordev/CleanArchitecture/tree/main/src/WebUI
    if (env.IsDevelopment()) {
      app.UseDeveloperExceptionPage();
      app.UseMigrationsEndPoint();
    } else {
      app.UseExceptionHandler("/Error");
      // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
      app.UseHsts();
    }

    app.UseHealthChecks("/health");
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    if (!env.IsDevelopment()) {
      app.UseSpaStaticFiles();
    }
    app.UseSwaggerUi3(settings => {
      settings.Path = "/api";
      settings.DocumentPath = "/api/specification.json";
    });

    app.UseRouting();

    app.UseAuthentication();
    //TODO  app.UseIdentityServer();
    app.UseAuthorization();
    app.UseEndpoints(endpoints => {
      endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller}/{action=Index}/{id?}");
      endpoints.MapRazorPages();
    });
    app.UseSpa(spa => {
      // To learn more about options for serving an Angular SPA from ASP.NET Core,
      // see https://go.microsoft.com/fwlink/?linkid=864501

      spa.Options.SourcePath = "ClientApp";

      if (env.IsDevelopment()) {
        //spa.UseAngularCliServer(npmScript: "start");
        spa.UseProxyToSpaDevelopmentServer(configuration["SpaBaseUrl"] ?? "http://localhost:4200");
      }
    });
  }


  public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app) => app.UseMiddleware<ExceptionMiddleware>();

  public static void ConfigureSwagger(this IApplicationBuilder app) {
    SwaggerBuilderExtensions.UseSwagger(app);// app.UseSwagger();
    app.UseSwaggerUI(options => {
      options.SwaggerEndpoint("/swagger/OpenAPISpecification/swagger.json", "Onion Architecture API");
      options.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNetCoreHero.Boilerplate.Api");
      options.RoutePrefix = "OpenAPI";
      options.RoutePrefix = "swagger";
      options.DisplayRequestDuration();
    });
  }

  public static void ConfigureSwagger(this ILoggerFactory loggerFactory) => loggerFactory.AddSerilog();

  public static void UseHealthCheck(this IApplicationBuilder app) {
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
    });
  }
}

