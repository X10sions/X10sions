//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;

using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection {
  public static class IServiceCollectionExtensions {

    //    public static IServiceCollection ConfigureSingletonOptions<TOptions>(this IServiceCollection services, IConfiguration configuration) where TOptions : class, new()
    //      => services.Configure<TOptions>(configuration).AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);

    //    public static IServiceCollection ConfigureSingletonOptionsSnapshot<TOptions>(this IServiceCollection services, IConfiguration configuration) where TOptions : class, new()
    //      => services.Configure<TOptions>(configuration).AddSingleton(x => x.GetRequiredService<IOptionsSnapshot<TOptions>>().Value);

    //    public static IServiceCollection ConfigureSingletonOptionsMonitor<TOptions>(this IServiceCollection services, IConfiguration configuration) where TOptions : class, new()
    //      => services.Configure<TOptions>(configuration).AddSingleton(x => x.GetRequiredService<IOptionsMonitor<TOptions>>().CurrentValue); e);

    public static IServiceCollection AddScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory, ILogger logger) where TService : class {
      logger.LogInformation($"{nameof(AddScoped)}<{typeof(TService)}>");
      return services.AddScoped(implementationFactory);
    }

    public static IServiceCollection AddScoped<TService>(this IServiceCollection services, ILogger logger) where TService : class {
      logger.LogInformation($"{nameof(AddScoped)}<{typeof(TService)}>");
      return services.AddScoped<TService>();
    }

    public static void LogServices<TService>(this IServiceCollection services, ILogger logger) where TService : class {
      // https://ardalis.com/how-to-list-all-services-available-to-an-asp-net-core-app/
      logger.LogDebug($"Total Services Registered: {services.Count}");
      foreach (var service in services) {
        logger.LogDebug($"Service: {service.ServiceType.FullName} \nLifetime: {service.Lifetime} \nInstance: {service.ImplementationType?.FullName}");
      }
    }


  }
}
