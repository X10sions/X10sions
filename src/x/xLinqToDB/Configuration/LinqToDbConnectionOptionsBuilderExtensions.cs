using LinqToDB.DataProvider;
using System.Configuration;
using System.Data.Common;
using System.Data;

namespace LinqToDB.Configuration {
  public static class LinqToDbConnectionOptionsBuilderExtensions {

    //public static LinqToDbConnectionOptionsBuilder UseConnectionString<TConnection>(LinqToDbConnectionOptionsBuilder options, string connectionString) {
    //  IDataProvider dataProvider ;
    //  return   options.UseConnectionString(dataProvider, connectionString);
    //}

    public static LinqToDbConnectionOptionsBuilder UseConnectionStringSettings(this LinqToDbConnectionOptionsBuilder options, IDataProvider dataProvider, ConnectionStringSettings connectionStringSettings) {
      //logger.Information($"{nameof(UseConnectionStringSettings)};Name:{connectionStringSettings.Name};Provider:{connectionStringSettings.ProviderName};CS:{connectionStringSettings.ConnectionString}");
      return options.UseConnectionString(dataProvider, connectionStringSettings.ConnectionString);
    }

    public static LinqToDbConnectionOptionsBuilder UseConnectionString<TConnection, TDataReader>(this LinqToDbConnectionOptionsBuilder options, string connectionString)
      where TConnection : DbConnection, new()
      where TDataReader : IDataReader {
      //logger.Information($"{nameof(UseConnectionStringSettings)}<{typeof(TConnection)},{typeof(TDataReader)};Name:{connectionStringSettings.Name};Provider:{connectionStringSettings.ProviderName};CS:{connectionStringSettings.ConnectionString}");
      IDataProvider dataProvider = GenericDataProviderList.GetInstance<TConnection, TDataReader>(connectionString);
      return options.UseConnectionString(dataProvider, connectionString);
    }

    public static LinqToDbConnectionOptionsBuilder UseConnectionStringSettings<TConnection, TDataReader>(this LinqToDbConnectionOptionsBuilder options, ConnectionStringSettings connectionStringSettings)
      where TConnection : DbConnection, new()
      where TDataReader : IDataReader {
      //logger.Information($"{nameof(UseConnectionStringSettings)}<{typeof(TConnection)},{typeof(TDataReader)};Name:{connectionStringSettings.Name};Provider:{connectionStringSettings.ProviderName};CS:{connectionStringSettings.ConnectionString}");
      return options.UseConnectionString<TConnection, TDataReader>(connectionStringSettings.ConnectionString);
    }

  }
}
