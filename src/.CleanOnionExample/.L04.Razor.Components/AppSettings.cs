using CleanOnionExample.Data;
using Common.App.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CleanOnionExample.Razor.Components;
public interface IAppSettings : IDataAppSettings {
  string AppBaseAddress { get; }
  string AppContentPath { get; }
  ApplicationDetailAppSettings AppDetail { get; }
}

public class AppSettings : DataAppSettings, IAppSettings {
  public const string ComponentsRazorClassLibraryName = "CleanOnionExample.Razor.Components";
  public const string PagesRazorClassLibraryName = "CleanOnionExample.Razor.Pages";
  public const string ComponentsContentPath = "_content/" + ComponentsRazorClassLibraryName;
  public const string PagesContentPath = "_content/" + PagesRazorClassLibraryName;

  public string AppBaseAddress { get; set; } = "htttps://unknown/";
  public string AppContentPath { get; set; } = "_content/unknown/";
  public ApplicationDetailAppSettings AppDetail { get; set; }

  public static AppSettings Configure(WebApplicationBuilder builder) {
    builder.Services.Configure<AppSettings>(builder.Configuration);
    builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsMonitor<AppSettings>>().CurrentValue);
    return builder.Configuration.Get<AppSettings>() ?? throw new ArgumentNullException();
  }

}

public static class AppSettingsExtensions {
}