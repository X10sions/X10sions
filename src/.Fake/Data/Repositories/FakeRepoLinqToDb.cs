using FirebirdSql.Data.FirebirdClient;
using LinqToDB;
using LinqToDB.AspNet.Logging;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.Firebird;
using LinqToDB.DataProvider.MySql;
using LinqToDB.DataProvider.Oracle;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.DataProvider.SqlServer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using MySqlConnector;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using System.Data.SQLite;
using X10sions.Fake.Data.Enums;

namespace X10sions.Fake.Data.Repositories {
  public class FakeRepoLinqToDb : FakeRepo {
    public FakeRepoLinqToDb(ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) : base(name, configuration) {
      DataConnection = new LinqToDbDataConnections.BaseDataConnection(name.GetLinqToDBConnectionOptions(configuration, loggerFactory));
    }

    public DataConnection DataConnection { get; }
    public override void CreateTable<T>() => DataConnection.CreateTable<T>();
    public override void DropTable<T>() => DataConnection.DropTable<T>();

  }

  public static class LinqToDBExtensions {

    public static IDataProvider GetLinqToDbDataProvider(this DbProviderFactory factory) => factory switch {
      //OdbcFactory => acc
      FirebirdClientFactory => FirebirdTools.GetDataProvider(),
      MySqlClientFactory => MySqlTools.GetDataProvider(ProviderName.MySql),
      MySqlConnectorFactory => MySqlTools.GetDataProvider(ProviderName.MySqlConnector),
      NpgsqlFactory => PostgreSQLTools.GetDataProvider(),
      OracleClientFactory => OracleTools.GetDataProvider(OracleVersion.v12, OracleProvider.Managed),
      SqliteFactory => SQLiteTools.GetDataProvider(ProviderName.SQLiteMS),
      SQLiteFactory => SQLiteTools.GetDataProvider(ProviderName.SQLiteClassic),
      Microsoft.Data.SqlClient.SqlClientFactory => SqlServerTools.GetDataProvider(SqlServerVersion.v2022, SqlServerProvider.MicrosoftDataSqlClient),
      System.Data.SqlClient.SqlClientFactory => SqlServerTools.GetDataProvider(SqlServerVersion.v2022, SqlServerProvider.SystemDataSqlClient),
      _ => throw new NotImplementedException(factory.ToString())
    };

    public static IDataProvider GetLinqToDbDataProvider(this ConnectionStringName name) => name.GetDbProviderFactory().GetLinqToDbDataProvider();

    public static LinqToDBConnectionOptions<T> GetLinqToDBConnectionOptions<T>(this ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) => name.GetLinqToDBConnectionOptionsBuilder(configuration, loggerFactory).Build<T>();
    public static LinqToDBConnectionOptions GetLinqToDBConnectionOptions(this ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) => name.GetLinqToDBConnectionOptionsBuilder(configuration, loggerFactory).Build();

    static LinqToDBConnectionOptionsBuilder GetLinqToDBConnectionOptionsBuilder(this ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) {
      var connectionString = name.GetConnectionString(configuration);
      var factory = name.GetDbProviderFactory();
      var dataProvider = factory.GetLinqToDbDataProvider();
      var builder = new LinqToDBConnectionOptionsBuilder();
      builder.UseLoggerFactory(loggerFactory);
      builder.UseConnectionString(dataProvider, connectionString);
      return builder;
    }

    public static IServiceCollection AddLinqToDb_Fake(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory) {

      services.AddTransient<LinqToDbDataConnections._Resolver>(serviceProvider => name => {
        switch (name) {
          case ConnectionStringName.Access_Odbc: return serviceProvider.GetRequiredService<LinqToDbDataConnections.Access_Odbc>();
          case ConnectionStringName.Access_OleDb: return serviceProvider.GetRequiredService<LinqToDbDataConnections.Access_OleDb>();
          case ConnectionStringName.DB2_IBM: return serviceProvider.GetRequiredService<LinqToDbDataConnections.DB2_IBM>();
          case ConnectionStringName.DB2_Odbc: return serviceProvider.GetRequiredService<LinqToDbDataConnections.DB2_Odbc>();
          case ConnectionStringName.DB2iSeries_IBM: return serviceProvider.GetRequiredService<LinqToDbDataConnections.DB2iSeries_IBM>();
          case ConnectionStringName.DB2iSeries_Odbc: return serviceProvider.GetRequiredService<LinqToDbDataConnections.DB2iSeries_Odbc>();
          case ConnectionStringName.DB2iSeries_OleDb: return serviceProvider.GetRequiredService<LinqToDbDataConnections.DB2iSeries_OleDb>();
          case ConnectionStringName.Firebird: return serviceProvider.GetRequiredService<LinqToDbDataConnections.Firebird>();
          case ConnectionStringName.MariaDb: return serviceProvider.GetRequiredService<LinqToDbDataConnections.MariaDb>();
          case ConnectionStringName.MySql_Client: return serviceProvider.GetRequiredService<LinqToDbDataConnections.MySql_Client>();
          case ConnectionStringName.MySql_Connector: return serviceProvider.GetRequiredService<LinqToDbDataConnections.MySql_Connector>();
          case ConnectionStringName.Oracle: return serviceProvider.GetRequiredService<LinqToDbDataConnections.Oracle>();
          case ConnectionStringName.PostgreSql: return serviceProvider.GetRequiredService<LinqToDbDataConnections.PostgreSql>();
          case ConnectionStringName.Sqlite_Microsoft: return serviceProvider.GetRequiredService<LinqToDbDataConnections.Sqlite_Microsoft>();
          case ConnectionStringName.Sqlite_System: return serviceProvider.GetRequiredService<LinqToDbDataConnections.Sqlite_System>();
          case ConnectionStringName.SqlServer_Microsoft: return serviceProvider.GetRequiredService<LinqToDbDataConnections.SqlServer_Microsoft>();
          case ConnectionStringName.SqlServer_System: return serviceProvider.GetRequiredService<LinqToDbDataConnections.SqlServer_System>();
          default: throw new NotSupportedException($"RepositoryResolver, key: {name}");
        }
      });

      services.AddScoped(x => new LinqToDbDataConnections.Access_Odbc(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.Access_OleDb(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.DB2_IBM(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.DB2_Odbc(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.DB2iSeries_IBM(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.DB2iSeries_Odbc(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.DB2iSeries_OleDb(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.Firebird(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.MariaDb(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.MySql_Client(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.MySql_Connector(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.Oracle(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.PostgreSql(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.Sqlite_Microsoft(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.Sqlite_System(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.SqlServer_Microsoft(configuration, loggerFactory));
      services.AddScoped(x => new LinqToDbDataConnections.SqlServer_System(configuration, loggerFactory));

      return services;
    }

  }

}
