using CleanOnionExample.Data.Entities;
using CleanOnionExample.Files;
using CleanOnionExample.Services;
using Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using xCleanOnionExample.Services.Identity;

namespace System;
public interface IAppSettingsPaths {
  List<string> AppSettingsPaths { get; }
}
public static class _Extensions {

  public static WebApplicationBuilder ImportConfigurationJsonFiles(this WebApplicationBuilder builder, IEnumerable<string> remoteAppSettingsPaths, string appSettingsTargetFileName) {
    var remoteAppSettngsFileInfo = new FileInfo(Path.Combine(builder.Environment.ContentRootPath, appSettingsTargetFileName));
    if (!remoteAppSettngsFileInfo.Exists) {
      JObject jsonObject = JObject.Parse("{}");
      foreach (var path in remoteAppSettingsPaths) {
        var file = new FileInfo(path);
        using (var r = new StreamReader(file.FullName)) {
          jsonObject.Merge(JObject.Parse($@"{{""GenaretedFromPaths"":[""{file.FullName.Replace("\\", "\\\\")}""]}}"), new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
          jsonObject.Merge(JObject.Parse(r.ReadToEnd()), new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
        }
      }
      File.WriteAllText(remoteAppSettngsFileInfo.FullName, jsonObject.ToString());
    }
    builder.Configuration
    .AddJsonFile(remoteAppSettngsFileInfo.Name, true, true)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);
    return builder;
  }

  public static IServiceCollection AddInfrastructure_CleanArch(this IServiceCollection services, IConfiguration configuration) {
    if (configuration.GetValue<bool>("UseInMemoryDatabase")) {
      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseInMemoryDatabase("CleanArchitectureDb"));
    } else {
      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(
              configuration.GetConnectionString("DefaultConnection"),
              b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
    }

    services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
    services.AddScoped<IDomainEventService, DomainEventService>();
    services.AddDefaultIdentity<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
    services.AddIdentityServer().AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
    services.AddTransient<IDateTimeService, DateTimeService>();
    services.AddTransient<IIdentityService, IdentityService>();
    services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
    services.AddAuthentication().AddIdentityServerJwt();
    services.AddAuthorization(options => options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));
    return services;
  }

}


