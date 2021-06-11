using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;
using System.Linq;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.NSarris {
  public class DB2iSeriesSqlOptimizerV5R4 : BasicSqlOptimizer {
    public DB2iSeriesSqlOptimizerV5R4(SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags) {
    }

    public override SqlStatement TransformStatement(SqlStatement statement) {
      statement = SeparateDistinctFromPagination(statement, q => q.Select.SkipValue != null);
      statement = ReplaceDistinctOrderByWithRowNumber(statement, q => q.Select.SkipValue != null);
      statement = ReplaceTakeSkipWithRowNumber(statement, query => query.Select.SkipValue != null && SqlProviderFlags.GetIsSkipSupportedFlag(query.Select.TakeValue, query.Select.SkipValue), true);

      return statement.QueryType switch {
        QueryType.Delete => GetAlternativeDelete((SqlDeleteStatement)statement),
        QueryType.Update => GetAlternativeUpdate((SqlUpdateStatement)statement),
        _ => statement,
      };
    }

    private static string SanitizeAliasOrParameterName(string text, string alternative) {
      if (string.IsNullOrWhiteSpace(text))
        return null;
      if (text.Equals("_"))
        return "underscore_";
      if (!text.All(t => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".Contains(t)))
        return alternative;
      return text;
    }

    public override SqlStatement Finalize(SqlStatement statement) {
      static long getAbsoluteHashCode(object o) => (long)o.GetHashCode() + (long)int.MaxValue;

      new QueryVisitor().Visit(statement, expr => {
        switch (expr.ElementType) {
          case QueryElementType.SqlParameter: {
              var p = (SqlParameter)expr;
              p.Name = SanitizeAliasOrParameterName(p.Name, $"P{getAbsoluteHashCode(p)}");
              break;
            }
          case QueryElementType.TableSource: {
              var table = (SqlTableSource)expr;
              table.Alias = SanitizeAliasOrParameterName(table.Alias, $"T{table.SourceID}");
              break;
            }
          case QueryElementType.Column: {
              var column = (SqlColumn)expr;
              column.Alias = SanitizeAliasOrParameterName(column.Alias, $"C{getAbsoluteHashCode(column)}");
              break;
            }
        }
      });

      static void setQueryParameter(IQueryElement element) {
        if (element.ElementType == QueryElementType.SqlParameter) {
          ((SqlParameter)element).IsQueryParameter = false;
        }
      }

      if (statement.SelectQuery != null)
        (new QueryVisitor()).Visit(statement.SelectQuery.Select, setQueryParameter);

      return base.Finalize(statement);
    }

    //public override ISqlExpression ConvertExpression(ISqlExpression expr, bool withParameters) {
    public override ISqlExpression ConvertExpressionImpl(ISqlExpression expression, ConvertVisitor visitor, EvaluationContext context) {
      expression = base.ConvertExpressionImpl(expression, visitor, context);
      if (expression is SqlBinaryExpression be) {
        switch (be.Operation) { 
          case "%":
            if (true) {
              var expr1 = !LinqToDB.Extensions.ReflectionExtensions.IsIntegerType(be.Expr1.SystemType) ? new SqlFunction(typeof(int), "Int", be.Expr1) : be.Expr1;
              return new SqlFunction(be.SystemType, "Mod", expr1, be.Expr2);
            }
          case "&": return new SqlFunction(be.SystemType, "BitAnd", be.Expr1, be.Expr2);
          case "|": return new SqlFunction(be.SystemType, "BitOr", be.Expr1, be.Expr2);
          case "^": return new SqlFunction(be.SystemType, "BitXor", be.Expr1, be.Expr2);
          case "+": return be.SystemType == typeof(string) ? new SqlBinaryExpression(be.SystemType, be.Expr1, "||", be.Expr2, be.Precedence) : expression;
        } 
      } else if (expression is SqlFunction func) {
        switch (func.Name.ToLower()) {
          case "exists":
            return AlternativeExists(func);
          case "convert":
            if (LinqToDB.Extensions.ReflectionExtensions.ToUnderlying(func.SystemType) == typeof(bool)) {
              var ex = AlternativeConvertToBoolean(func, 1);
              if (ex != null) {
                return ex;
              }
            }
            if (func.Parameters[0] is SqlDataType sqlType) {
              var type = sqlType.Type;
              if (type.SystemType == typeof(string) && func.Parameters[1].SystemType != typeof(string)) {
                return new SqlFunction(func.SystemType, "RTrim", new SqlFunction(typeof(string), "Char", func.Parameters[1]));
              } else if (type.Length > 0) {
                return new SqlFunction(func.SystemType, type.DataType.ToString(), func.Parameters[1], new SqlValue(type.Length));
              } else if (type.Precision > 0 && type.Scale > 0) {
                return new SqlFunction(func.SystemType, type.DataType.ToString(), func.Parameters[1], new SqlValue(type.Precision), new SqlValue(type.Scale));
              } else if (type.Precision > 0) {
                return new SqlFunction(func.SystemType, type.DataType.ToString(), func.Parameters[1], new SqlValue(type.Precision));
              } else {
                return new SqlFunction(func.SystemType, type.DataType.ToString(), func.Parameters[1]);
              }
            }
            if (func.Parameters[0] is SqlFunction f) {
              //Conversion is setup with the datatype as the left operand. Character datatypes are presented as
              //functions e.g. VarChar(1000). DB2 has a convert function for almost all datatypes named after the type.
              //So Linq2db Convert(VarChar(1000),SomeValue) needs to be converted to VarChar(SomeValue)
              if (f.Name == "Char" || f.Name == "Graphic" || f.Name == "VarChar" || f.Name == "VarGraphic")
                return new SqlFunction(func.SystemType, f.Name, func.Parameters[1]);

              if (f.Parameters.Length == 1)
                return new SqlFunction(func.SystemType, f.Name, func.Parameters[1], f.Parameters[0]);

              return new SqlFunction(func.SystemType, f.Name, func.Parameters[1], f.Parameters[0], f.Parameters[1]);
            }
            var e = (SqlExpression)func.Parameters[0];
            return new SqlFunction(func.SystemType, e.Expr, func.Parameters[1]);
          case "millisecond":
            return Div(new SqlFunction(func.SystemType, "Microsecond", func.Parameters), 1000);
          case "smalldatetime":
          case "datetime":
          case "datetime2":
            return new SqlFunction(func.SystemType, "TimeStamp", func.Parameters);
          case "uint16":
            return new SqlFunction(func.SystemType, "Int", func.Parameters);
          case "uint32":
            return new SqlFunction(func.SystemType, "BigInt", func.Parameters);
          case "uint64":
            return new SqlFunction(func.SystemType, "Decimal", func.Parameters);
          case "byte":
          case "sbyte":
          case "int16":
            return new SqlFunction(func.SystemType, "SmallInt", func.Parameters);
          case "int32":
            return new SqlFunction(func.SystemType, "Int", func.Parameters);
          case "int64":
            return new SqlFunction(func.SystemType, "BigInt", func.Parameters);
          case "double":
            return new SqlFunction(func.SystemType, "Float", func.Parameters);
          case "Single":
            return new SqlFunction(func.SystemType, "Real", func.Parameters);
          case "money":
            return new SqlFunction(func.SystemType, "Decimal", func.Parameters[0], new SqlValue(19), new SqlValue(4));
          case "smallmoney":
            return new SqlFunction(func.SystemType, "Decimal", func.Parameters[0], new SqlValue(10), new SqlValue(4));
          case "nchar":
          case "nvarchar":
            return new SqlFunction(func.SystemType, "Graphic", func.Parameters);
        }
      }
      return expression;
    }

    protected ISqlExpression AlternativeExists(SqlFunction func) {
      var query = (SelectQuery)func.Parameters[0];
      if (query.Select.Columns.Count == 0)
        query.Select.Columns.Add(new SqlColumn(query, new SqlExpression("'.'")));
      query.Select.Take(1, null);
      var sc = new SqlSearchCondition();
      sc.Conditions.Add(new SqlCondition(false, new SqlPredicate.IsNull(query, true)));
      return sc;
    }

  }

}
