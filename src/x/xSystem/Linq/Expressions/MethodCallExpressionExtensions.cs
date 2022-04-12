namespace System.Linq.Expressions;
public static class MethodCallExpressionExtensions {

  public static List<Expression> GetSqlArguments(this MethodCallExpression methodCallExpression) {
    var sqlArguments = new List<Expression> { methodCallExpression.Object };
    if (methodCallExpression.Arguments.Count == 1) {
      object? obj = (methodCallExpression.Arguments[0] as ConstantExpression)?.Value;
      var chars = new List<char>();
      char[]? collection;
      if (obj is char @char) {
        chars.Add(obj is char ? @char : '\0');
      } else if ((collection = obj as char[]) != null) {
        chars.AddRange(collection);
      }
      if (chars.Count > 0) {
        sqlArguments.Add(Expression.Constant(new string(chars.ToArray()), typeof(string)));
      }
    }
    return sqlArguments;
  }

}
