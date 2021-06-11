namespace System.Data.Linq.SqlClient {
  internal class SqlSharedExpression : SqlExpression {
    private SqlExpression expr;

    internal SqlSharedExpression(SqlExpression expr)
      : base(SqlNodeType.SharedExpression, expr.ClrType, expr.SourceExpression) {
      this.expr = expr;
    }

    internal SqlExpression Expression {
      get => expr;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (!ClrType.IsAssignableFrom(value.ClrType)
            && !value.ClrType.IsAssignableFrom(ClrType))
          throw Error.ArgumentWrongType("value", ClrType, value.ClrType);
        expr = value;
      }
    }

    internal override ProviderType SqlType => expr.SqlType;
  }

}
