using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace X10sions.Playground.Razor;
public class AppSettings {
  public string AppTitle { get; set; } = "-Unknown-";
  public string AppBaseAddress { get; set; } = "htttps://unknown/";
  public string AppContentPath { get; set; } = "_content/unknown/";

  public DateTime Updated { get; set; } = DateTime.Now;

}

public static class AppSettingsExtensions {
  //public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration) {
  //  services.Configure<AppSettings>(configuration);
  //  services.AddScoped(sp => sp.GetRequiredService<IOptionsMonitor<AppSettings>>().CurrentValue);
  //  return services;
  //}

  public static IServiceCollection AddAppSettings(this IServiceCollection services) {
    services.Configure<AppSettings>(options => { });
    services.AddScoped(sp => sp.GetRequiredService<IOptionsMonitor<AppSettings>>().CurrentValue);
    return services;
  }

}