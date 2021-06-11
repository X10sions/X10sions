namespace System.Data.Linq.SqlClient {
  internal class SqlDoNotVisitExpression : SqlExpression {
    private SqlExpression expression;

    internal SqlDoNotVisitExpression(SqlExpression expr)
        : base(SqlNodeType.DoNotVisit, expr.ClrType, expr.SourceExpression) {
      if (expr == null)
        throw Error.ArgumentNull("expr");
      expression = expr;
    }

    internal SqlExpression Expression => expression;

    internal override ProviderType SqlType => expression.SqlType;
  }

}
