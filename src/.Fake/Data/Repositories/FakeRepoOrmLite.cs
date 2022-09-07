using ServiceStack.OrmLite;
using System.Data;
using System.Data.Common;
using X10sions.Fake.Data.Enums;

namespace X10sions.Fake.Data.Repositories {
  public class FakeRepoOrmLite : FakeRepo {
    public FakeRepoOrmLite(IDbConnection db) : base(db) { }
    public override void CreateTable<T>() => DbConnection.DropTable<T>();
    public override void DropTable<T>() => DbConnection.CreateTable<T>();

  }

  public static class OrmLiteExtensions {

    public static IOrmLiteDialectProvider GetOrmLiteDialectProvider(this DbTypes dbType) => dbType switch {
      DbTypes.Firebird => FirebirdDialect.Provider,
      DbTypes.MySql => MySqlDialect.Provider,
      DbTypes.MySqlConnector => MySqlConnectorDialect.Provider,
      DbTypes.Oracle => OracleDialect.Provider,
      DbTypes.PostgreSql => PostgreSqlDialect.Provider,
      DbTypes.Sqlite => SqliteDialect.Provider,
      DbTypes.SqlServer => SqlServerDialect.Provider,
      _ => throw new NotImplementedException(dbType.ToString())
    };

  }
}
