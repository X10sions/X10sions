using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder;
public static class WebApplicationBuilderExtensions {

  public static WebApplicationBuilder AddScopedConfigurationOptionWithValidation<TIAppSettings, TAppSettings>(this WebApplicationBuilder builder)
    where TIAppSettings : class where TAppSettings : class, TIAppSettings {
    builder.AddScopedConfigurationOptionWithValidation<TAppSettings>();
    builder.Services.AddScoped<TIAppSettings, TAppSettings>(sp => sp.GetRequiredService<IOptionsMonitor<TAppSettings>>().CurrentValue);
    //builder.Services.AddScoped<TIAppSettings, TAppSettings>(sp => sp.GetRequiredService<IOptionsSnapshot<AppSettings>>().Value);
    return builder;
  }

  public static WebApplicationBuilder AddScopedConfigurationOptionWithValidation<TAppSettings>(this WebApplicationBuilder builder) where TAppSettings : class {
    builder.ConfigureWithValidation<TAppSettings>();
    //https://stackoverflow.com/questions/62761483/how-to-customise-configuration-binding-in-asp-net-core
    builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsMonitor<TAppSettings>>().CurrentValue);
    //builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsSnapshot<AppSettings>>().Value);
    return builder;
  }

  public static WebApplicationBuilder ConfigureAppSetting<T>(this WebApplicationBuilder builder, IConfiguration configuration, bool doValidate, ServiceLifetime? lifetime = null) where T : class {
    builder.Services.Configure<T>(configuration);
    if (doValidate) builder.Services.PostConfigureValidate<T>();
    switch (lifetime) {
      case ServiceLifetime.Singleton: builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptionsMonitor<T>>().CurrentValue); break;
      case ServiceLifetime.Scoped: builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsSnapshot<T>>().Value); break;
      case ServiceLifetime.Transient: builder.Services.AddScoped(sp => sp.GetRequiredService<IOptions<T>>().Value); break;
    }
    return builder;
  }

  public static WebApplicationBuilder ConfigureAppSetting<TService, TImplementation>(this WebApplicationBuilder builder, IConfiguration configuration, bool doValidate, ServiceLifetime? lifetime = null)
    where TService : class where TImplementation : class, TService {
    builder.ConfigureAppSetting<TImplementation>(configuration, doValidate, lifetime);
    switch (lifetime) {
      case ServiceLifetime.Singleton: builder.Services.AddSingleton<TService, TImplementation>(sp => sp.GetRequiredService<IOptionsMonitor<TImplementation>>().CurrentValue); break;
      case ServiceLifetime.Scoped: builder.Services.AddScoped<TService, TImplementation>(sp => sp.GetRequiredService<IOptionsSnapshot<TImplementation>>().Value); break;
      case ServiceLifetime.Transient: builder.Services.AddScoped<TService, TImplementation>(sp => sp.GetRequiredService<IOptions<TImplementation>>().Value); break;
    }
    return builder;
  }

  public static WebApplicationBuilder ConfigureAppSetting<T>(this WebApplicationBuilder builder, bool doValidate, ServiceLifetime? lifetime = null)
    where T : class => builder.ConfigureAppSetting<T>(builder.Configuration, doValidate, lifetime);

  public static WebApplicationBuilder ConfigureAppSetting<TService, TImplementation>(this WebApplicationBuilder builder, bool doValidate, ServiceLifetime? lifetime = null)
    where TService : class where TImplementation : class, TService
    => builder.ConfigureAppSetting<TService, TImplementation>(builder.Configuration, doValidate, lifetime);

  public static WebApplicationBuilder ConfigureAppSetting<T>(this WebApplicationBuilder builder, string sectionName, bool doValidate, ServiceLifetime? lifetime = null)
    where T : class => builder.ConfigureAppSetting<T>(builder.Configuration.GetSection(sectionName), doValidate, lifetime);

  public static WebApplicationBuilder ConfigureAppSetting<TService, TImplementation>(this WebApplicationBuilder builder, string sectionName, bool doValidate, ServiceLifetime? lifetime = null)
    where TService : class where TImplementation : class, TService
    => builder.ConfigureAppSetting<TService, TImplementation>(builder.Configuration.GetSection(sectionName), doValidate, lifetime);

  public static IServiceCollection ConfigureWithValidation<T>(this WebApplicationBuilder builder) where T : class => builder.Services
    .Configure<T>(builder.Configuration)
    .PostConfigure<T>(settings => {
      var configErrors = settings.GetValidationErrorMessages();
      if (configErrors.Any()) {
        var aggrErrors = string.Join(",", configErrors);
        var count = configErrors.Count();
        var configType = typeof(T).Name;
        throw new ApplicationException($"Found {count} configuration error(s) in {configType}: {aggrErrors}");
      }
    });



}