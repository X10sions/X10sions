using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace xEFCore.xAS400.Query.ExpressionTranslators.Internal.MethodCallTranslators {
  public class AS400StringTrimStartTranslator : IMethodCallTranslator {

    // Method defined in netcoreapp2.0 only
    static readonly MethodInfo _methodInfoWithoutArgs
      = typeof(string).GetRuntimeMethod(nameof(string.TrimStart), new Type[] { });

    static readonly MethodInfo _methodInfoWithCharArg
      = typeof(string).GetRuntimeMethod(nameof(string.TrimStart), new[] { typeof(char) });

    // Method defined in netstandard2.0
    static readonly MethodInfo _methodInfoWithCharArrayArg
      = typeof(string).GetRuntimeMethod(nameof(string.TrimStart), new[] { typeof(char[]) });

    public virtual Expression Translate(MethodCallExpression methodCallExpression) {
      var methodInfos = from x in new[] { _methodInfoWithoutArgs, _methodInfoWithCharArg, _methodInfoWithCharArrayArg }
                        where x.Equals(methodCallExpression.Method)
                        select x;
      if (methodInfos.Count() == 0) {
        return null;
      }
      var sqlArguments = methodCallExpression.GetSqlArguments();
      return new SqlFunctionExpression("LTrim", methodCallExpression.Type, sqlArguments);
    }

  }
}