using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlBetween : SqlSimpleTypeExpression {
    SqlExpression expression;
    SqlExpression start;
    SqlExpression end;

    internal SqlBetween(Type clrType, ProviderType sqlType, SqlExpression expr, SqlExpression start, SqlExpression end, Expression source)
        : base(SqlNodeType.Between, clrType, sqlType, source) {
      expression = expr;
      this.start = start;
      this.end = end;
    }

    internal SqlExpression Expression {
      get => expression;
      set => expression = value;
    }

    internal SqlExpression Start {
      get => start;
      set => start = value;
    }

    internal SqlExpression End {
      get => end;
      set => end = value;
    }
  }

}
