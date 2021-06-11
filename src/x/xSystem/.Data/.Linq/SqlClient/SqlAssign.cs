using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlAssign : SqlStatement {
    private SqlExpression leftValue;
    private SqlExpression rightValue;

    internal SqlAssign(SqlExpression lValue, SqlExpression rValue, Expression sourceExpression)
        : base(SqlNodeType.Assign, sourceExpression) {
      LValue = lValue;
      RValue = rValue;
    }

    internal SqlExpression LValue {
      get => leftValue;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (rightValue != null && !value.ClrType.IsAssignableFrom(rightValue.ClrType))
          throw Error.ArgumentWrongType("value", rightValue.ClrType, value.ClrType);
        leftValue = value;
      }
    }

    internal SqlExpression RValue {
      get => rightValue;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (leftValue != null && !leftValue.ClrType.IsAssignableFrom(value.ClrType))
          throw Error.ArgumentWrongType("value", leftValue.ClrType, value.ClrType);
        rightValue = value;
      }
    }
  }

}
