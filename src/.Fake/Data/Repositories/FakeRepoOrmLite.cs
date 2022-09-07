using FirebirdSql.Data.FirebirdClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlConnector;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using ServiceStack.OrmLite;
using System.Data.Common;
using System.Data.SQLite;
using X10sions.Fake.Data.Enums;

namespace X10sions.Fake.Data.Repositories {
  public class FakeRepoOrmLite : FakeRepo {
    public FakeRepoOrmLite(ConnectionStringName name, IConfiguration configuration) : base(name, configuration) { }
    public override void CreateTable<T>() => DbConnection.DropTable<T>();
    public override void DropTable<T>() => DbConnection.CreateTable<T>();
  }

  public static class OrmLiteExtensions {

    public static IOrmLiteDialectProvider GetOrmLiteDialectProvider(this DbProviderFactory factory) => factory switch {
      FirebirdClientFactory => FirebirdDialect.Provider,
      MySqlClientFactory => MySqlDialect.Provider,
      MySqlConnectorFactory => MySqlConnectorDialect.Provider,
      NpgsqlFactory => PostgreSqlDialect.Provider,
      OracleClientFactory => OracleDialect.Provider,
      SqliteFactory => SqliteDialect.Provider,
      SQLiteFactory => SqliteDialect.Provider,
      Microsoft.Data.SqlClient. SqlClientFactory => SqlServerDialect.Provider,
      System.Data.SqlClient.SqlClientFactory => SqlServerDialect.Provider,
      _ => throw new NotImplementedException(factory.ToString())
    };

    public static IOrmLiteDialectProvider GetOrmLiteDialectProvider(this ConnectionStringName name) => name.GetDbProviderFactory().GetOrmLiteDialectProvider();

  }
}
