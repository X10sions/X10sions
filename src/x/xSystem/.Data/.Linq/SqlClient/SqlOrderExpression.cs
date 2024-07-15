using System.Data.Linq.SqlClient;

namespace System.Data.Linq.SqlClient {
  internal class SqlOrderExpression : IEquatable<SqlOrderExpression> {
    private SqlExpression expression;

    internal SqlOrderType OrderType { get; set; }

    internal SqlExpression Expression {
      get => expression;
      set {
        if (value == null) {
          throw Error.ArgumentNull("value");
        }
        if (expression != null && !expression.ClrType.IsAssignableFrom(value.ClrType)) {
          throw Error.ArgumentWrongType("value", expression.ClrType, value.ClrType);
        }
        expression = value;
      }
    }

    internal SqlOrderExpression(SqlOrderType type, SqlExpression expr) {
      OrderType = type;
      Expression = expr;
    }

    public override bool Equals(object obj) {
      if (EqualsTo(obj as SqlOrderExpression)) {
        return true;
      }
      return base.Equals(obj);
    }

    public bool Equals(SqlOrderExpression other) {
      if (EqualsTo(other)) {
        return true;
      }
      return base.Equals(other);
    }

    private bool EqualsTo(SqlOrderExpression other) {
      if (other == null) {
        return false;
      }
      if (this == other) {
        return true;
      }
      if (OrderType != other.OrderType) {
        return false;
      }
      if (!Expression.SqlType.Equals(other.Expression.SqlType)) {
        return false;
      }
      var sqlColumn = UnwrapColumn(Expression);
      var sqlColumn2 = UnwrapColumn(other.Expression);
      if (sqlColumn == null || sqlColumn2 == null) {
        return false;
      }
      return sqlColumn == sqlColumn2;
    }

    public override int GetHashCode() => UnwrapColumn(Expression)?.GetHashCode() ?? base.GetHashCode();

    private static SqlColumn UnwrapColumn(SqlExpression expr) {
      var sqlUnary = expr as SqlUnary;
      if (sqlUnary != null) {
        expr = sqlUnary.Operand;
      }
      var sqlColumn = expr as SqlColumn;
      if (sqlColumn != null) {
        return sqlColumn;
      }
      return (expr as SqlColumnRef)?.GetRootColumn();
    }
  }

}
