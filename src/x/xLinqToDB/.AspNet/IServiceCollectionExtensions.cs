using LinqToDB.AspNet.Logging;
using LinqToDB.Configuration;
using LinqToDB.DataProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace LinqToDB.AspNet;
public static class ServiceConfigurationExtensions {

  public static IServiceCollection AddLinqToDBContext<TContext, TConnection, TDataReader>(this IServiceCollection services, string connectionString, ILogger logger)
   where TContext : IDataContext
   where TConnection : DbConnection, new()
   where TDataReader : IDataReader {
    var genericDataProvider = GenericDataProvider<TConnection>.GetInstance<TDataReader>(connectionString);
    if (genericDataProvider != null) {
      services.AddLinqToDBContext<TContext, TConnection>(genericDataProvider, connectionString, logger);
    }
    return services;
  }

  public static IServiceCollection AddLinqToDBContext<TContext, TConnection, TDataReader>(this IServiceCollection services, string connectionString, Func<LinqToDBConnectionOptions<TContext>, TContext> newContext, ILogger logger)
    where TContext : class, IDataContext
    where TConnection : DbConnection, new()
    where TDataReader : IDataReader {
    logger.LogInformation($"{nameof(AddLinqToDBContext)}<{typeof(TConnection)},{typeof(TContext)}>CS:{{connectionString}}");
    return services.AddScoped(x => GenericDataProvider<TConnection>.GetDataContext<TContext, TDataReader>(connectionString, newContext));
  }

  public static IServiceCollection AddLinqToDBContextScoped<TContext, TConnection, TDataReader>(this IServiceCollection services, TConnection connection, Func<IDataProvider, string, TContext> newContext, ILogger logger)
    where TContext : class, IDataContext
    where TConnection : DbConnection, new()
    where TDataReader : IDataReader {
    logger.LogInformation($"{nameof(AddLinqToDBContext)}<{typeof(TConnection)},{typeof(TContext)}>CS:{{connectionString}}");
    return services.AddScoped(x => GenericDataProvider<TConnection>.GetDataContext<TContext, TDataReader>(connection, newContext));
  }

  public static IServiceCollection AddLinqToDBContextScoped<TContext, TConnection, TDataReader>(this IServiceCollection services, TConnection connection, Func<LinqToDBConnectionOptions<TContext>, TContext> newContext, ILogger logger)
    where TContext : class, IDataContext
    where TConnection : DbConnection, new()
    where TDataReader : IDataReader {
    logger.LogInformation($"{nameof(AddLinqToDBContext)}<{typeof(TConnection)},{typeof(TContext)}>CS:{{connectionString}}");
    return services.AddScoped(x => GenericDataProvider<TConnection>.GetDataContext<TContext, TDataReader>(connection, newContext));
  }

  public static IServiceCollection AddLinqToDBContext<TContext, TConnection>(this IServiceCollection services, IDataProvider dataProvider, string connectionString, ILogger logger)
    where TContext : IDataContext
    where TConnection : IDbConnection {
    logger.LogInformation($"{nameof(AddLinqToDBContext)}<{typeof(TConnection)},{typeof(TContext)}>CS:{{connectionString}}");
    services.AddLinqToDBContext<TContext>((provider, options) => {
      options.UseConnectionString(dataProvider, connectionString).UseDefaultLogging(provider);
    });
    return services;
  }

  public static IServiceCollection AddScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory, ILogger logger) where TService : class {
    logger.LogInformation($"{nameof(AddScoped)}<{typeof(TService)}>");
    return services.AddScoped(implementationFactory);
  }

  public static IServiceCollection AddScoped<TService>(this IServiceCollection services, ILogger logger) where TService : class {
    logger.LogInformation($"{nameof(AddScoped)}<{typeof(TService)}>");
    return services.AddScoped<TService>();
  }

}