using Common.Data;
using IBM.EntityFrameworkCore;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.DataProvider.Access;
using LinqToDB.DataProvider.MySql;
using LinqToDB.DataProvider.Oracle;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.DataProvider.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data.Odbc;
using System.Data.OleDb;

namespace CleanOnionExample.Data.DbContexts;


public static class Extensions {

  public static IServiceCollection AddScopedRepository_CleanOnionExample(this IServiceCollection services) {
    services.AddScoped(typeof(IEFCoreRepository<>), typeof(EFCoreRepository<>));
    services.AddScoped(typeof(IHttpClientRepository<>), typeof(HttpClientRepository<>));
    services.AddScoped(typeof(ILinq2DBRepository<>), typeof(Linq2DbRepository<>));
    services.AddScoped(typeof(INHibernateRepository<>), typeof(NHibernateRepository<>));
    services.AddScoped(typeof(RepositoryManager<>));
    return services;
  }

  public static IServiceCollection AddEFCore_CleanOnionExample(this IServiceCollection services, IDataAppSettings appSettings, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory) {
    services.AddDbContext<EFCoreDbContexts.Access>(options => { options.UseLoggerFactory(loggerFactory).UseJet(appSettings.ConnectionStrings.Access_OleDb()); });
    services.AddDbContext<EFCoreDbContexts.DB2>(options => {
      options.UseLoggerFactory(loggerFactory).UseDb2(appSettings.ConnectionStrings.DB2iSeries_IBM(), p => {
        p.SetServerInfo(IBMDBServerType.LUW);
        p.UseRowNumberForPaging();
      });
    });
    services.AddDbContext<EFCoreDbContexts.DB2iSeries>(options => {
      options.UseLoggerFactory(loggerFactory).UseDb2(appSettings.ConnectionStrings.DB2iSeries_IBM(), p => {
        p.SetServerInfo(IBMDBServerType.AS400);
        p.UseRowNumberForPaging();
      });
    });
    var mysql = appSettings.ConnectionStrings.MySql();
    var maria = appSettings.ConnectionStrings.MariaDb();
    services.AddDbContext<EFCoreDbContexts.MariaDb>(options => { options.UseLoggerFactory(loggerFactory).UseMySql(maria, ServerVersion.AutoDetect(maria)); });
    services.AddDbContext<EFCoreDbContexts.MySql>(options => { options.UseLoggerFactory(loggerFactory).UseMySql(mysql, ServerVersion.AutoDetect(mysql)); });
    services.AddDbContext<EFCoreDbContexts.PostgreSql>(options => { options.UseLoggerFactory(loggerFactory).UseNpgsql(appSettings.ConnectionStrings.PostgreSql()); });
    services.AddDbContext<EFCoreDbContexts.Oracle>(options => { options.UseLoggerFactory(loggerFactory).UseOracle(appSettings.ConnectionStrings.Oracle()); });
    services.AddDbContext<EFCoreDbContexts.SqlServer>(options => { options.UseLoggerFactory(loggerFactory).UseSqlServer(appSettings.ConnectionStrings.SqlServer()); });
    services.AddDbContext<EFCoreDbContexts.Sqlite>(options => { options.UseLoggerFactory(loggerFactory).UseSqlite(appSettings.ConnectionStrings.Sqlite()); });
    services.AddScoped<DbContextManager>();
    return services;
  }

  public static IServiceCollection AddLinqToDb_CleanOnionExample(this IServiceCollection services, IDataAppSettings appSettings, ILogger logger) {
    services.AddLinqToDBContext<LinqToDbDataConnections.Access, OleDbConnection>(AccessTools.GetDataProvider(ProviderName.Access), appSettings.ConnectionStrings.Access_OleDb(), logger);
    //services.AddLinqToDbContext<LinqToDbDataConnections.DB2, DB2Connection>(new DB2DataProvider(nameof(IBM.Data.Db2), DB2Version.LUW), appSettings.ConnectionStrings.DB2(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.DB2iSeries_Odbc, OdbcConnection, OdbcDataReader>(appSettings.ConnectionStrings.DB2iSeries_Odbc(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.DB2iSeries_OleDb, OleDbConnection, OleDbDataReader>(appSettings.ConnectionStrings.DB2iSeries_OleDb(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.MariaDb, MySql.Data.MySqlClient.MySqlConnection>(MySqlTools.GetDataProvider(ProviderName.MySql), appSettings.ConnectionStrings.MariaDb(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.MySql, MySqlConnector.MySqlConnection>(MySqlTools.GetDataProvider(ProviderName.MySqlConnector), appSettings.ConnectionStrings.MySql(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.PostgreSql, NpgsqlConnection>(PostgreSQLTools.GetDataProvider(), appSettings.ConnectionStrings.PostgreSql(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.Oracle, OracleConnection>(OracleTools.GetDataProvider(OracleVersion.v12, OracleProvider.Managed), appSettings.ConnectionStrings.Oracle(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.SqlServer, SqlConnection>(SqlServerTools.GetDataProvider(SqlServerVersion.v2012, SqlServerProvider.MicrosoftDataSqlClient), appSettings.ConnectionStrings.SqlServer(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.Sqlite, SqliteConnection>(SQLiteTools.GetDataProvider(ProviderName.SQLiteMS), appSettings.ConnectionStrings.Sqlite(), logger);
    return services;
  }
}

/*

add-migration v1

update-database


 */