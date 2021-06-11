using Microsoft.Data.SqlClient;

namespace System.Data {
  public static class DbTypeExtensions {
    public static SqlParameter NewSqlParameter(this DbType dbType) => new SqlParameter { DbType = dbType };
    public static SqlDbType ToSqlDbType(this DbType dbType) => dbType.NewSqlParameter().SqlDbType;
    public static SqlDbType AsSqlDbType(this DbType @this) => @this.ToSqlDbType();
  }
}