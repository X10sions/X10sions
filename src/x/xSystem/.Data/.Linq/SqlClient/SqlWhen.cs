namespace System.Data.Linq.SqlClient {
  internal class SqlWhen {
    private SqlExpression matchExpression;
    private SqlExpression valueExpression;

    internal SqlWhen(SqlExpression match, SqlExpression value) {
      // 'match' may be null when this when represents the ELSE condition.
      if (value == null)
        throw Error.ArgumentNull("value");
      Match = match;
      Value = value;
    }

    internal SqlExpression Match {
      get => matchExpression;
      set {
        if (matchExpression != null && value != null && matchExpression.ClrType != value.ClrType
            // Exception: bool types, because predicates can have type bool or bool?
            && !TypeSystem.GetNonNullableType(matchExpression.ClrType).Equals(typeof(bool))
            && !TypeSystem.GetNonNullableType(value.ClrType).Equals(typeof(bool)))
          throw Error.ArgumentWrongType("value", matchExpression.ClrType, value.ClrType);
        matchExpression = value;
      }
    }

    internal SqlExpression Value {
      get => valueExpression;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (valueExpression != null && !valueExpression.ClrType.IsAssignableFrom(value.ClrType))
          throw Error.ArgumentWrongType("value", valueExpression.ClrType, value.ClrType);
        valueExpression = value;
      }
    }
  }

}
