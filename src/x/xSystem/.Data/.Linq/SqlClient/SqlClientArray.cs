using System.Collections.Generic;
using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlClientArray : SqlSimpleTypeExpression {
    private List<SqlExpression> expressions;

    internal SqlClientArray(Type clrType, ProviderType sqlType, SqlExpression[] exprs, Expression sourceExpression)
        : base(SqlNodeType.ClientArray, clrType, sqlType, sourceExpression) {
      expressions = new List<SqlExpression>();
      if (exprs != null)
        Expressions.AddRange(exprs);
    }

    internal List<SqlExpression> Expressions => expressions;
  }

}
