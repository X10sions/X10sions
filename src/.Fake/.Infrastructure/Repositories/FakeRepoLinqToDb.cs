using Common.Data;
using LinqToDB;
using LinqToDB.AspNet.Logging;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.DB2;
using LinqToDB.DataProvider.DB2iSeries;
using LinqToDB.DataProvider.Oracle;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.DataProvider.SqlServer;
using LinqToDB.DataProvider.X10sions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using SQLite;
using System.Data.Odbc;
using System.Data.OleDb;

namespace X10sions.Fake.Data.Repositories;
public class FakeRepoLinqToDb : FakeRepo {
  public FakeRepoLinqToDb(ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) : base(name, configuration) {
    DataConnection = new LinqToDbDataConnections.BaseDataConnection(name.GetLinqToDBConnectionOptions(configuration, loggerFactory));
  }

  public DataConnection DataConnection { get; }
  public override void CreateTable<T>() => DataConnection.CreateTable<T>();
  public override void DropTable<T>() => DataConnection.DropTable<T>();
  public override long Delete<T>(IEnumerable<T> rows) {
    var count = 0;
    foreach (var o in rows) {
      count += DataConnection.Delete(o);
    }
    return count;
  }
  public override IQueryable<T> GetQueryable<T>() => DataConnection.GetTable<T>();
  public override long Insert<T>(IEnumerable<T> rows) {
    var count = 0;
    foreach (var o in rows) {
      count += DataConnection.Insert(o);
    }
    return count;
  }
  public override T InsertWithIdentity<T>(T row) => DataConnection.GetTable<T>().InsertWithOutput(row);
  public override long Update<T>(IEnumerable<T> rows) {
    var count = 0;
    foreach (var o in rows) {
      count += DataConnection.Update(o);
    }
    return count;
  }

}

public static class LinqToDBExtensions {

  //public static IDataProvider GetLinqToDbDataProvider(this DbProviderFactory factory) => factory switch {
  //  //OdbcFactory =>   AccessTools.GetDataProvider(),
  //  //OleDbFactory => AccessTools.GetDataProvider(),
  //  FirebirdClientFactory => FirebirdTools.GetDataProvider(),
  //  MySqlClientFactory => MySqlTools.GetDataProvider(ProviderName.MySql),
  //  MySqlConnectorFactory => MySqlTools.GetDataProvider(ProviderName.MySqlConnector),
  //  NpgsqlFactory => PostgreSQLTools.GetDataProvider(),
  //  OracleClientFactory => OracleTools.GetDataProvider(OracleVersion.v12, OracleProvider.Managed),
  //  SqliteFactory => SQLiteTools.GetDataProvider(ProviderName.SQLiteMS),
  //  SQLiteFactory => SQLiteTools.GetDataProvider(ProviderName.SQLiteClassic),
  //  Microsoft.Data.SqlClient.SqlClientFactory => SqlServerTools.GetDataProvider(SqlServerVersion.v2022, SqlServerProvider.MicrosoftDataSqlClient),
  //  System.Data.SqlClient.SqlClientFactory => SqlServerTools.GetDataProvider(SqlServerVersion.v2022, SqlServerProvider.SystemDataSqlClient),
  //  _ => throw new NotImplementedException(factory.ToString())
  //};

  //public static IDataProvider GetLinqToDbDataProvider(this ConnectionStringName name) => name.GetDbProviderFactory().GetLinqToDbDataProvider();

  //public static DataOptions<T> GetLinqToDBConnectionOptions<T>(this ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) where T : IDataContext
  //  => new DataOptions<T>(name.GetLinqToDBConnectionOptionsBuilder(configuration, loggerFactory));

  public static DataOptions GetLinqToDBConnectionOptions<T>(this ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) where T : IDataContext
    => name.GetLinqToDBConnectionOptionsBuilder(configuration, loggerFactory);

  public static DataOptions GetLinqToDBConnectionOptions(this ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory)
    => name.GetLinqToDBConnectionOptionsBuilder(configuration, loggerFactory);

  static readonly IDataProvider xDB2DataProviderzOdbc = new DB2WrappedDataProvider<OdbcConnection, OdbcDataReader>(801);
  static readonly IDataProvider xDB2DataProviderzOleDb = new DB2WrappedDataProvider<OleDbConnection, OleDbDataReader>(802);

  static readonly IDataProvider xDB2ISeriesDataProviderzOdbc = new DB2iSeriesWrappedDataProvider<OdbcConnection, OdbcDataReader>(901, DB2iSeriesProviderType.Odbc);
  static readonly IDataProvider xDB2ISeriesDataProviderzOleDb = new DB2iSeriesWrappedDataProvider<OleDbConnection, OleDbDataReader>(902, DB2iSeriesProviderType.OleDb);

  static DataOptions GetLinqToDBConnectionOptionsBuilder(this ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) {
    var connectionString = name.GetConnectionString(configuration);
    var builder = new DataOptions();
    builder.UseLoggerFactory(loggerFactory);
    switch (name) {
      case ConnectionStringName.Access_Odbc: builder.UseAccessOdbc(connectionString); break;
      case ConnectionStringName.Access_OleDb: builder.UseAccessOleDb(connectionString); break;
      case ConnectionStringName.DB2_IBM: builder.UseDB2(connectionString, DB2Version.LUW); break;
      case ConnectionStringName.DB2_Odbc: builder.UseDB2(connectionString, DB2Version.LUW); break;
      case ConnectionStringName.DB2_OleDb: builder.UseDB2(connectionString, DB2Version.LUW); break;

      //case ConnectionStringName.DB2iSeries_IBM: builder.UseDB2iSeries(connectionString, x => { x.WithProviderType(DB2iSeriesProviderType.DB2); }); break;
      //case ConnectionStringName.DB2iSeries_Odbc: builder.UseDB2iSeries(connectionString, x => { x.WithProviderType(DB2iSeriesProviderType.Odbc); }); break;
      //case ConnectionStringName.DB2iSeries_OleDb: builder.UseDB2iSeries(connectionString, x => { x.WithProviderType(DB2iSeriesProviderType.OleDb); }); break;

      //case ConnectionStringName.DB2iSeries_Odbc: builder.UseConnectionString(xDB2DataProviderzOdbc, connectionString); break;
      //case ConnectionStringName.DB2iSeries_OleDb: builder.UseConnectionString(xDB2DataProviderzOleDb, connectionString); break;

      case ConnectionStringName.DB2iSeries_Odbc: builder.UseConnectionString(xDB2ISeriesDataProviderzOdbc, connectionString); break;
      case ConnectionStringName.DB2iSeries_OleDb: builder.UseConnectionString(xDB2ISeriesDataProviderzOleDb, connectionString); break;

      case ConnectionStringName.Firebird: builder.UseFirebird(connectionString); break;
      case ConnectionStringName.MariaDb: builder.UseMySqlConnector(connectionString); break;
      case ConnectionStringName.MySql_Client: builder.UseMySql(connectionString); break;
      case ConnectionStringName.MySql_Connector: builder.UseMySqlConnector(connectionString); break;
      case ConnectionStringName.PostgreSql: builder.UsePostgreSQL(connectionString); break;
      case ConnectionStringName.Oracle: builder.UseOracle(connectionString, OracleVersion.v12, OracleProvider.Managed); break;
      case ConnectionStringName.Sqlite_Microsoft: builder.UseSQLiteMicrosoft(connectionString); break;
      case ConnectionStringName.Sqlite_System: builder.UseSQLiteOfficial(connectionString); break;
      //case ConnectionStringName.Sqlite_System: builder.UseConnection(SQLiteTools.GetDataProvider(ProviderName.SQLiteClassic), new System.Data.SQLite.SQLiteConnection(connectionString, false),true); break;
      case ConnectionStringName.SqlServer_Microsoft: builder.UseSqlServer(connectionString, SqlServerVersion.v2012, SqlServerProvider.MicrosoftDataSqlClient); break;
      case ConnectionStringName.SqlServer_System: builder.UseSqlServer(connectionString, SqlServerVersion.v2012, SqlServerProvider.SystemDataSqlClient); break;
      case ConnectionStringName.SqlServer_Odbc: builder.UseSqlServer(connectionString); break;
      case ConnectionStringName.SqlServer_OleDb: builder.UseSqlServer(connectionString); break;
      default: throw new NotImplementedException(name.ToString());
    }
    //var factory = name.GetDbProviderFactory();
    //var dataProvider = factory.GetLinqToDbDataProvider();
    //builder.UseConnectionString(dataProvider, connectionString);
    return builder;
  }

  public static IServiceCollection AddLinqToDb_Fake(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory) {

    services.AddTransient<LinqToDbDataConnections._Resolver>(serviceProvider => name => {
      return name switch {
        ConnectionStringName.Access_Odbc => serviceProvider.GetRequiredService<LinqToDbDataConnections.Access_Odbc>(),
        ConnectionStringName.Access_OleDb => serviceProvider.GetRequiredService<LinqToDbDataConnections.Access_OleDb>(),
        ConnectionStringName.DB2_IBM => serviceProvider.GetRequiredService<LinqToDbDataConnections.DB2_IBM>(),
        ConnectionStringName.DB2_Odbc => serviceProvider.GetRequiredService<LinqToDbDataConnections.DB2_Odbc>(),
        ConnectionStringName.DB2_OleDb => serviceProvider.GetRequiredService<LinqToDbDataConnections.DB2_OleDb>(),
        ConnectionStringName.DB2iSeries_IBM => serviceProvider.GetRequiredService<LinqToDbDataConnections.DB2iSeries_IBM>(),
        ConnectionStringName.DB2iSeries_Odbc => serviceProvider.GetRequiredService<LinqToDbDataConnections.DB2iSeries_Odbc>(),
        ConnectionStringName.DB2iSeries_OleDb => serviceProvider.GetRequiredService<LinqToDbDataConnections.DB2iSeries_OleDb>(),
        ConnectionStringName.Firebird => serviceProvider.GetRequiredService<LinqToDbDataConnections.Firebird>(),
        ConnectionStringName.MariaDb => serviceProvider.GetRequiredService<LinqToDbDataConnections.MariaDb>(),
        ConnectionStringName.MySql_Client => serviceProvider.GetRequiredService<LinqToDbDataConnections.MySql_Client>(),
        ConnectionStringName.MySql_Connector => serviceProvider.GetRequiredService<LinqToDbDataConnections.MySql_Connector>(),
        ConnectionStringName.Oracle => serviceProvider.GetRequiredService<LinqToDbDataConnections.Oracle>(),
        ConnectionStringName.PostgreSql => serviceProvider.GetRequiredService<LinqToDbDataConnections.PostgreSql>(),
        ConnectionStringName.Sqlite_Microsoft => serviceProvider.GetRequiredService<LinqToDbDataConnections.Sqlite_Microsoft>(),
        ConnectionStringName.Sqlite_System => serviceProvider.GetRequiredService<LinqToDbDataConnections.Sqlite_System>(),
        ConnectionStringName.SqlServer_Microsoft => serviceProvider.GetRequiredService<LinqToDbDataConnections.SqlServer_Microsoft>(),
        ConnectionStringName.SqlServer_System => serviceProvider.GetRequiredService<LinqToDbDataConnections.SqlServer_System>(),
        ConnectionStringName.SqlServer_Odbc => serviceProvider.GetRequiredService<LinqToDbDataConnections.SqlServer_Odbc>(),
        ConnectionStringName.SqlServer_OleDb => serviceProvider.GetRequiredService<LinqToDbDataConnections.SqlServer_OleDb>(),
        _ => throw new NotSupportedException($"RepositoryResolver, key: {name}"),
      };
    });

    services.AddScoped(x => new LinqToDbDataConnections.Access_Odbc(configuration, loggerFactory));
    services.AddScoped(x => new LinqToDbDataConnections.Access_OleDb(configuration, loggerFactory));
    services.AddScoped(x => new LinqToDbDataConnections.DB2_IBM(configuration, loggerFactory));
    services.AddScoped(x => new LinqToDbDataConnections.DB2_Odbc(configuration, loggerFactory));
    services.AddScoped(x => new LinqToDbDataConnections.DB2_OleDb(configuration, loggerFactory));
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
    services.AddScoped(x => new LinqToDbDataConnections.SqlServer_Odbc(configuration, loggerFactory));
    services.AddScoped(x => new LinqToDbDataConnections.SqlServer_OleDb(configuration, loggerFactory));

    return services;
  }

}