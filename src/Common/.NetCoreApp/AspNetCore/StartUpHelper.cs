using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.AspNetCore {

  public class StartUpHelper {

    public List<StartUpOrdererdConfiguration> OrdererdConfigurations { get; } = new List<StartUpOrdererdConfiguration>();

    //_configureServicesDelegates
    public IEnumerable<Action<IServiceCollection>> ConfigureServicesActions => from x in OrdererdConfigurations where x != null orderby  x.Order select x.ConfigureServicesAction;

    //_configureDelegates
    public IEnumerable<Action<IApplicationBuilder>> ConfigureActions => from x in OrdererdConfigurations where x != null orderby x.Order select x.ConfigureAction ;

    public StartUpHelper AddAction(int order, string description, Action<IApplicationBuilder> appAction, Action<IServiceCollection> servicesAction) {
      OrdererdConfigurations.Add(new StartUpOrdererdConfiguration(order, appAction, servicesAction,description));
      return this;
    }

    public IServiceCollection ConfigureServices(IServiceCollection services) {
      foreach (var x in ConfigureServicesActions) {
        x(services);
      }
      return services;
    }

    public IApplicationBuilder Configure(IApplicationBuilder app) {
      // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/index?view=aspnetcore-2.1#order

      // Order Matters (Common Example)
      // 1. Exception/error handling
      // 2. HTTP Strict Transport Security Protocol
      // 3. HTTPS redirection
      // 4. Static file server
      // 5. Cookie policy enforcement
      // 6. Authentication
      // 7. Session
      // 8. MVC       
      foreach (var x in ConfigureActions) {
        x(app);
      }
      return app;
    }


  }
}
