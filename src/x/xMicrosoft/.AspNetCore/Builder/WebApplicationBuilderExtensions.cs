using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder;
public static class WebApplicationBuilderExtensions {

  public static T? ConfigureAppSettings<T>(this WebApplicationBuilder builder, ServiceLifetime? lifetime, bool doValidate) where T : class, new()
    => builder.ConfigureAppSettings<T>(builder.Configuration, lifetime, doValidate);

  public static T? ConfigureAppSettings<T>(this WebApplicationBuilder builder, string configurationSectionName, ServiceLifetime? lifetime, bool doValidate) where T : class, new()
    => builder.ConfigureAppSettings<T>(builder.Configuration.GetSection(configurationSectionName, doValidate), lifetime, doValidate);

  //static T? ConfigureAppSettingsBind<T>(this WebApplicationBuilder builder, IConfiguration configuration, ServiceLifetime? lifetime, bool doValidate) where T : class, new() {
  //  if (doValidate) builder.Services.PostConfigureValidate<T>();
  //  var t = new T();
  //  configuration.Bind(t);
  //  if (lifetime is not null) builder.Services.AddOptions<T>(lifetime.Value);
  //  return t;
  //}

  static T? ConfigureAppSettings<T>(this WebApplicationBuilder builder, IConfiguration configuration, ServiceLifetime? lifetime, bool doValidate) where T : class, new() {
    builder.Services.Configure<T>(configuration);
    if (doValidate) builder.Services.PostConfigureValidate<T>();
    if (lifetime is not null) builder.Services.AddOptions<T>(lifetime.Value);
    return builder.Configuration.Get<T>();
  }

  public static T? ConfigureAppSettings<TInterface, T>(this WebApplicationBuilder builder, ServiceLifetime? lifetime, bool doValidate) where TInterface : class where T : class, TInterface
    => builder.ConfigureAppSettings<TInterface, T>(builder.Configuration, lifetime, doValidate);

  public static T? ConfigureAppSettings<TInterface, T>(this WebApplicationBuilder builder, string configurationSectionName, ServiceLifetime? lifetime, bool doValidate) where TInterface : class where T : class, TInterface
    => builder.ConfigureAppSettings<TInterface, T>(builder.Configuration.GetSection(configurationSectionName, doValidate), lifetime, doValidate);

  static T? ConfigureAppSettings<TInterface, T>(this WebApplicationBuilder builder, IConfiguration configuration, ServiceLifetime? lifetime, bool doValidate)
   where TInterface : class where T : class, TInterface {
    builder.Services.Configure<T>(configuration);
    if (doValidate) builder.Services.PostConfigureValidate<T>();
    if (lifetime is not null) builder.Services.AddOptions<TInterface, T>(lifetime.Value);
    return builder.Configuration.Get<T>();
  }

}