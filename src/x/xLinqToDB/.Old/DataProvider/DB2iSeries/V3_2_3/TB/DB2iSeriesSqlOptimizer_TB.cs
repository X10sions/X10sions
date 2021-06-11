using LinqToDB;
using LinqToDB.Extensions;
using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB {
  internal class DB2iSeriesSqlOptimizer_TB : _BaseDB2iSeriesSqlOptimizer {
    public DB2iSeriesSqlOptimizer_TB(SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags) { }
  }

  internal class DB2iSeriesSqlOptimizer_RoyChase : _BaseDB2iSeriesSqlOptimizer {
    public DB2iSeriesSqlOptimizer_RoyChase(SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags) { }

    public override ISqlExpression ConvertExpressionImpl(ISqlExpression expression, ConvertVisitor visitor, EvaluationContext context) {
      //    public override ISqlExpression ConvertExpression(ISqlExpression expr, bool withParameters) {
      expression = base.ConvertExpressionImpl(expression, visitor, context);
      if (expression is SqlFunction func) {
        switch (func.Name.ToLower()) {
          case "exists": return AlternativeExists(func);
        }
      }
      return expression;
    }

    protected ISqlExpression AlternativeExists(SqlFunction func) {
      var query = (SelectQuery)func.Parameters[0];
      if (query.Select.Columns.Count == 0)
        query.Select.Columns.Add(new SqlColumn(query, new SqlExpression("'.'")));
      query.Select.Take(1, null);
      return new SqlSearchCondition {
        Conditions = {
          new SqlCondition( false, new SqlPredicate.IsNull(query,  true))
        }
      };
    }
  }

  internal abstract class _BaseDB2iSeriesSqlOptimizer : BasicSqlOptimizer {
    public _BaseDB2iSeriesSqlOptimizer(SqlProviderFlags sqlProviderFlags)
    : base(sqlProviderFlags) { }

    protected override ISqlExpression ConvertFunction(SqlFunction func) {
      func = ConvertFunctionParameters(func, false);
      func = func.Name.ToLower() switch {
        "coalesce" => new SqlFunction(func.SystemType, "Value", func.Parameters[0], func.Parameters[1]),
        "replicate" => new SqlFunction(func.SystemType, "Repeat", func.Parameters[0], func.Parameters[1]),
        "x_upper" => new SqlFunction(func.SystemType, "sqlbuilder_UCase", func.Parameters[0]),
        _=> func
      };
      return base.ConvertFunction(func);
    }

    public override SqlStatement Finalize(SqlStatement statement) {
      new QueryVisitor().Visit(statement.SelectQuery.Select, SetQueryParameter);
      statement = base.Finalize(statement);
      switch (statement.QueryType) {
        case QueryType.Delete: return GetAlternativeDelete((SqlDeleteStatement)statement);
        case QueryType.Update: return GetAlternativeUpdate((SqlUpdateStatement)statement);
        default: return statement;
      }
    }
     
    public override ISqlExpression ConvertExpressionImpl(ISqlExpression expression, ConvertVisitor visitor, EvaluationContext context) {
      expression = base.ConvertExpressionImpl(expression, visitor, context);
      if (expression is SqlBinaryExpression be) {
        switch (be.Operation) {
          case "%": {
              var expr2 = (!ReflectionExtensions.IsIntegerType(be.Expr1.SystemType)) ? new SqlFunction(typeof(int), "Int", be.Expr1) : be.Expr1;
              return new SqlFunction(be.SystemType, "Mod", expr2, be.Expr2);
            }
          case "&": return new SqlFunction(be.SystemType, "BitAnd", be.Expr1, be.Expr2);
          case "|": return new SqlFunction(be.SystemType, "BitOr", be.Expr1, be.Expr2);
          case "^": return new SqlFunction(be.SystemType, "BitXor", be.Expr1, be.Expr2);
          case "+": return (be.SystemType == typeof(string)) ? new SqlBinaryExpression(be.SystemType, be.Expr1, "||", be.Expr2, be.Precedence) : expression;
        } 
      } else if (expression is SqlFunction func) {
        switch (func.Name.ToLower()) {
          case "convert": {
              if (ReflectionExtensions.ToUnderlying(func.SystemType) == typeof(bool)) {
                var ex = AlternativeConvertToBoolean(func, 1); 
                if (ex != null) {
                  return ex;
                }
              }
              if (func.Parameters[0] is SqlDataType) {
                var type = (SqlDataType)func.Parameters[0];
                if (type.Type.SystemType == typeof(string) && func.Parameters[1].SystemType != typeof(string)) {
                  return new SqlFunction(func.SystemType, "RTrim", new SqlFunction(typeof(string), "Char", func.Parameters[1]));
                }
                var length = type.Type.Length;
                if ((length.HasValue ? new bool?(length.GetValueOrDefault() > 0) : null).GetValueOrDefault()) {
                  return new SqlFunction(func.SystemType, type.Type.DataType.ToString(), func.Parameters[1], new SqlValue(type.Type.Length));
                }
                length = type.Type.Precision;
                if ((length.HasValue ? new bool?(length.GetValueOrDefault() > 0) : null).GetValueOrDefault()) {
                  return new SqlFunction(func.SystemType, type.Type.DataType.ToString(), func.Parameters[1], new SqlValue(type.Type.Precision), new SqlValue(type.Type.Scale));
                }
                return new SqlFunction(func.SystemType, type.Type.DataType.ToString(), func.Parameters[1]);
              }
              if (func.Parameters[0] is SqlFunction) {
                var f = (SqlFunction)func.Parameters[0];
                return (f.Name.ToLower() == "char")
                  ? new SqlFunction(func.SystemType, f.Name, func.Parameters[1])
                  : ((f.Parameters.Length == 1)
                  ? new SqlFunction(func.SystemType, f.Name, func.Parameters[1], f.Parameters[0])
                  : new SqlFunction(func.SystemType, f.Name, func.Parameters[1], f.Parameters[0], f.Parameters[1]));
              }
              var e = (SqlExpression)func.Parameters[0];
              return new SqlFunction(func.SystemType, e.Expr, func.Parameters[1]);
            }
          case "millisecond": return Div(new SqlFunction(func.SystemType, "Microsecond", func.Parameters), 1000);
          case "smallDateTime":
          case "dateTime":
          case "dateTime2": return new SqlFunction(func.SystemType, "TimeStamp", func.Parameters);
          case "uint16": return new SqlFunction(func.SystemType, "Int", func.Parameters);
          case "uint32": return new SqlFunction(func.SystemType, "BigInt", func.Parameters);
          case "uint64": return new SqlFunction(func.SystemType, "Decimal", func.Parameters);
          case "byte":
          case "sbyte":
          case "int16": return new SqlFunction(func.SystemType, "SmallInt", func.Parameters);
          case "int32": return new SqlFunction(func.SystemType, "Int", func.Parameters);
          case "int64": return new SqlFunction(func.SystemType, "BigInt", func.Parameters);
          case "double": return new SqlFunction(func.SystemType, "Float", func.Parameters);
          case "single": return new SqlFunction(func.SystemType, "Real", func.Parameters);
          case "money": return new SqlFunction(func.SystemType, "Decimal", func.Parameters[0], new SqlValue(19), new SqlValue(4));
          case "smallmoney": return new SqlFunction(func.SystemType, "Decimal", func.Parameters[0], new SqlValue(10), new SqlValue(4));
          case "varchar":
            if (ReflectionExtensions.ToUnderlying(func.Parameters[0].SystemType) == typeof(decimal))
              return new SqlFunction(func.SystemType, "Char", func.Parameters[0]);
            break;
          case "nchar":
          case "nvarchar": return new SqlFunction(func.SystemType, "Char", func.Parameters);
          case "datediff":
            switch ((Sql.DateParts)((SqlValue)func.Parameters[0]).Value) {
              case Sql.DateParts.Day: return new SqlExpression(typeof(int), "((Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))) / 86400", 80, func.Parameters[2], func.Parameters[1]);
              case Sql.DateParts.Hour: return new SqlExpression(typeof(int), "((Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))) / 3600", 80, func.Parameters[2], func.Parameters[1]);
              case Sql.DateParts.Minute: return new SqlExpression(typeof(int), "((Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))) / 60", 80, func.Parameters[2], func.Parameters[1]);
              case Sql.DateParts.Second: return new SqlExpression(typeof(int), "(Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))", 60, func.Parameters[2], func.Parameters[1]);
              case Sql.DateParts.Millisecond: return new SqlExpression(typeof(int), "((Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))) * 1000 + (MICROSECOND({0}) - MICROSECOND({1})) / 1000", 60, func.Parameters[2], func.Parameters[1]);
            }
            break;
            //case "ltrim":            throw new NotImplementedException("ConvertExpression.ltrim");
            //case "rtrim":            throw new NotImplementedException("ConvertExpression.rtrim");
        }
      }
      return expression;
    }

    static void SetQueryParameter(IQueryElement element) {
      if (element.ElementType == QueryElementType.SqlParameter) {
        ((SqlParameter)element).IsQueryParameter = false;
      }
    }

  }

}
