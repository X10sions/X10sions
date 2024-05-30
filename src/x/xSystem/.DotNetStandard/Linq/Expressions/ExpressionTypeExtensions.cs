namespace System.Linq.Expressions {
  public static class ExpressionTypeExtensions {

    public static string ToSqlString(this ExpressionType type) {
      switch (type) {
        case ExpressionType.Equal: return "=";
        case ExpressionType.NotEqual: return "!=";
        case ExpressionType.LessThan: return "<";
        case ExpressionType.LessThanOrEqual: return "<=";
        case ExpressionType.GreaterThan: return ">";
        case ExpressionType.GreaterThanOrEqual: return ">=";
        case ExpressionType.AndAlso:
        case ExpressionType.And:
          return "AND";
        case ExpressionType.Or:
        case ExpressionType.OrElse:
          return "OR";
        case ExpressionType.Default: return string.Empty;
        default:
          throw new NotImplementedException(type.ToString());
      }
    }

  }
}