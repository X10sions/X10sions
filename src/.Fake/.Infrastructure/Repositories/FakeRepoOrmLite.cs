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

namespace X10sions.Fake.Data.Repositories;
public class FakeRepoOrmLite : FakeRepo {
  public FakeRepoOrmLite(ConnectionStringName name, IConfiguration configuration) : base(name, configuration) { }
  public override void CreateTable<T>() => DbConnection.CreateTable<T>();
  public override long Delete<T>(IEnumerable<T> objects) => DbConnection.DeleteAll(objects);
  public override void DropTable<T>() => DbConnection.DropTable<T>();

  public override IQueryable<T> GetQueryable<T>() {
    throw new NotImplementedException();
  }
  public override long Insert<T>(IEnumerable<T> objects) {
    long count = 0;
    foreach (var o in objects) {
      count += DbConnection.Insert(o);
      //count += DbConnection.Save(o) == true ? 1 : 0;
    }
    return count;
  }

  public override T InsertWithIdentity<T>(T row) {
    DbConnection.Save(row);
    return row;
  }

  public override long Update<T>(IEnumerable<T> objects) => DbConnection.UpdateAll(objects);
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
    Microsoft.Data.SqlClient.SqlClientFactory => SqlServerDialect.Provider,
    System.Data.SqlClient.SqlClientFactory => SqlServerDialect.Provider,
    _ => throw new NotImplementedException(factory.ToString())
  };

  public static IOrmLiteDialectProvider GetOrmLiteDialectProvider(this ConnectionStringName name) => name.GetDbProviderFactory().GetOrmLiteDialectProvider();

}
