namespace xSystem.Data.Linq.SqlClient {
  /// <summary>
  /// A single WHEN clause for ClientCase.
  /// </summary>
  internal class SqlClientWhen {
    private SqlExpression matchExpression;
    private SqlExpression matchValue;

    internal SqlClientWhen(SqlExpression match, SqlExpression value) {
      // 'match' may be null when this when represents the ELSE condition.
      if (value == null)
        throw Error.ArgumentNull("value");
      Match = match;
      Value = value;
    }

    internal SqlExpression Match {
      get => matchExpression;
      set {
        if (matchExpression != null && value != null && matchExpression.ClrType != value.ClrType)
          throw Error.ArgumentWrongType("value", matchExpression.ClrType, value.ClrType);
        matchExpression = value;
      }
    }

    internal SqlExpression Value {
      get => matchValue;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (matchValue != null && matchValue.ClrType != value.ClrType)
          throw Error.ArgumentWrongType("value", matchValue.ClrType, value.ClrType);
        matchValue = value;
      }
    }
  }

}
