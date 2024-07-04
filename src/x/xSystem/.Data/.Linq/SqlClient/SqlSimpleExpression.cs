using System.Data.Linq;
using System.Data.Linq.SqlClient;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlSimpleExpression : SqlExpression {
    private SqlExpression expr;

    internal SqlSimpleExpression(SqlExpression expr)
        : base(SqlNodeType.SimpleExpression, expr.ClrType, expr.SourceExpression) {
      this.expr = expr;
    }

    internal SqlExpression Expression {
      get => expr;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (!TypeSystem.GetNonNullableType(ClrType).IsAssignableFrom(TypeSystem.GetNonNullableType(value.ClrType)))
          throw Error.ArgumentWrongType("value", ClrType, value.ClrType);
        expr = value;
      }
    }

    internal override ProviderType SqlType => expr.SqlType;
  }

}
