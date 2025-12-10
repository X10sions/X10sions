
using IBM.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Reflection;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query.Internal;


class DB2iSeriesDateTimeMethodTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMethodCallTranslator {


  private static readonly Dictionary<MethodInfo, string> MethodInfoDatePartMapping = new() {
    { typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddYears), [typeof(int)])!, "years" },
    { typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddMonths), [typeof(int)])!, "months" },
    { typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddDays), [typeof(double)])!, "days" },
    { typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddHours), [typeof(double)])!, "hours" },
    { typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddMinutes), [typeof(double)])!, "minutes" },
    { typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddSeconds), [typeof(double)])!, "seconds" },
    { typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddMilliseconds), [typeof(double)])!, "milliseconds" },
    //{ typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddMicroseconds), [typeof(double)])!, "microseconds" },
    //{ typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddTicks), [typeof(double)])!, "ticks" },

    { typeof(DateTimeOffset).GetRuntimeMethod(nameof(DateTimeOffset.AddYears), [typeof(int)])!, "years" },
    { typeof(DateTimeOffset).GetRuntimeMethod(nameof(DateTimeOffset.AddMonths), [typeof(int)])!, "months" },
    { typeof(DateTimeOffset).GetRuntimeMethod(nameof(DateTimeOffset.AddDays), [typeof(double)])!, "days" },
    { typeof(DateTimeOffset).GetRuntimeMethod(nameof(DateTimeOffset.AddHours), [typeof(double)])!, "hours" },
    { typeof(DateTimeOffset).GetRuntimeMethod(nameof(DateTimeOffset.AddMinutes), [typeof(double)])!, "minutes" },
    { typeof(DateTimeOffset).GetRuntimeMethod(nameof(DateTimeOffset.AddSeconds), [typeof(double)])!, "seconds" },
    { typeof(DateTimeOffset).GetRuntimeMethod(nameof(DateTimeOffset.AddMilliseconds), [typeof(double)])!, "milliseconds" },

    { typeof(DateOnly).GetRuntimeMethod(nameof(DateOnly.AddYears), [typeof(int)])!, " years" },
    { typeof(DateOnly).GetRuntimeMethod(nameof(DateOnly.AddMonths), [typeof(int)])!, " months" },
    { typeof(DateOnly).GetRuntimeMethod(nameof(DateOnly.AddDays), [typeof(int)])!, " days" },

    { typeof(DateOnly).GetRuntimeMethod(nameof(TimeOnly.AddHours), [typeof(int)])!, " hours" },
    { typeof(DateOnly).GetRuntimeMethod(nameof(TimeOnly.AddMinutes), [typeof(int)])!, " minutes" },

  };


  public SqlExpression? Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger) {
    if (method.DeclaringType != typeof(DateTime) && method.DeclaringType != typeof(DateTimeOffset)) {
      return null;
    }

    if (MethodInfoDatePartMapping.TryGetValue(method, out var datePart) && instance != null) {
      return sqlExpressionFactory.Function("DateTimeAdd", [sqlExpressionFactory.Constant(datePart), arguments[0], instance], true, [true, true, true], instance.Type, instance.TypeMapping);
    }

    if (MethodInfoDatePartMapping.TryGetValue(method, out var intervalSqlLiteral)) {
      var intervalFragment = sqlExpressionFactory.Fragment(intervalSqlLiteral);
      var amountToAdd = sqlExpressionFactory.Convert(arguments[0], typeof(int));
      var db2FunctionCall = sqlExpressionFactory.Function("TIMESTAMPADD", [intervalFragment, amountToAdd, instance], false, [true, true, true], method.ReturnType);
      return db2FunctionCall;
    }

    return null;
  }

}
