using IBM.EntityFrameworkCore.Infrastructure.Internal;
using IBM.EntityFrameworkCore.Migrations.Design;
using IBM.EntityFrameworkCore.Query.Expressions.Internal;
using IBM.EntityFrameworkCore.Query.Internal;
using IBM.EntityFrameworkCore.Storage;
using IBM.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Reflection;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query.Internal;

public class Db2DateAddTranslator(ISqlExpressionFactory sqlExpressionFactory, IRelationalTypeMappingSource typeMappingSource) : IMethodCallTranslator {
  private readonly Dictionary<MethodInfo, string> _methodInfoDatePartMapping = new Dictionary<MethodInfo, string>  {
        {     typeof(DateTime).GetRuntimeMethod("AddYears", new Type[1] { typeof(int) }),            "YEAR"        },
        {            typeof(DateTime).GetRuntimeMethod("AddMonths", new Type[1] { typeof(int) }),            "MONTH"        },
        {            typeof(DateTime).GetRuntimeMethod("AddDays", new Type[1] { typeof(double) }),            "DAY"        },
        {            typeof(DateTime).GetRuntimeMethod("AddHours", new Type[1] { typeof(double) }),            "HOUR"        },
        {            typeof(DateTime).GetRuntimeMethod("AddMinutes", new Type[1] { typeof(double) }),            "MINUTE"        },
        {            typeof(DateTime).GetRuntimeMethod("AddSeconds", new Type[1] { typeof(double) }),            "SECOND"        },
        {            typeof(DateTime).GetRuntimeMethod("AddMilliseconds", new Type[1] { typeof(double) }),         "MICROSECOND"        },

        {            typeof(DateTimeOffset).GetRuntimeMethod("AddYears", new Type[1] { typeof(int) }),            "YEAR"        },
        {            typeof(DateTimeOffset).GetRuntimeMethod("AddMonths", new Type[1] { typeof(int) }),            "MONTH"        },
        {            typeof(DateTimeOffset).GetRuntimeMethod("AddDays", new Type[1] { typeof(double) }),            "DAY"        },
        {            typeof(DateTimeOffset).GetRuntimeMethod("AddHours", new Type[1] { typeof(double) }),            "HOUR"        },
        {            typeof(DateTimeOffset).GetRuntimeMethod("AddMinutes", new Type[1] { typeof(double) }),            "MINUTE"        },
        {            typeof(DateTimeOffset).GetRuntimeMethod("AddSeconds", new Type[1] { typeof(double) }),            "SECOND"        },
        {            typeof(DateTimeOffset).GetRuntimeMethod("AddMilliseconds", new Type[1] { typeof(double) }),       "MICROSECOND"        }
    };


  private readonly string funName = " + ";


  public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments) {
    if (_methodInfoDatePartMapping.TryGetValue(method, out var value)) {
      SqlExpression sqlExpression = arguments[0];
      if (!value.Equals("year", StringComparison.OrdinalIgnoreCase) && !value.Equals("month", StringComparison.OrdinalIgnoreCase) && sqlExpression is SqlConstantExpression sqlConstantExpression && ((double)sqlConstantExpression.Value >= 2147483647.0 || (double)sqlConstantExpression.Value <= -2147483648.0)) {
        return null;
      }

      string text = funName;
      Type returnType = method.ReturnType;
      Expression[] arguments2;
      if (!(sqlExpression is SqlBinaryExpression binExp)) {
        if (sqlExpression.NodeType != ExpressionType.Convert || !(sqlExpression is SqlUnaryExpression { Operand: SqlBinaryExpression operand })) {
          Expression[] array = [
            instance,
            new SqlFragmentExpression(funName),
            sqlExpressionFactory.ApplyTypeMapping(sqlExpression, typeMappingSource.FindMapping(sqlExpression.Type)),
            new SqlFragmentExpression( " " + value)
          ];
          arguments2 = array;
        } else {
          arguments2 = GenerateBinaryArguments(operand, value, instance).ToArray();
        }
      } else {
        arguments2 = GenerateBinaryArguments(binExp, value, instance).ToArray();
      }

      return new Db2DateArithmeticExpression(text, returnType, arguments2, typeMappingSource.FindMapping(method.ReturnType));
    }
    return null;
  }

  public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger) {
    if (_methodInfoDatePartMapping.TryGetValue(method, out var value)) {
      SqlExpression sqlExpression = arguments[0];
      if (!value.Equals("year", StringComparison.OrdinalIgnoreCase) && !value.Equals("month", StringComparison.OrdinalIgnoreCase) && sqlExpression is SqlConstantExpression sqlConstantExpression && ((double)sqlConstantExpression.Value >= 2147483647.0 || (double)sqlConstantExpression.Value <= -2147483648.0)) {
        return null;
      }
      string text = funName;
      Type returnType = method.ReturnType;
      Expression[] arguments2;
      if (!(sqlExpression is SqlBinaryExpression binExp)) {
        if (sqlExpression.NodeType != ExpressionType.Convert || !(sqlExpression is SqlUnaryExpression { Operand: SqlBinaryExpression operand })) {
          Expression[] array = [
            instance,
            new SqlFragmentExpression(funName),
            sqlExpressionFactory.ApplyTypeMapping(sqlExpression, typeMappingSource.FindMapping(sqlExpression.Type)),
            new SqlFragmentExpression(" " + value)
          ];
          arguments2 = array;
        } else {
          arguments2 = GenerateBinaryArguments(operand, value, instance).ToArray();
        }
      } else {
        arguments2 = GenerateBinaryArguments(binExp, value, instance).ToArray();
      }
      return new Db2DateArithmeticExpression(text, returnType, arguments2, typeMappingSource.FindMapping(method.ReturnType));
    }
    return null;
  }

  private List<Expression> GenerateBinaryArguments(SqlBinaryExpression binExp, string datePart, Expression Object) {
    List<Expression> list = new List<Expression>();
    if (binExp.Left is SqlBinaryExpression binExp2) {
      list = GenerateBinaryArguments(binExp2, datePart, Object);
    }
    if (list.Count() == 0) {
      list.Add(Object);
      list.Add(sqlExpressionFactory.Fragment(funName));
      list.Add(binExp.Left);
      list.Add(sqlExpressionFactory.Fragment(" " + datePart));
    }
    list.Add(sqlExpressionFactory.Fragment(funName));
    list.Add(binExp.Right);
    list.Add(sqlExpressionFactory.Fragment(" " + datePart));
    return list;
  }
}