using xSystem.Data.Linq.SqlClient;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlOptionalValue : SqlSimpleTypeExpression {
    private SqlExpression hasValue;

    private SqlExpression expressionValue;

    internal SqlExpression HasValue {
      get => hasValue;
      set {
        hasValue = value ?? throw Error.ArgumentNull("value");
      }
    }

    internal SqlExpression Value {
      get => expressionValue;
      set {
        if (value == null) {
          throw Error.ArgumentNull("value");
        }
        if (value.ClrType != base.ClrType) {
          throw Error.ArgumentWrongType("value", base.ClrType, value.ClrType);
        }
        expressionValue = value;
      }
    }

    internal SqlOptionalValue(SqlExpression hasValue, SqlExpression value)
      : base(SqlNodeType.OptionalValue, value.ClrType, value.SqlType, value.SourceExpression) {
      HasValue = hasValue;
      Value = value;
    }
  }

}
