using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Reflection;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query.Internal;

class DB2iSeriesMathMethodTranslator : IMethodCallTranslator {

  private static readonly Dictionary<MethodInfo, string> supportedMethodTranslations = new() {
    { typeof(Math).GetRuntimeMethod(nameof(Math.Abs),     [typeof(decimal)])!, "ABS" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Abs),     [typeof(double)])!, "ABS" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Abs),     [typeof(float)])!, "ABS" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Abs),     [typeof(int)])!, "ABS" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Abs),     [typeof(long)])!, "ABS" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Abs),     [typeof(sbyte)])!, "ABS" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Abs),     [typeof(short)])!, "ABS" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Ceiling), [typeof(decimal)])!, "CEILING" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Ceiling), [typeof(double)])!, "CEILING" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Floor),   [typeof(decimal)])!, "FLOOR" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Floor),   [typeof(double)])!, "FLOOR" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Round),   [typeof(decimal)])!, "ROUND" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Round),   [typeof(double)])! , "ROUND" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Round),   [typeof(decimal), typeof(int)])!, "ROUND" },
    { typeof(Math).GetRuntimeMethod(nameof(Math.Round),   [typeof(double), typeof(int)])!, "ROUND" },
  };

  private readonly ISqlExpressionFactory sqlExpressionFactory;
  public DB2iSeriesMathMethodTranslator(ISqlExpressionFactory sqlExpressionFactory) => this.sqlExpressionFactory = sqlExpressionFactory;

  public virtual SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger) {
    if (supportedMethodTranslations.TryGetValue(method, out var sqlFunctionName)) {
      var typeMapping = ExpressionExtensions.InferTypeMapping(arguments.ToArray());
      var newArguments = arguments.Select(a => sqlExpressionFactory.ApplyTypeMapping(a, typeMapping)).ToList();
      var argumentsPropagateNullability = newArguments.Select(_ => true).ToList();
      return sqlExpressionFactory.Function(sqlFunctionName, newArguments, true, argumentsPropagateNullability, method.ReturnType, typeMapping);
    }
    return null;
  }

}