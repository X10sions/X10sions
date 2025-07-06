#if !NET
namespace System;
  [PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.DependencyInjection.Abstractions/src/ServiceProviderServiceExtensions.cs")]
  public static class IServiceProviderExtensions {
    public static T? GetService<T>(this IServiceProvider provider) {
      ThrowHelper.ThrowIfNull(provider);
      return (T?)provider.GetService(typeof(T));
    }
  }

#endif