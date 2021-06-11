namespace System.Data.Linq.SqlClient {
  internal class SqlDoNotVisitExpression : SqlExpression {
    internal new SqlExpression Expression { get; }
    internal override ProviderType SqlType => Expression.SqlType;

    internal SqlDoNotVisitExpression(SqlExpression expr)
      : base(SqlNodeType.DoNotVisit, expr.ClrType, expr.SourceExpression) {
      Expression = expr ?? throw Error.ArgumentNull("expr");
    }
  }

}
