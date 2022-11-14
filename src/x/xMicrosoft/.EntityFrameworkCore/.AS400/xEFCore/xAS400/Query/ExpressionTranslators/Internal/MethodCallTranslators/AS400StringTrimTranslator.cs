using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace xEFCore.xAS400.Query.ExpressionTranslators.Internal.MethodCallTranslators {
  public class AS400StringTrimTranslator : IMethodCallTranslator {
    // Method defined in netstandard2.0
    static readonly MethodInfo _methodInfoWithoutArgs
      = typeof(string).GetRuntimeMethod(nameof(string.Trim), new Type[] { });

    // Method defined in netstandard2.0
    static readonly MethodInfo _methodInfoWithCharArrayArg
      = typeof(string).GetRuntimeMethod(nameof(string.Trim), new[] { typeof(char[]) });

    public virtual Expression Translate(MethodCallExpression methodCallExpression) {
      var methodInfos = from x in new[] { _methodInfoWithoutArgs, _methodInfoWithCharArrayArg }
                        where x.Equals(methodCallExpression.Method)
                        select x;
      if (methodInfos.Count() == 0) {
        return null;
      }
      List<Expression> sqlArguments = new List<Expression> { methodCallExpression.Object };
      return new SqlFunctionExpression("LTRIM", methodCallExpression.Type, new[] {
        new SqlFunctionExpression("RTRIM", methodCallExpression.Type, sqlArguments)
      });
    }

  }
}