using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Reflection;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query.Internal;

class DB2iSeriesStringMethodTranslator : IMethodCallTranslator {

  private readonly ISqlExpressionFactory sqlExpressionFactory;
  public DB2iSeriesStringMethodTranslator(ISqlExpressionFactory sqlExpressionFactory) => this.sqlExpressionFactory = sqlExpressionFactory;

  private static readonly MethodInfo SubstringMethodInfoWithOneArg = typeof(string).GetRuntimeMethod(nameof(string.Substring), [typeof(int)])!;
  private static readonly MethodInfo SubstringMethodInfoWithTwoArgs = typeof(string).GetRuntimeMethod(nameof(string.Substring), [typeof(int), typeof(int)])!;
  private static readonly MethodInfo ToLowerMethodInfo = typeof(string).GetRuntimeMethod(nameof(string.ToLower), Type.EmptyTypes)!;
  private static readonly MethodInfo ToUpperMethodInfo = typeof(string).GetRuntimeMethod(nameof(string.ToUpper), Type.EmptyTypes)!;
  //private static readonly MethodInfo TrimStartMethodInfoWithoutArgs = typeof(string).GetRuntimeMethod(nameof(string.TrimStart), Type.EmptyTypes)!;
  //private static readonly MethodInfo TrimStartMethodInfoWithCharArg = typeof(string).GetRuntimeMethod(nameof(string.TrimStart), [typeof(char)])!;
  //private static readonly MethodInfo TrimEndMethodInfoWithoutArgs = typeof(string).GetRuntimeMethod(nameof(string.TrimEnd), Type.EmptyTypes)!;
  //private static readonly MethodInfo TrimEndMethodInfoWithCharArg = typeof(string).GetRuntimeMethod(nameof(string.TrimEnd), [typeof(char)])!;
  private static readonly MethodInfo TrimMethodInfoWithoutArgs = typeof(string).GetRuntimeMethod(nameof(string.Trim), Type.EmptyTypes)!;
  //private static readonly MethodInfo TrimMethodInfoWithCharArg = typeof(string).GetRuntimeMethod(nameof(string.Trim), [typeof(char)])!;



  public SqlExpression? Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger) {

    if (ToLowerMethodInfo.Equals(method)) {
      return sqlExpressionFactory.Function("LCASE", [instance], true, [true], method.ReturnType, instance.TypeMapping);
    }
    if (ToUpperMethodInfo.Equals(method)) {
      return sqlExpressionFactory.Function("UCASE", [instance], true, [true], method.ReturnType, instance.TypeMapping);
    }
    if (TrimMethodInfoWithoutArgs.Equals(method)) {
      return sqlExpressionFactory.Function("TRIM", [instance], true, [true], typeof(string), instance.TypeMapping);
    }
    if (SubstringMethodInfoWithOneArg.Equals(method)) {
      var start = sqlExpressionFactory.Add(arguments[0], sqlExpressionFactory.Fragment("1"));
      return sqlExpressionFactory.Function("SUBSTR", new[] { instance, start }, true, [true], method.ReturnType, instance.TypeMapping);
    }
    if (SubstringMethodInfoWithTwoArgs.Equals(method)) {
      var start = sqlExpressionFactory.Add(arguments[0], sqlExpressionFactory.Fragment("1"));
      return sqlExpressionFactory.Function("SUBSTR", new[] { instance, start, arguments[1] }, true, [true], method.ReturnType, instance.TypeMapping);
    }
    return null;
  }



}
