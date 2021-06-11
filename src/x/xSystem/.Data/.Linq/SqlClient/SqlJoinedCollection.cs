using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlJoinedCollection : SqlSimpleTypeExpression {
    private SqlExpression expression;
    private SqlExpression count;

    internal SqlJoinedCollection(Type clrType, ProviderType sqlType, SqlExpression expression, SqlExpression count, Expression sourceExpression)
        : base(SqlNodeType.JoinedCollection, clrType, sqlType, sourceExpression) {
      this.expression = expression;
      this.count = count;
    }

    internal SqlExpression Expression {
      get => expression;
      set {
        if (value == null || expression != null && expression.ClrType != value.ClrType)
          throw Error.ArgumentWrongType(value, expression.ClrType, value.ClrType);
        expression = value;
      }
    }

    internal SqlExpression Count {
      get => count;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (value.ClrType != typeof(int))
          throw Error.ArgumentWrongType(value, typeof(int), value.ClrType);
        count = value;
      }
    }
  }

}
