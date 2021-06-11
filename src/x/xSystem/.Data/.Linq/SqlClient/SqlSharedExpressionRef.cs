namespace System.Data.Linq.SqlClient {
  internal class SqlSharedExpressionRef : SqlExpression {
    private SqlSharedExpression expr;

    internal SqlSharedExpressionRef(SqlSharedExpression expr)
        : base(SqlNodeType.SharedExpressionRef, expr.ClrType, expr.SourceExpression) {
      this.expr = expr;
    }

    internal SqlSharedExpression SharedExpression => expr;

    internal override ProviderType SqlType => expr.SqlType;
  }

}
