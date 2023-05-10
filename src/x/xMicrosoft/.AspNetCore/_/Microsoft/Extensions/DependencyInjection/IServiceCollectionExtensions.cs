using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;
public static class IServiceCollectionExtensions {

  public static IServiceCollection PostConfigureValidate<T>(this IServiceCollection services) where T : class => services.PostConfigure<T>(settings => {
    var configErrors = settings.GetValidationAttributeErrors();
    if (configErrors.Any()) {
      var aggrErrors = string.Join(",", configErrors);
      var count = configErrors.Count();
      var configType = typeof(T).Name;
      throw new ApplicationException($"Found {count} configuration error(s) in {configType}: {aggrErrors}");
    }
  });

  //public static IServiceCollection AddScopedOptionsSnapshot<T>(this IServiceCollection services, T defaultT) where T : class => services.AddScoped(sp => sp.GetService<IOptionsSnapshot<T>>()?.Value ?? defaultT);
  //public static IServiceCollection AddScopedRequiredOptionsSnapshot<T>(this IServiceCollection services) where T : class => services.AddScoped(sp => sp.GetRequiredService<IOptionsSnapshot<T>>().Value);
  //public static IServiceCollection AddSingletonOptionsMonitor<T>(this IServiceCollection services, T defaultT) where T : class => services.AddSingleton(sp => sp.GetService<IOptionsMonitor<T>>()?.CurrentValue ?? defaultT);
  //public static IServiceCollection AddSingletonRequiredOptionsMonitor<T>(this IServiceCollection services) where T : class => services.AddSingleton(sp => sp.GetRequiredService<IOptionsMonitor<T>>().CurrentValue);

  public static IServiceCollection ConfigureOptionsAndInstance<T>(this IServiceCollection services, IConfigurationSection section) where T : class, IOptions<T>, new() {
    // https://gist.github.com/yetanotherchris/9ffe48f732b9842805564347c4b2e99d
    services.Configure<T>(section).AddScoped(provider => provider.GetService<IOptionsMonitor<T>>().CurrentValue);
    return services;
  }

}
