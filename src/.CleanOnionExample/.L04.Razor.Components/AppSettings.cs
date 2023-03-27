using CleanOnionExample.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CleanOnionExample.Razor.Components;
public interface IAppSettings : Data.IAppSettings {
  string AppBaseAddress { get; }
  string AppContentPath { get; }
  ApplicationDetailAppSettings AppDetail { get; }
}

public class AppSettings : Data.AppSettings, IAppSettings {
  public const string ComponentsRazorClassLibraryName = "CleanOnionExample.Razor.Components";
  public const string PagesRazorClassLibraryName = "CleanOnionExample.Razor.Pages";
  public const string ComponentsContentPath = "_content/" + ComponentsRazorClassLibraryName;
  public const string PagesContentPath = "_content/" + PagesRazorClassLibraryName;

  public string AppBaseAddress { get; set; } = "htttps://unknown/";
  public string AppContentPath { get; set; } = "_content/unknown/";
  public ApplicationDetailAppSettings AppDetail { get; set; }

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