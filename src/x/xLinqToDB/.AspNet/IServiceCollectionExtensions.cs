using LinqToDB.DataProvider;
using LinqToDB.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Data;
using LinqToDB.AspNet.Logging;
using Microsoft.Extensions.Logging;

namespace LinqToDB.AspNet {
  public static class ServiceConfigurationExtensions {

    //public static IServiceCollection AddLinqToDbContext<TContext>(this IServiceCollection serviceCollection, IDataProvider dataProvider, ConnectionStringSettings connectionStringSettings) where TContext : IDataContext {
    //  return serviceCollection.AddLinqToDbContext<TContext>((provider, options) => options.UseConnectionStringSettings(dataProvider, connectionStringSettings).UseDefaultLogging(provider));
    //}

    public static IServiceCollection AddLinqToDbContext<TContext, TConnection, TDataReader>(this IServiceCollection services, string connectionString, ILogger logger)
      where TContext : IDataContext
      where TConnection : DbConnection, new()
      where TDataReader : IDataReader {
      IDataProvider dataProvider = GenericDataProviderList.GetInstance<TConnection, TDataReader>(connectionString);
      return services.AddLinqToDbContext<TContext, TConnection>(dataProvider, connectionString, logger);

      //logger.LogInformation($"{nameof(AddLinqToDbContext)}<{typeof(TConnection)},{typeof(TDataReader)},{typeof(TContext)}>;CS:{{connectionString}}");
      //services.AddLinqToDbContext<TContext>((provider, options) => {
      //  options.UseConnectionString<TConnection, TDataReader>(connectionString, logger).UseDefaultLogging(provider);
      //});
      //return services;
    }

    public static IServiceCollection AddLinqToDbContext<TContext, TConnection>(this IServiceCollection services, IDataProvider dataProvider, string connectionString, ILogger logger)
      where TContext : IDataContext
      where TConnection : IDbConnection {
      logger.LogInformation($"{nameof(AddLinqToDbContext)}<{typeof(TConnection)},{typeof(TContext)}>CS:{{connectionString}}");
      services.AddLinqToDbContext<TContext>((provider, options) => {
        options.UseConnectionString(dataProvider, connectionString).UseDefaultLogging(provider);
      });
      return services;
    }

  }
}
