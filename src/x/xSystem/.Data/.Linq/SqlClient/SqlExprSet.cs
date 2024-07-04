using System.Collections.Generic;
using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlExprSet : SqlExpression {
    private List<SqlExpression> expressions;

    internal SqlExprSet(Type clrType, IEnumerable<SqlExpression> exprs, Expression sourceExpression)
        : base(SqlNodeType.ExprSet, clrType, sourceExpression) {
      expressions = new List<SqlExpression>(exprs);
    }

    internal List<SqlExpression> Expressions => expressions;

    /// <summary>
    /// Get the first non-set expression of the set by drilling
    /// down the left expressions.
    /// </summary>
    internal SqlExpression GetFirstExpression() {
      var expr = expressions[0];
      while (expr is SqlExprSet) {
        expr = ((SqlExprSet)expr).Expressions[0];
      }
      return expr;
    }

    internal override ProviderType SqlType => expressions[0].SqlType;
  }

}
