//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection {
  public static class IServiceCollectionExtensions {

    //    public static IServiceCollection ConfigureSingletonOptions<TOptions>(this IServiceCollection services, IConfiguration configuration) where TOptions : class, new()
    //      => services.Configure<TOptions>(configuration).AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);

    //    public static IServiceCollection ConfigureSingletonOptionsSnapshot<TOptions>(this IServiceCollection services, IConfiguration configuration) where TOptions : class, new()
    //      => services.Configure<TOptions>(configuration).AddSingleton(x => x.GetRequiredService<IOptionsSnapshot<TOptions>>().Value);

    //    public static IServiceCollection ConfigureSingletonOptionsMonitor<TOptions>(this IServiceCollection services, IConfiguration configuration) where TOptions : class, new()
    //      => services.Configure<TOptions>(configuration).AddSingleton(x => x.GetRequiredService<IOptionsMonitor<TOptions>>().CurrentValue); e);

  }
}
