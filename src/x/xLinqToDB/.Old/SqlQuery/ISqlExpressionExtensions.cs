using System;

namespace LinqToDB.SqlQuery {

  public static class ISqlExpressionExtensions {

    public static int GetPrecedence(this ISqlExpression expr) => expr.Precedence;

    public static SqlField GetUnderlayingField(this ISqlExpression expr) {
      switch (expr.ElementType) {
        case QueryElementType.SqlField: return (SqlField)expr;
        case QueryElementType.Column: return GetUnderlayingField(((SqlColumn)expr).Expression);
      }
      throw new InvalidOperationException();
    }

    public static bool IsBooleanParameter(this ISqlExpression expr, int count, int i) {
      if ((i % 2 == 1 || i == count - 1) && expr.SystemType == typeof(bool) || expr.SystemType == typeof(bool?)) {
        switch (expr.ElementType) {
          case QueryElementType.SearchCondition: return true;
        }
      }
      return false;
    }

    public static bool IsDateDataType(this ISqlExpression expr, string dateName) {
      switch (expr.ElementType) {
        case QueryElementType.SqlDataType: return ((SqlDataType)expr).Type.DataType == DataType.Date;
        case QueryElementType.SqlExpression: return ((SqlExpression)expr).Expr == dateName;
      }
      return false;
    }

    public static bool IsTimeDataType(this ISqlExpression expr) {
      switch (expr.ElementType) {
        case QueryElementType.SqlDataType: return ((SqlDataType)expr).Type.DataType == DataType.Time;
        case QueryElementType.SqlExpression: return ((SqlExpression)expr).Expr == nameof(DataType.Time);
      }
      return false;
    }

  }
}
