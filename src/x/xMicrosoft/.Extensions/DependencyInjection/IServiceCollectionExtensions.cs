using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection {
  public static class IServiceCollectionExtensions {

    public static IServiceCollection AddOptions<T>(this IServiceCollection services, ServiceLifetime lifetime) where T : class {
      return lifetime switch {
        ServiceLifetime.Singleton => services.AddSingletonOptions<T>(),
        ServiceLifetime.Scoped => services.AddScopedOptions<T>(),
        ServiceLifetime.Transient => services.AddTransientOptions<T>(),
        _ => services
      };
    }

    public static IServiceCollection AddOptions<TInterface, T>(this IServiceCollection services, ServiceLifetime lifetime) where TInterface : class where T : class, TInterface {
      return lifetime switch {
        ServiceLifetime.Singleton => services.AddSingletonOptions<TInterface, T>(),
        ServiceLifetime.Scoped => services.AddScopedOptions<TInterface, T>(),
        ServiceLifetime.Transient => services.AddTransientOptions<TInterface, T>(),
        _ => services
      };
    }

    public static IServiceCollection AddSingletonOptions<T>(this IServiceCollection services) where T : class => services.AddSingleton(sp => sp.GetRequiredService<IOptionsMonitor<T>>().CurrentValue);
    public static IServiceCollection AddSingletonOptions<TInterface, T>(this IServiceCollection services) where TInterface : class where T : class, TInterface => services.AddSingleton<TInterface, T>(sp => sp.GetRequiredService<IOptionsMonitor<T>>().CurrentValue);
    public static IServiceCollection AddScopedOptions<T>(this IServiceCollection services) where T : class => services.AddScoped(sp => sp.GetRequiredService<IOptionsSnapshot<T>>().Value);
    public static IServiceCollection AddScopedOptions<TInterface, T>(this IServiceCollection services) where TInterface : class where T : class, TInterface => services.AddScoped<TInterface, T>(sp => sp.GetRequiredService<IOptionsSnapshot<T>>().Value);
    public static IServiceCollection AddTransientOptions<T>(this IServiceCollection services) where T : class => services.AddTransient(sp => sp.GetRequiredService<IOptions<T>>().Value);
    public static IServiceCollection AddTransientOptions<TInterface, T>(this IServiceCollection services) where TInterface : class where T : class, TInterface => services.AddTransient<TInterface, T>(sp => sp.GetRequiredService<IOptions<T>>().Value);

    public static IServiceCollection AddScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory, ILogger logger) where TService : class {
      logger.LogInformation($"{nameof(AddScoped)}<{typeof(TService)}>");
      return services.AddScoped(implementationFactory);
    }

    public static IServiceCollection AddScoped<TService>(this IServiceCollection services, ILogger logger) where TService : class {
      logger.LogInformation($"{nameof(AddScoped)}<{typeof(TService)}>");
      return services.AddScoped<TService>();
    }

    public static void LogServices(this IServiceCollection services, ILogger logger) {
      // https://ardalis.com/how-to-list-all-services-available-to-an-asp-net-core-app/
      logger.LogDebug($"Total Services Registered: {services.Count}");
      foreach (var service in services) {
        logger.LogDebug(service.LogString());
        //logger.LogDebug($"Service: {service.ServiceType.FullName} \nLifetime: {service.Lifetime} \nInstance: {service.ImplementationType?.FullName}");
      }
    }

    public static string LogString(this ServiceDescriptor service) => $"Services.Add{service.Lifetime}<{service.ServiceType.FullName},{service.ImplementationType?.FullName}>()";

    public static IServiceCollection PostConfigureValidate<T>(this IServiceCollection services) where T : class => services.PostConfigure<T>(settings => {
      var configErrors = settings.GetValidationErrorMessages();
      if (configErrors.Any()) {
        var aggrErrors = string.Join(",", configErrors);
        var count = configErrors.Count();
        var configType = typeof(T).Name;
        throw new ApplicationException($"Found {count} configuration error(s) in {configType}: {aggrErrors}");
      }
    });

  }
}
