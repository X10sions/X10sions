using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Options {
  public static class IPostConfigureOptionsExtensions {
    public static void AddToServiceCollection(this IPostConfigureOptions<StaticFileOptions> options, IServiceCollection services) => services.ConfigureOptions(options);
    //public static void AddToServiceCollection<T>(this T options, IServiceCollection services) where T : IPostConfigureOptions<StaticFileOptions> => services.ConfigureOptions(typeof(T));
    //public static void AddToServiceCollection<T>(this T options, IServiceCollection services) where T : IPostConfigureOptions<StaticFileOptions> => services.ConfigureOptions<T>();
  }
}