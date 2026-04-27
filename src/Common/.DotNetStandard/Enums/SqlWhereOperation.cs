using System.Globalization;

namespace Common.Enums;

public enum SqlWhereOperation {
  Equal,
  Between,
  Like,
  LikeStart,
  LikeEnd,
  InList,
  IsNull,
  GreaterThan,
  GreaterThanOrEqual,
  LessThan,
  LessThanOrEqual,
  NotEqual
}

public static class SqlWhereOperationExtensions {

  public static string ToSqlExpression(this SqlWhereOperation operation) => operation.ToSqlExpression(null, null);
  public static string ToSqlExpression(this SqlWhereOperation operation, object value) => operation.ToSqlExpression(value, value);
  public static string ToSqlExpression(this SqlWhereOperation operation, params object[] values) => operation.ToSqlExpression(values.Min(), values.Max());

  public static string ToSqlExpression(this SqlWhereOperation operation, object value, object valueMax) {
    switch (operation) {
      case SqlWhereOperation.Between: return "Between " + value.ToSqlQualifiedValue() + " And " + valueMax.ToSqlQualifiedValue();
      case SqlWhereOperation.Like: return "Like '%" + value + "%'";
      case SqlWhereOperation.LikeEnd: return "Like '%" + value + "'";
      case SqlWhereOperation.Equal: return "= " + value.ToSqlQualifiedValue();
      case SqlWhereOperation.GreaterThan: return "> " + value.ToSqlQualifiedValue();
      case SqlWhereOperation.GreaterThanOrEqual: return ">= " + value.ToSqlQualifiedValue();
      case SqlWhereOperation.LessThan: return "< " + value.ToSqlQualifiedValue();
      case SqlWhereOperation.LessThanOrEqual: return "<= " + value.ToSqlQualifiedValue();
      case SqlWhereOperation.NotEqual: return "<> " + value.ToSqlQualifiedValue();
      case SqlWhereOperation.LikeStart: return "Like '" + value + "%'";
      case SqlWhereOperation.IsNull: return "Is Null";
      default: throw new Exception();
    }
  }

  public static string ToSqlQualifiedValue(this object value) {
    if (value == null || value == DBNull.Value) {
      return "NULL";
    }
    switch (value) {
      case string str: return "'" + str.Replace("'", "''") + "'";
      case bool boolean: return boolean ? "1" : "0";
      case DateTime date: return "'" + date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'";
      case int:
      case long:
      case short:
      case byte:
        return value.ToString();
      case double:
      case float:
      case decimal:
        return Convert.ToString(value, CultureInfo.InvariantCulture);
      default:
        string fallback = value.ToString();
        return "'" + fallback.Replace("'", "''") + "'";
    }
  }


}

