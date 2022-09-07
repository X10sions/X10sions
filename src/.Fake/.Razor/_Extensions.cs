using IBM.Data.Db2;
using IBM.EntityFrameworkCore;
using LinqToDB.DataProvider.Access;
using LinqToDB.DataProvider.DB2;
using LinqToDB.DataProvider.DB2iSeries;
using LinqToDB.DataProvider.MySql;
using LinqToDB.DataProvider.Oracle;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.DataProvider.SqlServer;
using LinqToDB;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data.Odbc;
using System.Data.OleDb;
using Microsoft.Extensions.Configuration;
using X10sions.Fake.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace X10sions.Fake {
  public static class RazorExtensions {
    public static ConnectionStrings ConnectionStrings(this IConfiguration configuration) => new ConnectionStrings(configuration);

    public static IServiceCollection AddEFCore_Fake(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory) {
      var connectionStrings = configuration.ConnectionStrings();
      services.AddDbContext<EFCoreDbContexts.Access>(options => { options.UseLoggerFactory(loggerFactory).UseJet(connectionStrings.Access_Oledb); });
      services.AddDbContext<EFCoreDbContexts.DB2>(options => {
        options.UseLoggerFactory(loggerFactory).UseDb2(connectionStrings.DB2_IBM, p => {
          p.SetServerInfo(IBMDBServerType.LUW);
          p.UseRowNumberForPaging();
        });
      });
      services.AddDbContext<EFCoreDbContexts.DB2iSeries>(options => {
        options.UseLoggerFactory(loggerFactory).UseDb2(connectionStrings.DB2iSeries_IBM, p => {
          p.SetServerInfo(IBMDBServerType.AS400);
          p.UseRowNumberForPaging();
        });
      });
      string mysql = connectionStrings.MySql;
      string maria = connectionStrings.MariaDb;
      services.AddDbContext<EFCoreDbContexts.MariaDb>(options => { options.UseLoggerFactory(loggerFactory).UseMySql(maria, ServerVersion.AutoDetect(maria)); });
      services.AddDbContext<EFCoreDbContexts.MySql>(options => { options.UseLoggerFactory(loggerFactory).UseMySql(mysql, ServerVersion.AutoDetect(mysql)); });
      services.AddDbContext<EFCoreDbContexts.PostgreSql>(options => { options.UseLoggerFactory(loggerFactory).UseNpgsql(connectionStrings.PostgreSql); });
      services.AddDbContext<EFCoreDbContexts.Oracle>(options => { options.UseLoggerFactory(loggerFactory).UseOracle(connectionStrings.Oracle); });
      services.AddDbContext<EFCoreDbContexts.SqlServer>(options => { options.UseLoggerFactory(loggerFactory).UseSqlServer(connectionStrings.SqlServer); });
      services.AddDbContext<EFCoreDbContexts.Sqlite>(options => { options.UseLoggerFactory(loggerFactory).UseSqlite(connectionStrings.Sqlite); });
      return services;
    }

    public static IServiceCollection AddLinqToDb_Fake(this IServiceCollection services, IConfiguration configuration, ILogger logger) {
      var connectionStrings = configuration.ConnectionStrings();
      services.AddLinqToDBContext<LinqToDbDataConnections.Access_OleDb, OleDbConnection>(AccessTools.GetDataProvider(ProviderName.Access), connectionStrings.Access_Oledb, logger);
      services.AddLinqToDBContext<LinqToDbDataConnections.DB2, DB2Connection>(DB2Tools.GetDataProvider(DB2Version.LUW), connectionStrings.DB2_IBM, logger);

      services.AddLinqToDBContext<LinqToDbDataConnections.DB2iSeries_Odbc, OdbcConnection>(DB2iSeriesTools.GetDataProvider(DB2iSeriesVersion.V7_4, DB2iSeriesProviderType.Odbc, DB2iSeriesMappingOptions.Default), connectionStrings.DB2iSeries_Odbc, logger);
      services.AddLinqToDBContext<LinqToDbDataConnections.DB2iSeries_OleDb, OleDbConnection>(DB2iSeriesTools.GetDataProvider(DB2iSeriesVersion.V7_4, DB2iSeriesProviderType.OleDb, DB2iSeriesMappingOptions.Default), connectionStrings.DB2iSeries_OleDb, logger);

      //services.AddLinqToDBContext<LinqToDbDataConnections.DB2iSeries_Odbc, OdbcConnection, OdbcDataReader>(connectionStrings.DB2iSeries_Odbc, logger);
      //services.AddLinqToDBContext<LinqToDbDataConnections.DB2iSeries_OleDb, OleDbConnection, OleDbDataReader>(connectionStrings.DB2iSeries_OleDb, logger);

      services.AddLinqToDBContext<LinqToDbDataConnections.MariaDb, MySql.Data.MySqlClient.MySqlConnection>(MySqlTools.GetDataProvider(nameof(MySql)), connectionStrings.MariaDb, logger);
      services.AddLinqToDBContext<LinqToDbDataConnections.MySql, MySqlConnector.MySqlConnection>(MySqlTools.GetDataProvider(nameof(MySqlConnector)), connectionStrings.MySql, logger);
      services.AddLinqToDBContext<LinqToDbDataConnections.PostgreSql, NpgsqlConnection>(PostgreSQLTools.GetDataProvider(), connectionStrings.PostgreSql, logger);
      services.AddLinqToDBContext<LinqToDbDataConnections.Oracle, OracleConnection>(OracleTools.GetDataProvider(OracleVersion.v12, OracleProvider.Managed), connectionStrings.Oracle, logger);
      services.AddLinqToDBContext<LinqToDbDataConnections.SqlServer, SqlConnection>(SqlServerTools.GetDataProvider(SqlServerVersion.v2012, SqlServerProvider.MicrosoftDataSqlClient), connectionStrings.SqlServer, logger);
      services.AddLinqToDBContext<LinqToDbDataConnections.Sqlite, SqliteConnection>(SQLiteTools.GetDataProvider(ProviderName.SQLiteMS), connectionStrings.Sqlite, logger);
      return services;
    }

    public static IServiceCollection AddFake(this IServiceCollection services, IConfiguration configuration, ILogger logger, ILoggerFactory loggerFactory) => services.AddEFCore_Fake(configuration, loggerFactory).AddLinqToDb_Fake(configuration, logger);


  }
}
