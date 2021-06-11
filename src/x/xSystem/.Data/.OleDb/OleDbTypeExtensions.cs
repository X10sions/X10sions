namespace System.Data.OleDb {
  public static class OleDbTypeExtensions {

    public static OleDbParameter ToNewOleDbParameter(this OleDbType @this) => new OleDbParameter { OleDbType = @this };
    public static DbType AsDbType(this OleDbType @this) => @this.ToNewOleDbParameter().DbType;

  }
}