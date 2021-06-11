namespace System.Data.Linq.SqlClient {
  internal class SqlTypeCaseWhen {
    private SqlExpression match;
    internal SqlExpression Match {
      get {
        return match;
      }
      set {
        if (match != null && value != null && match.ClrType != value.ClrType) {
          throw Error.ArgumentWrongType("value", match.ClrType, value.ClrType);
        }
        match = value;
      }
    }

    internal SqlExpression TypeBinding { get; set; }

    internal SqlTypeCaseWhen(SqlExpression match, SqlExpression typeBinding) {
      Match = match;
      TypeBinding = typeBinding;
    }
  }

}
