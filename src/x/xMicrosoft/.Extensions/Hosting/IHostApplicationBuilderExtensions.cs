using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;
public static class IHostApplicationBuilderExtensions {

  public static T ConfigureAppSettings<T>(this IHostApplicationBuilder builder, ServiceLifetime? lifetime, bool doValidate) where T : class, new()
    => builder.ConfigureAppSettings<T>(builder.Configuration, lifetime, doValidate);

  public static T ConfigureAppSettings<T>(this IHostApplicationBuilder builder, string configurationSectionName, ServiceLifetime? lifetime, bool doValidate) where T : class, new()
    => builder.ConfigureAppSettings<T>(builder.Configuration.GetSection(configurationSectionName, doValidate), lifetime, doValidate);

  static T ConfigureAppSettings<T>(this IHostApplicationBuilder builder, IConfiguration configuration, ServiceLifetime? lifetime, bool doValidate) where T : class, new() {
    builder.Services.Configure<T>(configuration);
    if (doValidate) builder.Services.PostConfigureValidate<T>();
    if (lifetime is not null) builder.Services.AddOptions<T>(lifetime.Value);
    return builder.Configuration.Get<T>();
  }

  public static T ConfigureAppSettings<TInterface, T>(this IHostApplicationBuilder builder, ServiceLifetime? lifetime, bool doValidate) where TInterface : class where T : class, TInterface
    => builder.ConfigureAppSettings<TInterface, T>(builder.Configuration, lifetime, doValidate);

  public static T ConfigureAppSettings<TInterface, T>(this IHostApplicationBuilder builder, string configurationSectionName, ServiceLifetime? lifetime, bool doValidate) where TInterface : class where T : class, TInterface
    => builder.ConfigureAppSettings<TInterface, T>(builder.Configuration.GetSection(configurationSectionName, doValidate), lifetime, doValidate);

  static T ConfigureAppSettings<TInterface, T>(this IHostApplicationBuilder builder, IConfiguration configuration, ServiceLifetime? lifetime, bool doValidate)
   where TInterface : class where T : class, TInterface {
    builder.Services.Configure<T>(configuration);
    if (doValidate) builder.Services.PostConfigureValidate<T>();
    if (lifetime is not null) builder.Services.AddOptions<TInterface, T>(lifetime.Value);
    return builder.Configuration.Get<T>();
  }

  //static T ConfigureAppSettings<TInterface, T>(this IServiceCollection services, IConfiguration configuration, ServiceLifetime? lifetime, bool doValidate)
  // where TInterface : class where T : class, TInterface {
  //  services.Configure<T>(configuration);
  //  if (doValidate) services.PostConfigureValidate<T>();
  //  if (lifetime is not null) services.AddOptions<TInterface, T>(lifetime.Value);
  //  return configuration.Get<T>();
  //}

}