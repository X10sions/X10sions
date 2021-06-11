using System.Data.OleDb;

namespace System.Data {
  public static class DbTypeExtensions {
    public static OleDbParameter NewOleDbParameter(this DbType dbType) => new OleDbParameter { DbType = dbType };
    public static OleDbType ToOleDbType(this DbType dbType) => dbType.NewOleDbParameter().OleDbType;
    public static OleDbType AsOleDbType(this DbType @this) => @this.ToOleDbType();
  }
}
