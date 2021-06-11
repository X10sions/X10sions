using System.Collections.Generic;

namespace System.Linq.Expressions {
  public static class MethodCallExpressionExtensions {

    public static List<Expression> GetSqlArguments(this MethodCallExpression methodCallExpression) {
      var sqlArguments = new List<Expression> { methodCallExpression.Object };
      if (methodCallExpression.Arguments.Count == 1) {
        object obj = (methodCallExpression.Arguments[0] as ConstantExpression)?.Value;
        var chars = new List<char>();
        object obj2 = obj;
        bool num = obj2 is char;
        char item = num ? ((char)obj2) : '\0';
        char[] collection;
        if (num) {
          chars.Add(item);
        } else if ((collection = (obj as char[])) != null) {
          chars.AddRange(collection);
        }
        if (chars.Count > 0) {
          sqlArguments.Add(Expression.Constant(new string(chars.ToArray()), typeof(string)));
        }
      }
      return sqlArguments;
    }

  }
}