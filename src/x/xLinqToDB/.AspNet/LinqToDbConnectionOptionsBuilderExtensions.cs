using LinqToDB.DataProvider;
using System.Configuration;
using System.Data.Common;
using System.Data;
using Microsoft.Extensions.Logging;

namespace LinqToDB.Configuration {
  public static class LinqToDbConnectionOptionsBuilderExtensions {

    //public static LinqToDbConnectionOptionsBuilder UseConnectionString<TConnection>(LinqToDbConnectionOptionsBuilder options, string connectionString) {
    //  IDataProvider dataProvider ;
    //  return   options.UseConnectionString(dataProvider, connectionString);
    //}

    public static LinqToDbConnectionOptionsBuilder UseConnectionStringSettings(this LinqToDbConnectionOptionsBuilder options, IDataProvider dataProvider, ConnectionStringSettings connectionStringSettings, ILogger logger) {
      logger.LogInformation($"{nameof(UseConnectionStringSettings)};Name:{connectionStringSettings.Name};Provider:{connectionStringSettings.ProviderName},{dataProvider.GetType()};CS:{connectionStringSettings.ConnectionString}");
      return options.UseConnectionString(dataProvider, connectionStringSettings.ConnectionString);
    }

    public static LinqToDbConnectionOptionsBuilder UseConnectionString<TConnection, TDataReader>(this LinqToDbConnectionOptionsBuilder options, string connectionString, ILogger logger)
      where TConnection : DbConnection, new()
      where TDataReader : IDataReader {
      IDataProvider dataProvider = GenericDataProviderList.GetInstance<TConnection, TDataReader>(connectionString);
      logger.LogInformation($"{nameof(UseConnectionStringSettings)}<{typeof(TConnection)},{typeof(TDataReader)},{dataProvider.GetType()};CS:{connectionString}");
      return options.UseConnectionString(dataProvider, connectionString);
    }

    public static LinqToDbConnectionOptionsBuilder UseConnectionStringSettings<TConnection, TDataReader>(this LinqToDbConnectionOptionsBuilder options, ConnectionStringSettings connectionStringSettings, ILogger logger)
      where TConnection : DbConnection, new()
      where TDataReader : IDataReader {
      return options.UseConnectionString<TConnection, TDataReader>(connectionStringSettings.ConnectionString, logger);
    }

  }
}
