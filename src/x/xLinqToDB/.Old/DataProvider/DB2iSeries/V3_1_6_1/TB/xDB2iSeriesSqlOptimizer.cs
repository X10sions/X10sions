using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;
using System;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.TB {
  public class xDB2iSeriesSqlOptimizer : BasicSqlOptimizer {
    public xDB2iSeriesSqlOptimizer(SqlProviderFlags sqlProviderFlags)
      : base(sqlProviderFlags) {
    }

    public override SqlStatement TransformStatement(SqlStatement statement) {
      statement = SeparateDistinctFromPagination(statement, (SelectQuery q) => q.Select.SkipValue != null);
      statement = ReplaceDistinctOrderByWithRowNumber(statement, (SelectQuery q) => q.Select.SkipValue != null);
      statement = ReplaceTakeSkipWithRowNumber(statement, (SelectQuery query) => query.Select.SkipValue != null && base.SqlProviderFlags.GetIsSkipSupportedFlag(query.Select.TakeValue, query.Select.SkipValue), true);
      switch (statement.QueryType) {
        case QueryType.Delete:
          return GetAlternativeDelete((SqlDeleteStatement)statement);
        case QueryType.Update:
          return GetAlternativeUpdate((SqlUpdateStatement)statement);
        default:
          return statement;
      }
    }

    public override SqlStatement Finalize(SqlStatement statement) {
      new QueryVisitor().Visit(statement, delegate (IQueryElement expr) {
        switch (expr.ElementType) {
          case QueryElementType.SqlParameter: {
              var sqlParameter = (SqlParameter)expr;
              sqlParameter.Name = xDB2iSeriesTools.FixUnderscore(sqlParameter.Name, $"P{sqlParameter.GetHashCode()}");
              break;
            }
          case QueryElementType.TableSource: {
              var sqlTableSource = (SqlTableSource)expr;
              sqlTableSource.Alias = xDB2iSeriesTools.FixUnderscore(sqlTableSource.Alias, $"T{sqlTableSource.SourceID}");
              break;
            }
          case QueryElementType.Column: {
              var sqlColumn = (SqlColumn)expr;
              sqlColumn.Alias = xDB2iSeriesTools.FixUnderscore(sqlColumn.Alias, $"C{sqlColumn.GetHashCode()}");
              break;
            }
        }
      });
      if (statement.SelectQuery != null) {
        new QueryVisitor().Visit(statement.SelectQuery!.Select, delegate (IQueryElement element) {
          if (element.ElementType == QueryElementType.SqlParameter) {
            ((SqlParameter)element).IsQueryParameter = false;
          }
        });
      }
      return base.Finalize(statement);
    }

    public override ISqlExpression ConvertExpressionImpl(ISqlExpression expression, ConvertVisitor visitor, EvaluationContext context) {
      expression = base.ConvertExpressionImpl(expression, visitor, context);
      if (expression is SqlBinaryExpression be) {
        switch (be.Operation) {
          case "%":
            var expr1 = be.Expr1.SystemType.IsIntegerType() ? be.Expr1 : new SqlFunction(typeof(int), "Int", be.Expr1);
            return new SqlFunction(be.SystemType, "Mod", expr1, be.Expr2); 
          case "&": return new SqlFunction(be.SystemType, "BitAnd", be.Expr1, be.Expr2);
          case "|": return new SqlFunction(be.SystemType, "BitOr", be.Expr1, be.Expr2);
          case "^": return new SqlFunction(be.SystemType, "BitXor", be.Expr1, be.Expr2);
          case "+": return be.SystemType == typeof(string) ? new SqlBinaryExpression(be.SystemType, be.Expr1, "||", be.Expr2, be.Precedence) : expression;
        } 
      } else if (expression is SqlFunction func) {
        switch (func.Name.ToLower()) {
          case "convert": {
              if (func.SystemType.ToUnderlying() == typeof(bool)) {
                var ex = AlternativeConvertToBoolean(func, 1);
                if (ex != null) {
                  return ex;
                }
              }
              var sqlType = func.Parameters[0] as SqlDataType;
              if (sqlType != null) {
                var type = sqlType.Type;
                if (type.SystemType == typeof(string) && func.Parameters[1].SystemType != typeof(string)) {
                  return new SqlFunction(func.SystemType, "RTrim", new SqlFunction(typeof(string), "Char", func.Parameters[1]));
                }
                if (type.Length > 0) {
                  return new SqlFunction(func.SystemType, type.DataType.ToString(), func.Parameters[1], new SqlValue(type.Length));
                }
                if (type.Precision > 0 && type.Scale > 0) {
                  return new SqlFunction(func.SystemType, type.DataType.ToString(), func.Parameters[1], new SqlValue(type.Precision), new SqlValue(type.Scale));
                }
                if (type.Precision > 0) {
                  return new SqlFunction(func.SystemType, type.DataType.ToString(), func.Parameters[1], new SqlValue(type.Precision));
                }
                return new SqlFunction(func.SystemType, type.DataType.ToString(), func.Parameters[1]);
              }
              var f = func.Parameters[0] as SqlFunction;
              if (f != null) {
                if (!(f.Name.ToLower() == "char")) {
                  if (f.Parameters.Length != 1) {
                    return new SqlFunction(func.SystemType, f.Name, func.Parameters[1], f.Parameters[0], f.Parameters[1]);
                  }
                  return new SqlFunction(func.SystemType, f.Name, func.Parameters[1], f.Parameters[0]);
                }
                return new SqlFunction(func.SystemType, f.Name, func.Parameters[1]);
              }
              var e = (SqlExpression)func.Parameters[0];
              return new SqlFunction(func.SystemType, e.Expr, func.Parameters[1]);
            }
          case "exists": return func.AlternativeExists();
          case "millisecond": return Div(new SqlFunction(func.SystemType, "Microsecond", func.Parameters), 1000);
          case "smalldatetime":
          case "datetime":
          case "datetime2": return new SqlFunction(func.SystemType, "TimeStamp", func.Parameters);
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
            if (func.Parameters[0].SystemType.ToUnderlying() == typeof(decimal)) {
              return new SqlFunction(func.SystemType, "Char", func.Parameters[0]);
            }
            break;
          case "nchar":
          case "nvarchar": return new SqlFunction(func.SystemType, "Char", func.Parameters);
        }
      }
      return expression;
    }
  }

}
