using Chloe.Infrastructure;
using FirebirdSql.Data.FirebirdClient;
using LinqToDB;
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
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlConnector;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SQLite;
using X10sions.Fake.Data.Enums;

namespace X10sions.Fake.Data.Repositories {
  public class FakeRepoLinqToDb : FakeRepo {
    public FakeRepoLinqToDb(ConnectionStringName name, IConfiguration configuration) : base(name, configuration) {
      DataConnection = new LinqToDbDataConnections.BaseDataConnection(name.GetLinqToDBConnectionOptions(configuration));
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

    public static LinqToDBConnectionOptions<T> GetLinqToDBConnectionOptions<T>(this ConnectionStringName name, IConfiguration configuration) => new LinqToDBConnectionOptions<T>(name.GetLinqToDBConnectionOptionsBuilder(configuration));
    public static LinqToDBConnectionOptions GetLinqToDBConnectionOptions(this ConnectionStringName name, IConfiguration configuration) => new LinqToDBConnectionOptions(name.GetLinqToDBConnectionOptionsBuilder(configuration));

    public static LinqToDBConnectionOptionsBuilder GetLinqToDBConnectionOptionsBuilder(this ConnectionStringName name, IConfiguration configuration) {
      var connectionString = name.GetConnectionString(configuration);
      var factory = name.GetDbProviderFactory();
      var dataProvider = factory.GetLinqToDbDataProvider();
      var builder = new LinqToDBConnectionOptionsBuilder();
      builder.UseConnectionString(dataProvider, connectionString);
      return builder;
    }

  }

}
