using System.Linq.Expressions;
using xSystem.Data.Linq.SqlClient;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlLike : SqlSimpleTypeExpression {
    private SqlExpression expression;
    private SqlExpression pattern;
    private SqlExpression escape;

    internal SqlLike(Type clrType, ProviderType sqlType, SqlExpression expr, SqlExpression pattern, SqlExpression escape, Expression source)
        : base(SqlNodeType.Like, clrType, sqlType, source) {
      if (expr == null)
        throw Error.ArgumentNull("expr");
      if (pattern == null)
        throw Error.ArgumentNull("pattern");
      Expression = expr;
      Pattern = pattern;
      Escape = escape;
    }

    internal SqlExpression Expression {
      get => expression;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (value.ClrType != typeof(string))
          throw Error.ArgumentWrongType("value", "string", value.ClrType);
        expression = value;
      }
    }

    internal SqlExpression Pattern {
      get => pattern;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (value.ClrType != typeof(string))
          throw Error.ArgumentWrongType("value", "string", value.ClrType);
        pattern = value;
      }
    }

    internal SqlExpression Escape {
      get => escape;
      set {
        if (value != null && value.ClrType != typeof(string))
          throw Error.ArgumentWrongType("value", "string", value.ClrType);
        escape = value;
      }
    }
  }

}
