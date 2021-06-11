using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common {
  public class StartUpOrdererdConfiguration {

    public StartUpOrdererdConfiguration(
      int order,
      Action<IApplicationBuilder> configureAction,
      Action<IServiceCollection> configureServicesAction,
      string description) {
      Order = order;
      ConfigureAction = configureAction;
      ConfigureServicesAction = configureServicesAction;
      Description = description;
    }

    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/index?view=aspnetcore-2.1#order

    /// <summary>
    /// Order in which the middleware components are invoked on requests and the reverse order for the response. 
    /// </summary>
    public int Order { get; }

    public string Description { get; }

    /// <summary>
    /// _configure Delegate
    /// </summary>
    public Action<IApplicationBuilder> ConfigureAction { get; }

    /// <summary>
    /// _configureServices Delegate
    /// </summary>
    public Action<IServiceCollection> ConfigureServicesAction { get; }

  }
}
